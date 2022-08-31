using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour {

    public Card card;

    public Text LabelText;
    public Text DescriptionText;

    public Text QIDText;
    public Text EnzymeClassText;
    public Text CofactorsText;
    public Text EnergyRequiredText;
    public Text PubchemlinkText;
    public Text RegulationText;

    public Text ChargeText;
    public Text MolecularFormulaText;
    public Text IUPACNamesText;
    public Text StructuralDescriptionText;

    // Start is called before the first frame update
    void Start() {
        LabelText.text = card.Label;
        DescriptionText.text = card.Description;
        QIDText.text = card.QID;
        ChargeText.text = card.Charge;
        EnzymeClassText.text = card.EnzymeClass;
        CofactorsText.text = card.Cofactors;
        EnergyRequiredText.text = card.EnergyRequired;
        PubchemlinkText.text = card.Pubchemlink;
        RegulationText.text = card.Regulation;
        MolecularFormulaText.text = card.MolecularFormula;
        IUPACNamesText.text = card.IUPACNames;
        StructuralDescriptionText.text = card.StructuralDescription;
    }


}
