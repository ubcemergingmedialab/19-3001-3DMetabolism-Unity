using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationControl : MonoBehaviour
{
    private AnimationMediator mediator;

    
    void Awake()
    {
        this.mediator = AnimationMediator.Instance;
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Pause()
    {
        this.mediator.OnPause();
    }

    public void Resume()
    {
        this.mediator.OnPlay();
    }
}
