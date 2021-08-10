using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pathway", menuName = "Pathway")]
public class PathwaySO : ScriptableObject
{
    public string QID;
    public List<NodeSO> nodes;
    public List<EdgeSO> edges;
    public string Label;
    public string Description;
}
