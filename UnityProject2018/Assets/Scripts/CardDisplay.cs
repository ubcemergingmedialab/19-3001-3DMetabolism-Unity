using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour {

    public Card card;

    public Text LabelText;
    public Text DescriptionText;

    public Text QIDText;
    // public Text ChargeText;
    public Text EnergyConsumedText;
    public Text EnergyProducedText;
    public Text GibbsFreeEnergyText;
    public Text MolecularFormulaText;
    public Text IUPACNamesText;
    public Text StructuralDescriptionText;

    // Start is called before the first frame update
    void Start() {
        LabelText.text = card.Label;
        DescriptionText.text = card.Description;
        QIDText.text = card.QID;
        // ChargeText.text = card.Charge;
        EnergyConsumedText.text = card.EnergyConsumed;
        EnergyProducedText.text = card.EnergyProduced;
        GibbsFreeEnergyText.text = card.GibbsFreeEnergy;
        MolecularFormulaText.text = card.MolecularFormula;
        IUPACNamesText.text = card.IUPACNames;
        StructuralDescriptionText.text = card.StructuralDescription;
    }


}
