using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Opens the link displayed in the Sidecard in the user's default browser
/// </summary>
public class OpenLink : MonoBehaviour
{

    public string url = "https://www.genome.jp/kegg/";

/// <summary>
/// opens the link stored in url
/// </summary>
   public void OpenChannel() {
       Application.OpenURL(url);
   }
}
