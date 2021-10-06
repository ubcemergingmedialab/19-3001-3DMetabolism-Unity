using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    // Awake is called before start
    private void Awake()
    {
        parent = transform.parent;
        defaultScale = new Vector3(parent.localScale.x, parent.localScale.y, parent.localScale.z);
        accentScale = new Vector3(parent.localScale.x * 1.1f, parent.localScale.y * 1.1f, parent.localScale.z * 1.1f);
        _propBlock = new MaterialPropertyBlock();
    }
    // Start is called before the first frame update
    void Start()
    {
        UpdateHighlight();
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

    // Update is called once per frame
    void Update()
    {

    }

    public bool IsDoubleHighlighted() {
        return (HighlightService.Instance.CheckState(this) == HighlightPathway.HighlightState.Accented);
    }
    
    private void ActivateArrows()
    {
        if (activeArrows != null)
        {
            foreach (GameObject arrow in activeArrows) {
                arrow.SetActive(true);
            }
        }
    }

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


    public void UpdateHighlight()
    {
        Debug.Log(parent.name + " " + highlightCounter);
        HighlightPathway.HighlightState currentState = HighlightService.Instance.CheckState(this);   // finds the new highlight state and sets the visuals accordingly

        if (currentState == HighlightPathway.HighlightState.Default)                                    // if Default state
        {
            parent.GetComponent<Renderer>().material.SetColor("_WiggleColor", defaultColor);  // change the color
            if (GetComponent<NodeDataDisplay>() != null)
            {
                GetComponent<NodeDataDisplay>().TransparentText();                                      // change the text color
            }
            parent.localScale = defaultScale;
            DeactivateArrows();
        }
        else if (currentState == HighlightPathway.HighlightState.Highlighted)                           // if Highlight state
        {
            parent.GetComponent<Renderer>().material.SetColor("_WiggleColor", highlightColor);
            if (GetComponent<NodeDataDisplay>() != null)
            {
                GetComponent<NodeDataDisplay>().OpaqueText();
            }
            parent.localScale = defaultScale;
            DeactivateArrows();
        }
        else if (currentState == HighlightPathway.HighlightState.Accented)                              // if Accent state
        {
            parent.GetComponent<Renderer>().material.SetColor("_WiggleColor", accentColor);
            if (GetComponent<NodeDataDisplay>() != null)                                             // text color is already set when highlighted
            {
                //GetComponent<NodeDataDisplay>().OpaqueText();
            }
            parent.localScale = accentScale;
            ActivateArrows();
        }
        if(animatorComponent != null)
        {
            animatorComponent.WriteDefaultValues();
        }
    }
}
