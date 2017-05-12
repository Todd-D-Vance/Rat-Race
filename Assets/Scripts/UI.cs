using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour {

    private GameStateManager gsm = GameStateManager.instance;
    private MusicPlayer music = MusicPlayer.instance;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Options() {
        if (gsm.state == GameStateManager.State.POST_GAME_MODE_HIGH_SCORES || gsm.state == GameStateManager.State.POST_GAME_MODE_NEW_HIGH_SCORE) {
            gsm.state = GameStateManager.State.POST_GAME_MODE_OPTIONS;
        } else {
            gsm.state = GameStateManager.State.GAME_MODE_OPTIONS;
        }
        music.paused = true;
    }
}
