using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Node", menuName = "Node")]
public class NodeSO : ScriptableObject
{
 public string Label;
    public Vector3 Position;
    public string QID;
    public Quaternion Rotation;
    [TextArea(10, 100)]
    public string Description;
    [TextArea(10, 100)]
    public string EnzymeRequired;
    public string MolecularFormula;
    [TextArea(10, 100)]
    public string IUPACNames;
    [TextArea(10, 100)]
    public string StructuralDescription;
    public string Charge;
    public string Pubchemlink;
    public string link;

    public void init(string name, string newQID, string desc, string moleForm, string IUPAC, string StrucDesc, string charge, string pubchem) {
        this.name = name;
        this.Label = name;
        this.QID = newQID;
        this.Description = desc;
        this.MolecularFormula = moleForm;
        this.IUPACNames = IUPAC;
        this.StructuralDescription = StrucDesc;
        this.Charge = charge;
        this.Pubchemlink = pubchem;
    }
}
