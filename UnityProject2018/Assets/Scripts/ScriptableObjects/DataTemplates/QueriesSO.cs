using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "List Of Queries", menuName ="List Of Queries")]
[System.Serializable]
public class QueriesSO : ScriptableObject
{
    [TextArea(10, 100)]
    public string Label;
    [TextArea(10, 100)]
    public string Description;

    [TextArea(10, 100)]
    public string pathwayEdges;

    [TextArea(10, 100)]
    public string pathwayEdgesDescription;

    [TextArea(10, 100)]
    public string pathwayNodes;

    [TextArea(10, 100)]
    public string pathwayNodesDescription;

    [TextArea(10, 100)]
    public string nodeData;
    
    [TextArea(10, 100)]
    public string testOnWikiData;



    
}
