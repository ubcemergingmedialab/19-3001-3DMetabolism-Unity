using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonFactory : MonoBehaviour
{
    public List<PathwaySO> pathways;
    public GameObject buttonPrefab;

    // Start is called before the first frame update
    public float buttonSpacing;
    private int buttonCounter;


 
    void Start()
    {
        foreach(PathwaySO pathway in pathways) {
            GenerateButton(pathway);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    GameObject GenerateButton(PathwaySO pathway) {
        GameObject generated = Instantiate(buttonPrefab, transform);
        generated.GetComponent<HighlightPathway>().pathwayToHighlight = pathway;
        generated.GetComponent<PathwayUIOnClick>().so = pathway;
        generated.GetComponent<Button>().onClick.AddListener(delegate {Debug.Log(pathway.name);});
        return generated;
    }

}