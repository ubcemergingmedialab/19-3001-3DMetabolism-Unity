using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenLink : MonoBehaviour
{
   public void OpenChannel() {
       Application.OpenURL("https://www.ncbi.nlm.nih.gov/pmc/articles/PMC6167041/");
   }
}
