using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;


public class GeneralSettingsCofactorToggle : MonoBehaviour
{
    public CofactorType CofactorType = CofactorType.amp;

    public Color activeColor = new Color(83f / 255f, 109f / 255f, 254f / 255f, 1f);
    public Color inactiveColor = new Color(83f / 255f, 109f / 255f, 254f / 255f, 0f);
    public GameObject text;

    private void Start()
    {
        UpdateBackground(GetComponent<Toggle>().isOn);
        Clicked();
    }

    public void Clicked()
    {      
        GeneralSettingsManager.Instance.ToggleCofactor(CofactorType, GetComponent<Toggle>().isOn);
        UpdateBackground(GetComponent<Toggle>().isOn);
    }

    public void UpdateBackground(bool visibility)
    {
        if (visibility)
        {
            GetComponent<Image>().color = activeColor;
            text.GetComponent<TMP_Text>().color = Color.black;
        }
        else
        {
            GetComponent<Image>().color = inactiveColor;
            text.GetComponent<TMP_Text>().color = Color.white;
        }
            
    }

}
