using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Manages the text labelling of nodes/metabolites as well as updates the nodeSO attached to sidecard UI to contain data from the currently selected node.
/// </summary>
[ExecuteAlways]
public class NodeDataDisplay : MonoBehaviour
{
    public NodeSO nodeData;
    public TextMeshPro labelText;
    public Card DisplayData;

    private List<string> blackListedList;

    void Start()
    {
        //Gets a list of strings that we don't want to show in labels
        blackListedList = Constants.GetBlackListedLabels();
    }

    private void OnEnable()
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
            labelText.SetText("<mark=#00000000><font=\"LiberationSans SDF\">" + nodeData.Label + "</font></mark>");
            //Debug.Log("<mark=#000000aa>" + nodeData.Label + "</mark>");
            //labelText.transform.localPosition = localPosition + (nodeData.Position / 10);
        }
    }

    /// <summary>
    /// Sets the node label to a specific string.
    /// TODO - is it necessary to call this on every update for every single node? (seems overkill and inefficient).
    /// </summary>
    private void MaintainLabelText()
    {
        bool blackListedCharsFound = false;
        if (nodeData != null)
        {
            foreach (string blackListedChar in blackListedList)
            {
                if (nodeData.Label.Contains(blackListedChar))
                {
                    blackListedCharsFound = true;
                    labelText.SetText("<mark=#00000000><font=\"LiberationSans SDF\">" + nodeData.Label.Replace(blackListedChar, "") + "</font></mark>");
                }
            }

            //if we found a blacklisted char, we don't need to render labelText again
            if (!blackListedCharsFound)
            {
                labelText.SetText("<mark=#00000000><font=\"LiberationSans SDF\">" + nodeData.Label + "</font></mark>");
            }
        }
    }

    public void TransparentText()
    {
        TextMeshPro textMesh = transform.Find("Label").GetComponent<TextMeshPro>();
        Color tempColor = textMesh.color;
        tempColor.a = 0.0f;
        textMesh.color = tempColor;
    }

    public void OpaqueText()
    {

        TextMeshPro textMesh = transform.Find("Label").GetComponent<TextMeshPro>();
        Color tempColor = textMesh.color;
        tempColor.a = 1f;
        textMesh.color = tempColor;
    }

    public void DisableText()
    {

    }

    public void UpdateScriptableObject()
    {
        DisplayData.Label = nodeData.Label;
        DisplayData.QID = nodeData.QID;
        DisplayData.Description = nodeData.Description;
        DisplayData.Charge = nodeData.Charge;
        DisplayData.MolecularFormula = nodeData.MolecularFormula;
        DisplayData.IUPACNames = nodeData.IUPACNames;
        DisplayData.Pubchemlink = nodeData.Pubchemlink;
        DisplayData.StructuralDescription = nodeData.StructuralDescription;
        if (UIPresenter.UIList.NodeUI != null)
            UIPresenter.Instance.NotifyUIUpdate(UIPresenter.UIList.NodeUI, false);
        else Debug.Log("Error in callin NodeUI list");
        DisplayData.link = nodeData.link;
        DisplayData.CID = nodeData.CID;
    }
}

