using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class Recorder : MonoBehaviour {

    public static Recorder instance;

    private Prefabs pf;

    private Dictionary<string, GameObject> prefabs =
    new Dictionary<string, GameObject>();

    private List<GameObject> objects = new List<GameObject>();
    private Dictionary<GameObject, int> indices =
        new Dictionary<GameObject, int>();

    private bool recording = false;
    private StreamWriter saveGame;
    private StreamReader playback;

    private void Awake() {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        pf = Prefabs.instance;

        prefabs.Add("cheese", pf.cheese);
        prefabs.Add("cat", pf.cat);
        prefabs.Add("mouse", pf.mouse);
        prefabs.Add("exterior", pf.exterior);
        foreach (GameObject g in pf.solidRegions) {
            SpriteRenderer sr = g.GetComponent<SpriteRenderer>();
            prefabs.Add(sr.name.ToLower(), g);
        }
        foreach (GameObject g in pf.wallTiles) {
            if (g) { //make sure g is not ``None''
                SpriteRenderer sr = g.GetComponent<SpriteRenderer>();
                prefabs.Add(sr.name.ToLower(), g);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    GameObject CreateInstance(string name) {
        GameObject g = Instantiate(prefabs[name.ToLower()]);
        Component c = g.GetComponent<AIController>();
        if (c) {
            Destroy(c);
        }
        c = g.GetComponent<PlayerController>();
        if (c) {
            Destroy(c);
        }
        c = g.GetComponent<Edible>();
        if (c) {
            Destroy(c);
        }
        return g;
    }

    public void ResetRecorder() {
        objects.Clear();
        indices.Clear();
    }

    public void StartPlayback() {
        playback = File.OpenText("Assets/SavedGames/Demo.txt");
        ResetRecorder();
    }

    public void PlayNextFrame() {
        bool sameFrame;
        do {
            string cmd = playback.ReadLine();
            if (cmd == null) {
                break;
            }
            sameFrame = Play(cmd);

        } while (sameFrame);
    }

    /*
  * Return true if still in same frame
  */
    public bool Play(string cmd) {
        //remove the end of line character
        if (cmd.EndsWith("\n")) {
            cmd = cmd.Substring(0, cmd.Length - 1);
        }

        //get the command character
        string c = cmd.Substring(0, 1).ToUpper();


        //remove command letter and the space right after it
        if (cmd.Length > 2) {
            cmd = cmd.Substring(2);
        } else {
            cmd = "";
        }

        //get the arguments of the command as strings
        string[] args = cmd.Split(' ');

        //decide how to execute the command
        switch (c) {
            case "F":
                //new frame, just return false
                return false;
            case "C":
                //create the instance and add to objects.
                GameObject g = CreateInstance(args[0]);
                objects.Add(g);
                //don't need to add to ``indices'' for playback
                break;
            case "M":
                //get the index of the object to be moeed
                int i = int.Parse(args[0]);

                //get the coordinates to move to
                float x = float.Parse(args[1]);
                float y = float.Parse(args[2]);

                //if no z given, use current value
                float z = objects[i].transform.position.z;
                if (args.Length > 3) {
                    z = float.Parse(args[3]);
                }

                //move the object
                objects[i].transform.position = new Vector3(x, y, z);
                break;
            case "R":
                // rotate the object
                i = int.Parse(args[0]);
                z = float.Parse(args[1]);
                objects[i].transform.rotation = Quaternion.Euler(new Vector3(0, 0, z));
                break;
            case "S":
                //scale the object
                i = int.Parse(args[0]);
                x = float.Parse(args[1]);
                y = x;
                z = 1;

                objects[i].transform.localScale = new Vector3(x, y, z);
                break;
            case "T":
                //tint the object
                i = int.Parse(args[0]);
                //figure out the color from the hex value
                float r = FromHex(args[1].Substring(0, 2)) / 255f;
                float gr = FromHex(args[1].Substring(2, 2)) / 255f;
                float b = FromHex(args[1].Substring(4, 2)) / 255f;
                float a = FromHex(args[1].Substring(6, 2)) / 255f;
                Color col = new Color(r, gr, b, a);
                foreach (SpriteRenderer sr in objects[i].GetComponentsInChildren<SpriteRenderer>()) {
                    sr.color = col;
                }
                break;
            case "D":
                //Destroy the object
                i = int.Parse(args[0]);
                Destroy(objects[i]);
                break;
            default:
                //for debugging, in case we forgot 
                //to handle a command
                throw new UnityException("Unrecognized command: " + cmd);
        }


        return true;
    }

    public void StartRecording() {
        saveGame = File.CreateText(
             "Assets/SavedGames/Demo.txt");
        ResetRecorder();
        recording = true;
        CreateFrame();
    }

    void Generate(string cmd) {
        saveGame.Write(cmd);
    }

    public void CreateFrame() {
        Generate("F\n");
    }

    public void RecordIfRecording(string cmd) {
        if (recording) {
            Generate(cmd);
        } else {
            Debug.LogError("not recording");
        }
    }

    public void StopRecording() {
        recording = false;
        saveGame.Close();
    }

    /*
 * output a string for creating a game object from one in game
 */
    public string CreateCommand(GameObject obj) {
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        objects.Add(obj);
        indices[obj] = objects.Count - 1;
        string name = sr.name;
        //remove the "(clone)" suffix
        int i = name.IndexOf('(');
        if (i >= 0) {
            name = name.Substring(0, i);
        }
        string result = "C " + name + "\n";
        return result + ModifyCommand(obj);
    }

    /*
    * output a string for modifying a game object from one in game
    */
    public string ModifyCommand(GameObject obj) {
        int i = indices[obj];
        string result = "M " + i + " " + obj.transform.position.x + " " + obj.transform.position.y;
        if (obj.transform.position.z != 0) {
            result += " " + obj.transform.position.z;
        }
        result += "\n";
        result += "S " + i + " " + obj.transform.localScale.x + "\n";
        result += "R " + i + " " + obj.transform.eulerAngles.z + "\n";
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();

        result += "T " + i + " "
                        + ToHex(sr.color.r * 255)
                        + ToHex(sr.color.g * 255)
                        + ToHex(sr.color.b * 255)
                        + ToHex(sr.color.a * 255)
            + "\n";

        return result;
    }

    /*
     * output a string for moving (with rotation and scale, 
     * but no change in color) a game object from one in game
     */
    public string MoveCommand(GameObject obj) {
        int i = indices[obj];
        string result = "M " + i + " " + obj.transform.position.x + " " + obj.transform.position.y + "\n";
        result += "R " + i + " " + obj.transform.eulerAngles.z + "\n";
        result += "S " + i + " " + obj.transform.localScale.x + "\n";

        return result;
    }


    public string DestroyCommand(GameObject obj) {
        int i = indices[obj];
        return "D " + i + "\n";
    }

    public static string ToHex(float num) {
        string result = "";
        for (int i = 0; i < 2; i++) {
            int c = (int)(num % 16);
            num = num / 16;
            if (c < 10) {
                result = (char)('0' + c) + result;
            } else {
                result = (char)('A' + (c - 10)) + result;
            }
        }
        return result;
    }

    public static float FromHex(string h) {
        float result = 0;
        foreach (char c in h) {
            if (c >= '0' && c <= '9') {
                result = 16 * result + (c - '0');
            } else if (c >= 'A' && c <= 'F') {
                result = 16 * result + (c - 'A' + 10);
            } else if (c >= 'a' && c <= 'f') {
                result = 16 * result + (c - 'a' + 10);
            } else {
                throw new UnityException("Illegal character '" + c + "' in hex value");
            }
        }
        return result;
    }

}
