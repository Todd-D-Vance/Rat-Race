using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demo : MonoBehaviour {
    private Recorder recorder = Recorder.instance;

    // Use this for initialization
    void Start() {
        recorder.StartPlayback();
    }

    // Update is called once per frame
    void Update() {
        recorder.PlayNextFrame();
    }
}
