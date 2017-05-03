using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edible : MonoBehaviour {

    private SoundPlayer sound = SoundPlayer.instance;
    private Score score;
    private Recorder recorder = Recorder.instance;
    private GameStateManager gsm = GameStateManager.instance;

    // Use this for initialization
    void Start() {
        foreach (Score s in FindObjectsOfType<Score>()) {
            if (s.gameObject.name == "Score") {
                score = s;
            }
        }
        if (!score) {
            throw new UnityException("Score object not found");
        }
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            //update recording
            if (gsm.framesInState > 7 && gsm.recordThisGame && gsm.state == GameStateManager.State.GAME_MODE_PLAY) {
                recorder.RecordIfRecording(recorder.DestroyCommand(gameObject));
            }
            if (gameObject.tag == "Growth") {
                //make cats flee
                foreach (AIController enemy in FindObjectsOfType<AIController>()) {
                    enemy.flee += 10;
                }
                score.Add(100);
            } else {
                score.Add(10);
            }
            sound.Gulp();
            GameObject.Destroy(gameObject, .1f);//give it 1/10 second to be swallowed
        }
    }
}
