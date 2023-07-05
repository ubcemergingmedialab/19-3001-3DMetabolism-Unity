using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Keeps track of and updates the highlight state of pathway passed down the highlight pipeline and its components.
/// Need one instance per pathwaySO.
/// Not MonoBehaviour
/// </summary>
public class HighlightPathway
{
    /// <summary>
    /// 3 highlight states
    /// </summary>
    public enum HighlightState
    {
        Default,
        Highlighted,
        Accented
    }

    public PathwaySO pathwayToHighlight;
    public HighlightState state;

    /// <summary>
    /// Constructor. Set the fields of the class
    /// </summary>
    /// <param name="pathway"> pathway that needs HighlightPathway component</param>
    public HighlightPathway(PathwaySO pathway){
        pathwayToHighlight = pathway;
        state = HighlightState.Default;
    }

    /// <summary>
    /// set the highlight state of pathway to single highlight and update all of its components
    /// </summary>
    public void SetHighlighted()
    {
        state = HighlightState.Highlighted;
        UpdateAllComponents();
    }

    /// <summary>
    /// set the highlight state of pathway to no highlight (Default) and update all of its components
    /// </summary>
    public void SetDefault()
    {
        state = HighlightState.Default;
        UpdateAllComponents();
    }

    /// <summary>
    /// set the highlight state of pathway to double highlight (Accent) and update all of its components
    /// </summary>
    public void SetAccented()
    {
        state = HighlightState.Accented;
        UpdateAllComponents();

    }
           
    /// <summary>
    /// Update the highlight state of HighlightHandler for each node/edge of the pathway accessed through pathwaySO
    /// </summary>
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
