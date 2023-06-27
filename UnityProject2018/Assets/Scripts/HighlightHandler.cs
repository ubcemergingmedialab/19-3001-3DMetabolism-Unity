using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implement the visual consequences of the highlight state on pathway components, namely material color, directional arrows, text display strategy (labels).
/// Last step of the highlight pipeline
/// </summary>
public class HighlightHandler : MonoBehaviour
{
    public Color accentColor = Color.yellow;
    public Color highlightColor = Color.blue;
    public Color defaultColor = new Color(0.4352941f, 0.4352941f, 0.4352941f, 0.1f);
    public bool bidirectional = false;
    public GameObject upArrow;
    public GameObject downArrow;
    private List<GameObject> activeArrows;
    private int highlightCounter = 0;
    private MaterialPropertyBlock _propBlock;
    private Animator animatorComponent;

    private Vector3 defaultScale;
    private Vector3 accentScale;

    private Transform parent;
    
    /// <summary>
    /// Assings parent tranform, and initializes highlight scales, and material
    /// </summary>
    private void Awake()
    {
        parent = transform.parent;
        defaultScale = new Vector3(parent.localScale.x, parent.localScale.y, parent.localScale.z);
        accentScale = new Vector3(parent.localScale.x * 1.1f, parent.localScale.y * 1.1f, parent.localScale.z * 1.1f);
        _propBlock = new MaterialPropertyBlock();
    }
    
    /// <summary>
    /// Assigns component's animator and directional arrows for accented state based on the reactions directionality
    /// </summary>
    void Start()
    {
        // UpdateHighlight();
        if (upArrow != null)
        {
            activeArrows = new List<GameObject>();
            activeArrows.Add(upArrow);
            if (bidirectional)
            {
                activeArrows.Add(downArrow);
            }
        }
        animatorComponent = parent.GetComponent<Animator>();
    }

    /// <summary>
    /// True if pathway is doubled higlhighlighted
    /// </summary>
    /// <returns></returns>
    public bool IsDoubleHighlighted() {
        return (StatusController.Instance.ElementCheckState(this) == HighlightPathway.HighlightState.Accented);
    }
    
    /// <summary>
    /// Activates the assigned directional arrows 
    /// </summary>
    private void ActivateArrows()
    {
        if (activeArrows != null)
        {
            foreach (GameObject arrow in activeArrows) {
                arrow.SetActive(true);
            }
        }
    }
    /// <summary>
    /// Deactivates the assigned directional arrows 
    /// </summary>
    private void DeactivateArrows()
    {
        if (activeArrows!= null)
        {
            foreach (GameObject arrow in activeArrows)
            {
                arrow.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Assing material color to components and text display strategy based on their highlight state.
    /// if pathway is accented, activate the arrows, else deactivate
    /// </summary>
    public void UpdateHighlight()
    {
        HighlightPathway.HighlightState currentState = StatusController.Instance.ElementCheckState(this);   // finds the new highlight state and sets the visuals accordingly

        if (currentState == HighlightPathway.HighlightState.Default)                                        // if Default state
        {
            parent.GetComponent<Renderer>().material.SetColor("_WiggleColor", defaultColor);                // change the color
            //if (GetComponent<NodeDataDisplay>() != null)
            //{
            //    NodeTextDisplay.Instance.UpdateTextDisplay();                                       // change the text color
            //}
            //parent.localScale = defaultScale;
            DeactivateArrows();
        }
        else if (currentState == HighlightPathway.HighlightState.Highlighted)                               // if Highlight state
        {
            parent.GetComponent<Renderer>().material.SetColor("_WiggleColor", highlightColor);
            //if (GetComponent<NodeDataDisplay>() != null)
            //{
            //    NodeTextDisplay.Instance.UpdateTextDisplay();
            //}
            //parent.localScale = defaultScale;
            DeactivateArrows();
        }
        else if (currentState == HighlightPathway.HighlightState.Accented)                                  // if Accent state
        {
            parent.GetComponent<Renderer>().material.SetColor("_WiggleColor", accentColor);
            //if (GetComponent<NodeDataDisplay>() != null)                                                    // text color is already set when highlighted
            //{
            //    NodeTextDisplay.Instance.UpdateTextDisplay();
            //}
            //parent.localScale = accentScale;
            ActivateArrows();
        }
        if(animatorComponent != null)
        {
            animatorComponent.WriteDefaultValues();
        }


        GeneralSettingsManager.Instance.UpdateLabelFilter();
    }
}
