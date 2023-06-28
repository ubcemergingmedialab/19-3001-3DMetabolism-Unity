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
public enum CofactorType { atp, adp, nadpos, nadh, h2o, co2, fad, fadh, p1, amp }

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

