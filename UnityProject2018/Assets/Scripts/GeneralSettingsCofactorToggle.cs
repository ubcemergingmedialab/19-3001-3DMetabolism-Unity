using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GeneralSettingsCofactorToggle : MonoBehaviour
{
    public CofactorType CofactorType = CofactorType.amp;

    public Color activeColor = new Color(83f / 255f, 109f / 255f, 254f / 255f, 1f);
    public Color inactiveColor = new Color(83f / 255f, 109f / 255f, 254f / 255f, 0f);

    public void Clicked()
    {      
        GeneralSettingsManager.Instance.ToggleCofactor(CofactorType, GetComponent<Toggle>().isOn);
        UpdateBackground(GetComponent<Toggle>().isOn);
    }

    public void UpdateBackground(bool visibility)
    {
        if (visibility)
            GetComponent<Image>().color = activeColor;
        else
            GetComponent<Image>().color = inactiveColor;
    }

}
