using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeTextDisplay : MonoBehaviour
{
    //the Show all label toggle.
    public Toggle toggle;

    private static NodeTextDisplay _instance;
    public static NodeTextDisplay Instance
    {
        get { return _instance; }
    }

    TextDisplayStrategy activeStrategy;

    public TextDisplayStrategyEnum activeStrategyEnum = TextDisplayStrategyEnum.AllTextStrategy;

    public enum TextDisplayStrategyEnum
    {
        HighlightedPathwaysStrategy,
        AccentedPathwaysStrategy,
        AllTextStrategy,
        NoTextStrategy,
    }

    private Dictionary<TextDisplayStrategyEnum, TextDisplayStrategy> availableStrategies;
    private bool allValue = true;
    private int filterValue = 2;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(this.gameObject);

        availableStrategies = new Dictionary<TextDisplayStrategyEnum, TextDisplayStrategy>();
        availableStrategies.Add(TextDisplayStrategyEnum.HighlightedPathwaysStrategy, HighlightedPathwaysStrategy.Instance);
        availableStrategies.Add(TextDisplayStrategyEnum.AccentedPathwaysStrategy, AccentedPathwaysStrategy.Instance);
        availableStrategies.Add(TextDisplayStrategyEnum.AllTextStrategy, AllTextStrategy.Instance);
        availableStrategies.Add(TextDisplayStrategyEnum.NoTextStrategy, NoTextStrategy.Instance);
        activeStrategy = AllTextStrategy.Instance;
        activeStrategyEnum = TextDisplayStrategyEnum.AllTextStrategy;
    }

    public void UpdateTextDisplay()
    {
        activeStrategy.UpdateTextDisplay();
    }

    public void ChangeStrategy(int stratCode)
    {
        Debug.Log("Calling change strategy " + (TextDisplayStrategyEnum)stratCode);
        TextDisplayStrategy stratToUse;
        if(availableStrategies.TryGetValue((TextDisplayStrategyEnum)stratCode, out stratToUse))
        {
            Debug.Log("TEXT DISPLAY UPDATE");
            activeStrategy = stratToUse;
            activeStrategyEnum = (TextDisplayStrategyEnum)stratCode;
            activeStrategy.UpdateTextDisplay();
        }
    }

    public void ChangeAllValue(bool value) {
        allValue = value;
        if(allValue == false) {
            ChangeStrategy(3);
        } else {
            ChangeStrategy(filterValue);
        }
        UpdateToggleDisplay();
    }
    public void ChangeFilterValue(int value) {
        filterValue = value;
        if(allValue == false) {
            ChangeStrategy(3);
        } else {
            ChangeStrategy(filterValue);
        }
    }

    private void UpdateToggleDisplay()
    {
        toggle.isOn = allValue;
    }
}
