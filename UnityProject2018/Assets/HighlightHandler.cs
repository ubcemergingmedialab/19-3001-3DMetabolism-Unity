using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightHandler : MonoBehaviour
{
    public Color accentColor = Color.yellow;
    public Color highlightColor = Color.blue;
    public Color defaultColor = new Color(0.4352941f, 0.4352941f, 0.4352941f, 0.2f);
    private int highlightCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        UpdateHighlight();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HighlightElement()
    {
        highlightCounter += 1;
        UpdateHighlight();
    }

    public void DeHighlightElement()
    {
        highlightCounter -= 1;
        if (highlightCounter < 0) highlightCounter = 0;
        UpdateHighlight();
    }


    private void UpdateHighlight()
    {
        Debug.Log(transform.parent.name + " " + highlightCounter);
        if(highlightCounter == 0)
        {
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            transform.parent.GetComponent<Renderer>().GetPropertyBlock(block);
            block.SetColor("_Color", defaultColor);
            transform.parent.GetComponent<Renderer>().SetPropertyBlock(block, 0);
            transform.parent.GetComponent<Renderer>().material.SetColor("_Color", defaultColor);
            if(GetComponent<NodeDataDisplay>() != null)
            {
                GetComponent<NodeDataDisplay>().TransparentText();
            }
        }
        else if (highlightCounter == 1)
        {
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            transform.parent.GetComponent<Renderer>().GetPropertyBlock(block);
            block.SetColor("_Color", highlightColor);
            transform.parent.GetComponent<Renderer>().SetPropertyBlock(block, 0);
            if (GetComponent<NodeDataDisplay>() != null)
            {
                GetComponent<NodeDataDisplay>().OpaqueText();
            }
        }
        else if (highlightCounter > 1)
        {
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            transform.parent.GetComponent<Renderer>().GetPropertyBlock(block);
            block.SetColor("_Color", accentColor);
            transform.parent.GetComponent<Renderer>().SetPropertyBlock(block, 0);
            if (GetComponent<NodeDataDisplay>() != null)
            {
                //GetComponent<NodeDataDisplay>().OpaqueText();
            }
        }
    }
}
