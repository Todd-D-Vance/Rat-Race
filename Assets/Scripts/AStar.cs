using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour {

    public GameObject[] avoid;

    private GridPoint destination = new GridPoint() { x = 2, y = 2 };

    private int[,] grid;


    //call this whenever the destination changes
    public void RebuildAI(int x, int y) {
        destination.x = x;
        destination.y = y;
        ResetAI();
        foreach (GameObject a in avoid) {
            int ax = Mathf.RoundToInt(a.transform.position.x);
            int ay = Mathf.RoundToInt(a.transform.position.y);
            if (grid[ax, ay] > 0) {
                grid[ax, ay] = 100;//make cats undesireable to each other
            }
        }
        BuildPaths(destination);
    }

    //use this to initialize
    public void Initialize(int xSize, int ySize) {
        grid = new int[xSize, ySize];
        for (int x = 0; x < xSize; x++) {
            for (int y = 0; y < ySize; y++) {
                grid[x, y] = int.MaxValue;
            }
        }
    }

    public void ResetAI() {
        for (int x = 0; x < grid.GetLength(0); x++) {
            for (int y = 0; y < grid.GetLength(1); y++) {
                if (grid[x, y] >= 0) {
                    grid[x, y] = int.MaxValue;
                }
            }
        }
    }

    //add locations that are inaccessible
    public void AddBlocked(params GridPoint[] gps) {
        foreach (GridPoint gp in gps) {
            grid[gp.x, gp.y] = -1;
        }
        //force rebuild
        if (grid[destination.x, destination.y] == 0) {
            grid[destination.x, destination.y] = int.MaxValue;
        }
    }

    private void BuildPaths(GridPoint destination) {
        Queue<GridPoint> frontier = new Queue<GridPoint>();
        if (grid[destination.x, destination.y] > 0) {
            frontier.Enqueue(destination);
            grid[destination.x, destination.y] = 0;
        } else {
            return; //destination is unreachable
        }

        while (frontier.Count > 0) {
            GridPoint y = frontier.Dequeue();

            GridPoint x = new GridPoint() { x = y.x + 1, y = y.y };
            if (x.x < grid.GetLength(0) && grid[x.x, x.y] == int.MaxValue) {
                frontier.Enqueue(x);
                grid[x.x, x.y] = grid[y.x, y.y] + 1;
            }

            x = new GridPoint() { x = y.x - 1, y = y.y };
            if (x.x >= 0 && grid[x.x, x.y] == int.MaxValue) {
                frontier.Enqueue(x);
                grid[x.x, x.y] = grid[y.x, y.y] + 1;
            }

            x = new GridPoint() { x = y.x, y = y.y + 1 };
            if (x.y < grid.GetLength(1) && grid[x.x, x.y] == int.MaxValue) {
                frontier.Enqueue(x);
                grid[x.x, x.y] = grid[y.x, y.y] + 1;
            }

            x = new GridPoint() { x = y.x, y = y.y - 1 };
            if (x.y >= 0 && grid[x.x, x.y] == int.MaxValue) {
                frontier.Enqueue(x);
                grid[x.x, x.y] = grid[y.x, y.y] + 1;
            }
        }
    }

    //number of steps from specified location to destination
    public int GetDist(int x, int y) {
        if (x < 0 || y < 0 || x >= grid.GetLength(0) || y >= grid.GetLength(1)) {
            return -1;
        }
        if (grid[x, y] == int.MaxValue) {
            return -1;
        }
        return grid[x, y];
    }

    //get next point for moving toward destination
    public bool GetNext(GridPoint start, out GridPoint next) {
        next = start;
        if (grid[start.x, start.y] <= 0 || grid[start.x, start.y] == int.MaxValue) {
            return false;
        }

        GridPoint x = new GridPoint() { x = start.x + 1, y = start.y };
        if (x.x < grid.GetLength(0) && grid[x.x, x.y] >= 0 && grid[x.x, x.y] < grid[next.x, next.y]) {
            next = x;
        }

        x = new GridPoint() { x = start.x - 1, y = start.y };
        if (x.x >= 0 && grid[x.x, x.y] >= 0 && grid[x.x, x.y] < grid[next.x, next.y]) {
            next = x;
        }

        x = new GridPoint() { x = start.x, y = start.y + 1 };
        if (x.y < grid.GetLength(1) && grid[x.x, x.y] >= 0 && grid[x.x, x.y] < grid[next.x, next.y]) {
            next = x;
        }

        x = new GridPoint() { x = start.x, y = start.y - 1 };
        if (x.y >= 0 && grid[x.x, x.y] >= 0 && grid[x.x, x.y] < grid[next.x, next.y]) {
            next = x;
        }

        return true;
    }

}

[Serializable]
public struct GridPoint {
    public int x;
    public int y;
}
