using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Edge", menuName = "Edge")]
public class EdgesSO : ScriptableObject
{
    public string Label;
    public Vector3 Position;
    public string QID;
}
