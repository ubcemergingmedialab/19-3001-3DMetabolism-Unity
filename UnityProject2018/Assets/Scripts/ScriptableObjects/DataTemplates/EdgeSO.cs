using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * ScriptableObject for Edges
 * - Equals and GetHashCode overrides are required for HashSet<EdgeSO>
 */

/// <summary>
/// ScriptableObject for Edges
/// </summary>
[CreateAssetMenu(fileName = "New Edge", menuName = "Edge")]
[System.Serializable]
public class EdgeSO : GenericSO
{
//    public string Label;
    public Vector3 Position;
//    public string QID;
//    public string Description;
    public string Enzyme;
    public string EnzymeClass;
    public string EnergyRequired;
    public string Pubchemlink;
    public string Regulation;

    public List<NodeSO> reactants;
    public List<NodeSO> products;

    public List<Cofactor> cofactors; // to avoid duplicates

    public bool bidirectional;

    
    public string AuxLabel;
    public Vector3 AuxPosition;
    public string AuxQID;
    public string AuxDescription;
    public string AuxEnzymeClass;
    public string AuxCofactors;
    public string AuxEnergyRequired;
    public string AuxPubchemlink;
    public string AuxRegulation;

    /// <summary>
    /// Initialize EdgeSO with essential fields
    /// </summary>
    public void init(string name, string newQID, string desc, string enzymeLabel, string enzymeclass, string energyreq, string pubchem, string regulation, bool bidirectionality = false){

        this.QID = newQID;
        this.name = enzymeLabel;
        this.Label = name;
        this.Description = desc;
        this.Enzyme = enzymeLabel;
        this.bidirectional = bidirectionality;
        this.reactants = new List<NodeSO>();
        this.products = new List<NodeSO>();

        this.cofactors = new List<Cofactor>();

        this.EnzymeClass = enzymeclass;
        this.EnergyRequired = energyreq;
        this.Pubchemlink = pubchem;
        this.Regulation = regulation;
    }

    /// <summary>
    /// Add reactant to the edge
    /// </summary>
    public void AddReactant(NodeSO node){
        foreach(NodeSO temp in this.reactants){
            if(temp.Label == node.Label){
                return;
            }
        }
        this.reactants.Add(node);
        
    }

    // add product to the edge
    public void AddProduct(NodeSO node){
        foreach(NodeSO temp in this.products){
            if(temp.Label == node.Label){
                return;
            }
        }
        this.products.Add(node);
    }


    // add cofactor if not in the list
    public void AddCofactor(Cofactor cofactor)
    {
        foreach (Cofactor temp in this.cofactors)
        {
            if (temp.label == cofactor.label && temp.isReactant == cofactor.isReactant)
            {
                return;
            }
        }

        cofactors.Add(cofactor);
        
        
    }

    public override bool Equals(object obj)
    {
        return obj is EdgeSO sO &&
               base.Equals(obj) &&
               QID == sO.QID;
    }

    public override int GetHashCode()
    {
        int hashCode = -1760945465;
        hashCode = hashCode * -1521134295 + base.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(QID);
        return hashCode;
    }
}
