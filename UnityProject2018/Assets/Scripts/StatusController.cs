using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Centralized class to keep track of highlight statuses in pathways/nodes/edges. checkState, calculating max/min highlight, sending change state class
public class StatusController : MonoBehaviour
{
    private Dictionary<HighlightHandler, List<HighlightPathway>> statusList;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
