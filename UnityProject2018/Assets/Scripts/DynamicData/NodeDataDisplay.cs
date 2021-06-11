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

    void InitializeLabelText()
    {
        Vector3 localPosition = labelText.transform.localPosition;
        labelText.text = nodeData.Label;
        labelText.transform.localPosition = localPosition + (nodeData.Position / 10);
    }

    void MaintainLabelText()
    {
        labelText.text = nodeData.Label;
    }

}

