using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MyCustomEditor;

[ExecuteInEditMode]
public class SetEdgeTagOnStart : MonoBehaviour
{
    EdgeDataDisplay dataDisplay;
    // Start is called before the first frame update
    void Start()
    {
        EdgeDataDisplay dataDisplay = GetComponent<EdgeDataDisplay>();
        if (dataDisplay != null && dataDisplay.edgeData != null)
        {
            gameObject.transform.parent.tag = "Untagged";
            gameObject.tag = dataDisplay.edgeData.name;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
#if UNITY_EDITOR
        dataDisplay = GetComponent<EdgeDataDisplay>();
        if (dataDisplay != null && dataDisplay.edgeData != null)
        {
            TagsAndLayers.AddTag(dataDisplay.edgeData.name);
            gameObject.transform.parent.tag = "Untagged";
            gameObject.tag = dataDisplay.edgeData.name;
        }
#endif
    }
}
