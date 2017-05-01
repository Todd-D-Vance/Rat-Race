using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Singletons : MonoBehaviour {


    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {
        SceneManager.LoadScene("Title");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
