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

    //public NodeSOBeta parent;
    public List<NodeSOBeta> reactants;
    public List<NodeSOBeta> products;
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
    public void init(string name, bool bidirectionality = false){

        this.name = name;
        this.Label = name;
        this.bidirectional = bidirectionality;
        this.reactants = new List<NodeSOBeta>();
        this.products = new List<NodeSOBeta>();
    }

    // add reactant to the edge
    public void AddReactant(NodeSOBeta node){
        reactants.Add(node);
    }

    // add product to the edge
    public void AddProduct(NodeSOBeta node){
        products.Add(node);
    }

}
