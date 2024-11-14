using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public GameObject o_pointMarker;
    public PointMarker s_pointMarker;

    public int width;
    public int height;

    //private GameObject worldObject;
    private Mesh worldMap;
    private Material material;

    private Vector2[] newUV;

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
        s_pointMarker.ClearMarkers();
        GenerateWorldMap();
    }

    // The triangles don't generate correctly with certain dimensions, and never work when height is greater than width. A square map always seems to work.
    void GenerateWorldMap()
    {
        // Add Vertices
        Vector3[] newVertices = new Vector3[(width + 1) * (height + 1)];
        int index = 0;

        for (int i = 0; i < width; i++)
        {
            for(int k = 0; k < height; k++)
            {
                newVertices[index] = new Vector3(i, 0, k);
                s_pointMarker.CreateMarker(newVertices[index], Quaternion.identity);
                index++;
            }
        }

        // Add Triangles
        int[] newTriangles = new int[width * height * 6];
        int tIndex = 0;

        for (int i = 0; i < width * height; i++)
        {
            newTriangles[tIndex] = i + width;
            newTriangles[tIndex + 1] = i;
            newTriangles[tIndex + 2] = i + 1;

            newTriangles[tIndex + 3] = i + width;
            newTriangles[tIndex + 4] = i + 1;
            newTriangles[tIndex + 5] = i + width + 1;

            tIndex += 6;
        }

        worldMap.Clear();

        worldMap.vertices = newVertices;
        worldMap.triangles = newTriangles;
    }
}
