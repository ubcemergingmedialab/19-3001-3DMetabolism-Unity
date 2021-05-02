using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationMediator : MonoBehaviour
{
    public delegate void Play();
    public static event Play PlayEvent;

    public delegate void Pause();
    public static event Pause PauseEvent;

    private AnimationColleague animationColleague;

    // make this a Singleton
    private static AnimationMediator _instance;
    public static AnimationMediator Instance
    {
        get {
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPause()
    {
        PauseEvent();
    }

    public void OnPlay()
    {
        PlayEvent();
    }
}
