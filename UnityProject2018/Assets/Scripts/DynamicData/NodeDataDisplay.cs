using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static NodeTextDisplay;

/// <summary>
/// Manages the text labelling of nodes/metabolites as well as updates the nodeSO attached to sidecard UI to contain data from the currently selected node.
/// </summary>
//[ExecuteAlways]
public class NodeDataDisplay : MonoBehaviour
{
    public NodeSO nodeData;
    public TextMeshPro labelText;
    public Card DisplayData;

    private List<string> blackListedList;

    private bool isHidden = false;

    void Awake()
    {
        //Gets a list of strings that we don't want to show in labels
        blackListedList = Constants.GetBlackListedLabels();

        // Forcing Label text to align to middle instead of top
        labelText.verticalAlignment = VerticalAlignmentOptions.Middle;
    }

    private void Start()
    {
        InitializeLabelText();
    }

    void Update()
    {
        MaintainLabelText();
    }


    /// <summary>
    /// 
    /// </summary>
    private void InitializeLabelText()
    {
        if (nodeData != null)
        {
            Vector3 localPosition = labelText.transform.localPosition;
            //labelText.SetText("<mark=#00000000><font=\"LiberationSans SDF\">" + nodeData.Label + "</font></mark>");
            //Debug.Log("<mark=#000000aa>" + nodeData.Label + "</mark>");
            //labelText.transform.localPosition = localPosition + (nodeData.Position / 10);

            bool blackListedCharsFound = false;

            foreach (string blackListedChar in Constants.GetBlackListedLabels())
            {
                if (nodeData.Label.Contains(blackListedChar))
                {
                    blackListedCharsFound = true;
                    searchCategory searchCategory = searchCategory.standard;
                    Enum.TryParse(blackListedChar.Replace("(", "").Replace(")", ""), out searchCategory);
                    nodeData.searchCategory = searchCategory;
                    nodeData.Label = nodeData.Label.Replace(blackListedChar, "");
                    labelText.SetText("<mark=#00000000><font=\"LiberationSans SDF\">" + nodeData.Label.Replace(blackListedChar, "") + "</font></mark>");
                    
                }
            }

            //if we found a blacklisted char, we don't need to render labelText again
            if (!blackListedCharsFound)
            {
                labelText.SetText("<mark=#00000000><font=\"LiberationSans SDF\">" + nodeData.Label + "</font></mark>");
            }

            labelText.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            labelText.enableWordWrapping = false;

        }
    }

    /// <summary>
    /// Sets the node label to a specific string.
    /// TODO - is it necessary to call this on every update for every single node? (seems overkill and inefficient).
    /// </summary>
    private void MaintainLabelText()
    {
        switch (NodeTextDisplay.Instance.activeStrategyEnum)
        {
            case TextDisplayStrategyEnum.HighlightedPathwaysStrategy:
                HighlightPathway.HighlightState highlightState = StatusController.Instance.ElementCheckState(GetComponent<HighlightHandler>());
                if (highlightState != HighlightPathway.HighlightState.Highlighted)
                    return;
                break;
            case TextDisplayStrategyEnum.AccentedPathwaysStrategy:
                HighlightPathway.HighlightState accentedState = StatusController.Instance.ElementCheckState(GetComponent<HighlightHandler>());
                if (accentedState != HighlightPathway.HighlightState.Accented)
                    return;
                break;
            case TextDisplayStrategyEnum.AllTextStrategy:
                break;
            case TextDisplayStrategyEnum.NoTextStrategy:
                return;
            default:
                break;
        }

        // Check if text is currently showing from hovering mouse over object
        if (GetComponent<ShowTextOnHover>())
        {
            if (GetComponent<ShowTextOnHover>().isShowingText)
            {
                // If text is showing and this target is not the main focus right now, return
                if (MouseOrbit.Instance.targetInFocus != gameObject)
                {
                    labelText.alpha = 1;
                    labelText.fontSize = 36.0f * MouseOrbit.Instance.cameraLabelController.MetabolitesFontSizeMultiplier;
                    return;
                }
            }
        }
        if (nodeData != null)
        {
            if (!isHidden)
            {
                // Calculate multiplier based on object distance to main camera
                float distanceToCameraMultiplier = MouseOrbit.Instance.cameraLabelController.GetAlphaValue(transform.position);

                // Perform fontsize and transparency calculations
                if (MouseOrbit.Instance.targetInFocus == gameObject)
                {
                    distanceToCameraMultiplier = 1;
                    labelText.alpha = distanceToCameraMultiplier;
                    labelText.fontSize = 36.0f * distanceToCameraMultiplier * MouseOrbit.Instance.cameraLabelController.MetabolitesFontSizeMultiplier;
                }
                else
                {
                    labelText.alpha = Mathf.Clamp(distanceToCameraMultiplier, 0.2f, 0.8f);
                    labelText.fontSize = 30.0f * distanceToCameraMultiplier * MouseOrbit.Instance.cameraLabelController.MetabolitesFontSizeMultiplier;
                }
            }


        }
    }

    public void TransparentText()
    {
        TextMeshPro textMesh = transform.Find("Label").GetComponent<TextMeshPro>();
        Color tempColor = textMesh.color;
        tempColor.a = 0.0f;
        textMesh.color = tempColor;
        isHidden = true;
    }

    public void OpaqueText()
    {

        //TextMeshPro textMesh = transform.Find("Label").GetComponent<TextMeshPro>();
        //Color tempColor = textMesh.color;
        //tempColor.a = 1f;
        //textMesh.color = tempColor;
        isHidden = false;
    }

    public void DisableText()
    {

    }

    public void UpdateScriptableObject()
    {

        MouseOrbit.Instance.targetInFocus = gameObject;
        DisplayData.Label = nodeData.Label;
        DisplayData.QID = nodeData.QID;
        DisplayData.Description = nodeData.Description;
        DisplayData.Charge = nodeData.Charge;
        DisplayData.MolecularFormula = nodeData.MolecularFormula;
        DisplayData.IUPACNames = nodeData.IUPACNames;
        DisplayData.Pubchemlink = nodeData.Pubchemlink;
        DisplayData.StructuralDescription = nodeData.StructuralDescription;
        DisplayData.link = nodeData.link;
        DisplayData.CID = nodeData.CID;
        if (UIPresenter.UIList.NodeUI != null)
            UIPresenter.Instance.NotifyUIUpdate(UIPresenter.UIList.NodeUI, false);
        else Debug.Log("Error in callin NodeUI list");


    }
}

