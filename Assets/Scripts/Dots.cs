using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dots : MonoBehaviour {

    public GameObject DotPrefab;

    private MazeBuilder maze = null;
    private AStar aStar;
    private bool readyToGo = false;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (!readyToGo) {
            if (maze) {
                DrawDots();
            } else {
                aStar = FindObjectOfType<AStar>();
                maze = FindObjectOfType<MazeBuilder>();
            }
        }
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
        if (numDots > 0) {
            readyToGo = true;
            Game game = FindObjectOfType<Game>();
            game.state = Game.State.PLAY;
        }
    }
}