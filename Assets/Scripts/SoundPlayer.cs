using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour {
    public static SoundPlayer instance;

    private AudioSource[] sources;
    private AudioSource meow;
    private AudioSource squeak;
    private AudioSource gulp;
    private Preferences preferences;

    private float lastMeowTime = 0;

    private void Awake() {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        preferences = Preferences.instance;
        sources = GetComponents<AudioSource>();
        meow = sources[0];
        squeak = sources[1];
        gulp = sources[2];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Meow() {
        if (Time.time - lastMeowTime > 0.5f) {
            meow.pitch = Random.Range(0.9f, 1.1f);
            meow.volume = 1 * preferences.sfxVolume;
            meow.Play();
            lastMeowTime = Time.time;
        }
    }

    public void Squeak() {
        squeak.pitch = Random.Range(0.9f, 1.1f);
        squeak.volume = 1 * preferences.sfxVolume;
        squeak.Play();
    }

    public void BigSqueak() {
        squeak.pitch = Random.Range(0.4f, .6f);
        squeak.volume = 1 * preferences.sfxVolume;    
        squeak.Play();
    }

    public void Gulp() {
        gulp.pitch = 1;
        gulp.volume = 0.5f * preferences.sfxVolume;
        gulp.Play();
    }

    public void BigGulp() {
        gulp.pitch = 0.8f;
        gulp.volume = 1 * preferences.sfxVolume;
        gulp.Play();
    }
}
