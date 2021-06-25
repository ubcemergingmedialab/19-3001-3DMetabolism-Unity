using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnValueChangedSlider : MonoBehaviour
{
    public AnimationLoader animationLoader;
    public AnimationDescriptionPresenter presenter;
    private string animationName;
    private float currentValue;
    private bool ignoreChanges;
    private void Start()
    {
        animationName = "";
        ignoreChanges = false;
    }

    public void OnValueChanged(float value)
    {
        if(animationName != "" && animationLoader != null)
        {
            if (!ignoreChanges)
            {
                Debug.Log("setting animation " + (int)(value * presenter.adList.Count) + 1);
                animationLoader.LoadAnimation(animationName, (int)(value * presenter.adList.Count) + 1);
            }
        }
    }

    public void ChangeAnimationName(string animation)
    {
        animationName = animation;
        
    }

    public void ChangeSliderIgnore(float value)
    {
        ignoreChanges = true;
        GetComponent<Slider>().value = value;
        ignoreChanges = false;
    }
}