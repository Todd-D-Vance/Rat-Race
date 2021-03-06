﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour {
    public static GameStateManager instance;

    public bool recordThisGame = true;
    public int numberOfLivesRemaining;
    public State state = State.INIT;
    public float timeInState = 0;
    public int framesInState = 0;
    public string titleScene = "Title";
    public string descriptionScene = "Description";
    public string highScoreScene = "HighScores";
    public string demoScene = "Demo";
    public GameObject gameOverPrefab;
    public GameObject pbPrefab;
    public int placeInHighScoreList = 0;

    private MusicPlayer music;
    private Recorder recorder;
    private HighScores highScores;

    private Queue<State> previousStates = new Queue<State>();
    private State stateLastFrame = State.INVALID;
    private State lastState = State.INVALID;
    private int numberOfLevelsPlayed = 0;


    private void Awake() {
        instance = this;
    }

    // Use this for initialization
    void Start() {
        music = MusicPlayer.instance;
        recorder = Recorder.instance;
        highScores = HighScores.instance;
    }

    // Update is called once per frame
    void Update() {
        if (stateLastFrame != state) {
            Debug.Log("Game State changed to " + state + " after " + timeInState + " seconds and " + framesInState + " frames");
            previousStates.Enqueue(stateLastFrame);
            lastState = stateLastFrame;
            stateLastFrame = state;
            timeInState = 0;
            framesInState = 0;
        } else {
            timeInState += Time.deltaTime;
            framesInState++;
        }

        switch (state) {
            case State.INVALID:
                throw new UnityException("Invalid Game State");

            case State.INIT:
                if (framesInState == 5) {
                    music.PlayTune("O4 R8 C E- G O5 C "
                           + "O4 E- G O5 C E-"
                           + "O4 G O5 C E- G"
                           + "R4 C O4 R2 C");
                }
                if (framesInState == 150) {
                    SceneManager.LoadScene(titleScene);
                    state = State.ATTRACT_MODE_TITLE;
                }
                break;

            case State.ATTRACT_MODE_TITLE:
                if (timeInState >= 10.0f || Input.GetKey("space") || Input.GetKey("right")) {
                    SceneManager.LoadScene(descriptionScene);
                    state = State.ATTRACT_MODE_DESCRIPTION;
                } else if (Input.GetKey("backspace") || Input.GetKey("left")) {
                    SceneManager.LoadScene(demoScene);
                    state = State.ATTRACT_MODE_DEMO;
                } else if (Input.GetButtonDown("Cancel")) {
                    state = State.ATTRACT_MODE_OPTIONS;
                }
                break;

            case State.ATTRACT_MODE_DESCRIPTION:
                if (timeInState >= 10.0f || Input.GetKey("space") || Input.GetKey("right")) {
                    SceneManager.LoadScene(highScoreScene);
                    state = State.ATTRACT_MODE_HIGH_SCORES;
                } else if (Input.GetKey("backspace") || Input.GetKey("left")) {
                    SceneManager.LoadScene(titleScene);
                    state = State.ATTRACT_MODE_TITLE;
                } else if (Input.GetButtonDown("Cancel")) {
                    state = State.ATTRACT_MODE_OPTIONS;
                }
                break;

            case State.ATTRACT_MODE_HIGH_SCORES:
                if (timeInState >= 10.0f || Input.GetKey("space") || Input.GetKey("right")) {
                    SceneManager.LoadScene(demoScene);
                    state = State.ATTRACT_MODE_DEMO;
                } else if (Input.GetKey("backspace") || Input.GetKey("left")) {
                    SceneManager.LoadScene(descriptionScene);
                    state = State.ATTRACT_MODE_DESCRIPTION;
                } else if (Input.GetButtonDown("Cancel")) {
                    state = State.ATTRACT_MODE_OPTIONS;
                }
                break;

            case State.ATTRACT_MODE_DEMO:
                if (timeInState >= 40.0f || Input.GetKey("space") || Input.GetKey("right")) {
                    SceneManager.LoadScene(titleScene);
                    state = State.ATTRACT_MODE_TITLE;
                } else if (Input.GetKey("backspace") || Input.GetKey("left")) {
                    SceneManager.LoadScene(highScoreScene);
                    state = State.ATTRACT_MODE_HIGH_SCORES;
                } else if (Input.GetButtonDown("Cancel")) {
                    state = State.ATTRACT_MODE_OPTIONS;
                }
                break;

            case State.ATTRACT_MODE_OPTIONS:
                if (framesInState == 0) {//if beginning of state
                    SceneManager.LoadSceneAsync("Options", LoadSceneMode.Additive);
                }
                break;

            case State.GAME_MODE_INTRO:
                if (framesInState == 0) {
                    if (recordThisGame) {
                        recorder.StartRecording();
                    }
                    numberOfLivesRemaining = 3;
                    numberOfLevelsPlayed = 0;
                }
                if (framesInState == 7) {//wait some ticks for scene to 
                                         //change to prevent glitch
                    if (music.paused) {
                        music.paused = false;
                    } else {
                        PlayerPrefs.SetInt("Score", 0);
                        GetScoreObject().Reset();
                        music.PlayTune("R4 O3 B- B- B- B- R8 O5 E- E- D E- F G F E- O3 B- O5 E- O4 E-");
                    }
                    GetHighScoreObject().Set(highScores.GetScore(1));
                }
                if (timeInState > 3f) {//TODO make dots draw before PLAY mode
                    state = State.GAME_MODE_PLAY;
                }

                break;

            case State.GAME_MODE_PLAY:
                music.paused = false;
                if (framesInState > 7 && recordThisGame) {//don't record first few frames
                                                          //advance frame
                    recorder.CreateFrame();
                }
                //delay so level doesn't end before dots are created
                if (timeInState > 0.1f) {
                    Dots dots = FindObjectOfType<Dots>();
                    if (dots && dots.CountDots() <= 0) {
                        state = State.GAME_MODE_END_LEVEL;
                    }
                }
                if (timeInState > 10f) {//wait till game is underway
                    Pickup p = FindObjectOfType<Pickup>();
                    if (!p) {//make sure there isn't a pickup already
                        if (UnityEngine.Random.value < 1f / (60f * 60f)) {
                            //every 3600 frames give or take (about every 60 seconds, but at random times)
                            //spawn a pickup
                            GameObject g = Instantiate(pbPrefab);
                            //place it near where the player started
                            g.transform.position = new Vector3(33, 27);
                        }
                    }
                }
                break;

            case State.GAME_MODE_START_LEVEL:
                //wait a bit to prevent glitch at beginning of level
                if (framesInState == 10) {
                    if (music) {
                        music.PlayTune("R8 O4 G G G G G");
                    }
                    GetHighScoreObject().Set(highScores.GetScore(1));
                    if (PlayerPrefs.GetInt("Score") > GetHighScoreObject().Get()) {
                        GetHighScoreObject().Set(PlayerPrefs.GetInt("Score"));
                    }
                }
                if (framesInState == 0) {
                    SceneManager.LoadScene("Game");//reload level
                }
                if (timeInState > 0.5f) {
                    state = State.GAME_MODE_RESET_PLAYER;
                }
                break;

            case State.GAME_MODE_DEATH:
                if (framesInState == 0) {
                    if (recordThisGame) {
                        recorder.StopRecording();
                        recordThisGame = false;
                    }
                }
                if (framesInState == 0) {
                    PlayerPrefs.SetInt("Score", GetScoreObject().Get());
                    if (music) {//death music
                        music.PlayTune("R8 O4 C O3 C");
                    }
                }
                if (framesInState == 7) {
                    numberOfLivesRemaining--;
                    if (numberOfLivesRemaining <= 0) {
                        state = State.GAME_MODE_GAME_OVER;
                    }
                }
                if (framesInState < 60) {//a "death animation"
                    PlayerController player = FindObjectOfType<PlayerController>();
                    player.transform.eulerAngles += new Vector3(0, 0, 5);
                }
                if (framesInState == 60) {
                    PlayerController player = FindObjectOfType<PlayerController>();
                    Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
                    state = State.GAME_MODE_RESET_PLAYER;
                    player.transform.position = player.initialPosition;
                    player.transform.eulerAngles = player.initialRotation;
                    rb.velocity = Vector3.zero;
                    foreach (AIController enemy in FindObjectsOfType<AIController>()) {
                        enemy.ResetEnemy();
                    }
                }
                break;

            case State.GAME_MODE_GAME_OVER:
                if (framesInState == 0) {
                    Instantiate(gameOverPrefab, FindObjectOfType<Canvas>().transform);
                    Destroy(FindObjectOfType<PlayerController>().gameObject);
                    placeInHighScoreList = highScores.NewHighScore(GetScoreObject().Get(), Environment.UserName);
                }
                if (framesInState == 240) {
                    if (placeInHighScoreList > 0) {
                        state = State.POST_GAME_MODE_NEW_HIGH_SCORE;
                    } else {
                        state = State.ATTRACT_MODE_TITLE;
                        SceneManager.LoadScene(titleScene);
                    }
                }
                break;

            case State.GAME_MODE_RESET_PLAYER:
                if (framesInState == 0) {
                    if (music) {
                        music.PlayTune("R8 O3 B- R4 B-");
                    }
                }
                if (framesInState == 7) {
                    GetScoreObject().Set(PlayerPrefs.GetInt("Score"));
                }
                if (timeInState > 1f) {
                    state = State.GAME_MODE_PLAY;
                }
                break;

            case State.GAME_MODE_END_LEVEL:
                if (framesInState == 0) {
                    if (recordThisGame) {
                        recorder.StopRecording();
                        recordThisGame = false;
                    }
                    numberOfLevelsPlayed++;
                }
                if (framesInState == 0) {
                    PlayerPrefs.SetInt("Score", GetScoreObject().Get());
                    if (music) {
                        music.PlayTune("R16 O4 E- G B- E- G B- E- G F A- G D E-");
                    }
                }
                if (timeInState > 2f) {
                    state = State.GAME_MODE_LEVEL_UP;
                }
                break;

            case State.GAME_MODE_LEVEL_UP:
                if (framesInState == 0) {
                    if (music) {
                        music.PlayTune("R8 O4 E- B-");
                    }
                }
                if (framesInState == 30) {// about 1/2 second
                    if (numberOfLevelsPlayed == 2 || numberOfLevelsPlayed == 4) {
                        state = GameStateManager.State.GAME_MODE_CUTSCENE;
                    } else {
                        state = GameStateManager.State.GAME_MODE_START_LEVEL;
                    }
                }
                break;

            case State.GAME_MODE_CUTSCENE:
                if (framesInState == 0) {
                    if (numberOfLevelsPlayed == 2) {
                        SceneManager.LoadScene("Cutscene1");
                    }
                    if (numberOfLevelsPlayed == 4) {
                        SceneManager.LoadScene("Cutscene2");
                    }
                }
                break;

            case State.GAME_MODE_OPTIONS:
                if (framesInState == 0) {//if beginning of state
                    SceneManager.LoadSceneAsync("Options",
                    LoadSceneMode.Additive);
                }
                break;

            case State.POST_GAME_MODE_NEW_HIGH_SCORE:
                if (framesInState == 0) {//if beginning of state
                    SceneManager.LoadScene("NewHighScore");
                }
                if (framesInState == 1800) {
                    state = State.POST_GAME_MODE_HIGH_SCORES;
                }
                break;

            case State.POST_GAME_MODE_HIGH_SCORES:
                if (framesInState == 0) {
                    SceneManager.LoadScene("HighScores");
                }
                if (framesInState == 5) {
                    FindObjectOfType<ListOfScores>().highlight = placeInHighScoreList;
                }
                if (timeInState > 10) {
                    state = State.ATTRACT_MODE_TITLE;
                    SceneManager.LoadScene(titleScene);
                }
                break;

            case State.POST_GAME_MODE_OPTIONS:
                if (framesInState == 0) {//if beginning of state
                    SceneManager.LoadSceneAsync("Options",
                    LoadSceneMode.Additive);
                }
                break;

            default:
                throw new UnityException("Unknown Game State");
        }
    }

    public void LoadPreviousState() {
        state = lastState;
    }

    Score GetScoreObject() {
        foreach (Score s in FindObjectsOfType<Score>()) {
            if (s.gameObject.name == "Score") {
                return s;
            }
        }
        throw new UnityException("Score object not found");
    }

    Score GetHighScoreObject() {
        foreach (Score s in FindObjectsOfType<Score>()) {
            if (s.gameObject.name != "Score") {
                return s;
            }
        }
        throw new UnityException("High Score object not found");
    }


    public enum State {
        INVALID,
        INIT,
        ATTRACT_MODE_TITLE,
        ATTRACT_MODE_DESCRIPTION,
        ATTRACT_MODE_HIGH_SCORES,
        ATTRACT_MODE_DEMO,
        ATTRACT_MODE_OPTIONS,
        GAME_MODE_INTRO,
        GAME_MODE_PLAY,
        GAME_MODE_START_LEVEL,
        GAME_MODE_DEATH,
        GAME_MODE_GAME_OVER,
        GAME_MODE_RESET_PLAYER,
        GAME_MODE_END_LEVEL,
        GAME_MODE_LEVEL_UP,
        GAME_MODE_CUTSCENE,
        GAME_MODE_OPTIONS,
        POST_GAME_MODE_NEW_HIGH_SCORE,
        POST_GAME_MODE_HIGH_SCORES,
        POST_GAME_MODE_OPTIONS
    }
}