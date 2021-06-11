using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationLoader : MonoBehaviour
{
    private Dictionary<string, List<AnimationDescription>> cachedAnimations;
    private AnimationControllerComponent controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<AnimationControllerComponent>();
        cachedAnimations = new Dictionary<string, List<AnimationDescription>>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadAnimation(string animationName)
    {
        List<AnimationDescription> swap;
        if (cachedAnimations.TryGetValue(animationName, out swap))
        {
            StartCoroutine(controller.ChangeAnimation(swap));
        } else
        {
            AnimationDescription[] box = Resources.LoadAll<AnimationDescription>("AnimDescription/" + animationName + "/");
            Array.Sort(box, delegate (AnimationDescription x, AnimationDescription y) { return int.Parse(x.name).CompareTo(int.Parse(y.name)); });
            swap = new List<AnimationDescription>(box);
            StartCoroutine(controller.ChangeAnimation(swap));
        }
    }
}
