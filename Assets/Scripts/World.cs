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
    public float islandMix;
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
            for(int i = 0; i <= width; i++) // For some reason distance isn't taken into account; all vertices are being raised by the same amount instead of their elevations varying by distance
            {
                //float nx = (2 * i) / width;
                //float ny = (2 * k) / height;
                //float nx = (i / width) * 0.5f;
                //float ny = (k / height) * 0.5f;
                float nx = i - width / 2;
                float ny = k - height / 2;
                float elevation = Mathf.PerlinNoise((i * noiseScale) + noiseOffsetX, (k * noiseScale) + noiseOffsetY);
                if (elevation < 0) elevation = 0;   
                if (elevation > 1) elevation = 1;

                //float nx = ((2 * i) / width) - 1;
                //float ny = ((2 * k) / height) - 1;
                //float nx = (i * noiseScale) + noiseOffsetX;
                //float ny = (k * noiseScale) + noiseOffsetY;
                float distFromCenter = 1 - (1 - Mathf.Pow(i, 2)) * (1 - Mathf.Pow(k, 2));
                //float distFromCenter = Mathf.Sqrt(Mathf.Pow(nx, 2) + Mathf.Pow(ny, 2));

                // closest attempt
                //Vector2 vectorToCenter = new Vector2(0, 0) - new Vector2(nx, ny);
                //float distFromCenter = vectorToCenter.magnitude;

                distFromCenter = 1 / distFromCenter;
                if (distFromCenter < 0.01f) distFromCenter = 0.01f;
                if (distFromCenter > 0.99f) distFromCenter = 0.99f;
                //if (distFromCenter < 0) distFromCenter = 0;
                //if (distFromCenter > 1) distFromCenter = 1;
                Debug.Log("dist: " + distFromCenter);
                //float elevation = Mathf.PerlinNoise(i * scaleOffsetX, k * scaleOffsetY);
                elevation = Mathf.Lerp(elevation, 1 - distFromCenter, islandMix);
                Debug.Log("e: " + elevation);

                //if (elevation < 0) elevation = 0;
                //if (elevation > 1) elevation = 1;
                //elevation = 1 / elevation;

                float altitude = elevation * elevationScale;
                //if (altitude <= waterLevel) altitude = waterLevel - 0.001f;

                //newVertices[index] = new Vector3(i, altitude, k);
                newVertices[index] = new Vector3(nx, altitude, ny);

                //newVertices[index] = new Vector3(i, elevation, k);

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
