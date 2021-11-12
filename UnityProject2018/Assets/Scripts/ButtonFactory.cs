using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* NOTE:
    ActivePathway info set from statuscontroller's instantiation.
    For each pathway SO, generate a button based on the prefab,
    and register an on click event for each button which sends the information
    about the selected pathway to HighlightSystem
 */
public class ButtonFactory : MonoBehaviour
{
    // singleton instantiation of ButtonFactory
    private static ButtonFactory _instance;
    public static ButtonFactory Instance
    {
        get { return _instance; }
    }

    // Pathway scriptable object obtained from StatusController,
    // Prefab for button instantiation
    public List<PathwaySO> ActivePathways;
    public GameObject buttonPrefab;

    // Determines the placement of buttons.
    // X value should remain constant, Y value should differ by the offset
    // at every button generation
    public static float buttonX = -550;
    public static float buttonYOffset = -75;
    public float buttonY = 400;

    Dictionary<GameObject, PathwaySO> buttons = new Dictionary<GameObject, PathwaySO>();


    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;
    }


        void Start()
    {
        ActivePathways = StatusController.Instance.activePathways;
        foreach (PathwaySO pathway in ActivePathways)
        {
            buttons.Add(GenerateButton(pathway), pathway);

        }
    }

    GameObject GenerateButton(PathwaySO pathway)
    {
        GameObject generated = GenerateButtonAndSetPosition();
        SetButtonTextFromPathway(pathway, generated);
        generated.GetComponent<PathwayButtonLogic>().pathwaySO = pathway;
        return generated;
    }

    private static void SetButtonTextFromPathway
        (PathwaySO pathway, GameObject generated)
    {
        Text childText = generated.GetComponentInChildren<Text>();
        childText.text = pathway.Label;
    }

    private GameObject GenerateButtonAndSetPosition()
    {
        GameObject generated = Instantiate(buttonPrefab, transform);
        RectTransform rect = generated.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector3(buttonX, buttonY, 0);
        buttonY += buttonYOffset;
        return generated;
    }

    public void UpdateAllButtonOnClick()
    {
        // ensure that only one button appears yellow
        foreach(GameObject button in buttons.Keys)
        {
            PathwayButtonLogic pathwayButtonLogic = button.GetComponent<PathwayButtonLogic>();
            pathwayButtonLogic.OnClickColourChange(pathwayButtonLogic.FindPathwayState(buttons[button]));
        }
    }

}
