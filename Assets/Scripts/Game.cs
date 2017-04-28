using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Game : MonoBehaviour {

    public State state = State.INTRO;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (state == State.PLAY && CountDots() <= 0) {
            state = State.END_LEVEL;
            SceneManager.LoadScene("Win");
        }
    }

    int CountDots() {
        Dots dots = FindObjectOfType<Dots>();
        if (!dots) {
            return 0;
        }
        int count = 0;
        foreach (Transform dot in dots.transform) {
            count++;
        }
        return count;
    }

    public enum State {
        INTRO,
        PLAY,
        START_LEVEL,
        DEATH,
        GAME_OVER,
        RESET_PLAYER,
        END_LEVEL,
        LEVEL_UP,
        CUTSCENE,
        OPTIONS
    }
}
