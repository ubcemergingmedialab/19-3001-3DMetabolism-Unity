using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightController : MonoBehaviour
{
    public GameObject UIContainer;
    private Dictionary<PathwaySO, HighlightPathway> pathwayHighlights;
    // Start is called before the first frame update
    void Start()
    {
        pathwayHighlights = new Dictionary<PathwaySO, HighlightPathway>();
        if(UIContainer == null) {
            Debug.Log("HighlightController needs UIContainer to find HighlightPathway Components");
        } else {
            foreach(HighlightPathway pathwayHL in UIContainer.GetComponentsInChildren<HighlightPathway>()){ //adds all the highlighted to dictionary
                pathwayHighlights.Add(pathwayHL.pathwayToHighlight, pathwayHL);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // takes care of all the highlights management, including keeping track of all 3 states and coordinating changes
    public void Highlight(PathwaySO targetPathway) {
        Debug.Log("calling highlight on " + targetPathway.name);

        // change state of target pathway, in case of accented, unaccent all other accented pathways:
        if(UIContainer != null) {
            pathwayHighlights.TryGetValue(targetPathway, out HighlightPathway found);
            switch (found.state)
            {
                case HighlightPathway.HighlightState.Default:
                    found.SetHighlighted();
                    break;
                case HighlightPathway.HighlightState.Highlighted:
                    found.SetAccented();
                    foreach(KeyValuePair<PathwaySO, HighlightPathway> entry in pathwayHighlights) {
                        if(entry.Value.state == HighlightPathway.HighlightState.Accented && entry.Value.pathwayToHighlight.name != targetPathway.name) {
                            entry.Value.SetHighlighted(); //downgrade all other accented pathways
                        }
                    }
                    break;
                case HighlightPathway.HighlightState.Accented:
                    found.SetDefault();
                    break;
                default:
                    break;
            }
        }
    }
}

