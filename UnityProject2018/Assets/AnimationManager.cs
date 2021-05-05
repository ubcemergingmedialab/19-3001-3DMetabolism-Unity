using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    // TODO: change animation 1, animation 2 to be played from `DefaultPathway` and edit animation 3 name
    private string[] gameObjectNames = {"animation_1", "animation_2", "DefaultPathway"}; // DefaultPathway is animation 3
    private Hashtable gameObjectNamesToAnimationColleagues = new Hashtable(); 
   
    // Start is called before the first frame update
    void Start()
    {
        // sets up Hashtable of << GO name : AnimationColleague >>
        // creates an AnimationColleague for each GO and sets the correct animator
        for (int i = 0; i < this.gameObjectNames.Length; i++) {
            string currName = this.gameObjectNames[i];
            GameObject currObject = GameObject.Find(currName);
            AnimationColleague newColleague = new AnimationColleague();
            // set the colleague's reference to the animator of the current game object
            newColleague.setAnimator(currObject.GetComponent<Animator>());
            this.gameObjectNamesToAnimationColleagues[currName] = newColleague;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Calls `Enable` on the current GO Animation Colleague
    // Calls `Disable` on all other Animation Collagues
    public void SetCurrentAnimation(string gameObjectName)
    {
        for (int i = 0; i < this.gameObjectNames.Length; i++) {
            string currName =  this.gameObjectNames[i];
            AnimationColleague colleague = (AnimationColleague) this.gameObjectNamesToAnimationColleagues[currName];
            if (!string.Equals(gameObjectName, currName)) {
                colleague.Disable();
            } else {
                colleague.Enable();
            }
        }
    }
}
