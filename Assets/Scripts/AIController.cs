using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour {
    public float speed = 10.0f;

    private MazeBuilder maze;
    private PlayerController player;
    private AStar aStar;


    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (maze) {
            int dx, dy;
            GetInput(out dx, out dy);
            Move(dx, dy);
        } else {
            maze = FindObjectOfType<MazeBuilder>();
            player = FindObjectOfType<PlayerController>();
            aStar = FindObjectOfType<AStar>();

            BuildPathsFromMaze();
        }
    }

    void GetInput(out int dx, out int dy) {
        dx = 0;
        dy = 0;
        int x = Mathf.RoundToInt(transform.position.x);
        int y = Mathf.RoundToInt(transform.position.y);

        //build paths to the player
        aStar.RebuildAI(Mathf.RoundToInt(player.transform.position.x), Mathf.RoundToInt(player.transform.position.y));

        //set current location to where the enemy is now
        GridPoint current = new GridPoint() { x = Mathf.RoundToInt(transform.position.x), y = Mathf.RoundToInt(transform.position.y) };

        //find next location in direction of player
        GridPoint next;
        if (aStar.GetNext(current, out next)) {
            dx = next.x - current.x;
            dy = next.y - current.y;

            //let us make sure there is no bug!  Can remove this code 
            //later if it slows the game down noticeably, but it won't 
            if (dx != 0 && dy != 0) {
                throw new UnityException("dx and dy are both nonzero!");
            }
            if (dx > 1 || dx < -1) {
                throw new UnityException("dx is not one of 0, 1, or -1!");
            }
            if (dy > 1 || dy < -1) {
                throw new UnityException("dy is not one of 0, 1, or -1!");
            }
        }

        if (!maze.IsPlayerSpace(x + dx, y + dy)) {
            dx = 0;
            dy = 0;
        }
    }

    void Move(int dx, int dy) {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (!rb) {
            throw new UnityException("Missing Rigidbody2D component");
        }
        if (dx != 0 || dy != 0) {
            rb.velocity = new Vector2(dx, dy) * speed;
        }

        Animator animator = GetComponent<Animator>();
        if (animator) {
            animator.SetBool("IsRunning", (rb.velocity.x != 0 ||
            rb.velocity.y != 0));
        } else {
            Debug.LogWarning("Missing Cat Animator");
        }

        if (dx > 0) {
            transform.rotation = Quaternion.Euler(0, 0, -90);
        } else if (dx < 0) {
            transform.rotation = Quaternion.Euler(0, 0, 90);
        } else if (dy > 0) {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        } else if (dy < 0) {
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }

        //correct position
        if (dx != 0) {
            int y = Mathf.RoundToInt(transform.position.y);
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
        }
        if (dy != 0) {
            int x = Mathf.RoundToInt(transform.position.x);
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
        }
    }

    void BuildPathsFromMaze() {
        aStar.Initialize(maze.xSize, maze.ySize);
        for (int x = 0; x < maze.xSize; x++) {
            for (int y = 0; y < maze.ySize; y++) {
                if (!maze.IsPlayerSpace(x, y)) {
                    aStar.AddBlocked(new GridPoint() { x = x, y = y });
                }
            }
        }
    }
}