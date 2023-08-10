using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static NodeTextDisplay;

public enum SettingsCategory { metabolite, reaction, pathway, cofactor, cofactorPart }
public enum SettingsType { all, highlight, accent }
public enum SettingsValue { on, off }
public enum CofactorType { atp, adp, nadpos, nadh, h2o, co2, fad, fadh, pi, amp, gdp, gtp, nadppos, nadph, fadh2, hpos, o2 }

public class GeneralSettingsManager : MonoBehaviour
{
    List<NodeDataDisplay> nodes;// = FindObjectsOfType<NodeDataDisplay>();
    List<EdgeDataDisplay> edges;// = FindObjectsOfType<EdgeDataDisplay>();
    List<PathwayDataDisplay> pathways;

    [HideInInspector]    public SettingsType metaboliteSettingsType = SettingsType.all;
    [HideInInspector]    public bool metaboliteSettingsState = true;
    [HideInInspector]    public SettingsType reactionSettingsType = SettingsType.all;
    [HideInInspector]    public bool reactionSettingsState = true;
    [HideInInspector]    public SettingsType pathwaySettingsType = SettingsType.all;
    [HideInInspector]    public bool pathwaySettingsState = true;
    [HideInInspector]    public SettingsType cofactorSettingsType = SettingsType.all;
    [HideInInspector]    public bool cofactorSettingsState = false;

    [HideInInspector]    public List<CofactorType> enabledCofactorTypes;

    [HideInInspector]    public float textsizePercentageSetting = 100f;
    [HideInInspector]    public bool reactionArrows = true;
    [HideInInspector]    public bool cameraLock = false;
    [HideInInspector]    public bool overridingToggles = false;

    List<GeneralSettingsToggle> generalSettingsToggles;


    //Debugging
    List<string> unparsableCofactorLabels = new List<string>();


    private void Start()
    {
        Debug.Log("Let's see");
    }

    private static GeneralSettingsManager _instance;
    public static GeneralSettingsManager Instance
    {
        get
        {            
            return _instance;
        }
    }

    public void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);
        enabledCofactorTypes = new List<CofactorType>();
        //filters = new List<FilterSettings>();

        //FilterSettings metaboliteSettings = new FilterSettings(SettingsCategory.metabolite, true, false, false);
        //FilterSettings reactionSettings = new FilterSettings(SettingsCategory.reaction, true, false, false);
        //FilterSettings pathwaySettings = new FilterSettings(SettingsCategory.pathway, true, false, false);
        //FilterSettings cofactorSettings = new FilterSettings(SettingsCategory.cofactor, false, false, false);

        //filters.Add(metaboliteSettings);
        //filters.Add(reactionSettings);
        //filters.Add(pathwaySettings);
        //filters.Add(cofactorSettings);

    }


    public void ChangeSetting(SettingsCategory category, SettingsType type, bool isOn)
    {
        switch (category)
        {
            case SettingsCategory.metabolite:
                metaboliteSettingsType = type;
                metaboliteSettingsState = isOn;
                break;
            case SettingsCategory.reaction:
                reactionSettingsType = type;
                reactionSettingsState = isOn;
                break;
            case SettingsCategory.pathway:
                pathwaySettingsType = type;
                pathwaySettingsState = isOn;
                break;
            case SettingsCategory.cofactor:
                cofactorSettingsType = type;
                cofactorSettingsState = isOn;
                break;
            default:
                break;
        }

        UpdateLabelFilter();

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

        CofactorLabelsManager.Instance.ToggleCofactorFilter(enabledCofactorTypes);

    }

    public void UpdateCofactorFilter()
    {
        CofactorLabelsManager.Instance.ToggleCofactorFilter(enabledCofactorTypes);
    }

    public void UpdateLabelFilter()
    {
        if (generalSettingsToggles == null)
        {
            generalSettingsToggles = new List<GeneralSettingsToggle>();
            generalSettingsToggles.AddRange(transform.GetComponentsInChildren<GeneralSettingsToggle>());

            generalSettingsToggles.RemoveAll(toggle => toggle.GetComponent<GeneralSettingsCofactorToggle>());

            nodes = FindObjectsOfType<NodeDataDisplay>().ToList();
            edges = FindObjectsOfType<EdgeDataDisplay>().ToList();
            pathways = PathwayLabelsManager.Instance.Pathways;

        }



        overridingToggles = true;

        for (int i = 0; i < generalSettingsToggles.Count; i++)
        {
            switch (generalSettingsToggles[i].category)
            {
                case SettingsCategory.metabolite:
                    if (generalSettingsToggles[i].type == metaboliteSettingsType)
                        generalSettingsToggles[i].GetComponent<Toggle>().isOn = metaboliteSettingsState;
                    else
                        generalSettingsToggles[i].GetComponent<Toggle>().isOn = false;
                    break;
                case SettingsCategory.reaction:
                    if (generalSettingsToggles[i].type == reactionSettingsType)
                        generalSettingsToggles[i].GetComponent<Toggle>().isOn = reactionSettingsState;
                    else
                        generalSettingsToggles[i].GetComponent<Toggle>().isOn = false;
                    break;
                case SettingsCategory.pathway:
                    if (generalSettingsToggles[i].type == pathwaySettingsType)
                        generalSettingsToggles[i].GetComponent<Toggle>().isOn = pathwaySettingsState;
                    else
                        generalSettingsToggles[i].GetComponent<Toggle>().isOn = false;
                    break;
                case SettingsCategory.cofactor:
                    if (generalSettingsToggles[i].type == cofactorSettingsType)
                        generalSettingsToggles[i].GetComponent<Toggle>().isOn = cofactorSettingsState;
                    else
                        generalSettingsToggles[i].GetComponent<Toggle>().isOn = false;
                    break;
                default:
                    break;
            }
        }

        // Do actual label handling here
        ToggleNodeLabels();
        ToggleEdgeLabels();
        TogglePathwayLabels();

        overridingToggles = false;



    }


    void DisableSettingsToggle(GeneralSettingsToggle toggle)
    {
        toggle.GetComponent<Toggle>().isOn = false;
        toggle.UpdateBackground(false);

    }


    void ToggleNodeLabels()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            if (metaboliteSettingsType == SettingsType.all && !metaboliteSettingsState)
                nodes[i].TransparentText();
            else
            {
                HighlightPathway.HighlightState nodeState = StatusController.Instance.ElementCheckState(nodes[i].GetComponent<HighlightHandler>());
                if (metaboliteSettingsState)
                {
                    if ((int)nodeState >= (int)metaboliteSettingsType)
                    {
                        nodes[i].OpaqueText();
                    }
                    else
                    {
                        nodes[i].TransparentText();
                    }
                }
                else
                {
                    nodes[i].TransparentText();
                }
            }
        }


    }

    void ToggleEdgeLabels()
    {
        for (int i = 0; i < edges.Count; i++)
        {
            if (reactionSettingsType == SettingsType.all && !reactionSettingsState)
                edges[i].TransparentText();
            else
            {
                HighlightPathway.HighlightState edgeState = StatusController.Instance.ElementCheckState(edges[i].GetComponent<HighlightHandler>());
                if (reactionSettingsState)
                {
                    if ((int)edgeState >= (int)reactionSettingsType)
                    {
                        edges[i].OpaqueText();
                    }
                    else
                    {
                        edges[i].TransparentText();
                    }
                }
                else
                {
                    edges[i].TransparentText();
                }
            }
        }
    }

    void TogglePathwayLabels()
    {
        for (int i = 0; i < pathways.Count; i++)
        {
            if (pathwaySettingsType == SettingsType.all && !pathwaySettingsState)
                pathways[i].TransparentText();
            else
            {
                HighlightPathway.HighlightState pathwayState = StatusController.Instance.PathwayCheckState(pathways[i].pathwaySO);
                if (reactionSettingsState)
                {
                    if ((int)pathwayState >= (int)pathwaySettingsType)
                    {
                        pathways[i].OpaqueText();
                    }
                    else
                    {
                        pathways[i].TransparentText();
                    }
                }
                else
                {
                    pathways[i].TransparentText();
                }
            }
        }
    }

    public Color GetCofactorColor(string cofactorLabel)
    {
        CofactorType cofactorType;

        string fixedCofactorLabel = cofactorLabel.Replace("⁺", "pos").Replace("₂", "2");

        if (fixedCofactorLabel.Contains("NAD"))
            fixedCofactorLabel = "nadh";
        if (fixedCofactorLabel.Contains("phosphate"))
            fixedCofactorLabel = "pi";
        if (fixedCofactorLabel.Contains("FADH"))
            fixedCofactorLabel = "FAD";

        if (Enum.TryParse<CofactorType>(fixedCofactorLabel, true, out cofactorType))
        {

        }
        else
        {
            Debug.Log("Could not get cofactor color from cofactor label: " + cofactorLabel);
        }

        

        Color newColor = Color.white;
        switch (cofactorType)
        {
            case CofactorType.atp:
                newColor = GetColorFromHex("#E9E29C");
                break;
            case CofactorType.adp:
                newColor = GetColorFromHex("#E9E29C");
                break;
            case CofactorType.nadpos:
                newColor = GetColorFromHex("#9CCB86");
                break;
            case CofactorType.nadh:
                newColor = GetColorFromHex("#9CCB86");
                break;
            case CofactorType.h2o:
                newColor = GetColorFromHex("#6BF5F5");
                break;
            case CofactorType.co2:
                newColor = GetColorFromHex("#E4F1F7");
                break;
            case CofactorType.fad:
                newColor = GetColorFromHex("#F2ACCA");
                break;
            case CofactorType.fadh:
                newColor = GetColorFromHex("#F2ACCA");
                break;
            case CofactorType.pi:
                newColor = GetColorFromHex("#EEB479");
                break;
            case CofactorType.amp:
                newColor = GetColorFromHex("#FFCA65");
                break;
            case CofactorType.gdp:
                newColor = GetColorFromHex("#E9E29C");
                break;
            case CofactorType.gtp:
                newColor = GetColorFromHex("#E9E29C");
                break;
            case CofactorType.nadppos:
                newColor = GetColorFromHex("#9CCB86");
                break;
            case CofactorType.nadph:
                newColor = GetColorFromHex("#9CCB86");
                break;
            case CofactorType.fadh2:
                newColor = GetColorFromHex("#F2ACCA");
                break;
            case CofactorType.hpos:
                newColor = GetColorFromHex("#EFA8A8");
                break;
        }

        return newColor;
    }

    public CofactorType GetCofactorTypeFromLabel(string label)
    {
        CofactorType cofactorType = CofactorType.amp;

        string replacedSupSubCharacters = label.Replace("⁺", "+").Replace("₂", "2").Replace("inorganic phosphate", "pi").Replace("₄", "4");

        switch (replacedSupSubCharacters.ToLower())
        {
            case "atp":
                return CofactorType.atp;
            case "adp":
                return CofactorType.adp;
            case "nadpos":
                return CofactorType.nadpos;
            case "nadh":
                return CofactorType.nadh;
            case "h2o":
                return CofactorType.h2o;
            case "co2":
                return CofactorType.co2;
            case "fad":
                return CofactorType.fad;
            case "fadh":
                return CofactorType.fadh;
            case "pi":
                return CofactorType.pi;
            case "amp":
                return CofactorType.amp;
            case "gtp":
                return CofactorType.gtp;
            case "inorganic phosphate":
                return CofactorType.pi;
            case "gdp":
                return CofactorType.gdp;
            case "nad+":
                return CofactorType.nadpos;
            case "nadp+":
                return CofactorType.nadppos;
            case "nadph":
                return CofactorType.nadph;
            case "fadh2":
                return CofactorType.fadh2;
            case "hpos":
                return CofactorType.hpos;
            case "o2":
                return CofactorType.o2;


            default:
                {                    
                    Debug.Log("Couldn't parse cofactorType: " + label);

                    if (!unparsableCofactorLabels.Contains(label))
                        unparsableCofactorLabels.Add(label);

                    return cofactorType;
                }
        }


        return cofactorType;

    }

    public string FixCofactorLabelForParsing(string label)
    {
        string returnString = label;

        returnString.Replace("", "");

        return returnString;
    }

    Color GetColorFromHex(string hex)
    {
        Color color = Color.white; // Default color if the conversion fails

        if (ColorUtility.TryParseHtmlString(hex, out color))
        {
            return color;
        }

        Debug.LogWarning("Invalid hex color code: " + hex);
        return color;
    }

}

//public class FilterSettings
//{
//    public SettingsCategory category;

//    public List<bool> toggleStates;

//    public FilterSettings(SettingsCategory category, bool toggleAll, bool toggleHightlight, bool toggleAccent)
//    {
//        for (int i = 0; i < Enum.GetValues(typeof(SettingsCategory)).Cast<int>().Max(); i++)
//        {
//            toggleStates.Add(false);
//        }

//        toggleStates[0] = toggleAll;
//        toggleStates[1] = toggleHightlight;
//        toggleStates[2] = toggleAccent;

//    }

//}

