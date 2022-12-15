using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// General class containing string data to be displayed on sidecards
/// </summary>
[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card: ScriptableObject {
    
    public string Label;
    public string QID;
    public string Description;
    public string Charge;
    public string EnzymeClass;
    public string Enzyme;
    public string Cofactors;
    public string EnergyRequired;
    public string Pubchemlink;
    public string Regulation;
    public string MolecularFormula;
    public string IUPACNames;
    public string StructuralDescription;
    public string link;
    public string CID;


    public string AuxLabel;
    public string AuxQID;
    public string AuxDescription;
    public string AuxCharge;
    public string AuxEnzymeClass;
    public string AuxEnzyme;
    public string AuxCofactors;
    public string AuxEnergyRequired;
    public string AuxPubchemlink;
    public string AuxRegulation;
    public string AuxMolecularFormula;
    public string AuxIUPACNames;
    public string Auxlink;

}

