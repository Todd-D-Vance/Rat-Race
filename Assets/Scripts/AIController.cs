using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AIController : MonoBehaviour {
    public float speed = 10.0f;

    private MazeBuilder maze;
    private Rigidbody2D rb;
    private PlayerController player;
    private AStar aStar;
    private Animator animator;


    private bool pathsBuilt = false;

    // Use this for initialization
    void Start() {
        maze = FindObjectOfType<MazeBuilder>();
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerController>();
        aStar = FindObjectOfType<AStar>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        if (!pathsBuilt) {
            BuildPathsFromMaze();
            pathsBuilt = true;
            return;
        }

        int dx, dy;
        GetInput(out dx, out dy);
        Move(dx, dy);
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
        }

        if (!maze.IsPlayerSpace(x + dx, y + dy)) {
            dx = 0;
            dy = 0;
        }
    }

    void Move(int dx, int dy) {
        if (dx != 0 || dy != 0) {
            rb.velocity = new Vector2(dx, dy) * speed;
        }

        animator.SetBool("IsRunning", (rb.velocity.x != 0
          || rb.velocity.y != 0));

        //correct position
        if (dx != 0) {
            int y = Mathf.RoundToInt(transform.position.y);
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
        }
        if (dy != 0) {
            int x = Mathf.RoundToInt(transform.position.x);
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
        }

        //correct rotation
        if (dx > 0) {
            transform.rotation =
                Quaternion.Euler(0, 0, -90);
        }
        if (dx < 0) {
            transform.rotation =
                Quaternion.Euler(0, 0, 90);
        }
        if (dy > 0) {
            transform.rotation =
                Quaternion.Euler(0, 0, 0);
        }
        if (dy < 0) {
            transform.rotation =
                Quaternion.Euler(0, 0, 180);
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

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
            SceneManager.LoadScene("GameOver");
        }
    }

}