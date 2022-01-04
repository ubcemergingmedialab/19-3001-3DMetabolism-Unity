using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FavouriteButtonLogic : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite defaultSprite;
    public Sprite hoverSprite;
    public Sprite clickSprite;

    public PathwaySO pathwaySO;

    Image image;

    bool isFaved = false;

    private void Start()
    {
        image = gameObject.GetComponent<Image>();
        Button button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(() => {
            OnClickColourChange();
        });

    }

    public void OnHoverColourChange()
    {
        if (isFaved)
        {
            image.sprite = defaultSprite;
        }
        else
        {
            image.sprite = hoverSprite;
        }
    }

    public void OnHoverExitColourChange()
    {
        if (isFaved)
        {
            image.sprite = clickSprite;
        }
        else
        {
            image.sprite = defaultSprite;
        }
    }
    public void OnClickColourChange()
    {
        isFaved = !isFaved;
        if (isFaved)
        {
            image.sprite = clickSprite;
            FavouriteButtonFactory.Instance.addToFaved(pathwaySO);
        } else
        {
            image.sprite = defaultSprite;
            FavouriteButtonFactory.Instance.removeFromFaved(pathwaySO);
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHoverColourChange();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnHoverExitColourChange();
    }
}
