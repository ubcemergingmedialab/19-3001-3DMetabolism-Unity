using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card: ScriptableObject {
    
    public string Label;
    public string QID;
    public string Description;
    public string Charge;
    public string EnergyConsumed;
    public string EnergyProduced;
    public string GibbsFreeEnergy;
    public string MolecularFormula;
    public string IUPACNames;
    public string link;


    public string AuxLabel;
    public string AuxQID;
    public string AuxDescription;
    public string AuxCharge;
    public string AuxEnergyConsumed;
    public string AuxEnergyProduced;
    public string AuxGibbsFreeEnergy;
    public string AuxMolecularFormula;
    public string AuxIUPACNames;
    public string Auxlink;

}

