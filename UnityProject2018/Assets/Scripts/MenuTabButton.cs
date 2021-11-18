using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class MenuTabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{

    public MenuTabGroup tabGroup;
    public Image targetImage;
    public Sprite tabHover;
    public Sprite tabActive;
    public Sprite tabIdle;

    public UnityEvent onTabSelected;
    public UnityEvent onTabDeselected;

    

    // Start is called before the first frame update
    void Start()
    { 
        targetImage = GetComponent<Image>();
        tabGroup.Subcribe(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData) {
        tabGroup.OnTabSelected(this);
    }
    public void OnPointerEnter(PointerEventData eventData) {
        tabGroup.OnTabEnter(this);
    }
    public void OnPointerExit(PointerEventData eventData) {
        tabGroup.OnTabExit(this);
    }

    public void Select(){
        if (onTabSelected != null){
            onTabSelected.Invoke();
        }
    }
    public void Deselect(){
        if (onTabDeselected != null){
            onTabDeselected.Invoke();
        }
    }
}
