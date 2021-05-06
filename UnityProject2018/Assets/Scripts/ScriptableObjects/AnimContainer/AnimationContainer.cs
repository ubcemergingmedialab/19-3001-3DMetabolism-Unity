using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAnimationContainer", menuName = "Animation Container", order = 51)]
public class AnimationContainer : ScriptableObject
{
    public List<AnimationClip> Animations;
}
