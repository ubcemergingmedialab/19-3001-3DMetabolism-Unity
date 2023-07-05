using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Logic for the general search buttons 
/// </summary>
public class SearchResultButtonLogic : MonoBehaviour
{
    // Start is called before the first frame update
    public GenericSO elementSO;
    public Card pathwayCardSO;

    /// <summary>
    /// When the search button is clicked , open the side card and shift camera focus on the element.
    /// if Node or Edge, use DataDisplay to trigger sidecard and camera shift
    /// if pathway, (TBA)
    /// </summary>
    public void ClickAndFocus()
    {
        PrefabService prefabService = GameObject.Find("PrefabService").GetComponent<PrefabService>();
        if (elementSO.GetType() == typeof(EdgeSO))
        {
            
            EdgeDataDisplay[] listOfEdgeTemplate = Object.FindObjectsOfType<EdgeDataDisplay>();
            foreach (EdgeDataDisplay edgeDD in listOfEdgeTemplate)
            {
                if (edgeDD.edgeData != null)
                {
                    if (edgeDD.edgeData.QID == elementSO.QID)
                    {
                        edgeDD.UpdateScriptableObject(); // set the side card

                        prefabService.DisableAllOutline(); // turn off all outlines
                        edgeDD.GetComponentInParent<Outline>().enabled = true; // enable new outline

                        if (!Camera.main.GetComponent<CameraController>().GetAutoLock())
                        {
                            Camera.main.GetComponent<CameraController>().MoveCameraToParentElement(edgeDD.transform);  // move the camera on to the edge
                        }
                    }
                }
                
            }
            
        }
        else if (elementSO.GetType() == typeof(NodeSO))
        {
            NodeDataDisplay[] listOfNodeTemplate = Object.FindObjectsOfType<NodeDataDisplay>();
            foreach (NodeDataDisplay nodeDD in listOfNodeTemplate)
            {
                if (nodeDD.nodeData != null)
                {
                    if (nodeDD.nodeData.QID == elementSO.QID)
                    {
                        nodeDD.UpdateScriptableObject(); // set the side card
                        prefabService.DisableAllOutline(); // turn off all outlines
                        nodeDD.GetComponentInParent<Outline>().enabled = true;

                        if (!Camera.main.GetComponent<CameraController>().GetAutoLock())
                        {
                            Camera.main.GetComponent<CameraController>().MoveCameraToParentElement(nodeDD.transform);  // move the camera on to the edge
                        }
                    }
                }
                    
            }
        }
        else if (elementSO.GetType() == typeof(PathwaySO))
        {
            this.UpdatePathwayUI();
            prefabService.DisableAllOutline(); // turn off all outlines
            OutlineAllPathwayComponent((PathwaySO)elementSO);
            Camera.main.GetComponent<CameraController>().MoveCameraToPathway((PathwaySO)elementSO);
        }

        ScriptableObjectSearch.Instance.DeleteSearchResult();
    }

    // in order to trigger the pathway side card if the user looks up pathway
    public void UpdatePathwayUI()
    {
        //Stops all animations.
        AnimationControllerComponent.Instance.StopAllAnimations();

        pathwayCardSO.Label = elementSO.Label;
        pathwayCardSO.QID = elementSO.QID;
        pathwayCardSO.Description = elementSO.Description;
        if (UIPresenter.UIList.PathwayUI != null)
            UIPresenter.Instance.NotifyUIUpdate(UIPresenter.UIList.PathwayUI, false);
        else Debug.Log("Error in calling PathwayUI list");

    }

    public void OutlineAllPathwayComponent(PathwaySO pathwaySO)
    {
        foreach (NodeSO nodeSO in pathwaySO.nodes)
        {
            foreach (GameObject node in GameObject.FindGameObjectsWithTag(nodeSO.name))
            {
                node.GetComponentInParent<Outline>().enabled = true;
            }

        }
        foreach (EdgeSO edgeSO in pathwaySO.edges)
        {
            foreach (GameObject edge in GameObject.FindGameObjectsWithTag(edgeSO.name))
            {
                edge.GetComponentInParent<Outline>().enabled = true;
            }
        }
    }

}
