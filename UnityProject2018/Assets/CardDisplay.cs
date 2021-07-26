using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour {

    public Card card;

    public Text LabelText;
    public Text DescriptionText;

    public Text QIDText; 

    // Start is called before the first frame update
    void Start() {
        LabelText.text = card.Label;
        DescriptionText.text = card.Description;
        QIDText.text = card.QID;
    }

}
