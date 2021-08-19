using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetEdgeBounds : MonoBehaviour
{
    Bounds bounds;
    // Start is called before the first frame update
    void Start()
    {
        if(GetComponent<MeshRenderer>())
        {
            bounds = GetComponent<MeshRenderer>().bounds;
        }
        if(bounds != null)
        {
            //Debug.Log("setting float to " + bounds.extents.z);
            GetComponent<MeshRenderer>().material.SetFloat("_Size", bounds.extents.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
