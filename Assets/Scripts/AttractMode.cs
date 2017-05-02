using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class AttractMode : MonoBehaviour {

    private GameStateManager gsm = GameStateManager.instance;

    public void PlayGame() {
        SceneManager.LoadScene("Game");
        gsm.state = GameStateManager.State.GAME_MODE_INTRO;
    }

    public void Options() {
        gsm.state = GameStateManager.State.ATTRACT_MODE_OPTIONS;
    }
}