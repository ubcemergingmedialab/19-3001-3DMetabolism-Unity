using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenLink : MonoBehaviour
{

    public string url = "https://www.ncbi.nlm.nih.gov/pmc/articles/PMC6167041/";
   public void OpenChannel() {
       Application.OpenURL(url);
   }
}
