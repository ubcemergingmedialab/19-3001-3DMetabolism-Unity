using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathwayUIOnClick : MonoBehaviour
{
    // Start is called before the first frame update
    public PathwaySO so;
    public Card dataSO;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateUI() {
        dataSO.Label = so.Label;
        dataSO.QID = so.QID;
        dataSO.Description = so.Description;
        if (UIPresenter.UIList.PathwayUI != null)
            UIPresenter.Instance.NotifyUIUpdate(UIPresenter.UIList.PathwayUI, false);
        else Debug.Log("Error in callin PathwayUI list");
    }
}
