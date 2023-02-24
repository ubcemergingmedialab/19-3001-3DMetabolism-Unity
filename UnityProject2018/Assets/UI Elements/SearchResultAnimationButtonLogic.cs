using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SearchResultAnimationButtonLogic : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite hoverColor;
    public Sprite exitColor;
    public Sprite clickColor;
    private Image image;

    //TODO do we need this?  probably?
    public PathwaySO pathwaySO;
    public Card dataSO;
    public List<ScriptableObject> listOfOrderedSO;

    // Start is called before the first frame update
    private void Start()
    {
        image = gameObject.GetComponent<Image>();
        Button button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            //AlertHighlightSystem(pathwaySO);

            OnClickHandler();
            OnClickColourChange();
            //UpdateUI();
            //ButtonFactory.Instance.UpdateAllButtonOnClick(); //TODO probably tell other animations to stop here? update other button states?
        });
    }

    // Update is called once per frame
    void Update()
    {


    }
    public void OnClickHandler()
    {
        //Animate the search results
        AnimationControllerComponent.Instance.AnimateSearchResults(listOfOrderedSO);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHoverColourChange();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnHoverExitColourChange();
    }

    public void OnHoverColourChange()
    {
        image.sprite = hoverColor;
    }

    public void OnHoverExitColourChange()
    {
        image.sprite = exitColor;
    }

    public void OnClickColourChange()
    {
        image.sprite = clickColor;
    }
}
