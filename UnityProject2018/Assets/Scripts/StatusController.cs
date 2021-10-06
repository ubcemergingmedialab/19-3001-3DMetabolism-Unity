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



    public HighlightPathway.HighlightState CheckState(HighlightHandler highlightHandler) {
        
        HighlightPathway.HighlightState tempState = HighlightPathway.HighlightState.Default;
        statusList.TryGetValue(highlightHandler, out List<HighlightPathway> currentList);

        if ( currentList != null) {
            foreach ( HighlightPathway hlpw in currentList) {
                HighlightPathway.HighlightState newState = hlpw.state;
                    if ( newState > tempState) {
                        tempState = newState;
                    }
            }
        } else {
            Debug.Log("no pathwaylist are to be found on the stateList Dictionary (NULL access)");
        }
        return tempState;
    }
}
