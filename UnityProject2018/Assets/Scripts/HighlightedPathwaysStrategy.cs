using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightedPathwaysStrategy : TextDisplayStrategy
{
    private HighlightedPathwaysStrategy()
    {
    }

    public static HighlightedPathwaysStrategy Instance { get { return Nested.instance; } }

    private class Nested
    {
        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Nested()
        {
        }

        internal static readonly HighlightedPathwaysStrategy instance = new HighlightedPathwaysStrategy();
    }

    override public void UpdateTextDisplay()
    {
        // iterate through all nodes, get their current state and decide what the text should do
        NodeDataDisplay[] nodes = Object.FindObjectsOfType<NodeDataDisplay>();
        StatusController status = StatusController.Instance;
        foreach(NodeDataDisplay node in nodes)
        {
            HighlightPathway.HighlightState nodeState = status.ElementCheckState(node.GetComponent<HighlightHandler>());
            if(nodeState >= HighlightPathway.HighlightState.Highlighted)
            {
                node.OpaqueText();
            } else
            {
                node.TransparentText();
            }
        }
    }
}
