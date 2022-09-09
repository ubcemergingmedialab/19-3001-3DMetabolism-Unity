using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EdgeUIElement : UIElement
{
    public Text LabelText;
    public Text DescriptionText;

    public Text QIDText;
    public Text EnzymeClassText;
    public Text EnzymeText;
    public Text EnergyRequiredText;
    public Text CofactorsText;
    public Text PubchemlinkText;
    public Text RegulationText;

    // public GameObject AuxUI;
    // public Text AuxLabelText;
    // public Text AuxDescriptionText;

    // public Text AuxQIDText;
    // public Text AuxEnzymeClassText;
    // public Text AuxCofactorsText;
    // public Text AuxPubchemlinkText;
    // public Text AuxRegulationText;


    override public void UpdateUI()
    {
        // Update Edge UI Element
        Debug.Log("Updating Edge UI");

        LabelText.text = ((Card)DataReference).Label;
        DescriptionText.text = ((Card)DataReference).Description;
        QIDText.text = ((Card)DataReference).QID;
        EnzymeClassText.text = ((Card)DataReference).EnzymeClass;
        EnzymeText.text = ((Card)DataReference).Enzyme;
        EnergyRequiredText.text = ((Card)DataReference).EnergyRequired;
        CofactorsText.text = ((Card)DataReference).Cofactors;
        PubchemlinkText.text = ((Card)DataReference).Pubchemlink;
        RegulationText.text = ((Card)DataReference).Regulation;
        // if(AuxUI != null) {
        //     AuxUI.SetActive(false);
        // }
  
    }

    override public void UpdateUI(bool hasPartner)
    {
        // Update Edge UI Element
        Debug.Log("Updating Edge UI");

        LabelText.text = ((Card)DataReference).Label;
        DescriptionText.text = ((Card)DataReference).Description;
        QIDText.text = ((Card)DataReference).QID;
        EnzymeClassText.text = ((Card)DataReference).EnzymeClass;
        CofactorsText.text = ((Card)DataReference).Cofactors;
        EnergyRequiredText.text = ((Card)DataReference).EnergyRequired;
        PubchemlinkText.text = ((Card)DataReference).Pubchemlink;
        RegulationText.text = ((Card)DataReference).Regulation;

        // if(AuxUI != null) {
        //     AuxUI.SetActive(true);
        //     AuxLabelText.text = ((Card)DataReference).AuxLabel;
        //     AuxDescriptionText.text = ((Card)DataReference).AuxDescription;
        //     AuxQIDText.text = ((Card)DataReference).AuxQID;
        //     AuxEnzymeClassText.text = ((Card)DataReference).AuxEnzymeClass;
        //     AuxCofactorsText.text = ((Card)DataReference).AuxCofactors;
        //     AuxPubchemlinkText.text = ((Card)DataReference).AuxPubchemlink;
        //     AuxRegulationText.text = ((Card)DataReference).AuxRegulation;
        // }
    }
}
