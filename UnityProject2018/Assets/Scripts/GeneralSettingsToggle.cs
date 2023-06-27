using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneralSettingsToggle : MonoBehaviour
{
    public SettingsCategory category = SettingsCategory.metabolite;
    public SettingsType type = SettingsType.all;

    public Color activeColor = new Color(83f / 255f, 109f / 255f, 254f / 255f, 1f);
    public Color inactiveColor = new Color(83f / 255f, 109f / 255f, 254f / 255f, 0f);


    public virtual void Clicked()
    {
        bool toggleState = GetComponent<Toggle>().isOn;
        if (!GeneralSettingsManager.Instance.overridingToggles)
        {            
            GeneralSettingsManager.Instance.ChangeSetting(category, type, toggleState);
        }

        UpdateBackground(toggleState);
    }



    public void UpdateBackground(bool visibility)
    {
        if (visibility)
            GetComponent<Image>().color = activeColor;
        else
            GetComponent<Image>().color = inactiveColor;
    }
}
