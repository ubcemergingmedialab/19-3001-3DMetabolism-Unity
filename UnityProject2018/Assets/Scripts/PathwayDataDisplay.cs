using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PathwayDataDisplay : MonoBehaviour
{
    public TextMeshPro label;
    public PathwaySO pathwaySO;

    public bool isHidden = false;

    private void Awake()
    {
        label = GetComponentInChildren<TextMeshPro>();
    }

    public void TransparentText()
    {
        Color tempColor = label.color;
        tempColor.a = 0.0f;
        label.color = tempColor;
        isHidden = true;
    }

    public void OpaqueText()
    {
    //    Color tempColor = label.color;
    //    tempColor.a = 1f;
    //    label.color = tempColor;
        isHidden = false;
    }


}
