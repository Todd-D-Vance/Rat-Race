﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Options : MonoBehaviour {

    public Slider sfxVolumeControl;
    public Slider musicVolumeControl;

    private Preferences preferences = Preferences.instance;
    private GameStateManager gsm = GameStateManager.instance;

    // Use this for initialization
    void Start() {
        musicVolumeControl.value = preferences.musicVolume;
        sfxVolumeControl.value = preferences.sfxVolume;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Cancel")) {
            Return();
        }
    }

    public void Return() {
        SceneManager.UnloadSceneAsync("Options");
        gsm.LoadPreviousState();
    }

    public void ChangeSFXVolume() {
        preferences.sfxVolume = sfxVolumeControl.value;
    }

    public void ChangeMusicVolume() {
        preferences.musicVolume = musicVolumeControl.value;
    }

    public void Quit() {
        Debug.Log("Quit button pressed");
        SceneManager.UnloadSceneAsync("Options");
        //go to game over scene if in game mode options
        if (gsm.state == GameStateManager.State.GAME_MODE_OPTIONS) {
            gsm.state = GameStateManager.State.GAME_MODE_GAME_OVER;
        }
        Application.Quit();
    }
}