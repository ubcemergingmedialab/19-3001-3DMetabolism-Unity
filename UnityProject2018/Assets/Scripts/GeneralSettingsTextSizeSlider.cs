using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneralSettingsTextSizeSlider : MonoBehaviour
{

    public void UpdateTextSize()
    {
        MouseOrbit.Instance.cameraLabelController.FontSizeMultiplier = GetComponent<Slider>().value / 100f;
    }
}
