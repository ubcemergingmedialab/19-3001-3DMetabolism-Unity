using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// activate/deactive a game object
public class PopUp : MonoBehaviour
{

    public GameObject PopupObject;
    private bool enable = false;

    public void PopUpEnable() {
        enable = (!enable);
        PopupObject.SetActive(enable);
    }
}
