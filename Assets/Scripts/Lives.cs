using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lives : MonoBehaviour {

    public GameObject MousePrefab;

    private GameStateManager gsm = GameStateManager.instance;
    private int numLives = 0;

    // Update is called once per frame
    void Update() {
        //if display needs updating
        if (numLives != gsm.numberOfLivesRemaining) {
            numLives = gsm.numberOfLivesRemaining;

            //delete the mice there
            foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>()) {
                Destroy(sr.gameObject);
            }

            //now add a new set of mice
            for (int i = 0; i < numLives - 1; i++) {
                float y = -numLives + i * 2;
                GameObject g = Instantiate(MousePrefab, transform);
                g.transform.localPosition = new Vector3(0, y, 0);
                g.transform.rotation = Quaternion.identity;
                g.transform.localScale = new Vector3(.5f, .5f, 1);
            }
        }
    }
}