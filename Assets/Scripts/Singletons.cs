using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Singletons : MonoBehaviour {

    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }
}
