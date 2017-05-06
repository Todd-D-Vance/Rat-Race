using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dots : MonoBehaviour {

    public GameObject DotPrefab;

    private MazeBuilder maze;
    private AStar aStar;

    int frame = 0;

    // Use this for initialization
    void Start() {
        aStar = FindObjectOfType<AStar>();
        maze = FindObjectOfType<MazeBuilder>();
    }

    // Update is called once per frame
    void Update() {
        if (frame == 2) {
            DrawDots();
        }
        frame++;        
    }

    void DrawDots() {
        int numDots = 0;
        for (int x = 0; x < maze.xSize; x++) {
            for (int y = 0; y < maze.ySize; y++) {
                int d = aStar.GetDist(x, y);
                if (maze.IsPlayerSpace(x, y) && d > 0 && d % 2 == 1) {
                    Instantiate(DotPrefab, new Vector3(x, y, 1), Quaternion.identity, transform);
                    numDots++;
                }
            }
        }        
    }
}