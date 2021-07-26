using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeUIElement : UIElement
{
    public Text LabelText;
    public Text DescriptionText;

    public Text QIDText;

    override public void UpdateUI()
    {
        //Update Node UI
        Debug.Log("Updating Node UI");

        LabelText.text = ((Card)DataReference).Label;
        DescriptionText.text = ((Card)DataReference).Description;
        QIDText.text = ((Card)DataReference).QID;
    }
}
