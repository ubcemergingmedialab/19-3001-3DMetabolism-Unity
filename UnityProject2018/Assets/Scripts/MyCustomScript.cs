using UnityEngine;
using System.Collections;
namespace MyScripts
{
    public class SomeClass
    {
        public static string Layer = "MyInitialLayerName";
    }
    public class MyCustomScript : MonoBehaviour
    {
        public string layer = SomeClass.Layer;
    }
}