using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pathway Beta", menuName = "Pathway Beta")]
public class PathwaySOBeta : ScriptableObject
{
    public string QID;
    public string Label;
    public string Description;
    // Note: How to manage edges if there is the connections are dealt with in nodes? we need edges for highlighting 
    public Dictionary<NodeSOBeta, List<EdgeSOBeta>> LocalNetwork;
    
}
