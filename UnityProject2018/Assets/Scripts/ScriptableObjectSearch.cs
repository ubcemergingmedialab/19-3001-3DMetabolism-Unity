using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScriptableObjectSearch : MonoBehaviour
{

    public HashSet<GenericSO> searchableObjects;
    public TMP_InputField userInputField;
    public GameObject resultGenerator;
    public Transform parentTransform;


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
        string searchInput = userInputField.GetComponent<TMP_InputField>().text;
        if (!string.IsNullOrWhiteSpace(searchInput)) // check to see if there is an actual input
        {
            SearchScriptableObjects(searchInput);   // trigger search
        }
        
        
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


            if (obj.GetType() == typeof(EdgeSO))
            {
                GameObject newButton = resultGenerator.GetComponent<SearchResultButtonFactory>().GenerateButton(parentTransform, obj);
                newButton.transform.Find("Name (TMP)").GetComponent<TMP_Text>().text = obj.Label;
                newButton.transform.Find("Type (TMP)").GetComponent<TMP_Text>().text = "Reaction";

            }
            else if (obj.GetType() == typeof(NodeSO))
            {
                GameObject newButton = resultGenerator.GetComponent<SearchResultButtonFactory>().GenerateButton(parentTransform, obj);
                newButton.transform.Find("Name (TMP)").GetComponent<TMP_Text>().text = obj.Label;
                newButton.transform.Find("Type (TMP)").GetComponent<TMP_Text>().text = "Metabolite";

            }
            else if (obj.GetType() == typeof(PathwaySO))
            {
                GameObject newButton = resultGenerator.GetComponent<SearchResultButtonFactory>().GenerateButton(parentTransform, obj);
                newButton.transform.Find("Name (TMP)").GetComponent<TMP_Text>().text = obj.Label;
                newButton.transform.Find("Type (TMP)").GetComponent<TMP_Text>().text = "Pathway";

            }

        }



        return searchResults;
    }

    public void deleteSearchResult()
    {
        if (Camera.main.GetComponent<MouseOrbit>().IsPointerOverUI()) // if over UI, dont delete
        {
            return;
        }
        foreach (Transform child in parentTransform)
        {
            Destroy(child.gameObject);
        }
    }


}
