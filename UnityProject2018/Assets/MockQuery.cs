using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockQuery : MonoBehaviour
{
    public List<EdgeSOBeta> edges;
    public List<NodeSOBeta> node;
    // Start is called before the first frame update
    void Start()
    {   
        //Glycogen sythanse pathway
        PathwaySOBeta glycogenSynthasePathway = ScriptableObject.CreateInstance<PathwaySOBeta>();
        glycogenSynthasePathway.LocalNetwork = new Dictionary<NodeSOBeta, List<EdgeSOBeta>>();
        // hexokinase , glucose -> glucose6phospahate 
        EdgeSOBeta hexokinase = ScriptableObject.CreateInstance<EdgeSOBeta>();
        hexokinase.Label = "hexokinase";

        NodeSOBeta glucose = ScriptableObject.CreateInstance<NodeSOBeta>();
        glucose.Label = "glucose";
        NodeSOBeta glucose6phosphate = ScriptableObject.CreateInstance<NodeSOBeta>();
        glucose6phosphate.Label = "glucose-6-phosphate";

        hexokinase.reactants = new List<NodeSOBeta>();
        hexokinase.reactants.Add(glucose);
        hexokinase.products = new List<NodeSOBeta>();
        hexokinase.products.Add(glucose6phosphate);

        glycogenSynthasePathway.LocalNetwork.Add(glucose, new List<EdgeSOBeta>{hexokinase});
        glycogenSynthasePathway.LocalNetwork.Add(glucose6phosphate, new List<EdgeSOBeta>{hexokinase});

        // Phosphoglucose mutase , glucose 6-phosphate => glucose 1-phosphate
        EdgeSOBeta phosphoglucoseMutase = ScriptableObject.CreateInstance<EdgeSOBeta>();
        phosphoglucoseMutase.Label = "phosphoglucose mutase";

        NodeSOBeta glucose1phosphate = ScriptableObject.CreateInstance<NodeSOBeta>();
        glucose1phosphate.Label = "glucose-1-phosphate";

        phosphoglucoseMutase.reactants = new List<NodeSOBeta>();
        phosphoglucoseMutase.reactants.Add(glucose6phosphate);
        phosphoglucoseMutase.products = new List<NodeSOBeta>();
        phosphoglucoseMutase.products.Add(glucose1phosphate);

        glycogenSynthasePathway.LocalNetwork[glucose6phosphate].Add(phosphoglucoseMutase);
        glycogenSynthasePathway.LocalNetwork.Add(glucose1phosphate, new List<EdgeSOBeta>{phosphoglucoseMutase});

        // UDP-glucose pyrophosphorylase , UTP + glucose 1-phosphate <=> UDP-glucose + PPi     --> bi directional!
        EdgeSOBeta UDPGlucosePyrophosphorylase = ScriptableObject.CreateInstance<EdgeSOBeta>();
        UDPGlucosePyrophosphorylase.Label = "UDP-glucose pyrophosphorylase";

        NodeSOBeta UDPglucose = ScriptableObject.CreateInstance<NodeSOBeta>();
        UDPglucose.Label = "UDP-glucose";

        UDPGlucosePyrophosphorylase.reactants = new List<NodeSOBeta>();
        UDPGlucosePyrophosphorylase.reactants.Add(glucose1phosphate);
        UDPGlucosePyrophosphorylase.products = new List<NodeSOBeta>();
        UDPGlucosePyrophosphorylase.products.Add(UDPglucose);
        UDPGlucosePyrophosphorylase.bidirectional = true;

        glycogenSynthasePathway.LocalNetwork[glucose1phosphate].Add(UDPGlucosePyrophosphorylase);
        glycogenSynthasePathway.LocalNetwork.Add(UDPglucose, new List<EdgeSOBeta>{UDPGlucosePyrophosphorylase});


        // glycogen synthase, glycogen (n residues) + UDP-glucose => UDP + glycogen (n+1 residues)
        EdgeSOBeta glycogenSynthase = ScriptableObject.CreateInstance<EdgeSOBeta>();
        glycogenSynthase.Label = "glycogenSynthase";

        NodeSOBeta glycogen_n1 = ScriptableObject.CreateInstance<NodeSOBeta>();
        glycogen_n1.Label = "glycogen(n+1)";
        NodeSOBeta glycogen_n = ScriptableObject.CreateInstance<NodeSOBeta>();
        glycogen_n.Label = "glycogen(n)";


        glycogenSynthase.reactants = new List<NodeSOBeta>();
        glycogenSynthase.reactants.Add(glycogen_n);
        glycogenSynthase.reactants.Add(UDPglucose);
        glycogenSynthase.products = new List<NodeSOBeta>();
        glycogenSynthase.products.Add(glycogen_n1);

        glycogenSynthasePathway.LocalNetwork.Add(glycogen_n1, new List<EdgeSOBeta>{glycogenSynthase});
        glycogenSynthasePathway.LocalNetwork.Add(glycogen_n, new List<EdgeSOBeta>{glycogenSynthase});
        glycogenSynthasePathway.LocalNetwork[UDPglucose].Add(glycogenSynthase);
        // glycogen phosphorylase, glycogen (n+1 residues) + Pi => glycogen (n residues) + glucose 1-phosphate  --> not in wikibase!, needs to be checked with a theory
        EdgeSOBeta glycogenPhosphorylase = ScriptableObject.CreateInstance<EdgeSOBeta>();
        glycogenPhosphorylase.Label = "glycogen Phosphorylase";

        glycogenPhosphorylase.reactants = new List<NodeSOBeta>();
        glycogenPhosphorylase.reactants.Add(glycogen_n1);
        glycogenPhosphorylase.products = new List<NodeSOBeta>();
        glycogenPhosphorylase.products.Add(glycogen_n);
        glycogenPhosphorylase.products.Add(glucose1phosphate);

        glycogenSynthasePathway.LocalNetwork[glycogen_n1].Add(glycogenPhosphorylase);
        glycogenSynthasePathway.LocalNetwork[glucose1phosphate].Add(glycogenPhosphorylase);        
        glycogenSynthasePathway.LocalNetwork[glycogen_n].Add(glycogenPhosphorylase);

        //find all edges that glucose interacts with
        FindChildren(glycogenSynthasePathway, UDPglucose);


    }

    

    void Search(PathwaySOBeta pathway, NodeSOBeta nodeRoot) {
       Queue<EdgeSOBeta> BFSQueue = new Queue<EdgeSOBeta>();
       

    }

    void FindChildren(PathwaySOBeta pathway, NodeSOBeta current)
    {

        List<EdgeSOBeta> interactedEdges = pathway.LocalNetwork[current];
        foreach (EdgeSOBeta currentEdge in interactedEdges)
        {
            if (currentEdge.bidirectional)
            {
                Debug.Log("BFS found bidirectional edge");
                //search both reactants and products, return the one where we dont find glucose (if found in the other one)
                if (currentEdge.reactants.Contains(current))
                {
                    Debug.Log("BFS " + currentEdge.Label + " " + currentEdge.products[0].Label);
                }
                else
                {
                    if (currentEdge.products.Contains(current))
                    {
                        Debug.Log("BFS " + currentEdge.Label + " " + currentEdge.reactants[0].Label);
                    }
                }
                // result is null
            }
            else
            {
                if (currentEdge.reactants.Contains(current))
                {
                    Debug.Log("BFS " + currentEdge.Label + " " + currentEdge.products[0].Label);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
