using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPresenter : MonoBehaviour
{
    public enum UIList
    {
        NodeUI,
        EdgeUI
    }

    public UIElement NodeUIElement;
    public UIElement EdgeUIElement;
    public GameObject Panel;

    private static UIPresenter _instance;
    public static UIPresenter Instance
    {
        get { return _instance; }
    }

    private static Dictionary<UIList, UIElement> AvailableElements;

    // Start is called before the first frame update
    void Start()
    {
        AvailableElements = new Dictionary<UIList, UIElement>();
        if(NodeUIElement != null)
        {
            AvailableElements.Add(UIList.NodeUI, NodeUIElement);
        }
        if(EdgeUIElement != null)
        {
            AvailableElements.Add(UIList.EdgeUI, EdgeUIElement);
        }
    }

    public void NotifyUIUpdate(UIList el)
    {
        UIElement element;
        if(AvailableElements.TryGetValue(el, out element))
        {
            element.UpdateUI();
            OpenPanel();
        }
    }

    public void OpenPanel()
    {
        if (Panel != null)
            Panel.SetActive(true);

    }
    public void ClosePanel()
    {
        if (Panel != null)
            Panel.SetActive(false);

    }

}
