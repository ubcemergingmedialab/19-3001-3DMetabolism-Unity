using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableAnimationDescription : MonoBehaviour
{
    private AnimationLoader animationLoader;
    public string animationName;
    // Start is called before the first frame update
    void Start()
    {
        animationLoader = GameObject.Find("DefaultPathway").GetComponent<AnimationLoader>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SendAnimationMessage()
    {
        GameObject.Find("Sending Message");
        animationLoader.LoadAnimation(animationName, int.Parse(name));
    }

    public void SetInfo(string aName, int aNumber)
    {
        animationName = aName;
        this.name = "" + aNumber;
    }
}
