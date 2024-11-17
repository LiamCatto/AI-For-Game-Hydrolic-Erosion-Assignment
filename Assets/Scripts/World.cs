using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public GameObject o_pointMarker;
    public PointMarker s_pointMarker;
    public Material material;

    public int width;
    public int height;
    public int elevationScale;
    public float noiseScale;
    public float noiseOffsetX;
    public float noiseOffsetY;
    public float waterLevel;
    public float sandLevel;
    public float groundLevel;
    public bool updateMap;

    private Mesh worldMap;

    // Start is called before the first frame update
    void Start()
    {
        s_pointMarker = o_pointMarker.GetComponent<PointMarker>();

        worldMap = new Mesh();
        GetComponent<MeshFilter>().mesh = worldMap;

        GenerateWorldMap();
    }

    // Update is called once per frame
    void Update()
    {
        if (updateMap)
        {
            s_pointMarker.ClearMarkers();
            GenerateWorldMap();

            updateMap = false;
        }
    }

    void GenerateWorldMap()
    {
        // Add Vertices
        Vector3[] newVertices = new Vector3[(width + 1) * (height + 1)];
        int index = 0;

        for (int k = 0; k <= height; k++)
        {
            for(int i = 0; i <= width; i++)
            {
                float elevation = Mathf.PerlinNoise((i * noiseScale) + noiseOffsetX, (k * noiseScale) + noiseOffsetY);
                if (elevation < 0) elevation = 0;
                if (elevation > 1) elevation = 1;

                float altitude = elevation * elevationScale;
                //if (altitude <= waterLevel) altitude = waterLevel - 0.001f;

                newVertices[index] = new Vector3(i, altitude, k);

                s_pointMarker.CreateMarker(newVertices[index], Quaternion.identity);
                index++;
            }
        }

        // Add Triangles
        int[] newTriangles = new int[width * height * 6];
        int tIndex = 0;
        int vertex = 0;

        for (int k = 0; k < height; k++)
        {
            for (int i = 0; i < width; i++)
            {
                newTriangles[tIndex] = vertex;
                newTriangles[tIndex + 1] = vertex + width + 1;
                newTriangles[tIndex + 2] = vertex + 1;

                newTriangles[tIndex + 3] = vertex + 1;
                newTriangles[tIndex + 4] = vertex + width + 1;
                newTriangles[tIndex + 5] = vertex + width + 2;

                vertex++;
                tIndex += 6;
            }
            vertex++;
        }

        worldMap.Clear();

        worldMap.vertices = newVertices;
        worldMap.triangles = newTriangles;
        worldMap.RecalculateNormals();

        material.SetFloat("waterLevel", waterLevel);
        material.SetFloat("sandLevel", sandLevel);
        material.SetFloat("groundLevel", groundLevel);
    }
}
