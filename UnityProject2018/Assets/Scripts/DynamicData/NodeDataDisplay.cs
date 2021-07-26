using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class NodeDataDisplay : MonoBehaviour
{
    public NodeSO nodeData;
    public TextMesh labelText;
    public Card DisplayData;

    void Start()
    {
        InitializeLabelText();
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
            labelText.text = nodeData.Label;
            labelText.transform.localPosition = localPosition + (nodeData.Position / 10);
        }
    }

    private void MaintainLabelText()
    {
        if(nodeData != null)
        {
            labelText.text = nodeData.Label;
        }
    }

    public void TransparentText()
    {
        TextMesh textMesh = transform.Find("Label").GetComponent<TextMesh>();
        Color tempColor = textMesh.color;
        tempColor.a = 0.2f;
        textMesh.color = tempColor;
    }

    public void OpaqueText()
    {

        TextMesh textMesh = transform.Find("Label").GetComponent<TextMesh>();
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
        UIPresenter.Instance.NotifyUIUpdate(UIPresenter.UIList.NodeUI);

    }
}

