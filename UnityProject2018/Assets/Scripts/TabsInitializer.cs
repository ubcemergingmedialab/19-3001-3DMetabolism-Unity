using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TabsInitializer : MonoBehaviour
{

    private void Awake()
    {
        List<Transform> children = transform.GetComponentsInChildren<Transform>(true).ToList();

        for (int i = 0; i < children.Count; i++)
        {
            if (children[i].gameObject.name.Contains("MenuPage"))
            {
                bool isActive = children[i].gameObject.activeInHierarchy;
                children[i].gameObject.SetActive(true);
                children[i].gameObject.SetActive(isActive);
            }
        }
    }

}
