using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighlightPathway : MonoBehaviour
{
    public PathwaySO pathwayToHighlight;
    public Color highlightColor = Color.blue;
    public Color defaultColor = new Color(1f, 1f, 1f);
    public Color defaultTextColor = new Color(0.2196079f, 0.2196079f, 0.2196079f);
    public Color highlightTextColor = new Color(1f, 1f, 1f);
    private bool isPathwayHighlighted = false;
    // Start is called before the first frame update
    void Start()
    {
        if(pathwayToHighlight == null)
        {
            Debug.LogError("Pathway to highlight is not set on GameObject " + gameObject.name);
        }
        GetComponent<Image>().color = defaultColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Highlight()
    {
        if(pathwayToHighlight == null)
        {
            return;
        }
        foreach(NodeSO nodeSO in pathwayToHighlight.nodes)
        {
            foreach(GameObject node in GameObject.FindGameObjectsWithTag(nodeSO.name)) {
                if (isPathwayHighlighted)
                {
                    node.GetComponent<HighlightHandler>().DeHighlightElement();
                } else
                {

                    node.GetComponent<HighlightHandler>().HighlightElement();
                }
            }
        }
        foreach (EdgeSO edgeSO in pathwayToHighlight.edges)
        {
            foreach (GameObject edge in GameObject.FindGameObjectsWithTag(edgeSO.name))
            {
                if(isPathwayHighlighted)
                {
                    edge.GetComponent<HighlightHandler>().DeHighlightElement();
                }
                else
                {
                    edge.GetComponent<HighlightHandler>().HighlightElement();
                }
            }
        }
        if(isPathwayHighlighted)
        {
            isPathwayHighlighted = false;
            GetComponent<Image>().color = defaultColor;
            GetComponentInChildren<Text>().color = defaultTextColor;
        } else
        {
            isPathwayHighlighted = true;
            GetComponent<Image>().color = highlightColor;
            GetComponentInChildren<Text>().color = highlightTextColor;
        }
    }
}
