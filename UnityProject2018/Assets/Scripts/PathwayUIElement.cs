using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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

    override public void UpdateUI(bool hasPartner) {
        // not implemented, follow pattern if second metabolite card needed
        Debug.Log("Update UI called with boolean on PathwayUIElement, not implemented");
    }
}
