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
        DisplayData.EnergyConsumed = edgeData.EnergyConsumed;
        DisplayData.EnergyProduced = edgeData.EnergyProduced;
        DisplayData.GibbsFreeEnergy = edgeData.GibbsFreeEnergy;
        if(displayPartner) {
            Debug.Log("edge has partner");
            DisplayData.AuxLabel = partnerData.Label;
            DisplayData.AuxQID = partnerData.QID;
            DisplayData.AuxDescription = partnerData.Description;
            DisplayData.AuxEnergyConsumed = partnerData.EnergyConsumed;
            DisplayData.AuxEnergyProduced = partnerData.EnergyProduced;
            DisplayData.AuxGibbsFreeEnergy = partnerData.GibbsFreeEnergy;
        }
        UIPresenter.Instance.NotifyUIUpdate(UIPresenter.UIList.EdgeUI, displayPartner);
    }
}
