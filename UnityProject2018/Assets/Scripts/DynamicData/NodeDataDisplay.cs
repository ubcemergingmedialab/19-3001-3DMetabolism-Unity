using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NodeDataDisplay : MonoBehaviour
{
    public NodeSO nodeData;
    public TextMesh labelText;

    void Start()
    {
        labelText.text = nodeData.Label;
        Debug.Log(this.gameObject.name);
    }
}
