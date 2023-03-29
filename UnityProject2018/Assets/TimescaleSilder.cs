using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimescaleSilder : MonoBehaviour
{
    [SerializeField]
    private Text timeScaleText;

    public void UpdateTimeScale(float inputTimeScale)
    {
        //update the timescale
        Time.timeScale = inputTimeScale;

        //update UI
        if (inputTimeScale < 1)
        {
            //Paused
            timeScaleText.text = "Pause";
        }
        else
        {
            //1x, 2x, etc.
            timeScaleText.text = inputTimeScale + "x";
        }
    }
}
