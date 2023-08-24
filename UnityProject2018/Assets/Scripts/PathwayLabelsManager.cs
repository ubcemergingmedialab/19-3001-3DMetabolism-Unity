using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

/// <summary>
/// Controls the behavior of all pathway labels.
/// 
/// </summary>

public class PathwayLabelsManager : MonoBehaviour
{

    private static PathwayLabelsManager _instance;
    public static PathwayLabelsManager Instance { get { return _instance; } }

    List<PathwayDataDisplay> pathways;

    public List<PathwayDataDisplay> Pathways
    {
        get { return pathways; }
    }

    float startFadeDistance = 300f; // At which distance to the camera should a label start fading
    float maxFadeDistance = 200f; // At which distance to the camera should a label be completely gone

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        // Grab all child pathway labels
        pathways = new List<PathwayDataDisplay>();
        pathways = GetComponentsInChildren<PathwayDataDisplay>().ToList();


    }

    private void Update()
    {       
        // Calculate the alpha for every pathway label here
        for (int i = 0; i < pathways.Count; i++)
        {
            if (pathways[i].isHidden)
                continue;
            float alphaValue = 1;

            //float distanceToCamera = Vector3.Distance(pathways[i].transform.position + new Vector3(0, 0, -5), Camera.main.transform.position);
            float distanceToCamera = Vector3.Distance(pathways[i].transform.position, Camera.main.transform.position);

            if (distanceToCamera < maxFadeDistance)
                alphaValue = 0f;
            else if (distanceToCamera > startFadeDistance)
                alphaValue = 1f;
            else
            {
                alphaValue = (distanceToCamera - maxFadeDistance) / 100f;
                //alphaValue = ((distanceToCamera - maxFadeDistance) / maxFadeDistance);
            }

            pathways[i].label.alpha = alphaValue;
        }


    }



}
