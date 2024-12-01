using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public GameObject o_pointMarker;
    public PointMarker s_pointMarker;
    public Material material;

    // Map dimensions
    public int width;
    public int height;
    public int elevationScale;

    // Terrain generation data
    public float noiseScale;
    public float noiseOffsetX;
    public float noiseOffsetY;
    public float waterLevel;
    public float sandLevel;
    public float groundLevel;
    public float islandMix;

    // Terrain controls
    public bool aboveWaterLevelOnly;
    public bool randomGeneration;
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
        // User decides when the map updates. Since the program is unoptimized it causes severe lag when rendering the world each frame.
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
        int area = width * height;
        int randomPerlinX = Random.Range(0, 1000);
        int randomPerlinY = Random.Range(0, 1000);

        for (int k = 0; k <= height; k++)
        {
            for(int i = 0; i <= width; i++)
            {
                float nx = i - (width / 2);
                float ny = k - (height / 2);

                // Generate terrain heightmap
                float elevation = 0;
                if (randomGeneration) elevation = Mathf.PerlinNoise((i * noiseScale) + randomPerlinX, (k * noiseScale) + randomPerlinY);
                else elevation = Mathf.PerlinNoise((i * noiseScale) + noiseOffsetX, (k * noiseScale) + noiseOffsetY);
                if (elevation < 0) elevation = 0;   
                if (elevation > 1) elevation = 1;

                // Generate base heightmap for a central island
                float distFromCenter = (Mathf.Pow(width / 2, 2) - Mathf.Pow(nx, 2)) * (Mathf.Pow(height / 2, 2) - Mathf.Pow(ny, 2));
                distFromCenter /= Mathf.Pow(width / 2, 2) * Mathf.Pow(height / 2, 2);   // divided by the maximum value of distFromCenter to get the numbers between 0 and 1

                // Mix the two heightmaps together
                elevation = Mathf.Lerp(elevation, distFromCenter, islandMix);
                float altitude = elevation * elevationScale;
                
                // Toggle whether to render the terrain below water level
                if (aboveWaterLevelOnly) if (altitude <= waterLevel) altitude = waterLevel - 0.001f;


                newVertices[index] = new Vector3(nx, altitude, ny);

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

        // Send the height for each layer of terrain to the shader
        material.SetFloat("waterLevel", waterLevel);
        material.SetFloat("sandLevel", sandLevel);
        material.SetFloat("groundLevel", groundLevel);  
    }
}
