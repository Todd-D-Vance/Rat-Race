using System.Collections;
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
    void Start () {
        musicVolumeControl.value = preferences.musicVolume;
        sfxVolumeControl.value = preferences.sfxVolume;
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
}
