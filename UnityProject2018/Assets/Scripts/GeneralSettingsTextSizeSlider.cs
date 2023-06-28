using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneralSettingsTextSizeSlider : MonoBehaviour
{
    
    public void UpdateMetabolitesTextSize()
    {
        MouseOrbit.Instance.cameraLabelController.MetabolitesFontSizeMultiplier = GetComponent<Slider>().value / 100f;
    }
    public void UpdateReactionsTextSize()
    {
        MouseOrbit.Instance.cameraLabelController.ReactionsFontSizeMultiplier = GetComponent<Slider>().value / 100f;
    }
}
