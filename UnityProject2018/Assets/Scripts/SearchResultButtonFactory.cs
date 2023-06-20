using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchResultButtonFactory : MonoBehaviour
{
    public GameObject resultButtonPrefab;

    // singleton instantiation of ButtonFactory
    private static SearchResultButtonFactory _instance;
    public static SearchResultButtonFactory Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;

    }
    public GameObject GenerateButton(Transform ParentTramsform, ScriptableObject obj)
    {
        GameObject generated = Instantiate(resultButtonPrefab, ParentTramsform);
        generated.GetComponent<SearchResultButtonLogic>().ElementSO = (GenericSO) obj;
        return generated;
    }



}
