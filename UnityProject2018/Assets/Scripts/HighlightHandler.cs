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
    // Start is called before the first frame update
    private void Awake()
    {
        _propBlock = new MaterialPropertyBlock();
    }
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
        animatorComponent = transform.parent.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HighlightElement()
    {
        highlightCounter = 1;
        UpdateHighlight();
    }

    public void DeHighlightElement()
    {
        highlightCounter = 0;
        UpdateHighlight();
    }

    public bool IsDoubleHighlighted() {
        return highlightCounter >= 2;
    }
    
    public void AccentElement()
    {
        highlightCounter = 2;
        UpdateHighlight();
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


    private void UpdateHighlight()
    {
        Debug.Log(transform.parent.name + " " + highlightCounter);
        if (highlightCounter == 0)
        {
            transform.parent.GetComponent<Renderer>().material.SetColor("_WiggleColor", defaultColor);
            if (GetComponent<NodeDataDisplay>() != null)
            {
                GetComponent<NodeDataDisplay>().TransparentText();
            }
            DeactivateArrows();
        }
        else if (highlightCounter == 1)
        {
            transform.parent.GetComponent<Renderer>().material.SetColor("_WiggleColor", highlightColor);
            if (GetComponent<NodeDataDisplay>() != null)
            {
                GetComponent<NodeDataDisplay>().OpaqueText();
            }
            DeactivateArrows();
        }
        else if (highlightCounter > 1)
        {
            transform.parent.GetComponent<Renderer>().material.SetColor("_WiggleColor", accentColor);
            if (GetComponent<NodeDataDisplay>() != null)
            {
                //GetComponent<NodeDataDisplay>().OpaqueText();
            }
            ActivateArrows();
        }
        if(animatorComponent != null)
        {
            animatorComponent.WriteDefaultValues();
        }
    }
}
