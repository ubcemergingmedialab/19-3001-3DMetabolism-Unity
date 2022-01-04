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
public abstract class ButtonFactoryInterface : MonoBehaviour
{
    // singleton instantiation of ButtonFactory
    private static ButtonFactoryInterface _instance;
    public static ButtonFactoryInterface Instance
    {
        get { return _instance; }
    }

    public GameObject buttonPrefab;

    // Determines the placement of buttons.
    // X value should remain constant, Y value should differ by the offset
    // at every button generation
    public static float buttonX = 0;
    public static float buttonYOffset = -75;
    public float buttonY = 400;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;
    }

    protected GameObject GenerateButtonAndSetPosition()
    {
        GameObject generated = Instantiate(buttonPrefab, transform);
        RectTransform rect = generated.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector3(buttonX, buttonY, 0);
        buttonY += buttonYOffset;
        return generated;
    }

}
