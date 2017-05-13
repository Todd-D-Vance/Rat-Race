using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene2 : MonoBehaviour {

    public GameObject[] cats;
    public float xSpeed = 1;
    public float ySpeed = .1f;

    private SoundPlayer sound;
    private MusicPlayer music;

    private int[] direction = new int[] { 1, -1, 1, -1 };

    // Use this for initialization
    void Start() {
        sound = FindObjectOfType<SoundPlayer>();
        music = FindObjectOfType<MusicPlayer>();
        music.PlayTune("R8 O4 C R16 C D C D C D R8 O3 A B G E" +
            " F R16 A B A B A B R8 E D C G "
            + "A R16 A B O4 C O3 B O4 C D E D E F G A B O5 C"
            + "R8 D D C O4 B O5 R16 C B A G F E D E "
            + "C O3 B A G F E D E R4 C G O4 C G O5 C O6 C");
    }

    // Update is called once per frame
    void Update() {
        int i = 0;

        foreach (GameObject cat in cats) {
            float x = cat.transform.position.x;
            float y = cat.transform.position.y;
            if (x >= 45) {
                direction[i] = -1;
            } else if (x <= 1) {
                direction[i] = 1;
            }
            float lastx = cat.transform.position.x;
            float lasty = cat.transform.position.y;
            x += direction[i] * xSpeed;
            y -= ySpeed;
            cat.transform.position = new Vector3(x, y);
            if (direction[i] > 0) {
                cat.transform.eulerAngles = new Vector3(0, 0, -90);
            } else {
                cat.transform.eulerAngles = new Vector3(0, 0, 90);
            }
            i++;

            if (y < 0) {//when a cat goes off screen, end
                FindObjectOfType<GameStateManager>().state =
                 GameStateManager.State.GAME_MODE_START_LEVEL;
            }
        }

        if (UnityEngine.Random.value < 0.01f) {
            sound.Meow();
        }
    }
}