using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
NOTE:
This class will have to be modified! it will not extend MonoBehaviour.
it will be a class with contructor and attached to a pathwaySO. 
this way the functionality we can access it directly without accessing the UIContainer.
*/

public class HighlightPathway
{
    public enum HighlightState
    {
        Default,
        Highlighted,
        Accented
    }

    private PathwaySO pathwayToHighlight;
    public HighlightState state;

    // Start is called before the first frame update

    public HighlightPathway(PathwaySO pathway){
        pathwayToHighlight = pathway;
        state = HighlightState.Default;
    }
    // <> void Start()
    // {
    //     if (pathwayToHighlight == null)
    //     {
    //         Debug.LogError("Pathway to highlight is not set on GameObject " + gameObject.name);
    //     }
    //     //GetComponent<Image>().color = defaultColor;
    //     state = HighlightState.Default;
    // }

    // Update is called once per frame
    // void Update()
    // {

    // }
    
    public void SetHighlighted()
    {
        // GetComponent<Image>().color = highlightColor;                       // <> these will ahve to go to a new script callled button dispaly that will be added as a component to  the button in unity
        // GetComponentInChildren<Text>().color = highlightTextColor;          //
        state = HighlightState.Highlighted;
        UpdateAllComponents();
    }

    public void SetDefault()
    {
        // GetComponent<Image>().color = defaultColor;
        // GetComponentInChildren<Text>().color = defaultTextColor;
        state = HighlightState.Default;
        UpdateAllComponents();
    }

    public void SetAccented()
    {
        // GetComponent<Image>().color = accentColor;
        // GetComponentInChildren<Text>().color = accentTextColor;
        state = HighlightState.Accented;
        UpdateAllComponents();

    }
           
    // Updates the highlight status of each node and edge in a pathway
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
