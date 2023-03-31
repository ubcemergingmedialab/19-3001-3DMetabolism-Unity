using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to store default values for settings.  The purpose of placing the values in one class here is because it's easier to toggle them
/// in one place rather than hunting down the game object that contains the settings and then changing it there.
/// It's also easier to build a GUI from one class rather than several (in the future)
/// </summary>
public class Constants : MonoBehaviour
{
    private static List<string> blackListedList;

    //---Default Values Goes Here for the application---

    //FONT SIZES
    //public static int DEFAULT_FONT_SIZE_NODE_LABEL = 36;
    //public static int DEFAULT_FONT_SIZE_EDGE_LABEL = 36;

    //DEFAULT TOGGLE SETTING
    ///...labels show all vs. highlighted, etc.

    //DEFAULT COLORS
    //...label starting colors

    void Awake()
    {
        InitializeBlacklist();
    }

    //This list represents the characters we want to remove from the Node labels displayed in the model.
    private void InitializeBlacklist()
    {
        blackListedList = new List<string>();
        blackListedList.Add("(cytosol)");
        blackListedList.Add("(mitochondrion)");
    }

    public static List<string> GetBlackListedLabels()
    {
        return blackListedList;
    }

}
