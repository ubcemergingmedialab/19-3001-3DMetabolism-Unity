using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenLink : MonoBehaviour
{

    public string url = "https://www.genome.jp/kegg/";
   public void OpenChannel() {
       Application.OpenURL(url);
   }
}
