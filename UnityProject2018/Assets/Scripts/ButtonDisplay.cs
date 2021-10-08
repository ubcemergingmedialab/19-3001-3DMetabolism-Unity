using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDisplay : MonoBehaviour
{
// takes care of the 5 states that buttons will have, including hover states. previously done in Highlight Pathway , now commented out in that class
// <> hover color states need to be added
    public Color defaultColor = new Color(1f, 1f, 1f);
    public Color defaultTextColor = new Color(0.2196079f, 0.2196079f, 0.2196079f);
    public Color highlightColor = Color.blue;
    public Color highlightTextColor = new Color(1f, 1f, 1f);
    public Color accentColor = Color.yellow;
    public Color accentTextColor = new Color(0.2196079f, 0.2196079f, 0.2196079f);

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().color = defaultColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

// checks the state of the pathway and set s the button color and text color ( gets staet by passing PWSO to status contorller)
    private void SetDisplayColor() {
        // <> TO BE DEVELOPED
    }
}