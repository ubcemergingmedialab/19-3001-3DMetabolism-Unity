using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchResultButtonFactory : MonoBehaviour
{
    public GameObject resultButtonPrefab;

    public GameObject GenerateButton()
    {
        GameObject generated = Instantiate(resultButtonPrefab);

        return generated;
    }

}
