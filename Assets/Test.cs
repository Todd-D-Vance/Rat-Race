using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {
    public int startX = 6;
    public int startY = 1;
    public int destX = 2;
    public int destY = 5;
    public GridPoint[] blocked;


    public GameObject startPrefab;
    public GameObject destPrefab;
    public GameObject pathPrefab;
    public GameObject blockedPrefab;


    AStar aStar;

    Queue<GameObject> todelete = new Queue<GameObject>();

    // Use this for initialization
    void Start() {
        aStar = GetComponent<AStar>();
        aStar.Initialize(8, 8);
        foreach (GridPoint blocked in blocked) {
            aStar.AddBlocked(blocked);
        }
    }

    // Update is called once per frame
    void Update() {
        while (todelete.Count > 0) {
            Destroy(todelete.Dequeue());
        }
        todelete.Enqueue(Instantiate(startPrefab, new Vector3(startX, startY, 0), Quaternion.identity));
        todelete.Enqueue(Instantiate(destPrefab, new Vector3(destX, destY, 0), Quaternion.identity));

        foreach (GridPoint blocked in blocked) {
            todelete.Enqueue(Instantiate(blockedPrefab, new Vector3(blocked.x, blocked.y, 0), Quaternion.identity));

        }

        aStar.RebuildAI(destX, destY);
        GridPoint current = new GridPoint() { x = startX, y = startY };
        GridPoint next;
        while (aStar.GetNext(current, out next)) {
            if (current.x != startX || current.y != startY) {
                todelete.Enqueue(Instantiate(pathPrefab, new Vector3(current.x, current.y, 0), Quaternion.identity));
            }
            current = next;
        }

    }
}