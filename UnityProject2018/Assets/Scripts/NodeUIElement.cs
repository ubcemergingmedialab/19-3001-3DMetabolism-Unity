using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Fills Node Sidecard UI with data from the currently selected Card
/// </summary>
public class NodeUIElement : UIElement
{
    public Text LabelText;
    public Text DescriptionText;

    public Text QIDText;
    public Text ChargeText;
    public TextMeshProUGUI MolecularFormulaText;
    public Text IUPACNamesText;
    public Text StructuralDescriptionText;
    public Text PubchemlinkText;
    public GameObject LinkButton;

/// <summary>
/// /// <summary>
    /// Updates UI of the Node Sidecard with the values from the Data Reference
    /// </summary>
/// </summary>
    override public void UpdateUI()
    {
        //Update Node UI
        Debug.Log("Updating Node UI");

        LabelText.text = ((Card)DataReference).Label;
        DescriptionText.text = ((Card)DataReference).Description;
        QIDText.text = ((Card)DataReference).QID;
        ChargeText.text = ((Card)DataReference).Charge;
        PubchemlinkText.text = ((Card)DataReference).Pubchemlink;
        MolecularFormulaText.text = ((Card)DataReference).MolecularFormula;
        IUPACNamesText.text = ((Card)DataReference).IUPACNames;
        StructuralDescriptionText.text = ((Card)DataReference).StructuralDescription;
        if (((Card)DataReference).link != null && ((Card)DataReference).link.Length > 0)
        {
            Debug.Log("updating link");
            LinkButton.GetComponent<OpenLink>().url = ((Card)DataReference).link;
        }
    }

/// <summary>
/// Not implemented - Updates the UI with values from DataReference when the node has a partner
/// </summary>
/// <param name="hasPartner"> indicates if the edge has a partner</param>
    override public void UpdateUI(bool hasPartner)
    {
        //Update Node UI
        Debug.Log("Tried to call NodeUI UpdateUI function with boolean, not implemented");
    }
}
