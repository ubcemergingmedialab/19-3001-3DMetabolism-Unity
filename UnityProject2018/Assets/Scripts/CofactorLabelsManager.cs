using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CofactorLabelsManager : MonoBehaviour
{
    private static CofactorLabelsManager _instance;

    List<CofactorParent> cofactorParents;
    List<CofactorLabel> cofactorLabels;

    public static CofactorLabelsManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public List<CofactorLabel> CofactorLabels
    { get { return cofactorLabels; } }

    public List<CofactorParent> CofactorParents
    { get { return cofactorParents; } }


    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);
        cofactorParents = new List<CofactorParent>();
        cofactorLabels = new List<CofactorLabel>();
    }


    public void AddCofactorParent(CofactorParent parent)
    {
        if (!cofactorParents.Contains(parent))
        {
            cofactorParents.Add(parent);
        }
    }

    public void AddCofactorLabel(CofactorLabel label)
    {
        if (!cofactorLabels.Contains(label))
        {
            cofactorLabels.Add(label);
        }
    }

    public void ToggleCofactorFilter(List<CofactorType> enabledCofactorTypes = null)
    {
        if (enabledCofactorTypes == null)
            enabledCofactorTypes = new List<CofactorType>();
            
        for (int i = 0; i < cofactorParents.Count; i++)
        {
            for (int j = 0; j < cofactorParents[i].cofactorLabels.Count; j++)
            {
                cofactorParents[i].cofactorLabels[j].gameObject.SetActive(enabledCofactorTypes.Contains(cofactorParents[i].cofactorLabels[j].CofactorType));                
            }

            // Toggle the arrow on or off depending on active cofactors
            cofactorParents[i].ToggleArrow(CountActiveChildren(cofactorParents[i].gameObject) != 0);
            

        }
    }

    int CountActiveChildren(GameObject parent)
    {
        int count = 0;

        // Iterate through each child of the parent GameObject
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            // Check if the child is active/enabled
            if (parent.transform.GetChild(i).gameObject.activeSelf)
            {
                count++;
            }
        }

        return count;
    }


}
