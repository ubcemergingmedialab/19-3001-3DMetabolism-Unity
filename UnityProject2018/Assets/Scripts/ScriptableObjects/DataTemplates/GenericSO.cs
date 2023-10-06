using UnityEngine;
using System.Collections;

public enum searchCategory { standard, mitochondrion, cytosol}

public class GenericSO : ScriptableObject
{
    public string QID;
    public string Label;
    public string Description;
    public searchCategory searchCategory = searchCategory.standard;

    protected string ReplaceMissingCharacters(string source)
    {
        return source.Replace("₀", "<sub>0</sub>")
                     .Replace("₁", "<sub>1</sub>")
                     .Replace("₂", "<sub>2</sub>")
                     .Replace("₃", "<sub>3</sub>")
                     .Replace("₄", "<sub>4</sub>")
                     .Replace("₅", "<sub>5</sub>")
                     .Replace("₆", "<sub>6</sub>")
                     .Replace("₇", "<sub>7</sub>")
                     .Replace("₈", "<sub>8</sub>")
                     .Replace("₉", "<sub>9</sub>")
                     .Replace("₋", "<sub>-</sub>")
                     .Replace("₊", "<sub>+</sub>")
                     .Replace("₍", "<sub>(</sub>")
                     .Replace("₎", "<sub>)</sub>")

                     .Replace("⁰", "<sup>0</sup>")
                     .Replace("¹", "<sup>1</sup>")
                     .Replace("²", "<sup>2</sup>")
                     .Replace("³", "<sup>3</sup>")
                     .Replace("⁴", "<sup>4</sup>")
                     .Replace("⁵", "<sup>5</sup>")
                     .Replace("⁶", "<sup>6</sup>")
                     .Replace("⁷", "<sup>7</sup>")
                     .Replace("⁸", "<sup>8</sup>")
                     .Replace("⁹", "<sup>9</sup>")
                     .Replace("⁻", "<sup>-</sup>")
                     .Replace("⁺", "<sup>+</sup>")
                     .Replace("⁽", "<sup>(</sup>")
                     .Replace("⁾", "<sup>)</sup>");
    }

}


