using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSceneData", menuName = "Scene Data", order = 51)]
public class SceneData : ScriptableObject {
    public HashSet<Animator> nodes;
    public HashSet<Animator> edges;
    public string[] nodeAnimations;
    public string[] edgeAnimations;

}
