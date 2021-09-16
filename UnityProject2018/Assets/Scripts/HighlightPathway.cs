using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighlightPathway : MonoBehaviour
{
    public PathwaySO pathwayToHighlight;
    public Color defaultColor = new Color(1f, 1f, 1f);
    public Color defaultTextColor = new Color(0.2196079f, 0.2196079f, 0.2196079f);
    public Color highlightColor = Color.blue;
    public Color highlightTextColor = new Color(1f, 1f, 1f);
    public Color accentColor = Color.yellow;
    public Color accentTextColor = new Color(0.2196079f, 0.2196079f, 0.2196079f);

    public enum HighlightState
    {
        Default,
        Highlighted,
        Accented
    }

    public HighlightState state;

    // Start is called before the first frame update
    void Start()
    {
        if (pathwayToHighlight == null)
        {
            Debug.LogError("Pathway to highlight is not set on GameObject " + gameObject.name);
        }
        GetComponent<Image>().color = defaultColor;
        state = HighlightState.Default;
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void SetHighlighted()
    {
        GetComponent<Image>().color = highlightColor;
        GetComponentInChildren<Text>().color = highlightTextColor;
        state = HighlightState.Highlighted;
        UpdateAllComponents();
        // foreach (NodeSO nodeSO in pathwayToHighlight.nodes)
        // {
        //     foreach (GameObject node in GameObject.FindGameObjectsWithTag(nodeSO.name))
        //     {
        //         node.GetComponent<HighlightHandler>().UpdateHighlight();
        //     }
        // }
        // foreach (EdgeSO edgeSO in pathwayToHighlight.edges)
        // {
        //     foreach (GameObject edge in GameObject.FindGameObjectsWithTag(edgeSO.name))
        //     {
        //         edge.GetComponent<HighlightHandler>().UpdateHighlight();
        //     }
        // }
    }

    public void SetDefault()
    {
        GetComponent<Image>().color = defaultColor;
        GetComponentInChildren<Text>().color = defaultTextColor;
        state = HighlightState.Default;
        UpdateAllComponents();
        // foreach (NodeSO nodeSO in pathwayToHighlight.nodes)
        // {
        //     foreach (GameObject node in GameObject.FindGameObjectsWithTag(nodeSO.name))
        //     {
        //         node.GetComponent<HighlightHandler>().UpdateHighlight();
        //     }

        // }
        // foreach (EdgeSO edgeSO in pathwayToHighlight.edges)
        // {
        //     foreach (GameObject edge in GameObject.FindGameObjectsWithTag(edgeSO.name))
        //     {
        //         edge.GetComponent<HighlightHandler>().UpdateHighlight();
        //     }
        // }
    }

    public void SetAccented()
    {
        GetComponent<Image>().color = accentColor;
        GetComponentInChildren<Text>().color = accentTextColor;
        state = HighlightState.Accented;
        UpdateAllComponents();
        // foreach (NodeSO nodeSO in pathwayToHighlight.nodes)
        // {
        //     foreach (GameObject node in GameObject.FindGameObjectsWithTag(nodeSO.name))
        //     {
        //         node.GetComponent<HighlightHandler>().UpdateHighlight();
        //     }

        // }
        // foreach (EdgeSO edgeSO in pathwayToHighlight.edges)
        // {
        //     foreach (GameObject edge in GameObject.FindGameObjectsWithTag(edgeSO.name))
        //     {
        //         edge.GetComponent<HighlightHandler>().UpdateHighlight();
        //     }
        // }
    }
    private void UpdateAllComponents() 
    {
        foreach (NodeSO nodeSO in pathwayToHighlight.nodes)
        {
            foreach (GameObject node in GameObject.FindGameObjectsWithTag(nodeSO.name))
            {
                node.GetComponent<HighlightHandler>().UpdateHighlight();
            }

        }
        foreach (EdgeSO edgeSO in pathwayToHighlight.edges)
        {
            foreach (GameObject edge in GameObject.FindGameObjectsWithTag(edgeSO.name))
            {
                edge.GetComponent<HighlightHandler>().UpdateHighlight();
            }
        }
        
    }
}
