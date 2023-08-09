using System.Collections;
using System.Collections.Generic;
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

            //Debug.Log("Fixing cofactorlabel: " + label + " cofactortype: ");
            //Debug.Log(GeneralSettingsManager.Instance.GetCofactorTypeFromLabel(label.cofactor.label));
        }
    }



}
