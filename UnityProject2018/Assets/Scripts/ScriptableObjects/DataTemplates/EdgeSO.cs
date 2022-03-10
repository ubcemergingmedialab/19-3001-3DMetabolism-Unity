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

    //public NodeSOBeta parent;
    public List<NodeSO> reactants;
    public List<NodeSO> products;
    public bool bidirectional;

    
    public string AuxLabel;
    public Vector3 AuxPosition;
    public string AuxQID;
    public string AuxDescription;
    public string AuxEnergyConsumed;
    public string AuxEnergyProduced;
    public string AuxGibbsFreeEnergy;

    /* initialize the essential fields 
    */
    public void init(string name, string newQID, bool bidirectionality = false){

        this.QID = newQID;
        this.name = name;
        this.Label = name;
        this.bidirectional = bidirectionality;
        this.reactants = new List<NodeSO>();
        this.products = new List<NodeSO>();
    }

    // add reactant to the edge
    public void AddReactant(NodeSO node){
        reactants.Add(node);
    }

    // add product to the edge
    public void AddProduct(NodeSO node){
        products.Add(node);
    }

}
