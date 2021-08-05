using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteAlways]
public class NodeDataDisplay : MonoBehaviour
{
    public NodeSO nodeData;
    public TextMeshPro labelText;
    public Card DisplayData;

    void Start()
    {
    }

    private void OnEnable()
    {
        InitializeLabelText();
    }

    void Update()
    {
        MaintainLabelText();
    }

    private void InitializeLabelText()
    {
        if(nodeData != null)
        {
            Vector3 localPosition = labelText.transform.localPosition;
            labelText.SetText("<mark=#00000088><font=\"LiberationSans SDF\">" + nodeData.Label + "</font></mark>");
            Debug.Log("<mark=#000000aa>" + nodeData.Label + "</mark>");
            //labelText.transform.localPosition = localPosition + (nodeData.Position / 10);
        }
    }

    private void MaintainLabelText()
    {
        if(nodeData != null)
        {
            labelText.SetText("<mark=#00000055><font=\"LiberationSans SDF\">" + nodeData.Label + "</font></mark>");
        }
    }

    public void TransparentText()
    {
        TextMeshPro textMesh = transform.Find("Label").GetComponent<TextMeshPro>();
        Color tempColor = textMesh.color;
        tempColor.a = 0.1f;
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
        if (UIPresenter.UIList.NodeUI != null)
            UIPresenter.Instance.NotifyUIUpdate(UIPresenter.UIList.NodeUI);
        else Debug.Log("Error in callin NodeUI list");

    }
}

