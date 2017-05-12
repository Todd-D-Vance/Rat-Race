using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prefabs : MonoBehaviour {
    public static Prefabs instance;

    public GameObject cheese;
    public GameObject cat;
    public GameObject mouse;
    public GameObject exterior;
    public GameObject[] solidRegions;
    public GameObject[] wallTiles;

    private void Awake() {
        instance = this;
    }
}