using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WikibaseBinding
{
    
    public WikibaseBindingElement prefixedEdge;
    public WikibaseBindingElement prefixedMetabolite;
    //public WikibaseBindingElement prefixedEnzyme;
    public WikibaseBindingElement prefixedPathway;
    
    public WikibaseBindingElement edgeLabel;
    public WikibaseBindingElement enzymeLabel;
    public WikibaseBindingElement metaboliteLabel;
    public WikibaseBindingElement pathwayLabel;
    
    public WikibaseBindingElement isBidirectional;
    public WikibaseBindingElement pathwayDesc;
    public WikibaseBindingElement edgeDesc;
    public WikibaseBindingElement edgeEnzymeTypeLabel;
    public WikibaseBindingElement edgeCofactorsLabel;
    public WikibaseBindingElement edgeEnergyReqLabel;
    public WikibaseBindingElement edgePubchem;
    public WikibaseBindingElement edgeRegulation;

    public WikibaseBindingElement metaboliteDesc;
    public WikibaseBindingElement metaboliteMoleFormula;
    public WikibaseBindingElement metaboliteIUPAC;
    public WikibaseBindingElement metaboliteStrucDesc;
    public WikibaseBindingElement metaboliteCharge;
    public WikibaseBindingElement metabolitePubchem;
    public WikibaseBindingElement metaboliteCID;

    public WikibaseBindingElement isReactant;
    public WikibaseBindingElement isProduct;
    public WikibaseBindingElement isEnzyme;

    public WikibaseBindingElement edgeQID;
    public WikibaseBindingElement metaboliteQID;
    public WikibaseBindingElement pathwayQID; 

    
}
