using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AIController : MonoBehaviour {
    public float speed = 10.0f;
    public Vector3 initialPosition;
    public Vector3 initialRotation;
    public float flee = 0;

    private MazeBuilder maze;
    private PlayerController player;
    private AStar aStar;
    private Animator animator;
    private Rigidbody2D rb;
    private BoxCollider2D theCollider;
    private SoundPlayer sound = SoundPlayer.instance;
    private GameStateManager gsm = GameStateManager.instance;
    private Recorder recorder = Recorder.instance;
    private Score score;

    private int initDx = 0;
    private int initDy = 0;
    private float delay = 0;
    private bool deployed = false;


    // Use this for initialization
    void Start() {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        theCollider = GetComponent<BoxCollider2D>();
        foreach (Score s in FindObjectsOfType<Score>()) {
            if (s.gameObject.name == "Score") {
                score = s;
            }
        }
        if (!score) {
            throw new UnityException("Score object not found");
        }
        ResetEnemy();
        if (gsm.recordThisGame) {
            recorder.RecordIfRecording(recorder.CreateCommand(gameObject));
        }
    }

    // Update is called once per frame
    void Update() {
        if (maze) {
            if (delay > 0) {
                delay -= Time.deltaTime;
            } else {
                int dx, dy;
                GetInput(out dx, out dy);
                Move(dx, dy);
            }
        } else {
            maze = FindObjectOfType<MazeBuilder>();
            player = FindObjectOfType<PlayerController>();
            aStar = FindObjectOfType<AStar>();

            BuildPathsFromMaze();
        }
        if (flee > 0) {
            flee -= Time.deltaTime;
        } else {
            flee = 0;
        }

        if (deployed && Mathf.RoundToInt((transform.position - player.transform.position).magnitude) == 10) {
            sound.Meow();
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

        int x = Mathf.RoundToInt(transform.position.x);
        int y = Mathf.RoundToInt(transform.position.y);

        if (flee > 0) {
            //build paths away from player
            int xx = 2;
            int yy = 2;
            if (player.transform.position.x < 24) {
                xx = 45;
            }
            if (player.transform.position.y < 32) {
                yy = 61;
            }

            aStar.RebuildAI(xx, yy);
        } else {
            //build paths to player
            aStar.RebuildAI(Mathf.RoundToInt(player.transform.position.x), Mathf.RoundToInt(player.transform.position.y));
        }


        //if still in box, move in direction it is facing
        if (!deployed && gsm.state==GameStateManager.State.GAME_MODE_PLAY) {
            float tx = transform.position.x;
            float ty = transform.position.y;
            float tz = transform.position.z;
            tx += initDx * speed * Time.deltaTime;
            ty += initDy * speed * Time.deltaTime;
            if (aStar.GetDist(x, y) < 0) {//still in the box
                transform.position = new Vector3(tx, ty, tz);
            } else { //ensure on a valid grid space and deploy      
                transform.position = new Vector3(x, y, tz);
                deployed = true;
                theCollider.enabled = true;//re-enable collider
            }
            return;
        }

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
        if (gsm.state != GameStateManager.State.GAME_MODE_PLAY) {
            if (animator) {
                animator.SetBool("IsRunning", false);
                rb.velocity = Vector2.zero;
            }
            return;
        }

        if (dx != 0 || dy != 0) {
            rb.velocity = new Vector2(dx, dy) * speed;
        }

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

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
            if (player && flee > 0) {
                //Eat the cat!
                sound.BigGulp();
                ResetEnemy();
                score.Add(player.GetCatValue());
                player.DoubleCatValue();
            } else {
                gsm.state = GameStateManager.State.GAME_MODE_DEATH;
            }
        }
    }

    public void ResetEnemy() {
        transform.position = initialPosition;
        transform.eulerAngles = initialRotation;
        int angle = Mathf.RoundToInt(initialRotation.z);
        if (angle < 0) {//need angle from 0 to 359
            angle += 360;
        }
        switch (angle) {
            case 0:
                initDy = 1;
                break;
            case 90:
                initDx = -1;
                break;
            case 180:
                initDy = -1;
                break;
            case 270:
                initDx = 1;
                break;
            default:
                throw new UnityException("Cat not pointing N,S,E, or W");
        }
        delay = angle / 9; //start one new cat every 10 seconds
        deployed = false;

        theCollider.enabled = false;//disable to allow escaping from cat box

        rb.velocity = Vector2.zero;
    }

}