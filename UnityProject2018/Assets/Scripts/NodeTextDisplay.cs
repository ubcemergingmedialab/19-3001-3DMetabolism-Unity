using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeTextDisplay : MonoBehaviour
{

    private static NodeTextDisplay _instance;
    public static NodeTextDisplay Instance
    {
        get { return _instance; }
    }

    TextDisplayStrategy activeStrategy;

    public enum TextDisplayStrategyEnum
    {
        HighlightedPathwaysStrategy,
        AccentedPathwaysStrategy,
        AllTextStrategy
    }

    private Dictionary<TextDisplayStrategyEnum, TextDisplayStrategy> availableStrategies;

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
        activeStrategy = HighlightedPathwaysStrategy.Instance;
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
            activeStrategy.UpdateTextDisplay();
        }
    }
}
