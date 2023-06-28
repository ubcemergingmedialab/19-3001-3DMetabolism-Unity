using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject for Nodes
/// </summary>
[CreateAssetMenu(fileName = "New Node", menuName = "Node")]
[System.Serializable]
public class NodeSO : GenericSO
{
    // public string Label;
    public Vector3 Position;
    // public string QID;
    public Quaternion Rotation;
    [TextArea(10, 100)]
    // public string Description;
    public string EnzymeRequired;
    public string MolecularFormula;
    [TextArea(10, 100)]
    public string IUPACNames;
    [TextArea(10, 100)]
    public string StructuralDescription;
    public string Charge;
    public string Pubchemlink;
    public string link;
    public string CID;

    /// <summary>
    /// Initialize NodeSO with essential fields
    /// </summary>
    public void init(string name, string newQID, string desc, string moleForm, string IUPAC, string StrucDesc, string charge, string pubchem, string cid) {
        this.name = name;

        this.Label = name;

        foreach (string blackListedChar in Constants.GetBlackListedLabels())
        {
            if (name.Contains(blackListedChar))
            {
                searchCategory tempSearchCategory = searchCategory.standard;
                Enum.TryParse(blackListedChar.Replace("(", "").Replace(")", ""), out tempSearchCategory);
                searchCategory = tempSearchCategory;
                this.Label = name.Replace(blackListedChar, "");                
            }
        }

        this.QID = newQID;
        this.Description = desc;
        this.MolecularFormula = moleForm;
        this.IUPACNames = IUPAC;
        this.StructuralDescription = StrucDesc;
        this.Charge = charge;
        this.Pubchemlink = pubchem;
        this.CID = cid;
    }

    /// <summary>
    /// Override required for usage as key in a Dictionary. 
    /// 2 NodeSOs are equal if their QID are equal. 
    /// </summary>
    public override bool Equals(object obj)
    {
        return obj is NodeSO sO &&
               base.Equals(obj) &&
               QID == sO.QID;
    }

    /// <summary>
    /// Override required for usage as key in a Dictionary. 
    /// 2 NodeSOs have identical hashcode if their QID are equal. 
    /// </summary>
    public override int GetHashCode()
    {
        int hashCode = -1760945465;
        hashCode = hashCode * -1521134295 + base.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(QID);
        return hashCode;
    }

}
