using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EdgeUIElement : UIElement
{
    public Text LabelText;
    public Text DescriptionText;

    public Text QIDText;
    public Text EnergyConsumedText;
    public Text EnergyProducedText;
    public Text GibbsFreeEnergyText;

    override public void UpdateUI()
    {
        // Update Edge UI Element
        Debug.Log("Updating Edge UI");

        LabelText.text = ((Card)DataReference).Label;
        DescriptionText.text = ((Card)DataReference).Description;
        QIDText.text = ((Card)DataReference).QID;
        EnergyConsumedText.text = ((Card)DataReference).EnergyConsumed;
        EnergyProducedText.text = ((Card)DataReference).EnergyProduced;
        GibbsFreeEnergyText.text = ((Card)DataReference).GibbsFreeEnergy;
  
    }

    // needs to be removed but some other components depend on it. We dont want partner cards anymore
    override public void UpdateUI(bool hasPartner)
    {
        // Update Edge UI Element
        Debug.Log("Updating Edge UI");

        LabelText.text = ((Card)DataReference).Label;
        DescriptionText.text = ((Card)DataReference).Description;
        QIDText.text = ((Card)DataReference).QID;
        EnergyConsumedText.text = ((Card)DataReference).EnergyConsumed;
        EnergyProducedText.text = ((Card)DataReference).EnergyProduced;
        GibbsFreeEnergyText.text = ((Card)DataReference).GibbsFreeEnergy;
    }
}
