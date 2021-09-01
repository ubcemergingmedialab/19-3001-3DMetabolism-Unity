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

    public void Highlight()
    {
        if (pathwayToHighlight == null)
        {
            return;
        }
        switch (state)
        {
            case HighlightState.Default:
                SetHighlighted();
                break;
            case HighlightState.Highlighted:
                SetAccented();
                break;
            case HighlightState.Accented:
                SetDefault();
                break;
            default:
                break;
        }
    }

    public void SetHighlighted()
    {
        foreach (NodeSO nodeSO in pathwayToHighlight.nodes)
        {
            foreach (GameObject node in GameObject.FindGameObjectsWithTag(nodeSO.name))
            {
                node.GetComponent<HighlightHandler>().HighlightElement();
            }
        }
        foreach (EdgeSO edgeSO in pathwayToHighlight.edges)
        {
            foreach (GameObject edge in GameObject.FindGameObjectsWithTag(edgeSO.name))
            {
                edge.GetComponent<HighlightHandler>().HighlightElement();
            }
        }
        GetComponent<Image>().color = highlightColor;
        GetComponentInChildren<Text>().color = highlightTextColor;
        state = HighlightState.Highlighted;
    }

    public void SetDefault()
    {
        foreach (NodeSO nodeSO in pathwayToHighlight.nodes)
        {
            foreach (GameObject node in GameObject.FindGameObjectsWithTag(nodeSO.name))
            {
                node.GetComponent<HighlightHandler>().DeHighlightElement();
            }

        }
        foreach (EdgeSO edgeSO in pathwayToHighlight.edges)
        {
            foreach (GameObject edge in GameObject.FindGameObjectsWithTag(edgeSO.name))
            {
                edge.GetComponent<HighlightHandler>().DeHighlightElement();
            }
        }
        GetComponent<Image>().color = defaultColor;
        GetComponentInChildren<Text>().color = defaultTextColor;
        state = HighlightState.Default;
    }

    public void SetAccented()
    {
        foreach (HighlightPathway pathway in transform.parent.GetComponentsInChildren<HighlightPathway>()) // need to make all other highlight buttons and their corresponding pathways un-accented
        {
            if (pathway.state == HighlightState.Accented)
            {
                pathway.SetHighlighted();
            }
        }
        foreach (NodeSO nodeSO in pathwayToHighlight.nodes)
        {
            foreach (GameObject node in GameObject.FindGameObjectsWithTag(nodeSO.name))
            {
                node.GetComponent<HighlightHandler>().AccentElement();
            }

        }
        foreach (EdgeSO edgeSO in pathwayToHighlight.edges)
        {
            foreach (GameObject edge in GameObject.FindGameObjectsWithTag(edgeSO.name))
            {
                edge.GetComponent<HighlightHandler>().AccentElement();
            }
        }
        GetComponent<Image>().color = accentColor;
        GetComponentInChildren<Text>().color = accentTextColor;
        state = HighlightState.Accented;
    }
}
