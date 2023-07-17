using UnityEngine;
using System.Collections;


// Class representing the cofactos within a reaction. made as a data type for a field in EdgeSO
public class Cofactor
{
    public string label;
    public bool isReactant; // if false -> the cofactor is a product

    public Cofactor(string _label,bool _isReactant)
    {
        label = _label;
        isReactant = _isReactant;
    }
}
