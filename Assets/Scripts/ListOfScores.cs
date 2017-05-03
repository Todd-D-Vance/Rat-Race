using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ListOfScores : MonoBehaviour {

    public Text names;
    public Text scores;

    private HighScores highScores = HighScores.instance;

    public int highlight = -1;
    private int oldHighlight = -1;
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
        oldHighlight = highlight;
    }

    // Update is called once per frame
    void Update() {
        if (oldHighlight != highlight) {
            Start();
        }
    }
}
