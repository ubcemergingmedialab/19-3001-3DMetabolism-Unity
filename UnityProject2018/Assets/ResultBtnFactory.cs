using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* 
 * Display search result as a list of buttons
 */
public class ResultBtnFactory : MonoBehaviour
{
    // singleton instantiation of ButtonFactory
    private static ResultBtnFactory _instance;
    public static ResultBtnFactory Instance
    {
        get { return _instance; }
    }

    public GameObject buttonPrefab;

    // Determines the placement of buttons.
    // X value should remain constant, Y value should differ by the offset
    // at every button generation
    public static float buttonX = 0;
    public static float buttonYOffset = -50;
    public float buttonY;
    public float currentButtonY; 

    public Card dataSO;

    //Dictionary<GameObject, PathwaySO> buttons = new Dictionary<GameObject, PathwaySO>();
    List<GameObject> resultBtns = new List<GameObject>();


    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        currentButtonY = buttonY; 
        _instance = this;

    }

    // TODO: refactor results
    public void MakeButtons(Dictionary<int, List<ScriptableObject>> paths)
    {
        foreach (KeyValuePair<int, List<ScriptableObject>> path in paths)
        {
            // Debug.Log("adding button for " + pathway);
            resultBtns.Add(GenerateButton(path.Key, path.Value));
            currentButtonY += buttonYOffset;
        }
    }

    // remove existing buttons
    public void ResetButtons()
    {
        foreach (GameObject go in resultBtns)
        {
            Destroy(go); 
        }
        currentButtonY = buttonY;
        resultBtns = new List<GameObject>(); 
    }

    GameObject GenerateButton(int n, List<ScriptableObject> path)
    {
        GameObject generated = InitButtonAndSetPosition();
        SetBtnText(n, path, generated);
        // TO REFACTOR: Populate with new button logic + actual paths
        int numPathways = StatusController.Instance.activePathways.Count; 
        generated.GetComponent<PathwayButtonLogic>().pathwaySO = StatusController.Instance.activePathways[n % numPathways]; 
        generated.GetComponent<PathwayButtonLogic>().dataSO = dataSO;
        return generated;
    }

    private static void SetBtnText
        (int n, List<ScriptableObject> path, GameObject generated)
    {
        Text childText = generated.GetComponentInChildren<Text>();
        childText.text = "Path " + n;
        //childText.text += " via " + path[path.Count / 2].name; 
    }

    private GameObject InitButtonAndSetPosition()
    {
        GameObject generated = Instantiate(buttonPrefab, transform);
        RectTransform rect = generated.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector3(buttonX, currentButtonY, 0);
        return generated;
    }

}
