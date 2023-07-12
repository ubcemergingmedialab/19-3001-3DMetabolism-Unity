using UnityEngine;
using System.Collections;


// Class representing the cofactos within a reaction. made as a data type for a field in EdgeSO
public class Cofactor
{
    private string _label;
    private bool _isReactant; // if false -> the cofactor is a product

    Cofactor(string label,bool isReactant)
    {
        _label = label;
        _isReactant = isReactant;
    }
}
