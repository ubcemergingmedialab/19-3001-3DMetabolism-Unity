using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Node Beta", menuName = "Node Beta")]
public class NodeSOBeta : ScriptableObject
{
    //public EdgeSOBeta parent;
    public string Label;
    public Vector3 Position;
    public string QID;
    public Quaternion Rotation;
    [TextArea(10, 100)]
    public string Description;
    public string Charge;
    [TextArea(10, 100)]
    public string EnzymeRequired;
    public string MolecularFormula;
    [TextArea(10, 100)]
    public string IUPACNames;
    public string link;

    public void init(string name) {
        this.name = name;
        this.Label = name;
    }
}
