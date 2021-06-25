using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationDescriptionSliderPresenter : AnimationDescriptionPresenter
{
    public float sliderLength = 1;
    public Slider sliderComponent;
    private float markerDistance;
    void Start()
    {
        if (elementPrefab == null)
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

    public override void AddAnimationDescription(string animationName, List<AnimationDescription> lst)
    {
        markerDistance = sliderLength/lst.Count; //could also set adCount and perhaps adList here instead of adding one by one in AddAnimtionDescription(string, int)
        lst.ForEach(delegate (AnimationDescription desc)
        {
            AddAnimationDescription(animationName, int.Parse(desc.name));
        });
    }

    public override void AddAnimationDescription(string animationName, int stepNumber)
    {
        GameObject newElement = Instantiate(elementPrefab, new Vector3(transform.position.x - (markerDistance * stepNumber), transform.position.y, transform.position.z), transform.rotation, transform);
        newElement.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        newElement.name = "" + stepNumber;
        adList.Add(newElement);
        adCount = adCount + 1;
    }

    public override void HighlightStep(int step)
    {
        if(adList.Count > 0)
        {
            adList.ForEach(delegate (GameObject e)
            {
                GameObject el = e.transform.Find("Marker").gameObject;
                Material tempMat = el.GetComponent<Renderer>().material;
                if (tempMat != null)
                {
                    tempMat.SetColor("_Color", defaultColor);
                }
            });
            Material mat = adList.Find(delegate (GameObject e)
            {
                return e.name.Equals("" + step);
            }).transform.Find("Marker").gameObject.GetComponent<Renderer>().material;
            if (mat != null)
            {
                mat.SetColor("_Color", highlightColor);
            }
            if (sliderComponent != null)
            {
                if (sliderComponent.value != (float)step / adList.Count) {
                    sliderComponent.GetComponent<OnValueChangedSlider>().ChangeSliderIgnore((float)step / adList.Count);
                }
            }
        }
    }
}
