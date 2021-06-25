using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAnimationDescription", menuName = "Animation Description", order = 51)]
public class AnimationDescription : ScriptableObject
{
    // Assuming that the index of the object and their animations are the same
    public List<string> AnimatedObjects;
    public List<string> TriggerToSet;
}
