using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed = 10.0f;

    private MazeBuilder maze = null;

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
        }
    }

    void GetInput(out int dx, out int dy) {
        dx = 0;
        dy = 0;

        int x = Mathf.RoundToInt(transform.position.x);
        int y = Mathf.RoundToInt(transform.position.y);

        if (Input.GetAxis("Horizontal") < 0) {
            dx = -1;
        } else if (Input.GetAxis("Horizontal") > 0) {
            dx = 1;
        } else if (Input.GetAxis("Vertical") < 0) {
            dy = -1;
        } else if (Input.GetAxis("Vertical") > 0) {
            dy = 1;
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
            Debug.LogWarning("Missing Mouse Animator");
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
}