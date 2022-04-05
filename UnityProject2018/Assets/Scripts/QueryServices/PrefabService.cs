using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PrefabService : MonoBehaviour
{
string ResourceFolderPath = "Assets/Resources/Data/QuerySO/";
    public void PrefabAssignment(){
        List<PathwaySO> pathways = GameObject.Find("StatusController").GetComponent<StatusController>().activePathways;
        
        
    }
}
