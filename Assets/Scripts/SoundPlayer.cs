using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour {
    public static SoundPlayer instance;

    private AudioSource[] sources;
    private AudioSource meow;
    private AudioSource squeak;
    private AudioSource gulp;

    private float lastMeowTime = 0;

    private void Awake() {
        instance = this;
    }

    // Use this for initialization
    void Start () {
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
            meow.Play();
            lastMeowTime = Time.time;
        }
    }

    public void Squeak() {
        squeak.pitch = Random.Range(0.9f, 1.1f);
        squeak.Play();
    }

    public void BigSqueak() {
        squeak.pitch = Random.Range(0.4f, .6f);
        squeak.Play();
    }

    public void Gulp() {
        gulp.pitch = 1;
        gulp.volume = 0.5f;
        gulp.Play();
    }

    public void BigGulp() {
        gulp.pitch = 0.8f;
        gulp.volume = 1;
        gulp.Play();
    }
}
