using UnityEngine;
using System.Collections;

public enum searchCategory { standard, mitochondrion, cytosol}

public class GenericSO : ScriptableObject
{
    public string QID;
    public string Label;
    public string Description;
    public searchCategory searchCategory = searchCategory.standard;

}
