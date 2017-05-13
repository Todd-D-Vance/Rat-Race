using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Points : MonoBehaviour {
    public int points;
    public Color color;

    private TextMesh tm;

    private float alpha = 1f;

    // Use this for initialization
    void Start() {
        tm = FindObjectOfType<TextMesh>();
        tm.text = points.ToString();
        tm.color = color;
    }

    // Update is called once per frame
    void Update() {
        tm.text = points.ToString();
        alpha -= 1f / 120f;
        if (alpha >= 0) {
            tm.color = new Color(color.r, color.g, color.b, alpha);
        } else {
            Destroy(gameObject);
        }

        transform.localScale *= 1.01f;//make it grow

        //make various-colored scores move different ways
        if (color.r > color.g) {
            if (color.r > color.b) {
                transform.position += new Vector3(0.02f, 0.02f);
            } else {
                transform.position += new Vector3(-0.02f, 0.02f);
            }
        } else {
            if (color.r > color.b) {
                transform.position += new Vector3(0.02f, -0.02f);
            } else {
                transform.position += new Vector3(-0.02f, -0.02f);
            }
        }
    }
}
