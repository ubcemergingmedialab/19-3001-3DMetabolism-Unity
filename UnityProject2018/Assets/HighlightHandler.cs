using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightHandler : MonoBehaviour
{
    public Color accentColor = Color.yellow;
    public Color highlightColor = Color.blue;
    public Color defaultColor = new Color(0.4352941f, 0.4352941f, 0.4352941f, 0.1f);
    private int highlightCounter = 0;
    private MaterialPropertyBlock _propBlock;
    // Start is called before the first frame update
    private void Awake()
    {
        _propBlock = new MaterialPropertyBlock();
    }
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
        highlightCounter = 1;
        UpdateHighlight();
    }

    public void DeHighlightElement()
    {
        highlightCounter = 0;
        UpdateHighlight();
    }

    public void AccentElement()
    {
        highlightCounter = 2;
        UpdateHighlight();
    }


    private void UpdateHighlight()
    {
        Debug.Log(transform.parent.name + " " + highlightCounter);
        if (highlightCounter == 0)
        {
            transform.parent.GetComponent<Renderer>().material.SetColor("_Color", defaultColor);
            if (GetComponent<NodeDataDisplay>() != null)
            {
                GetComponent<NodeDataDisplay>().TransparentText();
            }
        }
        else if (highlightCounter == 1)
        {
            transform.parent.GetComponent<Renderer>().material.SetColor("_Color", highlightColor);
            if (GetComponent<NodeDataDisplay>() != null)
            {
                GetComponent<NodeDataDisplay>().OpaqueText();
            }
        }
        else if (highlightCounter > 1)
        {
            transform.parent.GetComponent<Renderer>().material.SetColor("_Color", accentColor);
            if (GetComponent<NodeDataDisplay>() != null)
            {
                //GetComponent<NodeDataDisplay>().OpaqueText();
            }
        }
    }
}
