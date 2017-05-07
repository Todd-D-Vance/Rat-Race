using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeBuilder : MonoBehaviour {
    public GameObject exterior;
    public GameObject[] solidRegions;
    public GameObject[] wallTiles;

    public int xSize = 48;
    public int ySize = 64;

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
                Instantiate(exterior, new Vector3(x, y, 10),
    Quaternion.identity, transform);
                if (grid[x, y]) {

                    //instantiate solid regions
                    int directions = 0;

                    if ((y >= ySize - 1) || grid[x, y + 1]) {//up
                        directions += 1;
                    }
                    if ((y <= 0) || grid[x, y - 1]) {//down
                        directions += 4;
                    }
                    if ((x <= 0) || grid[x - 1, y]) {//left
                        directions += 8;
                    }
                    if ((x >= xSize - 1) || grid[x + 1, y]) {//right
                        directions += 2;
                    }
                    if ((directions & 9) == 9) {//up left
                        Instantiate(solidRegions[0], new Vector3(x, y, 9),
                        Quaternion.identity, transform);
                        directions += 16;
                    }
                    if ((directions & 3) == 3) {//up right
                        Instantiate(solidRegions[1], new Vector3(x, y, 9),
                        Quaternion.identity, transform);
                        directions += 32;
                    }
                    if ((directions & 6) == 6) {//down right
                        Instantiate(solidRegions[2], new Vector3(x, y, 9),
                        Quaternion.identity, transform);
                        directions += 64;
                    }
                    if ((directions & 12) == 12) {//down left
                        Instantiate(solidRegions[3], new Vector3(x, y, 9),
                        Quaternion.identity, transform);
                        directions += 128;
                    }

                    //Find the correct wall tile
                    int wallTile = 5;//let default be vertical
                    int count = (directions & 16) / 16 + (directions & 32) / 32
                    + (directions & 64) / 64 + (directions & 129) / 128;
                    if (count == 1) {
                        if ((directions & 16) != 0) {
                            wallTile = 9;//up and left
                        } else if ((directions & 32) != 0) {
                            wallTile = 3;//up and right
                        } else if ((directions & 64) != 0) {
                            wallTile = 6;//down and right
                        } else if ((directions & 128) != 0) {
                            wallTile = 12;//down and left
                        }
                    } else if ((directions & 15) == 10) {//horizontal
                        wallTile = 10;
                    } else if ((directions & 48) == 48) {//horizontal
                        wallTile = 10;
                    } else if ((directions & 192) == 192) {//horizontal
                        wallTile = 10;
                    }
                    //TODO more cases for more complex mazes


                    if (count != 4) {//count==4 means solid region, no wall
                        Instantiate(wallTiles[wallTile], new Vector3(x, y, 0),
                        Quaternion.identity, transform);
                    }
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