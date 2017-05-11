using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edible : MonoBehaviour {

    private SoundPlayer sound = SoundPlayer.instance;
    private GameStateManager gsm = GameStateManager.instance;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player" && gsm.state==GameStateManager.State.GAME_MODE_PLAY) {
            if (gameObject.tag == "Growth") {
                //make cats flee
                foreach (AIController enemy in FindObjectsOfType<AIController>()) {
                    enemy.flee += 10;
                }
            }
            sound.Gulp();
            GameObject.Destroy(gameObject, .1f);//give it 1/10 second to be swallowed
        }
    }
}
