using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectNodes : MonoBehaviour
{
    public Transform target;
    public Transform spawn;
    // Start is called before the first frame update
    void Start()
    {
        this.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, Vector3.Distance(spawn.position, target.position) / 2);

        transform.position = spawn.position;        // place bond here
        transform.LookAt(target);            // aim bond at atom
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
