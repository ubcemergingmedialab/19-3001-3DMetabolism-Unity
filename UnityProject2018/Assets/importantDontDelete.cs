using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class importantDontDelete : MonoBehaviour
{
    private int state = 0;
    public bool konamiMode = false;
    public GameObject camNotMario;
    public GameObject camIndeedMario;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            print("up key was pressed");
            if (state == 0 || state == 1)
                state += 1;
            else {
                state = 0;
            }
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow)) {
            if (state == 2 || state == 3) {
                state += 1;
            }
            else {
                state = 0;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            if (state == 4 || state == 6) {
                state += 1;
            }
            else {
                state = 0;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            if (state == 5 || state == 7) {
                state += 1;
            }
            else {
                state = 0;
            }
        }
        else if(Input.GetKeyDown(KeyCode.B)) {
            if (state == 8) {
                state += 1;
            }
            else {
                state = 0;
            }
        }
        else if(Input.GetKeyDown(KeyCode.A)) {
            if (state == 9) {
                state += 1;
            }
            else {
                state = 0;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Return)) {
            if (state == 10) {
                state = 0;
                this.konamiMode = true;
            }
        }
        if (this.konamiMode) {
            camNotMario.SetActive(false);
            camIndeedMario.SetActive(true);
        }
    }
}
