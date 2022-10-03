using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeDataDisplay : MonoBehaviour
{
    public EdgeSO edgeData;
    public EdgeSO partnerData;
    public Card DisplayData;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateScriptableObject()
    {
        bool displayPartner = (!GetComponent<HighlightHandler>().IsDoubleHighlighted()) && (partnerData != null);
        DisplayData.Label = edgeData.Label;
        DisplayData.QID = edgeData.QID;
        DisplayData.Description = edgeData.Description;
        DisplayData.EnzymeClass = edgeData.EnzymeClass;
        DisplayData.Enzyme = edgeData.Enzyme;
        DisplayData.Cofactors = edgeData.Cofactors;
        DisplayData.EnergyRequired = edgeData.EnergyRequired;
        DisplayData.Pubchemlink = edgeData.Pubchemlink;
        DisplayData.Regulation = edgeData.Regulation;
        if(displayPartner) {
            Debug.Log("edge has partner");
            DisplayData.AuxLabel = partnerData.Label;
            DisplayData.AuxQID = partnerData.QID;
            DisplayData.AuxDescription = partnerData.Description;
            DisplayData.AuxEnzymeClass = partnerData.EnzymeClass;
            DisplayData.AuxCofactors = partnerData.Cofactors;
            DisplayData.AuxEnergyRequired = partnerData.EnergyRequired;
            DisplayData.AuxPubchemlink = partnerData.Pubchemlink;
            DisplayData.AuxRegulation = partnerData.Regulation;
        }
        UIPresenter.Instance.NotifyUIUpdate(UIPresenter.UIList.EdgeUI, displayPartner);
    }
}
