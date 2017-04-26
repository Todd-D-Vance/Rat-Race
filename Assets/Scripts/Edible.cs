﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edible : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            GameObject.Destroy(gameObject, .1f);//give it 1/10 second to be swallowed
            Debug.Log("Gulp");//todo, add to score
        }
    }
}
