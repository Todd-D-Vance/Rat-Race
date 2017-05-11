using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Options : MonoBehaviour {

    public Slider musicVolumeControl;
    public Slider sfxVolumeControl;

    Preferences preferences = Preferences.instance;

	// Use this for initialization
	void Start () {
        musicVolumeControl.value = preferences.musicVolume;
        sfxVolumeControl.value = preferences.sfxVolume;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Return() {
        SceneManager.UnloadSceneAsync("Options");
    }

    public void ChangeSFXVolume() {
        preferences.sfxVolume = sfxVolumeControl.value;
    }

    public void ChangeMusicVolume() {
        preferences.musicVolume = musicVolumeControl.value;
    }
}
