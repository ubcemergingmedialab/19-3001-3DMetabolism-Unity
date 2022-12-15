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

    public Card dataSO;

    //Dictionary<GameObject, PathwaySO> buttons = new Dictionary<GameObject, PathwaySO>();


    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;

    }

    // TODO: refactor results
    public void MakeButtons(Dictionary<int, List<ScriptableObject>> paths)
    {
        foreach (KeyValuePair<int, List<ScriptableObject>> path in paths)
        {
            // Debug.Log("adding button for " + pathway);
            GenerateButton(path.Key, path.Value); 
            buttonY += buttonYOffset;
        }
    }

    public void ResetButtons()
    {

    }

    GameObject GenerateButton(int n, List<ScriptableObject> path)
    {
        GameObject generated = GenerateButtonAndSetPosition();
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

    private GameObject GenerateButtonAndSetPosition()
    {
        GameObject generated = Instantiate(buttonPrefab, transform);
        RectTransform rect = generated.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector3(buttonX, buttonY, 0);
        return generated;
    }

}
