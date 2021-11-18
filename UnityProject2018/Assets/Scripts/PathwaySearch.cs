using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PathwaySearch : MonoBehaviour
{

    public GameObject SearchInput;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PrintPathwaySearch() {
        List<PathwaySO> results = StatusController.Instance.activePathways;
        string input = SearchInput.GetComponent<TMP_InputField>().text;
        if (input == "") {return;}
        foreach (PathwaySO pw in results){

            if (pw.Label.Contains(input)){
                Debug.Log(""+ pw.Label);
                //break;
            } else{
                continue;
            }
        }
    }
}
