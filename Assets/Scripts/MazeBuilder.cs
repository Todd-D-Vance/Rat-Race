using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeBuilder : MonoBehaviour {
    public int xSize = 48;
    public int ySize = 64;

    public GameObject wallPrefab;

    private bool[,] grid;

    // Use this for initialization
    void Start() {
        Initialize();
        BasicMaze(10, 10);
        Draw();
    }

    // Update is called once per frame
    void Update() {

    }

    public void Initialize() {
        grid = new bool[xSize, ySize];
        for (int x = 0; x < xSize; x++) {
            for (int y = 0; y < ySize; y++) {
                grid[x, y] = true;
            }
        }
    }

    public void BasicMaze(int m, int n) {
        CarveRow(2);
        CarveRow(ySize - 3);
        CarveColumn(2);
        CarveColumn(xSize - 3);


        int[] RowWallWidth = new int[m - 1];
        for (int i = 0; i < m - 1; i++) {
            RowWallWidth[i] = 1;
        }
        for (int i = 0; i < ySize - 2 - 4 * m; i++) {
            int j = Random.Range(0, m - 1);
            RowWallWidth[j]++;
        }
        int r = 5;
        for (int i = 0; i < m - 2; i++) {
            r += RowWallWidth[i];
            CarveRow(r);
            r += 3;
        }

        int[] ColWallWidth = new int[n - 1];
        for (int i = 0; i < n - 1; i++) {
            ColWallWidth[i] = 1;
        }
        for (int i = 0; i < xSize - 2 - 4 * n; i++) {
            int j = Random.Range(0, n - 1);
            ColWallWidth[j]++;
        }
        int c = 5;
        for (int i = 0; i < n - 2; i++) {
            c += ColWallWidth[i];
            CarveColumn(c);
            c += 3;
        }
    }

    public void CarveRow(int y) {
        for (int x = 1; x <= xSize - 2; x++) {
            for (int j = y - 1; j <= y + 1; j++) {
                grid[x, j] = false;
            }
        }
    }

    public void CarveColumn(int x) {
        for (int y = 1; y <= ySize - 2; y++) {
            for (int j = x - 1; j <= x + 1; j++) {
                grid[j, y] = false;
            }
        }
    }

    public void Draw() {
        for (int x = 0; x < xSize; x++) {
            for (int y = 0; y < ySize; y++) {
                if (grid[x, y]) {
                    Instantiate(wallPrefab, new Vector3(x, y, wallPrefab.transform.position.z),
                    Quaternion.identity, transform);
                }

            }
        }
    }

    public bool IsPlayerSpace(int x, int y) {
        if (x - 1 < 0 || y - 1 < 0) {
            return false;
        }
        if (x + 1 >= xSize || y + 1 >= ySize) {
            return false;
        }
        for (int i = x - 1; i <= x + 1; i++) {
            for (int j = y - 1; j <= y + 1; j++) {
                if (grid[i, j]) {
                    return false;
                }
            }
        }
        return true;
    }
}