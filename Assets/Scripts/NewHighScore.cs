using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NewHighScore : MonoBehaviour {

    private HighScores highScores;
    private GameStateManager gsm;
    private InputField field;
    private MusicPlayer music;

    // Use this for initialization
    void Start() {
        highScores = FindObjectOfType<HighScores>();
        gsm = FindObjectOfType<GameStateManager>();
        field = FindObjectOfType<InputField>();
        music = FindObjectOfType<MusicPlayer>();

        //Set default text
        field.text = Environment.UserName;

        //play a tune
        music.PlayTune("R8 O4 F B- O5 D R3 F R8 D R2 F");

    }

    public void Submit() {
        string name = field.text;
        highScores.SetName(gsm.placeInHighScoreList, name);
        gsm.state = GameStateManager.State.POST_GAME_MODE_HIGH_SCORES;
    }
}