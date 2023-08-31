using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowTextOnHover : MonoBehaviour
{

    public TextMeshPro text;
    public bool isShowingText = false;
    private Color originalColor;


    bool useAsPseudoButton = false;

    // Start is called before the first frame update
    void Start()
    {
        if (text == null)
        {
            if (GetComponent<EdgeDataDisplay>())
            {

            }
            else if (transform.Find("Label"))
            {
                if (transform.Find("Label").GetComponent<TextMeshPro>())
                    text = transform.Find("Label").GetComponent<TextMeshPro>();
            }
            // Edge case for when a model doesn't have a convex collider that works.
            // This shouldn't happen, as the convex colliders should be included in the model.
            // They're not included yet, so this is a workaround.
            // This makes sure a child object with a collider can still be used to interact with the edge.
            else if (GetComponentInParent<EdgeDataDisplay>())
            {
                text = GetComponentInParent<EdgeDataDisplay>().edgeLabelObject.GetComponent<TextMeshPro>();
                useAsPseudoButton = true;
            }

        }
    }


    private void OnMouseEnter()
    {
        //Debug.Log("Entered");
        if (MouseOrbit.Instance.IsPointerOverNamedUIElements())
            return;

        // Workaround for edges
        if (useAsPseudoButton)
        {
            transform.parent.GetComponent<ShowTextOnHover>().WorkaroundMouseEnter();
        }
        else
        {
            // update original color, change color to show text
            if (text != null)
            {
                originalColor = text.color;
                isShowingText = true;
                text.color = new Color(1, 1, 1, 1);
            }
            this.transform.GetComponentInParent<Outline>().enabled = true;
        }
    }

    private void OnMouseExit()
    {
        if (useAsPseudoButton)
        {
            transform.parent.GetComponent<ShowTextOnHover>().WorkaroundMouseExit();
        }
        else
        {
            // change color back to original
            if (text != null)
            {
                isShowingText = false;
                text.color = originalColor;
            }
            this.transform.GetComponentInParent<Outline>().enabled = false;
        }
    }


    #region workaround functionality
    private void OnMouseUpAsButton()
    {
        if (MouseOrbit.Instance.IsPointerOverNamedUIElements())
            return;

        if (useAsPseudoButton)
        {
            if (GetComponentInParent<EdgeDataDisplay>())
            {
                GetComponentInParent<EdgeDataDisplay>().UpdateScriptableObject();
            }
        }
    }

    public void WorkaroundMouseEnter()
    {
        OnMouseEnter();
    }

    public void WorkaroundMouseExit()
    {
        OnMouseExit();
    }
    #endregion

}
