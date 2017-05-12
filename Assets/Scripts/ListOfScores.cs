using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListOfScores : MonoBehaviour {

    public Text names;
    public Text scores;
    public int highlight = -1;

    private int oldHighlightValue = -1;

    private HighScores highScores = HighScores.instance;

    // Use this for initialization
    void Start() {
        names.text = "";
        scores.text = "";
        for (int i = 1; i < 10; i++) {
            if (i == highlight) {
                names.text += "<b><color=red>" + highScores.GetName(i) + "</color></b>\n";
                scores.text += "<b><color=white>" + highScores.GetScore(i).ToString() + "</color></b>\n";
            } else {
                names.text += highScores.GetName(i) + "\n";
                scores.text += highScores.GetScore(i).ToString() + "\n";
            }
        }
    }

    // Update is called once per frame
    void Update () {
        if (oldHighlightValue != highlight) {
            Start();
            oldHighlightValue = highlight;
        }
	}
}
