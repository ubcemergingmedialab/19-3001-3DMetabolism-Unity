using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationDescriptionPresenter : MonoBehaviour
{
    protected int adCount;
    public Color highlightColor;
    public Color defaultColor;
    public GameObject elementPrefab;
    public List<GameObject> adList;
    // Start is called before the first frame update
    void Start()
    {
        if(elementPrefab == null)
        {
            Debug.LogError("AnimationDescriptionPresenter needs element");
        }
        adList = new List<GameObject>();
        adCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void AddAnimationDescription(string animationName, int stepNumber)
    {
        Debug.Log("adding ad element");
        GameObject newElement = Instantiate(elementPrefab, new Vector3(transform.position.x, transform.position.y -(float)adCount, transform.position.z), transform.rotation, transform);
        newElement.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
        newElement.name = "" + stepNumber;
        newElement.transform.Find("Text").GetComponent<Text>().text = "step " + stepNumber;
        newElement.GetComponent<ClickableAnimationDescription>().SetInfo(animationName, stepNumber);
        adList.Add(newElement);
        adCount = adCount + 1;
    }

    public virtual void AddAnimationDescription(string animationName, List<AnimationDescription> lst)
    {
        Debug.Log("adding ad element");
        lst.ForEach(delegate (AnimationDescription desc)
        {
            AddAnimationDescription(animationName, int.Parse(desc.name));
        });
    }


    public virtual void ClearAnimationDescriptions()
    {
        adCount = 0;
        adList.ForEach(delegate (GameObject go)
        {
            Destroy(go);
        });
        adList.Clear();
    }

    public virtual void HighlightStep(int step)
    {
        adList.ForEach(delegate (GameObject el)
        {
            Material tempMat = el.GetComponent<Material>();
            if (tempMat != null)
            {
                tempMat.color = defaultColor;
            }
        });
        Material mat = adList.Find(delegate (GameObject el)
        {
            return el.name == "" + step;
        }).GetComponent<Material>();
        if (mat != null)
        {
            mat.color = highlightColor;
        }
    }

}
