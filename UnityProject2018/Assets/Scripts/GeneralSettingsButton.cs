using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralSettingsButton : MonoBehaviour
{
    public SettingsType settingType = SettingsType.metabolite;
    public SettingsValue settingValue = SettingsValue.none;

    public void Clicked()
    {
        GeneralSettingsManager.Instance.ChangeSetting(settingType, settingValue);
    }
}
