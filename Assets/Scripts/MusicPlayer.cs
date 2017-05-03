using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {

    public static MusicPlayer instance;

    public bool paused = false;

    private AudioSource source;
    private Preferences preferences = Preferences.instance;

    private float currentOctave = 4;
    private float halfStep = 1.05946309436f;
    private float duration = 0.25f;
    private float timeRemaining = 0;

    private Queue<float> pitches = new Queue<float>();
    private Queue<float> durations = new Queue<float>();

    private void Awake() {
        instance = this;
    }

    // Use this for initialization
    void Start() {
        preferences = Preferences.instance;
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        if (!paused) {
            if (timeRemaining <= 0) {
                if (pitches.Count > 0) {//if any notes remain to be played
                    float pitch = pitches.Dequeue();
                    float duration = durations.Dequeue();
                    source.volume = preferences.musicVolume;
                    source.pitch = pitch;
                    source.Play();
                    timeRemaining = duration;
                }
            } else {
                timeRemaining -= Time.deltaTime;
            }
        }
    }

    public void PlayTune(string tune) {
        //remove any whitespace from the beginning
        // before parsing
        RemoveInitialWhitespace(ref tune);

        //while there is a command at the beginning...
        while (GetAndExecCommand(ref tune)) {
            //execute it, and then remove whitespace 
            //before checking again
            RemoveInitialWhitespace(ref tune);
        }
        //if something is left, it is neither a
        //command nor whitespace, so a syntax error
        if (tune.Length > 0) {
            throw new UnityException("Syntax Error: " + tune);
        }
    }

    bool GetAndExecCommand(ref string tune) {
        if (GetAndPlayNote(ref tune)) {
            return true;
        }
        if (GetAndSetOctave(ref tune)) {
            return true;
        }
        if (GetAndSetRatio(ref tune)) {
            return true;
        }
        return false;
    }

    bool GetAndPlayNote(ref string tune) {
        //compute the pitch to play on the fly
        float pitch = 0;
        if (tune.Length < 1) {
            return false;//empty string means no note
        }
        if (!Char.IsLetter(tune[0])) {
            return false;//non-letter means no note
        }
        //letter not from A to G means no note
        if (tune[0] < 'A' || tune[0] > 'G') {
            if (tune[0] < 'a' || tune[0] > 'g') {
                return false;
            }
        }
        //convert note to number of half steps above C
        int note = 0;//C is 2^0 == 1, so normalize to C=0
                     //set notes; see piano keyboard for when to add 1 vs 2 for next note
        switch (tune.Substring(0, 1).ToUpper()[0]) {
            case 'C':
                note = 0;
                break;
            case 'D':
                note = 2;
                break;
            case 'E':
                note = 4;
                break;
            case 'F':
                note = 5;
                break;
            case 'G':
                note = 7;
                break;
            case 'A':
                note = 9;
                break;
            case 'B':
                note = 11;
                break;
            default:
                return false;//should never get here
        }
        //Octave 4 is where pitch 1 is found, so normalize to that
        pitch = Mathf.Pow(2, note / 12f + currentOctave - 4);

        //remove the just-parsed letter from the string
        tune = tune.Substring(1);

        //remove whitespace before looking for + or -
        RemoveInitialWhitespace(ref tune);

        //if next character is + or -, adjust by a half step
        if (tune.Length > 0 && (tune[0] == '+' || tune[0] == '-')) {
            if (tune[0] == '+') {
                pitch *= halfStep;
            } else {
                pitch /= halfStep;
            }
            tune = tune.Substring(1);
        }

        //put the note and current duration on the queue
        pitches.Enqueue(pitch);
        durations.Enqueue(duration);
        return true;
    }

    bool GetAndSetOctave(ref string tune) {
        if (tune.Length < 1) {//empty string means 
                              //no command
            return false;
        }
        if (!Char.IsLetter(tune[0])) {//not a letter 
                                      //means no command
            return false;
        }
        //only O or o is an octave command
        if (tune[0] != 'O' && tune[0] != 'o') {
            return false;
        }

        //remove just-parsed letter
        tune = tune.Substring(1);

        //Set the octave to the next number 
        //in the string, and remove it 
        //from the string
        currentOctave = GetNumber(ref tune);
        return true;
    }

    bool GetAndSetRatio(ref string tune) {
        //not a ratio of empty string
        if (tune.Length < 1) {
            return false;
        }
        //not a ratio if not a letter
        if (!Char.IsLetter(tune[0])) {
            return false;
        }
        //not a ratio if not R or r
        if (tune[0] != 'R' && tune[0] != 'r') {
            return false;
        }

        //remove command letter from string
        tune = tune.Substring(1);

        //set duration to reciprocal of number in tune
        duration = 1f / GetNumber(ref tune);

        return true;
    }

    public static void RemoveInitialWhitespace(ref string s) {
        //count whitespace
        int i = 0;
        while (i < s.Length && Char.IsWhiteSpace(s[i])) {
            i++;
        }
        //remove first i characters from string
        s = s.Substring(i);
    }

    public static int GetNumber(ref string tune) {
        //remove any optional white space first
        RemoveInitialWhitespace(ref tune);

        //empty string is not a number
        if (tune.Length < 1) {
            throw new UnityException("Number expected: " + tune);
        }
        //all numbers begin with 1 through 9 in this code
        if (tune[0] < '1' || tune[0] > '9') {
            throw new UnityException("Number expected: " + tune);
        }

        //convert the digit to a number
        int num = tune[0] - '0';

        //remove the digit just found
        tune = tune.Substring(1);

        //look for more digits
        while (tune.Length > 0 && tune[0] >= '0' && tune[0] <= '9') {
            //if a digit is found, adjust the 
            //number to match
            num = 10 * num + (tune[0] - '0');
            //and remove digit from beginning of string
            tune = tune.Substring(1);
        }
        return num;
    }
}
