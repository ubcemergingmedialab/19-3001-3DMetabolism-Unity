using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameNodes : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach(TextMesh node in GetComponentsInChildren<TextMesh>())
        {
            node.text = node.transform.parent.name;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
