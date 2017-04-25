using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed = 10.0f;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        int dx, dy;
        GetInput(out dx, out dy);
        Move(dx, dy);
    }

    void GetInput(out int dx, out int dy) {
        dx = 0;
        dy = 0;
        if (Input.GetAxis("Horizontal") < 0) {
            dx = -1;
        } else if (Input.GetAxis("Horizontal") > 0) {
            dx = 1;
        } else if (Input.GetAxis("Vertical") < 0) {
            dy = -1;
        } else if (Input.GetAxis("Vertical") > 0) {
            dy = 1;
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
    }
}