using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneralSettingsToggle : MonoBehaviour
{
    public SettingsType settingType = SettingsType.metabolite;
    public SettingsValue settingValue = SettingsValue.none;    

    public Color activeColor = new Color(83f / 255f, 109f / 255f, 254f / 255f, 1f);
    public Color inactiveColor = new Color(83f / 255f, 109f / 255f, 254f / 255f, 0f);


    public virtual void Clicked()
    {
        if (!GeneralSettingsManager.Instance.overridingToggles)
            GeneralSettingsManager.Instance.ChangeSetting(settingType, settingValue);

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
