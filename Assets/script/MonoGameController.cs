using UnityEngine;
using System.Collections;

public class MonogameController : MonoBehaviour {

    static GameObject PM;
    static bool active;
    public static bool inited = false;
    public static bool paused { get; private set; }
    static int id = 0;

    public static int GetNewID()
    {
        id++;
        return id - 1;
    }

    public static void Pause()
    {
        active = !active;
        PM.SetActive(active);
        paused = !paused;
    }

    // Update is called once per frame
    public static void Update2()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Pause();
        }
    }

    void Start()
    {
        PM = GameObject.Find("/PauseMenu");
        PM.SetActive(false);
        active = false;
    }

    // Use this for initialization
    public static void Start2()
    {
        if(PM == null)
            PM = GameObject.Find("/PauseMenu");
        PM.SetActive(false);
        active = false;
        paused = false;
    }

    // Update is called once per frame
    void Update () {
	
	}
}
