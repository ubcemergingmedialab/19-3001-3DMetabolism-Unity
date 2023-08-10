using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CofactorLabel : MonoBehaviour
{
    public GameObject edgeObject;
    public Cofactor cofactor;
    public EdgeDataDisplay edgeDataDisplay;
    public CofactorParent cofactorParent;
    private CofactorType cofactorType;
    private bool cofactorTypeInitialized = false;

    public CofactorType CofactorType
    {
        get 
        {
            return GetCofactorType();
        }
    }

    public CofactorType GetCofactorType()
    {
        if (!cofactorTypeInitialized)
        {
            cofactorType = GeneralSettingsManager.Instance.GetCofactorTypeFromLabel(cofactor.label);
            cofactorTypeInitialized = true;
        }

        return cofactorType;

    }

}
