using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Score : MonoBehaviour {

    public Text value;
    public Score keepAbove;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update() {
        if (keepAbove) {
            int score = keepAbove.Get();
            int highScore = Get();
            if (score > highScore) {
                Set(score);
            }
        }
    }

    public void Set(int score) {
        value.text = score.ToString();
    }

    public void Reset() {
        Set(0);
    }

    public int Get() {
        try {
            return int.Parse(value.text);
        } catch (FormatException) {
            return 0;
        }
    }

    public void Add(int amount) {
        Set(Get() + amount);
    }
}
