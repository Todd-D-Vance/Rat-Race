using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/**
 * Maintain a top-ten list of high scores
 *
 */
public class HighScores : MonoBehaviour {

    public static HighScores instance;

    List<HighScoreEntry> scores = new List<HighScoreEntry>();

    private void Awake() {
        instance = this;
    }

    // Use this for initialization
    void Start() {
        //load high scores from memory
        for (int i = 1; i <= 10; i++) {
            string name;
            int score;
            double when;
            string sKey = "High Score " + i;
            string nKey = "Name For Score " + i;
            string wKey = "When For Score " + i;

            TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));

            if (PlayerPrefs.HasKey(sKey)) {
                score = PlayerPrefs.GetInt(sKey);
            } else {
                score = 0;
            }
            if (PlayerPrefs.HasKey(nKey)) {
                name = PlayerPrefs.GetString(nKey);
            } else {
                name = "<Nobody>";
            }
            if (PlayerPrefs.HasKey(wKey)) {
                when = PlayerPrefs.GetFloat(wKey);
            } else {
                when = span.TotalSeconds;
            }
            scores.Add(new HighScoreEntry() { score = score, name = name, when = span.TotalSeconds });
            scores.Sort();
        }
    }

    //Return positive index if this score makes the top ten
    public int NewHighScore(int score, string name) {
        int place = -1;
        for (int i = 10; i >= 1; i--) {
            if (score > scores[i - 1].score) {
                place = i;
            }
        }

        TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
        scores.Add(new HighScoreEntry() { score = score, name = name, when = span.TotalSeconds });
        scores.Sort();


        //save top tens scores
        for (int i = 1; i <= 10; i++) {

            string sKey = "High Score " + i;
            string nKey = "Name For Score " + i;
            string wKey = "When For Score " + i;

            if (i <= scores.Count) {
                PlayerPrefs.SetInt(sKey, scores[i - 1].score);
                PlayerPrefs.SetString(nKey, scores[i - 1].name);
                PlayerPrefs.SetFloat(wKey, (float)scores[i - 1].when);
            }
        }
        return place;
    }

    public int GetScore(int i) { //i should be from 1 to 10
        if (i > scores.Count) {
            return 0;
        }
        return scores[i - 1].score;
    }

    public string GetName(int i) { //i should be from 1 to 10
        if (i > scores.Count) {
            return "<Nobody>";
        }
        return scores[i - 1].name;
    }

    public void SetName(int i, string name) {//i should be from 1 to 10
        scores[i - 1].name = name;
        string nKey = "Name For Score " + i;
        PlayerPrefs.SetString(nKey, scores[i - 1].name);
    }


    public void Reset() {
        scores.Clear();
        for (int i = 1; i <= 10; i++) {
            string sKey = "High Score " + i;
            string nKey = "Name For Score " + i;
            string wKey = "When For Score " + i;
            PlayerPrefs.DeleteKey(sKey);
            PlayerPrefs.DeleteKey(nKey);
            PlayerPrefs.DeleteKey(wKey);
            TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
            scores.Add(new HighScoreEntry() { score = 0, name = "<Nobody>", when = span.TotalSeconds });
        }
    }
}

/**
 *  A high score entry.  Maintains name, score, and when.  The purpose of when is, if
 *  two people have the same score, whoever scored it first gets priority.
 */
public class HighScoreEntry : IComparable {
    public string name;
    public int score;
    public double when;

    public int CompareTo(object obj) {
        HighScoreEntry other = (HighScoreEntry)obj;
        if (score > other.score) { return -1; }
        if (score < other.score) { return 1; }
        if (when > other.when) { return 1; }
        if (when < other.when) { return -1; }
        return 0;
    }
}
