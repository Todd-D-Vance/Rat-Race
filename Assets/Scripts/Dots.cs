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

        //find suitable places for growth hormone cheese
        Transform lowerLeft = null;
        Transform lowerRight = null;
        Transform upperLeft = null;
        Transform upperRight = null;

        foreach (Transform dot in transform) {
            //is it at an intersection?
            int x = Mathf.RoundToInt(dot.position.x);
            int y = Mathf.RoundToInt(dot.position.y);
            int numWays = (maze.IsPlayerSpace(x + 2, y) ? 1 : 0)
                + (maze.IsPlayerSpace(x - 2, y) ? 1 : 0)
                 + (maze.IsPlayerSpace(x, y + 2) ? 1 : 0)
                  + (maze.IsPlayerSpace(x, y - 2) ? 1 : 0);
            if (numWays >= 3) {
                //determine which quadrant
                if (x < maze.xSize / 2 - 10 && y < maze.ySize / 2 - 10) {//lower left
                                                                         //if no lower left dot, or this dot is closer to corner than existing lower left dot
                    if (!lowerLeft || lowerLeft.position.magnitude > dot.position.magnitude) {
                        lowerLeft = dot;
                    }
                } else if (x < maze.xSize / 2 - 10 && y > maze.ySize / 2 + 10) {//upper left
                                                                                //if no upper left dot, or this dot is closer to corner than existing upper left dot
                    if (!upperLeft || upperLeft.position.x * upperLeft.position.x
                        + (maze.ySize - upperLeft.position.y) * (maze.ySize - upperLeft.position.y)
                        > x * x + (maze.ySize - y) * (maze.ySize - y)) {
                        upperLeft = dot;
                    }
                } else if (x > maze.xSize / 2 + 10 && y < maze.ySize / 2 - 10) {//lower right                            
                                                                                //if no lower right dot, or this dot is closer to corner than existing lower right dot
                    if (!lowerRight || (maze.xSize - lowerRight.position.x) * (maze.xSize - lowerRight.position.x)
                        + lowerRight.position.y * lowerRight.position.y
                        > (maze.xSize - x) * (maze.xSize - x) + y * y) {
                        lowerRight = dot;
                    }
                } else if (x > maze.xSize / 2 + 10 && y > maze.ySize / 2 + 10) {//upper right
                                                                                //if no upper right dot, or this dot is closer to corner than existing upper right dot
                    if (!upperRight || (maze.xSize - upperRight.position.x) * (maze.xSize - upperRight.position.x)
                        + (maze.ySize - upperRight.position.y) * (maze.ySize - upperRight.position.y)
                        > (maze.xSize - x) * (maze.xSize - x) + (maze.ySize - y) * (maze.ySize - y)) {
                        upperRight = dot;
                    }
                }
            }
        }
        if (lowerLeft) {
            lowerLeft.gameObject.tag = "Growth";
            lowerLeft.transform.localScale *= 3;
        }
        if (lowerRight) {
            lowerRight.gameObject.tag = "Growth";
            lowerRight.transform.localScale *= 3;
        }
        if (upperLeft) {
            upperLeft.gameObject.tag = "Growth";
            upperLeft.transform.localScale *= 3;
        }
        if (upperRight) {
            upperRight.gameObject.tag = "Growth";
            upperRight.transform.localScale *= 3;
        }

        if (numDots > 0) {
            readyToGo = true;
        }
    }

    public int CountDots() {       
        int count = 0;
        foreach (Transform dot in transform) {
            count++;
        }
        return count;
    }
}