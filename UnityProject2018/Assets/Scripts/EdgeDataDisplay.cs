using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static NodeTextDisplay;

/// <summary>
/// Takes the information stored in edgeData and transfers it into DisplayData
/// </summary>
public class EdgeDataDisplay : MonoBehaviour
{
    public EdgeSO edgeData;
    public EdgeSO partnerData;
    public Card DisplayData;

    public GameObject edgeLabelObject;

    public List<CofactorParent> cofactorParents;

    bool isHidden = false;

    // Start is called before the first frame update
    void Start()
    {
        InstantiateCofactors();
        CofactorLabelsManager.Instance.ToggleCofactorFilter();
    }

    // Update is called once per frame
    void Update()
    {
        MaintainLabelText();
    }

    /// <summary>
    /// Sets the DisplayData values to the equivalent in edgeData
    /// </summary>
    public void UpdateScriptableObject()
    {
        MouseOrbit.Instance.targetInFocus = gameObject;
        bool displayPartner = (!GetComponent<HighlightHandler>().IsDoubleHighlighted()) && (partnerData != null);
        DisplayData.Label = edgeData.Label;
        DisplayData.QID = edgeData.QID;
        DisplayData.Description = edgeData.Description;
        DisplayData.EnzymeClass = edgeData.EnzymeClass;
        DisplayData.Enzyme = edgeData.Enzyme;
        //DisplayData.Cofactors = edgeData.Cofactors;
        DisplayData.EnergyRequired = edgeData.EnergyRequired;
        DisplayData.Pubchemlink = edgeData.Pubchemlink;
        DisplayData.Regulation = edgeData.Regulation;
        if (displayPartner)
        {
            Debug.Log("edge has partner");
            DisplayData.AuxLabel = partnerData.Label;
            DisplayData.AuxQID = partnerData.QID;
            DisplayData.AuxDescription = partnerData.Description;
            DisplayData.AuxEnzymeClass = partnerData.EnzymeClass;
            //DisplayData.AuxCofactors = partnerData.Cofactors;
            DisplayData.AuxEnergyRequired = partnerData.EnergyRequired;
            DisplayData.AuxPubchemlink = partnerData.Pubchemlink;
            DisplayData.AuxRegulation = partnerData.Regulation;
        }
        UIPresenter.Instance.NotifyUIUpdate(UIPresenter.UIList.EdgeUI, displayPartner);
    }

    public void InstantiateEdgeLabel(GameObject parent)
    {
        GameObject edgeLabelPrefab = Resources.Load<GameObject>("Prefabs/EdgeLabel");

        if (edgeLabelPrefab != null)
        {
            //GameObject instantiatedLabel = Instantiate(edgeLabelPrefab, transform, false);
            GameObject instantiatedLabel = Instantiate(edgeLabelPrefab, parent.transform);
            instantiatedLabel.transform.position = transform.position;

            instantiatedLabel.GetComponent<TextMeshPro>().text = edgeData.Enzyme;

            bool blackListedCharsFound = false;

            foreach (string blackListedChar in Constants.GetBlackListedLabels())
            {
                if (edgeData.Enzyme.Contains(blackListedChar))
                {
                    blackListedCharsFound = true;
                    searchCategory searchCategory = searchCategory.standard;
                    Enum.TryParse(blackListedChar.Replace("(", "").Replace(")", ""), out searchCategory);
                    edgeData.searchCategory = searchCategory;
                    edgeData.Enzyme = edgeData.Enzyme.Replace(blackListedChar, "");
                    instantiatedLabel.GetComponent<TextMeshPro>().SetText("<mark=#00000000><font=\"LiberationSans SDF\">" + edgeData.Enzyme.Replace(blackListedChar, "") + "</font></mark>");
                }
            }


            edgeLabelObject = instantiatedLabel;

            if (!GetComponent<ShowTextOnHover>())
            {
                gameObject.AddComponent<ShowTextOnHover>();
                gameObject.GetComponent<ShowTextOnHover>().text = instantiatedLabel.GetComponent<TextMeshPro>();
            }

            GetComponent<BoxCollider>().enabled = false;
            gameObject.AddComponent<MeshCollider>();
            //GetComponent<MeshCollider>().convex = true;
            GetComponent<MeshCollider>().sharedMesh = GetComponentInParent<MeshFilter>().sharedMesh;

        }

        else
        {
            Debug.Log("Missing edgelabel prefab");
        }

    }

    public void InstantiateCofactors()
    {
        try
        {
            for (int i = 0; i < edgeData.cofactors.Count; i++)
            {
                GameObject cofactorPrefab = Resources.Load<GameObject>("Prefabs/Cofactor");
                GameObject instantiatedcofactor = Instantiate(cofactorPrefab);

                instantiatedcofactor.GetComponent<SpriteRenderer>().color = GeneralSettingsManager.Instance.GetCofactorColor(edgeData.cofactors[i].label);
                instantiatedcofactor.GetComponentInChildren<TextMeshPro>().text = edgeData.cofactors[i].label.Replace("⁺", "<sup>+</sup>").Replace("₂", "<sub>2</sub>").Replace("inorganic phosphate", "P<sub>i</sub>").Replace("₄", "<sub>4</sub>");

                CofactorLabel cofactorLabel = instantiatedcofactor.GetComponent<CofactorLabel>();
                cofactorLabel.edgeObject = gameObject;
                cofactorLabel.edgeDataDisplay = this;
                cofactorLabel.cofactor = edgeData.cofactors[i];

                // If edge is bidirectional, generate inverse cofactors as well
                if (edgeData.bidirectional)
                {
                    GameObject otherCofactor = Instantiate(cofactorPrefab);
                    otherCofactor.GetComponent<SpriteRenderer>().color = GeneralSettingsManager.Instance.GetCofactorColor(edgeData.cofactors[i].label);
                    otherCofactor.GetComponentInChildren<TextMeshPro>().text = edgeData.cofactors[i].label.Replace("⁺", "<sup>+</sup>").Replace("₂", "<sub>2</sub>").Replace("inorganic phosphate", "P<sub>i</sub>").Replace("₄", "<sub>4</sub>");


                    CofactorLabel otherCofactorLabel = otherCofactor.GetComponent<CofactorLabel>();
                    otherCofactorLabel.edgeObject = gameObject;
                    otherCofactorLabel.edgeDataDisplay = this;
                    otherCofactorLabel.cofactor = edgeData.cofactors[i];

                    ReparentCofactorLabel(otherCofactor, true);
                }

                ReparentCofactorLabel(instantiatedcofactor, false);



            }
        }
        catch
        {
            Debug.LogWarning("No edgedata found for edge: " + transform.parent.gameObject.name);
        }


    }

    void ReparentCofactorLabel(GameObject cofactorObject, bool secondDirection = false)
    {
        Transform parentLocation = transform;

        CofactorLabel cofactorLabel = cofactorObject.GetComponent<CofactorLabel>();
        Cofactor cofactor = cofactorLabel.GetComponent<CofactorLabel>().cofactor;

        if (FindCofactorLocationObject(cofactor, secondDirection, out parentLocation))
        {
            // Success
        }
        else
        {
            Debug.Log("No cofactor parent object found for cofactor: " + cofactor.label);
            return;
        }

        if (parentLocation != null)
        {

            Vector3 cofactorLocalPosition = Vector3.zero;

            // check if existing CofactorParent exists
            if (CofactorLabelsManager.Instance.CofactorParents.Exists(x => x.edgeDataDisplay == this && x.isReactant == cofactor.isReactant && x.secondDirection == secondDirection))
            {
                CofactorParent existingParent = CofactorLabelsManager.Instance.CofactorParents.Find(x => x.edgeDataDisplay == this && x.isReactant == cofactor.isReactant && x.secondDirection == secondDirection);

                if (!existingParent.cofactorLabels.Contains(cofactorLabel))
                    existingParent.cofactorLabels.Add(cofactorLabel);

                cofactorLocalPosition = existingParent.GetCofactorLabelLocalPosition(cofactorLabel);
                cofactorObject.transform.SetParent(existingParent.gameObject.transform, false);

                cofactorParents.Add(existingParent);
            }
            else
            {

                GameObject cofactorParent = Resources.Load<GameObject>("Prefabs/CofactorParent");
                GameObject instantiatedCofactorParent = Instantiate(cofactorParent);

                instantiatedCofactorParent.transform.SetParent(CofactorLabelsManager.Instance.gameObject.transform, true);
                instantiatedCofactorParent.transform.position = parentLocation.position;

                CofactorParent cofactorParentComponent = instantiatedCofactorParent.GetComponent<CofactorParent>();
                cofactorParentComponent.parentLocationName = parentLocation.name;
                cofactorParentComponent.parentObject = parentLocation.gameObject;
                cofactorParentComponent.edgeDataDisplay = cofactorLabel.edgeDataDisplay;
                cofactorParentComponent.isReactant = cofactor.isReactant;
                cofactorParentComponent.secondDirection = secondDirection;

                CofactorLabelsManager.Instance.AddCofactorParent(instantiatedCofactorParent.GetComponent<CofactorParent>());
                cofactorObject.transform.SetParent(instantiatedCofactorParent.transform, false);

                cofactorParentComponent.InitializeArrow();
                cofactorParentComponent.cofactorLabels.Add(cofactorLabel);

                cofactorParents.Add(cofactorParentComponent);
            }


            cofactorObject.transform.localPosition = cofactorLocalPosition;
            CofactorLabelsManager.Instance.AddCofactorLabel(cofactorLabel);
        }
        else
        {
            Debug.Log("Couldn't find a parent for Cofactor: " + cofactorLabel.edgeDataDisplay.edgeData.Enzyme);
        }
    }

    bool FindCofactorLocationObject(Cofactor cofactor, bool secondDirection, out Transform locationTransform)
    {

        bool parentFound = false;

        string nameToSearchFor = "Product";

        if (cofactor.isReactant)
            nameToSearchFor = "Reactant";

        // If first direction, grab first product/reactant location object
        if (!secondDirection)
        {
            for (int i = 0; i < transform.parent.childCount; i++)
            {
                if (transform.parent.GetChild(i).name.Contains(nameToSearchFor))
                {
                    parentFound = true;
                    locationTransform = transform.parent.GetChild(i).transform;
                    return parentFound;
                }
            }
        }
        else // If second direction, grab last product/reactant location object
        {
            if (cofactor.isReactant)
                nameToSearchFor = "Product";
            else
                nameToSearchFor = "Reactant";

            Transform lastTransformFound = transform;
            for (int i = 0; i < transform.parent.childCount; i++)
            {
                if (transform.parent.GetChild(i).name.Contains(nameToSearchFor))
                {
                    parentFound = true;
                    lastTransformFound = transform.parent.GetChild(i).transform;
                }
            }

            if (parentFound)
            {
                locationTransform = lastTransformFound;
                return parentFound;
            }
            else
            {
                locationTransform = null;
                return parentFound;
            }
        }

        locationTransform = null;
        return parentFound;
    }

    // Copied directly from nodedatadisplay
    /// <summary>
    /// Sets the node label to a specific string.
    /// TODO - is it necessary to call this on every update for every single node? (seems overkill and inefficient).
    /// </summary>
    private void MaintainLabelText()
    {
        //switch (NodeTextDisplay.Instance.activeStrategyEnum)
        //{
        //    case TextDisplayStrategyEnum.HighlightedPathwaysStrategy:
        //        HighlightPathway.HighlightState highlightState = StatusController.Instance.ElementCheckState(GetComponent<HighlightHandler>());
        //        if (highlightState != HighlightPathway.HighlightState.Highlighted)
        //            return;
        //        break;
        //    case TextDisplayStrategyEnum.AccentedPathwaysStrategy:
        //        HighlightPathway.HighlightState accentedState = StatusController.Instance.ElementCheckState(GetComponent<HighlightHandler>());
        //        if (accentedState != HighlightPathway.HighlightState.Accented)
        //            return;
        //        break;
        //    case TextDisplayStrategyEnum.AllTextStrategy:
        //        break;
        //    case TextDisplayStrategyEnum.NoTextStrategy:
        //        return;
        //    default:
        //        break;
        //}

        // Check if text is currently showing from hovering mouse over object
        if (GetComponent<ShowTextOnHover>())
        {
            if (GetComponent<ShowTextOnHover>().isShowingText)
            {
                // If text is showing and this target is not the main focus right now, return
                if (MouseOrbit.Instance.targetInFocus != gameObject)
                {
                    edgeLabelObject.GetComponent<TextMeshPro>().alpha = 1;
                    edgeLabelObject.GetComponent<TextMeshPro>().fontSize = 36.0f * MouseOrbit.Instance.cameraLabelController.ReactionsFontSizeMultiplier;
                    return;
                }
            }
        }

        if (edgeData != null)
        {
            if (!isHidden)
            {
                // Calculate multiplier based on object distance to main camera
                float distanceToCameraMultiplier = MouseOrbit.Instance.cameraLabelController.GetAlphaValue(transform.position);

                // Perform fontsize and transparency calculations
                if (MouseOrbit.Instance.targetInFocus == gameObject)
                {
                    distanceToCameraMultiplier = 1;
                    edgeLabelObject.GetComponent<TextMeshPro>().alpha = distanceToCameraMultiplier;
                    edgeLabelObject.GetComponent<TextMeshPro>().fontSize = 36.0f * distanceToCameraMultiplier * MouseOrbit.Instance.cameraLabelController.ReactionsFontSizeMultiplier;
                }
                else
                {
                    edgeLabelObject.GetComponent<TextMeshPro>().alpha = Mathf.Clamp(distanceToCameraMultiplier, 0.2f, 0.7f);
                    edgeLabelObject.GetComponent<TextMeshPro>().fontSize = 32.0f * distanceToCameraMultiplier * MouseOrbit.Instance.cameraLabelController.ReactionsFontSizeMultiplier;
                }
            }


        }
    }

    public void TransparentText()
    {
        if (edgeLabelObject != null)
        {
            edgeLabelObject.GetComponent<TextMeshPro>().alpha = 0.0f;
        }

        isHidden = true;
    }

    public void OpaqueText()
    {
        //if (edgeLabelObject != null)
        //    edgeLabelObject.GetComponent<TextMeshPro>().alpha = 1.0f;

        isHidden = false;
    }
}
