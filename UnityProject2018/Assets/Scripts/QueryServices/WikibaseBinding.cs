using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WikibaseBinding
{
    public WikibaseBindingElement prefixedEdge;
    public WikibaseBindingElement prefixedMetabolite;
    public WikibaseBindingElement prefixedEnzyme;
    public WikibaseBindingElement edgeLabel;
    public WikibaseBindingElement metaboliteLabel;
    public WikibaseBindingElement enzymeLabel;
    public WikibaseBindingElement isReactant;
    public WikibaseBindingElement isProduct;
    public WikibaseBindingElement edgeQID;
    public WikibaseBindingElement metaboliteQID;
    public WikibaseBindingElement enzymeQID;

    public WikibaseBindingElement isBidirectional;
}
