using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    Mesh worldMap;
    Vector3[] newVertices;
    Vector2[] newUV;
    int[] newTriangles;

    // Start is called before the first frame update
    void Start()
    {
        worldMap = new Mesh();
        GetComponent<MeshFilter>().mesh = worldMap;
        worldMap.vertices = newVertices;
        worldMap.uv = newUV;
        worldMap.triangles = newTriangles;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
