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

    public override bool Equals(object obj)
    {
        return obj is NodeSO sO &&
               base.Equals(obj) &&
               Label == sO.Label &&
               QID == sO.QID;
    }

    public override int GetHashCode()
    {
        int hashCode = -972108708;
        hashCode = hashCode * -1521134295 + base.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Label);
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(QID);
        return hashCode;
    }
}
