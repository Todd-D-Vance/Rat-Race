using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Preferences : MonoBehaviour {
    public static Preferences instance;

    //these variables hold the actual data
    //the question mark means it is allowed to be null as well.
    private float? _musicVolume = null;
    private string musicVolumeKey = "Music Volume";

    private float? _sfxVolume = null;
    private string sfxVolumeKey = "SFX Volume";

    private void Awake() {
        instance = this;
    }

    public float musicVolume {//the property
        get {//this is the code that returns the value of this property
            if (_musicVolume == null) {
                if (PlayerPrefs.HasKey(musicVolumeKey)) {
                    //magic code to load previously saved prefs
                    _musicVolume = PlayerPrefs.GetFloat(musicVolumeKey);
                } else {
                    _musicVolume = 0.8f;//a reasonable default
                }
            }
            return (float)_musicVolume;
        }

        set {//this is the code called when the property value is set
            _musicVolume = value;
            // magic code to save player prefs between games
            PlayerPrefs.SetFloat(musicVolumeKey, value);
        }
    }

    public float sfxVolume {//the property
        get {//this is the code that returns the value of this property
            if (_sfxVolume == null) {
                if (PlayerPrefs.HasKey(sfxVolumeKey)) {
                    //magic code to load previously saved prefs
                    _sfxVolume = PlayerPrefs.GetFloat(sfxVolumeKey);
                } else {
                    _sfxVolume = 0.8f;//a reasonable default
                }
            }
            return (float)_sfxVolume;
        }

        set {//this is the code called when the property value is set
            _sfxVolume = value;
            // magic code to save player prefs between games
            PlayerPrefs.SetFloat(sfxVolumeKey, value);
        }
    }
}