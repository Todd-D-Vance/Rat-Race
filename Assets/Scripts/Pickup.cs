using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

    public float speed = 1f;
    public GameObject pointsPrefab;

    //if negative, this kills the player instead
    public int pointValue = 100;

    public float timeToLive = 20f;  //how many seconds before it evaporates

    private MazeBuilder maze = null;
    private AStar aStar;
    private Rigidbody2D rb;
    private BoxCollider2D theCollider;
    private SoundPlayer sound;
    private GameStateManager gsm;
    private Score score;
    private Recorder recorder;




    // Use this for initialization
    void Start() {
        gsm = FindObjectOfType<GameStateManager>();
        sound = FindObjectOfType<SoundPlayer>();
        foreach (Score s in FindObjectsOfType<Score>()) {
            if (s.gameObject.name == "Score") {
                score = s;
            }
        }
        if (!score) {
            throw new UnityException("Score object not found");
        }
        recorder = FindObjectOfType<Recorder>();
        if (gsm.recordThisGame) {
            recorder.RecordIfRecording(recorder.CreateCommand(gameObject));
        }
        Destroy(gameObject, timeToLive);//self-destruct after time elapses
    }

    // Update is called once per frame
    void Update() {
        if (!theCollider) {
            theCollider = GetComponent<BoxCollider2D>();
        }
        if (!rb) {
            rb = GetComponent<Rigidbody2D>();
        }
        if (!maze) {
            maze = FindObjectOfType<MazeBuilder>();
        }
        if (!aStar) {
            aStar = FindObjectOfType<AStar>();
            if (aStar) {
                BuildPathsFromMaze(ref aStar);
            }
        }
        if (maze && aStar && gsm.state == GameStateManager.State.GAME_MODE_PLAY && gsm.framesInState > 7) {
            int dx, dy;
            GetInput(out dx, out dy);
            Move(dx, dy);
        }
    }

    private void LateUpdate() {
        //update recording
        if (gsm.framesInState > 7 && gsm.recordThisGame && gsm.state == GameStateManager.State.GAME_MODE_PLAY) {
            recorder.RecordIfRecording(recorder.MoveCommand(gameObject));
        }
    }

    void GetInput(out int dx, out int dy) {
        dx = 0;
        dy = 0;
        GridPoint current = new GridPoint() { x = Mathf.RoundToInt(transform.position.x), y = Mathf.RoundToInt(transform.position.y) };

        GridPoint next;
        if (aStar.GetNext(current, out next)) {
            dx = next.x - current.x;
            dy = next.y - current.y;
        }

        int x = Mathf.RoundToInt(transform.position.x);
        int y = Mathf.RoundToInt(transform.position.y);

        if (!maze.IsPlayerSpace(x + dx, y + dy)) {
            dx = 0;
            dy = 0;
        }
    }

    void Move(int dx, int dy) {
        if (!rb) {
            throw new UnityException("Missing Rigidbody2D component");
        }

        if (dx != 0 || dy != 0) {
            rb.velocity = new Vector2(dx, dy) * speed;
        }

        //Teleport
        int x = Mathf.RoundToInt(transform.position.x);
        int y = Mathf.RoundToInt(transform.position.y);

        if (y == 32) {//tunnel row
            if (x == 14 && rb.velocity.x > 0) {
                x = 32;
                transform.position = new Vector3(x, y, transform.position.z);
            } else if (x == 32 && rb.velocity.x < 0) {
                x = 14;
                transform.position = new Vector3(x, y, transform.position.z);
            }
        }

        //correct position
        if (dx != 0) {
            transform.position = new Vector3(transform.position.x, y,
            transform.position.z);
        }
        if (dy != 0) {
            transform.position = new Vector3(x, transform.position.y,
            transform.position.z);
        }
    }


    void BuildPathsFromMaze(ref AStar aStar) {
        aStar.Initialize(maze.xSize, maze.ySize);
        for (int x = 0; x < maze.xSize; x++) {
            for (int y = 0; y < maze.ySize; y++) {
                if (!maze.IsPlayerSpace(x, y)) {
                    aStar.AddBlocked(new GridPoint() { x = x, y = y });
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log("Trigger: " + collision.gameObject.name);
        if (collision.gameObject.tag == "Player") {
            if (pointValue < 0) {
                gsm.state = GameStateManager.State.GAME_MODE_DEATH;
            } else {
                //Eat the pickup!
                sound.BigGulp();
                GameObject g = Instantiate(pointsPrefab);
                Points p = g.GetComponent<Points>();
                p.color = new Color(0,0,1,0);
                p.points = pointValue;
                g.transform.position = transform.position;
                score.Add(pointValue);
                Destroy(gameObject);
            }
        }
    }

}
