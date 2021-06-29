using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NodeDataDisplay : MonoBehaviour
{
    public NodeSO nodeData;
    public TextMesh labelText;

    void Start()
    {
        InitializeLabelText();
    }

    void Update()
    {
        MaintainLabelText();
    }

    private void InitializeLabelText()
    {
        Vector3 localPosition = labelText.transform.localPosition;
        labelText.text = nodeData.Label;
        labelText.transform.localPosition = localPosition + (nodeData.Position / 10);
    }

    private void MaintainLabelText()
    {
        labelText.text = nodeData.Label;
    }

    /*
    public void UpdateScriptableObject(NodeSO newData)
    {
        nodeData.Label = newData.Label;
        nodeData.Position = newData.Position;
        nodeData.QID = newData.QID;
        nodeData.Rotation = newData.Rotation;
        nodeData.Description = newData.Description;
    }
    */
}

