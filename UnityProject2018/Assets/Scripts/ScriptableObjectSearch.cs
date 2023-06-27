using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// This class takes care of the general search through Scriptable objects (ie Node, Edge, Pathway)
/// </summary>
public class ScriptableObjectSearch : MonoBehaviour
{

    public HashSet<GenericSO> searchableObjects;
    public TMP_InputField userInputField;
    public GameObject resultGenerator;
    public Transform parentTransform;


    private static ScriptableObjectSearch _instance;
    public static ScriptableObjectSearch Instance
    {
        get { return _instance; }
    }
    // Use this for initialization
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(this.gameObject);



    }

    /// <summary>
    /// Add all NodeSO,EdgeSO, and PathwaySOs in to Searchable objescts
    /// </summary>
    void Start()
    {
        searchableObjects = new HashSet<GenericSO>();
        searchableObjects.UnionWith(StatusController.Instance.activePathways);
        searchableObjects.UnionWith(StatusController.Instance.AllNodeSOs);
        searchableObjects.UnionWith(StatusController.Instance.AllEdgeSOs);
    }

    /// <summary>
    /// Trigger the general search if there is an input in the input box
    /// </summary>
    public void TriggerSearch()
    {
        string searchInput = userInputField.GetComponent<TMP_InputField>().text;
        if (!string.IsNullOrWhiteSpace(searchInput)) // check to see if there is an actual input
        {
            SearchScriptableObjects(searchInput);   // trigger search
        }
    }

    /// <summary>
    /// Search through the searchable objects, hashset, if the lable of the objects contain the inpuit, generate the buttons for them based on the type of the SO they are
    /// </summary>
    /// <param name="userInput"></param>
    /// <returns> a traversable Enumerable of GenereicSO</returns>
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

    /// <summary>
    /// if mouse in not over UI, delete the search result buttons
    /// </summary>
    public void deleteSearchResultOutOfUI()
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

/// <summary>
/// Delete the searchbuttons
/// </summary>
    public void DeleteSearchResult()
    {
        foreach (Transform child in parentTransform)
        {
            Destroy(child.gameObject);
        }
    }


}
