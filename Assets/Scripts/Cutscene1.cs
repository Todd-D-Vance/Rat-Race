using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene1 : MonoBehaviour {

    public GameObject cat;
    public GameObject mouse;
    public GameObject cheese;

    public float frame;
    private float lastx;
    private float lasty;

    private SoundPlayer sound = SoundPlayer.instance;
    private MusicPlayer music = MusicPlayer.instance;

    // Use this for initialization
    void Start() {
        cat.transform.position = new Vector3(0, 64, 0);
        cat.transform.localRotation = Quaternion.Euler(0, 0, -90);
        mouse.transform.position = new Vector3(10, 64, 0);
        mouse.transform.localRotation = Quaternion.Euler(0, 0, -90);
        frame = 0;
        lastx = 10;
        lasty = 64;
    }

    // Update is called once per frame
    void Update() {
        float x = 35f / 2f * Mathf.Sin(Mathf.PI / 40f * frame - Mathf.PI / 4f) + 55f / 2f;
        float y = -2f / 5f * frame + 64;
        if (frame < 160) {
            mouse.transform.position = new Vector3(x, y, 0);
            float xt = x - lastx;
            float yt = y - lasty;
            mouse.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(yt, xt) * 180f / Mathf.PI - 90);
            lastx = x;
            lasty = y;

            //make the cat face the mouse
            xt = x - cat.transform.position.x;
            yt = y - cat.transform.position.y;
            cat.transform.localRotation = Quaternion.Euler(0, 0, Mathf.Atan2(yt, xt) * 180f / Mathf.PI - 90);

            //...and move in the direction it faces
            cat.GetComponent<Rigidbody2D>().velocity = new Vector2(xt, yt).normalized * 40;
        }

        if (frame == 160) {
            Destroy(cheese);
            mouse.transform.localScale = new Vector3(2, 2, 1);

            //face the cat before eating it
            mouse.transform.rotation = Quaternion.Euler(0, 0, -45);
        }

        if (frame == 168) {
            Destroy(cat);
        }

        if (frame == 30) {
            sound.Squeak();
        }
        if (frame == 60) {
            sound.Meow();
        }
        if (frame == 90) {
            sound.Squeak();
        }
        if (frame == 120) {
            sound.Squeak();
        }
        if (frame == 150) {
            sound.Meow();
        }
        if (frame == 160) {
            sound.Gulp();
        }
        if (frame == 168) {
            sound.BigGulp();
        }
        if (frame == 180) {
            sound.BigSqueak();
        }
        if (frame == 240) {
            sound.BigSqueak();
        }
        if (frame == 300) {
            sound.BigSqueak();
        }

        if (frame == 0) {
            music.PlayTune("R16 O4 E- G- O5 E- F E- O4 B- G-"
            + "E  E- G- O5 E- F E- O4 B- G-"
            + "E  E- G- O5 E- F E- O4 B- G- R4 E-");
        }

        if (frame == 350) {
            FindObjectOfType<GameStateManager>().state =
                  GameStateManager.State.GAME_MODE_START_LEVEL;
        }

        frame++;
    }
}