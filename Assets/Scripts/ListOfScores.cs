using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListOfScores : MonoBehaviour {

    public Text names;
    public Text scores;

    private HighScores highScores = HighScores.instance;

    // Use this for initialization
    void Start() {
        names.text = "";
        scores.text = "";
        for (int i = 1; i < 10; i++) {
            names.text += highScores.GetName(i) + "\n";
            scores.text += highScores.GetScore(i).ToString() + "\n";
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
