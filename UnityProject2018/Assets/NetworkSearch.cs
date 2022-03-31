using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkSearch : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    Dictionary<int,List<ScriptableObject>> SearchForPath(PathwaySO pathway, NodeSO nodeRoot, NodeSO nodeToFind) {

        int pathNum = 1;
        Dictionary<int,List<ScriptableObject>> CorrectPaths = new Dictionary<int,List<ScriptableObject>>();

        Queue<List<ScriptableObject>> BFSQueue = new Queue<List<ScriptableObject>>();
        Dictionary<string,bool> visited = new Dictionary<string, bool>();

        BFSQueue.Enqueue(new List<ScriptableObject>(){nodeRoot});
        visited.Add(nodeRoot.Label,true);

        while (BFSQueue.Count > 0) {
            List<ScriptableObject> currentPath = BFSQueue.Dequeue();
            NodeSO currentNode = (NodeSO) currentPath[currentPath.Count - 1];
            if (currentNode.Label == nodeToFind.Label){
                
                CorrectPaths.Add(pathNum,currentPath);
                pathNum++;
                //Debug.Log("found the node : " + currentNode.Label + " = " + nodeToFind.Label);
                //return currentPath;
            }

            Dictionary<EdgeSO,List<NodeSO>> children = FindChildren(pathway,visited,currentNode);
                
                foreach(KeyValuePair <EdgeSO, List<NodeSO>> entry in children) {
                    

                    foreach (NodeSO curr in entry.Value){
                        List<ScriptableObject> newPath = new List<ScriptableObject>(currentPath);
                        newPath.Add(entry.Key);
                        newPath.Add(curr);

                        if (visited.ContainsKey(curr.Label)){
                        continue;
                        }
                        if (curr.Label != nodeToFind.Label){
                            visited.Add(curr.Label,true);
                        }
                        BFSQueue.Enqueue(newPath);
                    }
                    
                }

       }

       if(CorrectPaths.Count == 0){
           Debug.Log("no paths found !");
       }
       return CorrectPaths;
    }

    Dictionary<EdgeSO,List<NodeSO>> FindChildren(PathwaySO pathway,Dictionary<string,bool> visited, NodeSO current)
    {
        
        Dictionary<EdgeSO,List<NodeSO>> nodesByEdge = new Dictionary<EdgeSO,List<NodeSO>>();
        List<EdgeSO> interactedEdges = pathway.LocalNetwork[current];
        foreach (EdgeSO currentEdge in interactedEdges){

            nodesByEdge.Add(currentEdge, new List<NodeSO>());
            if (currentEdge.bidirectional){
                Debug.Log("BFS found bidirectional edge");
                //search products and reactants, return the one where we dont find node in

                if (currentEdge.products.Contains(current)){

                    foreach (NodeSO node in currentEdge.reactants){
                        nodesByEdge[currentEdge].Add(node);
                    }
                } else {
                    if (currentEdge.reactants.Contains(current)){

                        foreach (NodeSO node in currentEdge.products){
                                nodesByEdge[currentEdge].Add(node);
                        }
                    }
                }
            } else {
                if (currentEdge.reactants.Contains(current)){

                    foreach (NodeSO node in currentEdge.products){
                            nodesByEdge[currentEdge].Add(node);
                    }
                }
            }
        }
        return nodesByEdge;
    }

    public void BFSTest(PathwaySO pathway, NodeSO start, NodeSO end) {
        Dictionary<int,List<ScriptableObject>> result =  SearchForPath(pathway,start,end);

        string printResult = "<BFS> search in " + pathway.name + "from node:" + start.Label +  " - end node:" + end.Label;

        foreach(KeyValuePair<int,List<ScriptableObject>> path in result){
        int n = 1;
        Debug.Log("<BFS> count =" + result.Count);
        printResult += "\npath number : " + path.Key;
            foreach (ScriptableObject step in path.Value){
                printResult += "\n" + n + " - " + step.name; //GetType().ToString();
                n++;   
            }
        }
        Debug.Log(printResult);
    }


// 
//     foreach(NodeSO nodeSO in pathwaySO.nodes) {                                                             // For every node in this pathway
//                 GameObject[] nodes = GameObject.FindGameObjectsWithTag(nodeSO.name);

//                 foreach(GameObject node in nodes) {
//                     if(node != null) {
//                         HighlightHandler hl = node.GetComponent<HighlightHandler>();                                // find the nodes handler
//                         List<HighlightPathway> sharingPathways;                                                     

//                         if(elementToPathways.TryGetValue(hl, out sharingPathways)) {                                // Find node in elementToPathways
//                             sharingPathways.Add(highlightPathway);                                                  // add the highlightPathway to the element's list of hpw
//                         } else {
//                             elementToPathways.Add(hl, new List<HighlightPathway>{highlightPathway});                // Make a new list if node is not yet in the Dictionary 
//                         }
//                     }
//                 }
//             }
// 
}
