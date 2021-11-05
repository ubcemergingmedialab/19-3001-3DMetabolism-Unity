using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/*
This class disables the user camera manipulation access when the mouse enters specified areas 
*/

public class DisableOrbit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData pointerEventData){
        //Debug.Log("Mouse Entered");
        MouseOrbit.Instance.SetDisableOrbit(true);
    }

    public void OnPointerExit(PointerEventData pointerEventData){
        //Debug.Log("Mouse Exited");
        MouseOrbit.Instance.SetDisableOrbit(false);
    }
}