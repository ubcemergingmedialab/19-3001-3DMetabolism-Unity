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
            GameObject node = GameObject.Find(nodeSO.originalName);
            if (node != null)
            {
                node.GetComponentInChildren<HighlightHandler>().UpdateHighlight();
            }
            else
            {
                Debug.LogError("cant find game object for node: " + nodeSO.name + " in pathway: " + pathwayToHighlight.Label);
            }
        }
        foreach (EdgeSO edgeSO in pathwayToHighlight.edges)
        {
            if (edgeSO.name == "carnitine-acyl-carnitine cotransporter (membrane)")
            {

            }
            GameObject edge = GameObject.Find(edgeSO.name);
            if (edge != null)
            {
                edge.GetComponentInChildren<HighlightHandler>().UpdateHighlight();
            }
            else
            {
                //Debug.LogError("cant find game object for edge: " + edgeSO.name + " in pathway: " + pathwayToHighlight.Label);

                // Check if edge is made of multiple GameObjects

                int checkForEdgeGameObjectsCount = 9;

                List<GameObject> edgeParts = new List<GameObject>();

                for (int i = 0; i < checkForEdgeGameObjectsCount; i++)
                {
                    GameObject edgePart = GameObject.Find(edgeSO.name + ".00" + i.ToString());
                    if (edgePart != null)
                        edgeParts.Add(edgePart);
                }

                for (int i = 0; i < edgeParts.Count; i++)
                {
                    edgeParts[i].GetComponentInChildren<HighlightHandler>().UpdateHighlight();
                }

            }
            
            
           
        }

        
    }

    
}
