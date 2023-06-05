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

    List<TextMeshPro> pathwayLabels;

    float startFadeDistance = 24f; // At which distance to the camera should a label start fading
    float maxFadeDistance = 15f; // At which distance to the camera should a label be completely gone

    private void Awake()
    {
        // Grab all child pathway labels
        pathwayLabels = new List<TextMeshPro>();
        pathwayLabels = GetComponentsInChildren<TextMeshPro>().ToList();


    }

    private void Update()
    {       
        // Calculate the alpha for every pathway label here
        for (int i = 0; i < pathwayLabels.Count; i++)
        {
            float alphaValue = 1;

            float distanceToCamera = Vector3.Distance(pathwayLabels[i].transform.position + new Vector3(0, 0, -5), Camera.main.transform.position);

            if (distanceToCamera < maxFadeDistance)
                alphaValue = 0f;
            else if (distanceToCamera > startFadeDistance)
                alphaValue = 1f;
            else
            {
                alphaValue = ((distanceToCamera - maxFadeDistance) / maxFadeDistance);
            }

            pathwayLabels[i].alpha = alphaValue;
        }


    }



}
