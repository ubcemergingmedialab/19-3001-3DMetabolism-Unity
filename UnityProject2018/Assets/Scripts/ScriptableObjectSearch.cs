using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ScriptableObjectSearch : MonoBehaviour
{

    public HashSet<GenericSO> searchableObjects;
    public TMP_InputField userInputField;


    void Start()
    {
        searchableObjects = new HashSet<GenericSO>();
        searchableObjects.UnionWith(StatusController.Instance.activePathways);
        searchableObjects.UnionWith(StatusController.Instance.AllNodeSOs);
        searchableObjects.UnionWith(StatusController.Instance.AllEdgeSOs);
        Debug.Log("Count "+ searchableObjects.Count);
    }

    public void TriggerSearch()
    {
        //
        SearchScriptableObjects(userInputField.GetComponent<TMP_InputField>().text);
        
    }

    public IEnumerable<GenericSO> SearchScriptableObjects(string userInput)
    {
        string input = userInput.ToLower();

        //Using LINQ to filter the searchables
        var searchResults = from obj in searchableObjects
                            where obj.Label.ToLower().Contains(input)
                            select obj;

        Debug.Log("<srch result> " + searchResults.ToList().Count);
        foreach (var obj in searchResults)
        {
            Debug.Log("<srch result> " + obj.Label);
        }

        return searchResults;
    }


}
