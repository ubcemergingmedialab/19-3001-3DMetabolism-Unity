using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;

public class DistributeDataHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public static string path = "Assets/Resources/Data/query.xml";
    public List<NodeSO> nodes;
    void Start()
    {
        LoadXML();       
    } 

    private void LoadXML ()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(path);

        //the name of the object
        XmlNodeList elem = doc.GetElementsByTagName("literal");
        List<string> node_names = new List<string>();
        foreach (XmlNode node in elem)
            node_names.Add(node.InnerText);

        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].Label = node_names[i];
        }
    }
}
