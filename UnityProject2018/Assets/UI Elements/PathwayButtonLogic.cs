using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PathwayButtonLogic : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite defaultSprite;
    public Sprite defaultHover;
    public Sprite hoverBlue;
    public Sprite clickBlue;
    public Sprite hoverYellow;
    public Sprite clickYellow;

    public PathwaySO pathwaySO;
    public Card dataSO;

    Image image;

    private void Start()
    {
        image = gameObject.GetComponent<Image>();
        Button button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(() => {
            AlertHighlightSystem(pathwaySO);
            OnClickColourChange(FindPathwayState(pathwaySO));
            UpdateUI();
            ButtonFactory.Instance.UpdateAllButtonOnClick();
        });

    }


    public void UpdateUI()
    {
        dataSO.Label = pathwaySO.Label;
        dataSO.QID = pathwaySO.QID;
        dataSO.Description = pathwaySO.Description;
        if (UIPresenter.UIList.PathwayUI != null)
            UIPresenter.Instance.NotifyUIUpdate(UIPresenter.UIList.PathwayUI, false);
        else Debug.Log("Error in calling PathwayUI list");
    }

    void AlertHighlightSystem(PathwaySO pathway)
    {
        HighlightService hs = HighlightService.Instance;
        hs.Highlight(pathway);
    }

    public HighlightPathway.HighlightState FindPathwayState(PathwaySO pathway)
    {
        return StatusController.Instance.PathwayCheckState(pathway);
    }

    public void OnHoverColourChange(HighlightPathway.HighlightState state)
    {
        switch (state)
        {
            case HighlightPathway.HighlightState.Default:
                image.sprite = hoverBlue;
                break;

            case HighlightPathway.HighlightState.Highlighted:
                image.sprite = hoverYellow;
                break;

            case HighlightPathway.HighlightState.Accented:
                image.sprite = defaultHover;
                break;
            default:
                break;
        }
    }

    public void OnHoverExitColourChange(HighlightPathway.HighlightState state)
    {
        switch (state)
        {
            case HighlightPathway.HighlightState.Default:
                image.sprite = defaultSprite;
                break;

            case HighlightPathway.HighlightState.Highlighted:
                image.sprite = clickBlue;
                break;

            case HighlightPathway.HighlightState.Accented:
                image.sprite = clickYellow;
                break;
            default:
                break;
        }
    }
    public void OnClickColourChange(HighlightPathway.HighlightState state)
    {
        switch (state)
        {
            case HighlightPathway.HighlightState.Default:
                image.sprite = defaultSprite;
                break;

            case HighlightPathway.HighlightState.Highlighted:
                image.sprite = clickBlue;
                break;

            case HighlightPathway.HighlightState.Accented:
                image.sprite = clickYellow;
                break;
            default:
                break;
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHoverColourChange(FindPathwayState(pathwaySO));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnHoverExitColourChange(FindPathwayState(pathwaySO));
    }
}
