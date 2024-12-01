using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PointMarker : MonoBehaviour
{
    // Highlights each vertex in the mesh for debugging

    public GameObject marker;
    public List<GameObject> markerList;

    public bool showMarkers;
    
    public void CreateMarker(Vector3 position, Quaternion rotation)
    {
        if (showMarkers) markerList.Add(Instantiate(marker, position, rotation));
    }

    public void ClearMarkers()
    {
        foreach (GameObject obj in markerList) Destroy(obj);
        markerList.Clear();  
    }
}
