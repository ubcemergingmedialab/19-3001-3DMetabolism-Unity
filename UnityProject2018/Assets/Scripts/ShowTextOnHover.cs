using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowTextOnHover : MonoBehaviour
{

    public TextMeshPro text;
    private Color originalColor;
    // Start is called before the first frame update
    void Start()
    {
        if (text == null)
        {
            text = transform.Find("Label").GetComponent<TextMeshPro>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        // update original color, change color to show text
        if(text != null)
        {
            originalColor = text.color;
            text.color = new Color(1, 1, 1, 1);
        }
    }

    private void OnMouseExit()
    {
        // change color back to original
        if(text != null)
        {
            text.color = originalColor;
        }
    }

}
