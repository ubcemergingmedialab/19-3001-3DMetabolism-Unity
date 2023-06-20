using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchResultButtonLogic : MonoBehaviour
{
    // Start is called before the first frame update
    public GenericSO ElementSO;


    public void ClickAndFocus()
    {
        if (ElementSO.GetType() == typeof(EdgeSO))
        {
            if (!Camera.main.GetComponent<CameraController>().GetAutoLock())
            {
                //Camera.main.GetComponent<CameraController>().MoveCameraToParentElement(ElementSO.transform);
                //Object.FindObjectsOfType<EdgeDataDisplay>();

            }
        }
        else if (ElementSO.GetType() == typeof(EdgeSO))
        {

        }
        else if (ElementSO.GetType() == typeof(EdgeSO))
        {

        }
    }
    

}
