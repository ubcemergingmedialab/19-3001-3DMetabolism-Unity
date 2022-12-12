using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Fills Pathway Sidecard UI with data from the currently selected Card
/// </summary>
public class PathwayUIElement : UIElement
{
    public Text LabelText;
    public Text DescriptionText;

    public Text QIDText;
     override public void UpdateUI()
    {

        // Update Edge UI Element
        Debug.Log("Updating Pathway UI");

       LabelText.text = ((Card)DataReference).Label;
       DescriptionText.text = ((Card)DataReference).Description;
       QIDText.text = ((Card)DataReference).QID;
  
    }

/// <summary>
/// Not implemented - Updates the UI with values from DataReference when the pathway has a partner
/// </summary>
/// <param name="hasPartner"> indicates if the pathway has a partner</param>
    override public void UpdateUI(bool hasPartner) {
        // not implemented, follow pattern if second metabolite card needed
        Debug.Log("Update UI called with boolean on PathwayUIElement, not implemented");
    }
}
