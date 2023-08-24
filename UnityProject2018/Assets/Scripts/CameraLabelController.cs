using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraLabelController : MonoBehaviour
{

    private List<GameObject> _allLabels;

    private float maxDistance = 400f;
    //private float minDistance = 200f;

    private float _metabolitesFontSizeMultiplier = 1f;
    private float _reactionsFontSizeMultiplier = 1f;

    public float MetabolitesFontSizeMultiplier
    {
        get
        {
            {
                return _metabolitesFontSizeMultiplier;
            }
        }
        set
        {
            _metabolitesFontSizeMultiplier = value;
        }
    }

    public float ReactionsFontSizeMultiplier
    {
        get
        {
            {
                return _reactionsFontSizeMultiplier;
            }
        }
        set
        {
            _reactionsFontSizeMultiplier = value;
        }
    }


    private void Awake()
    {
        _allLabels = new List<GameObject>();

        GameObject defaultPathwayObject = GameObject.Find("DefaultPathway");
        TextMeshPro[] labels = defaultPathwayObject.GetComponentsInChildren<TextMeshPro>();

        for (int i = 0; i < labels.Length; i++)
        {
            if (labels[i].gameObject.transform.parent.transform.parent.name != "PathwayLabels")
                _allLabels.Add(labels[i].gameObject);
        }

    }


    public float GetAlphaValue(Vector3 position)
    {
        float distance = Vector3.Distance(position, Camera.main.transform.position);

        if (distance >= maxDistance)
            return 0.2f;


        return 1 - (distance / maxDistance);

    }


}
