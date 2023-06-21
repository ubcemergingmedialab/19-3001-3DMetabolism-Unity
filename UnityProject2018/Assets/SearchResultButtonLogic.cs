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
        }

        ScriptableObjectSearch.Instance.DeleteSearchResult();
    }

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

        //Update strategy here.  We only want to show the relevent text labels from search.
    }

}
