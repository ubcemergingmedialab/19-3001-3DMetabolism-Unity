using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp : MonoBehaviour
{

    public GameObject PopupPanel;
    private bool enable = false;

    public void PopUpEnable() {
        enable = (!enable);
        PopupPanel.SetActive(enable);
    }
}
