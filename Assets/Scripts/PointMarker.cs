using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PointMarker : MonoBehaviour
{
    public GameObject marker;
    public List<GameObject> markerList;

    public bool showMarkers;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
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
