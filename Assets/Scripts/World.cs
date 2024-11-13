using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public int width;
    public int height;

    //private GameObject worldObject;
    private Mesh worldMap;
    private Material material;

    private Vector2[] newUV;

    // Start is called before the first frame update
    void Start()
    {
        worldMap = new Mesh();

        //worldObject = new GameObject("World", typeof(MeshRenderer), typeof(MeshFilter));

        //worldObject.GetComponent<MeshFilter>().mesh = worldMap;
        GetComponent<MeshFilter>().mesh = worldMap;
        GenerateWorldMap();
    }

    // Update is called once per frame
    void Update()
    {
        GenerateWorldMap();
    }

    void GenerateWorldMap()
    {
        // Add Vertices
        Vector3[] newVertices = new Vector3[(width + 1) * (height + 1)];
        int index = 0;

        for (int i = 0; i < width + 1; i++)
        {
            for(int k = 0; k < height + 1; k++)
            {
                newVertices[index] = new Vector3(i, 0, k);
                index++;
            }
        }
        /*newVertices[0] = new Vector3(0, 0, 0);
        newVertices[1] = new Vector3(0, 0, 0);
        newVertices[2] = new Vector3(0, 0, 0);
        newVertices[3] = new Vector3(0, 0, 0);*/

        // Add Triangles
        int numGridSquares = (width - 1) * (height - 1);

        int[] newTriangles = new int[numGridSquares * 6];
        int vertex = 0;
        int tIndex = 0;
        newTriangles[0] = 0;
        newTriangles[1] = 1;
        newTriangles[2] = 2;

        newTriangles[3] = 0;
        newTriangles[4] = 2;
        newTriangles[5] = 3;
        /*for (int i = 0; i < width - 1; i++)
        {
            for (int k = 0; k < height - 1; k++)
            {
                newTriangles[tIndex] = vertex;
                newTriangles[tIndex + 1] = vertex + width + 1;
                newTriangles[tIndex + 2] = vertex + 1;

                newTriangles[tIndex + 3] = vertex + 1;
                newTriangles[tIndex + 4] = vertex + width + 1;
                newTriangles[tIndex + 5] = vertex + width + 2;

                tIndex += 6;
                vertex++;
            }
            vertex++;
        }*/

        /*for (int i = 0; i < newVertices.Length; i ++)
        {
            newTriangles[i] = i + width + 1;
            newTriangles[i + 1] = i;
            newTriangles[i + 2] = i + 1;

            newTriangles[i + 3] = i + width + 1;
            newTriangles[i + 4] = i + 1;
            newTriangles[i + 5] = i + width + 2;
        }*/

        worldMap.Clear();

        worldMap.vertices = newVertices;
        worldMap.triangles = newTriangles;
    }
}
