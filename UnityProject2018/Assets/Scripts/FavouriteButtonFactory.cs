using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FavouriteButtonFactory : MonoBehaviour
{
    // singleton instantiation of ButtonFactory
    private static FavouriteButtonFactory _instance;
    public static FavouriteButtonFactory Instance
    {
        get { return _instance; }
    }

    // Pathway scriptable object obtained from StatusController,
    // Prefab for button instantiation
    public GameObject favButtonPrefab;
    public Dictionary<GameObject, PathwaySO> favButtons = new Dictionary<GameObject, PathwaySO>();
    public List<PathwaySO> favedPathways = new List<PathwaySO>();


    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;
    }

    public GameObject GenerateButtonAndSetPosition(float x, float y, PathwaySO pathway)
    {
        GameObject generated = Instantiate(favButtonPrefab, transform);
        generated.GetComponent<FavouriteButtonLogic>().pathwaySO = pathway;
        RectTransform rect = generated.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector3(x+100, y, 0);
        return generated;
    }

    public void addToFaved(PathwaySO so)
    {
        favedPathways.Add(so);
    }

    public void removeFromFaved(PathwaySO so)
    {
        favedPathways.Remove(so);
    }

    public bool pathwayIsFaved(PathwaySO so)
    {
        return favedPathways.Contains(so);
    }

}
