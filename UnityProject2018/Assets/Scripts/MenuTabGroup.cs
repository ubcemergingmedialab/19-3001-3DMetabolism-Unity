using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuTabGroup : MonoBehaviour
{
    public List<MenuTabButton> tabButtons;
    public MenuTabButton selectedTab;
    public List<GameObject> objectsToSwap;

    public Sprite tabHover;
    public Sprite tabActive;
    public Sprite tabIdle;

    
    public void Subcribe(MenuTabButton button) {
        if ( tabButtons == null) {
            tabButtons = new List<MenuTabButton>();
        }
        tabButtons.Add(button);
    }

    public void OnTabEnter(MenuTabButton button){
        ResetTabs();
        if (selectedTab == null || button !=selectedTab){
           button.background.sprite = tabHover; 
        }
    }
    public void OnTabExit(MenuTabButton button){
        ResetTabs();
        
    }
    public void OnTabSelected(MenuTabButton button){

        if (selectedTab != null){
            selectedTab.Deselect();
        }
        selectedTab = button;
        selectedTab.Select();

        ResetTabs();
        button.background.sprite = tabActive;

        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < objectsToSwap.Count; i++){
            objectsToSwap[i].SetActive((i == index));
        }
    }

    public void ResetTabs(){
        foreach(MenuTabButton button in tabButtons){
            if (selectedTab != null && button == selectedTab) {
                continue;
            }
            button.background.sprite = tabIdle;
        }
    }
        
}
