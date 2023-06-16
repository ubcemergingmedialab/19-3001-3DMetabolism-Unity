using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum SettingsType { metabolite, reaction, pathway, cofactor }
public enum SettingsValue { none, off, all, highlight, accent };
public enum CofactorType { atp, adp, nadpos, nadh, h20, co2, fad, fadh, p1 };

public class GeneralSettingsManager : MonoBehaviour
{

    public SettingsValue metaboliteSetting = SettingsValue.all;
    public SettingsValue reactionSetting = SettingsValue.all;
    public SettingsValue metabolicPathwaySetting = SettingsValue.all;
    public SettingsValue cofactorSetting = SettingsValue.off;

    public List<CofactorType> enabledCofactorTypes;

    public float textsizePercentageSetting = 100f;
    public bool reactionArrows = true;
    public bool cameraLock = false;

    private static GeneralSettingsManager _instance;
    public static GeneralSettingsManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GeneralSettingsManager();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        enabledCofactorTypes = new List<CofactorType>();
    }

    public void ChangeSetting(SettingsType type, SettingsValue value)
    {
        if (value == SettingsValue.none)
            return;

        switch (type)
        {
            case SettingsType.metabolite:
                switch (value)
                {
                    case SettingsValue.none:
                        break;
                    case SettingsValue.off:
                        break;
                    case SettingsValue.all:
                        break;
                    case SettingsValue.highlight:
                        break;
                    case SettingsValue.accent:
                        break;
                    default:
                        break;
                }
                break;
            case SettingsType.reaction:
                switch (value)
                {
                    case SettingsValue.none:
                        break;
                    case SettingsValue.off:
                        break;
                    case SettingsValue.all:
                        break;
                    case SettingsValue.highlight:
                        break;
                    case SettingsValue.accent:
                        break;
                    default:
                        break;
                }
                break;
            case SettingsType.pathway:
                switch (value)
                {
                    case SettingsValue.none:
                        break;
                    case SettingsValue.off:
                        break;
                    case SettingsValue.all:
                        break;
                    case SettingsValue.highlight:
                        break;
                    case SettingsValue.accent:
                        break;
                    default:
                        break;
                }
                break;
            case SettingsType.cofactor:
                switch (value)
                {
                    case SettingsValue.none:
                        break;
                    case SettingsValue.off:
                        break;
                    case SettingsValue.all:
                        break;
                    case SettingsValue.highlight:
                        break;
                    case SettingsValue.accent:
                        break;
                    default:
                        break;
                }
                break;
        }
    }

    public void ToggleCofactor(CofactorType type, bool enabled)
    {
        if (enabled)
        {
            if (!enabledCofactorTypes.Contains(type))
                enabledCofactorTypes.Add(type);
        }
        else
        {
            if (enabledCofactorTypes.Contains(type))
                enabledCofactorTypes.Remove(type);
        }

    }

}
