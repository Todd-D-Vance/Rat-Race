using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class AttractMode : MonoBehaviour {

    public float timeRemaining = 10.0f;
    public string nextScene = "Description";


    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0) {
            SceneManager.LoadScene(nextScene);
        }
    }

    public void PlayGame() {
        SceneManager.LoadScene("Game");
    }
}