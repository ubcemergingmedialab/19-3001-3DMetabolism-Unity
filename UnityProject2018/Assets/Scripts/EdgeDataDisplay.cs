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

    bool isHidden = false;

    // Start is called before the first frame update
    void Start()
    {

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
            GetComponent<MeshCollider>().sharedMesh = GetComponentInParent<MeshFilter>().sharedMesh;

        }

        else
        {
            Debug.Log("Missing edgelabel prefab");
        }

    }

    public void InstantiateCofactors()
    {      

        for (int i = 0; i < edgeData.cofactors.Count; i++)
        {
            GameObject cofactorPrefab = Resources.Load<GameObject>("Prefabs/Cofactor");
            GameObject instantiatedcofactor = Instantiate(cofactorPrefab);

            instantiatedcofactor.GetComponent<SpriteRenderer>().color = GeneralSettingsManager.Instance.GetCofactorColor(edgeData.cofactors[i].label);
            instantiatedcofactor.GetComponentInChildren<TextMeshPro>().text = edgeData.cofactors[i].label.Replace("⁺", "<sup>+</sup>").Replace("₂", "<sub>2</sub>").Replace("inorganic phosphate", "P<sub>i</sub>");

            instantiatedcofactor.GetComponent<CofactorLabel>().edgeObject = gameObject;
            instantiatedcofactor.GetComponent<CofactorLabel>().edgeDataDisplay = this;

            ReparentCofactor(instantiatedcofactor, edgeData.cofactors[i].isReactant);
            
        }

        if (edgeData.bidirectional)
        {
            for (int i = 0; i < edgeData.cofactors.Count; i++)
            {
                GameObject cofactorPrefab = Resources.Load<GameObject>("Prefabs/Cofactor");
                GameObject instantiatedcofactor = Instantiate(cofactorPrefab);

                instantiatedcofactor.GetComponent<SpriteRenderer>().color = GeneralSettingsManager.Instance.GetCofactorColor(edgeData.cofactors[i].label);
                instantiatedcofactor.GetComponentInChildren<TextMeshPro>().text = edgeData.cofactors[i].label.Replace("⁺", "<sup>+</sup>").Replace("₂", "<sub>2</sub>").Replace("inorganic phosphate", "P<sub>i</sub>");
                
                instantiatedcofactor.GetComponent<CofactorLabel>().edgeObject = gameObject;
                instantiatedcofactor.GetComponent<CofactorLabel>().edgeDataDisplay = this;

                ReparentCofactor(instantiatedcofactor, edgeData.cofactors[i].isReactant, true);


            }
        }

    }

    void ReparentCofactor(GameObject cofactor, bool isReactant, bool bidirectional = false)
    {
        Transform parent = transform;
        Transform biparent = transform;

        if (bidirectional)
        {
            if (isReactant)
            {
                if (bidirectional)
                {
                    parent = transform.parent.Find("Reactant.B");
                }
                else
                {
                    parent = transform.parent.Find("Reactant.A");
                }
            }
            else
            {
                if (bidirectional)
                {
                    parent = transform.parent.Find("Product.B");
                }
                else
                {
                    parent = transform.parent.Find("Product.A");
                }
            }
        }
        else
        {
            if (isReactant)
            {
                for (int i = 0; i < transform.parent.childCount; i++)
                {
                    if (transform.parent.GetChild(i).name.Contains("Reactant"))
                        parent = transform.parent.GetChild(i);
                }
            }
            else
            {
                for (int i = 0; i < transform.parent.childCount; i++)
                {
                    if (transform.parent.GetChild(i).name.Contains("Product"))
                        parent = transform.parent.GetChild(i);
                }
            }
        }
        if (parent != null)
        {
            GameObject cofactorParent = Resources.Load<GameObject>("Prefabs/CofactorParent");
            GameObject instantiatedCofactorParent = Instantiate(cofactorParent);

            instantiatedCofactorParent.transform.SetParent(CofactorLabelsManager.Instance.gameObject.transform, true);
            instantiatedCofactorParent.transform.position = parent.position;

            cofactorParent.GetComponent<CofactorParent>().parentID = parent.name;// cofactor.GetComponent<CofactorLabel>().edgeDataDisplay
            cofactorParent.GetComponent<CofactorParent>().edgeDataDisplay = cofactor.GetComponent<CofactorLabel>().edgeDataDisplay;
            CofactorLabelsManager.Instance.AddCofactorParent(cofactorParent.GetComponent<CofactorParent>());

            cofactor.transform.SetParent(instantiatedCofactorParent.transform, false);
            cofactor.transform.localPosition = Vector3.zero;
        }
        else
        {
            Debug.Log("Couldn't find a parent for Cofactor: " + cofactor.GetComponent<CofactorLabel>().edgeDataDisplay.edgeData.Enzyme);
        }
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
                    return;
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
