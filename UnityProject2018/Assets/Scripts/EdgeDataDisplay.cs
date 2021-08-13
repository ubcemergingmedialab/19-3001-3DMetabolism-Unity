using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeDataDisplay : MonoBehaviour
{
    public EdgeSO edgeData;
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
        DisplayData.Label = edgeData.Label;
        DisplayData.QID = edgeData.QID;
        DisplayData.Description = edgeData.Description;
        DisplayData.EnergyConsumed = edgeData.EnergyConsumed;
        DisplayData.EnergyProduced = edgeData.EnergyProduced;
        DisplayData.GibbsFreeEnergy = edgeData.GibbsFreeEnergy;
        if (UIPresenter.UIList.EdgeUI != null)
            UIPresenter.Instance.NotifyUIUpdate(UIPresenter.UIList.EdgeUI);
        else Debug.Log("Error in callin EdgeUI list");

    }
}
