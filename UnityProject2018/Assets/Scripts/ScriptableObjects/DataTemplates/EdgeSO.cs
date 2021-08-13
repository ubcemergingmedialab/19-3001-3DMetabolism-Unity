using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Edge", menuName = "Edge")]
public class EdgeSO : ScriptableObject
{
    public string Label;
    public Vector3 Position;
    public string QID;
    public string Description;
    public string EnergyConsumed;
    public string EnergyProduced;
    public string GibbsFreeEnergy;

}
