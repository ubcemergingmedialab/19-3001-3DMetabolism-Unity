using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockQuery : MonoBehaviour
{
    public PathwaySOBeta Pathway;
    public List<EdgeSOBeta> edges;
    public List<NodeSOBeta> node;
    // Start is called before the first frame update
    void Start()
    {
        EdgeSOBeta hexokinase = EdgeSOBeta.CreateInstance<EdgeSOBeta>();
        hexokinase.Label = "hexokinase";
        NodeSOBeta glucose = NodeSOBeta.CreateInstance<NodeSOBeta>();
        glucose.Label = "glucose";
        NodeSOBeta glucose6phosphate = NodeSoBeta.CreateInstance<NodeSOBeta>();
        Pathway.LocalNetwork.Add(glucose, new List<EdgeSOBeta>(hexokinase));

        // EdgeSOBeta;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
