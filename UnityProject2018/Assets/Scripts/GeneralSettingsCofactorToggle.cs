using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneralSettingsCofactorToggle : GeneralSettingsToggle
{
    public CofactorType CofactorType = CofactorType.none;

    public override void Clicked()
    {
        if (CofactorType == CofactorType.none)
            return;

        GeneralSettingsManager.Instance.ToggleCofactor(CofactorType, GetComponent<Toggle>().isOn);
        UpdateBackground(GetComponent<Toggle>().isOn);
    }

}
