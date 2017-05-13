using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheat : MonoBehaviour {

    public bool cheating = false;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.F1)) {
            cheating = !cheating;//toggle cheat mode
        }

        if (cheating) {
            if (Input.GetKeyDown(KeyCode.D)) { //Destroy all dots
                cheating = false;
                foreach (Edible dot in FindObjectsOfType<Edible>()) {
                    Destroy(dot.gameObject);
                }
            } else if (Input.GetKeyDown(KeyCode.C)) { //Deploy all cats
                cheating = false;
                foreach (AIController cat in FindObjectsOfType<AIController>()) {
                        cat.delay = 0;                    
                }
            } else if (Input.GetKeyDown(KeyCode.R)) { //Reset all cats
                cheating = false;
                foreach (AIController cat in FindObjectsOfType<AIController>()) {
                    cat.ResetEnemy();
                }
            } else if (Input.GetKeyDown(KeyCode.S)) { //add to score
                cheating = false;
                foreach (Score s in FindObjectsOfType<Score>()) {
                    if (s.gameObject.name == "Score") {
                        s.Add(1000);
                    }
                }
            } else if (Input.GetKeyDown(KeyCode.L)) {
                //add a life
                cheating = false;
                GameStateManager.instance.numberOfLivesRemaining++;
            }


        }

    }
}