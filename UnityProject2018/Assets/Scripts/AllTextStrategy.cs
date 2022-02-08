using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllTextStrategy : TextDisplayStrategy
{

    private AllTextStrategy()
    {
    }

    public static AllTextStrategy Instance { get { return Nested.instance; } }

    private class Nested
    {
        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Nested()
        {
        }

        internal static readonly AllTextStrategy instance = new AllTextStrategy();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void UpdateTextDisplay()
    {
        //Debug.Log("finding nodes");
        NodeDataDisplay[] nodes = Object.FindObjectsOfType<NodeDataDisplay>();
        StatusController status = StatusController.Instance;
        foreach (NodeDataDisplay node in nodes)
        {
            //Debug.Log("Updateing text" + node.name);
            node.OpaqueText();
        }
    }
}
