using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AttractMode : MonoBehaviour {

    public float timeRemaining = 10.0f;
    public string nextScene = "Description";

    private GameStateManager gsm = GameStateManager.instance;

    public void PlayGame() {
        SceneManager.LoadScene("Game");
        gsm.state = GameStateManager.State.GAME_MODE_INTRO;
    }

    public void Options() {
        if (gsm.state == GameStateManager.State.POST_GAME_MODE_HIGH_SCORES || gsm.state == GameStateManager.State.POST_GAME_MODE_NEW_HIGH_SCORE) {
            gsm.state = GameStateManager.State.POST_GAME_MODE_OPTIONS;
        } else {
            gsm.state = GameStateManager.State.ATTRACT_MODE_OPTIONS;
        }
    }
}