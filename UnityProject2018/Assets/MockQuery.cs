using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockQuery : MonoBehaviour
{
    public PathwaySOBeta Pathway;
    public List<EdgeSOBeta> edges;
    public List<NodeSOBeta> node;
    // Start is called before the first frame update
    void Start()
    {   //Glycogen sythanse pathway
        Pathway = PathwaySOBeta.CreateInstance<PathwaySOBeta>();

        // hexokinase , glucose -> glucose6phospahate 
        EdgeSOBeta hexokinase = EdgeSOBeta.CreateInstance<EdgeSOBeta>();
        hexokinase.Label = "hexokinase";

        NodeSOBeta glucose = NodeSOBeta.CreateInstance<NodeSOBeta>();
        glucose.Label = "glucose";
        NodeSOBeta glucose6phosphate = NodeSOBeta.CreateInstance<NodeSOBeta>();
        glucose6phosphate.Label = "glucose-6-phosphate";

        hexokinase.reactants.Add(glucose);
        hexokinase.products.Add(glucose6phosphate);

        Pathway.LocalNetwork.Add(glucose, new List<EdgeSOBeta>{hexokinase});
        Pathway.LocalNetwork.Add(glucose6phosphate, new List<EdgeSOBeta>{hexokinase});

        // Phosphoglucose mutase , glucose 6-phosphate => glucose 1-phosphate
        EdgeSOBeta phosphoglucoseMutase = EdgeSOBeta.CreateInstance<EdgeSOBeta>();
        phosphoglucoseMutase.Label = "phosphoglucose mutase";

        NodeSOBeta glucose1phosphate = NodeSOBeta.CreateInstance<NodeSOBeta>();
        glucose1phosphate.Label = "glucose-1-phosphate";

        phosphoglucoseMutase.reactants.Add(glucose6phosphate);
        phosphoglucoseMutase.products.Add(glucose1phosphate);

        Pathway.LocalNetwork[glucose6phosphate].Add(phosphoglucoseMutase);
        Pathway.LocalNetwork.Add(glucose1phosphate, new List<EdgeSOBeta>{phosphoglucoseMutase});

        // UDP-glucose pyrophosphorylase , UTP + glucose 1-phosphate <=> UDP-glucose + PPi     --> bi directional!
        EdgeSOBeta UDPGlucosePyrophosphorylase = EdgeSOBeta.CreateInstance<EdgeSOBeta>();
        UDPGlucosePyrophosphorylase.Label = "UDP-glucose pyrophosphorylase";

        NodeSOBeta UDPglucose = NodeSOBeta.CreateInstance<NodeSOBeta>();
        UDPglucose.Label = "UDP-glucose";

        UDPGlucosePyrophosphorylase.reactants.Add(glucose1phosphate);
        UDPGlucosePyrophosphorylase.products.Add(UDPglucose);
        UDPGlucosePyrophosphorylase.bidrectional = true;

        Pathway.LocalNetwork[glucose1phosphate].Add(UDPGlucosePyrophosphorylase);
        Pathway.LocalNetwork.Add(UDPglucose, new List<EdgeSOBeta>{UDPGlucosePyrophosphorylase});


        // glycogen synthase, glycogen (n residues) + UDP-glucose => UDP + glycogen (n+1 residues)
        EdgeSOBeta glycogenSynthase = EdgeSOBeta.CreateInstance<EdgeSOBeta>();
        glycogenSynthase.Label = "glycogenSynthase";

        NodeSOBeta glycogen_n1 = NodeSOBeta.CreateInstance<NodeSOBeta>();
        glycogen_n1.Label = "glycogen(n+1)";
        NodeSOBeta glycogen_n = NodeSOBeta.CreateInstance<NodeSOBeta>();
        glycogen_n.Label = "glycogen(n)";

        glycogenSynthase.reactants.Add(glycogen_n);
        glycogenSynthase.reactants.Add(UDPglucose);
        glycogenSynthase.products.Add(glycogen_n1);

        Pathway.LocalNetwork.Add(glycogen_n1, new List<EdgeSOBeta>{glycogenSynthase});
        Pathway.LocalNetwork.Add(glycogen_n, new List<EdgeSOBeta>{glycogenSynthase});
        Pathway.LocalNetwork[UDPglucose].Add(glycogenSynthase);
        // glycogen phosphorylase, glycogen (n+1 residues) + Pi => glycogen (n residues) + glucose 1-phosphate  --> not in wikibase!, needs to be checked with a theory
        EdgeSOBeta glycogenPhosphorylase = EdgeSOBeta.CreateInstance<EdgeSOBeta>();
        glycogenPhosphorylase.Label = "glycogen Phosphorylase";

        glycogenPhosphorylase.reactants.Add(glycogen_n1);
        glycogenPhosphorylase.products.Add(glycogen_n);
        glycogenPhosphorylase.products.Add(glucose1phosphate);

        Pathway.LocalNetwork[glycogen_n1].Add(glycogenPhosphorylase);
        Pathway.LocalNetwork[glucose1phosphate].Add(glycogenPhosphorylase);        
        Pathway.LocalNetwork[glycogen_n].Add(glycogenPhosphorylase);
        


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
