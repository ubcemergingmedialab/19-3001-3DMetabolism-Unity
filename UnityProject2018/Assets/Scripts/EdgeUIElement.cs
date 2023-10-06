using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Fills Edge Sidecard UI with data from the currently selected Card
/// </summary>
public class EdgeUIElement : UIElement
{
    public TextMeshProUGUI LabelText;
    public Text DescriptionText;
    public Text QIDText;
    public Text EnzymeClassText;
    public Text EnzymeText;
    public Text EnergyRequiredText;
    public Text CofactorsText;
    public Text PubchemlinkText;
    public Text RegulationText;


    /// <summary>
    /// Updates UI of the Edge Sidecard with the values from the Data Reference
    /// </summary>
    override public void UpdateUI()
    {
        // Update Edge UI Element
        Debug.Log("Updating Edge UI");
        CompoundControllerCamera.Instance.ResetCameraPosition();

        LabelText.text = ((Card)DataReference).Label.Replace("⁺", "<sup>+</sup>").Replace("₂", "<sub>2</sub>").Replace("inorganic phosphate", "P<sub>i</sub>").Replace("₄", "<sub>4</sub>").Replace("⁻", "<sup>-</sup>").Replace("₂", ""); ;
        DescriptionText.text = ((Card)DataReference).Description;
        QIDText.text = ((Card)DataReference).QID;
        EnzymeClassText.text = ((Card)DataReference).EnzymeClass;
        EnzymeText.text = ((Card)DataReference).Enzyme;
        EnergyRequiredText.text = ((Card)DataReference).EnergyRequired;
        CofactorsText.text = ((Card)DataReference).Cofactors;
        PubchemlinkText.text = ((Card)DataReference).Pubchemlink;
        RegulationText.text = ((Card)DataReference).Regulation;
  
    }

    /// <summary>
    /// Deprecated - Updates the UI with values from DataReference when the edge has a partner
    /// </summary>
    /// <param name="hasPartner"> indicates if the edge has a partner</param>
    override public void UpdateUI(bool hasPartner)
    {
        // Update Edge UI Element
        Debug.Log("Updating Edge UI");
        CompoundControllerCamera.Instance.ResetCameraPosition();

        LabelText.text = ((Card)DataReference).Label;
        DescriptionText.text = ((Card)DataReference).Description;
        QIDText.text = ((Card)DataReference).QID;
        EnzymeClassText.text = ((Card)DataReference).EnzymeClass;
        CofactorsText.text = ((Card)DataReference).Cofactors;
        EnergyRequiredText.text = ((Card)DataReference).EnergyRequired;
        PubchemlinkText.text = ((Card)DataReference).Pubchemlink;
        RegulationText.text = ((Card)DataReference).Regulation;

    }
}
