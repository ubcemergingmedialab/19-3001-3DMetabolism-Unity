using UnityEditor;
using UnityEngine;
using MyCustomEditor;
[ExecuteInEditMode]
public class SetNodeTagOnStart : MonoBehaviour
{

    NodeDataDisplay dataDisplay;
    // Start is called before the first frame update
    void Start()
    {
        dataDisplay = GetComponent<NodeDataDisplay>();
        if (dataDisplay != null && dataDisplay.nodeData != null)
        {
            gameObject.transform.parent.tag = "Untagged";
            gameObject.tag = dataDisplay.nodeData.name;
        }
    }

    private void OnEnable()
    {
#if UNITY_EDITOR
        dataDisplay = GetComponent<NodeDataDisplay>();
        if (dataDisplay != null && dataDisplay.nodeData != null)
        {
            TagsAndLayers.AddTag(dataDisplay.nodeData.name);
            gameObject.transform.parent.tag = "Untagged";
            gameObject.tag = dataDisplay.nodeData.name;
        }
#endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
