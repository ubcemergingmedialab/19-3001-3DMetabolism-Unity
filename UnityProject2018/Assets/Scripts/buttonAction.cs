using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonAction : MonoBehaviour
{
    public GameObject gameObject;
    bool active=true;
    public void OpenandClose()
    {
        if (active == false) {
            gameObject.transform.gameObject.SetActive(true);
            active = true;
        }
        else
        {
            gameObject.transform.gameObject.SetActive(false);
            active = false;
        }
    }
}
