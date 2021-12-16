using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Edge Beta", menuName = "Edge Beta")]
public class EdgeSOBeta : ScriptableObject
{
    public string Label;
    public Vector3 Position;
    public string QID;
    public string Description;
    public string EnergyConsumed;
    public string EnergyProduced;
    public string GibbsFreeEnergy;

    public List<NodeSOBeta> reactants;
    public List<NodeSOBeta> products;
    public bool bidirectional = false;

    
    public string AuxLabel;
    public Vector3 AuxPosition;
    public string AuxQID;
    public string AuxDescription;
    public string AuxEnergyConsumed;
    public string AuxEnergyProduced;
    public string AuxGibbsFreeEnergy;

}
