using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathwayController : MonoBehaviour
{
    public List<PathwaySO> allPathways;
    public Dictionary<PathwaySO, HighlightPathway> pathwayHighlights;
    // Start is called before the first frame update
    public Dictionary<HighlightHandler, List<HighlightPathway>> statusList;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
