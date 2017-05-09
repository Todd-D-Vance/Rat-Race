using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelUp : MonoBehaviour {

    // Use this for initialization
    void Start() {
        Invoke("NewLevel", 1.0f);
    }

    // Update is called once per frame
    void Update() {

    }

    void NewLevel() {
        SceneManager.LoadScene("Game");
    }
}