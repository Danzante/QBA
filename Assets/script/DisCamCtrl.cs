using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DisCamCtrl : NetworkBehaviour {

    Camera cam;

	// Use this for initialization
	void Start () {
        CheckPlayerName();
        cam = GameObject.Find(this.gameObject.name + "/Head/Camera").GetComponent<Camera>();
    }
	
    void CheckPlayerName()
    {
        if(this.gameObject.name == "Player(Clone)")
            this.gameObject.name = "Player" + gameController.GetNewID();
    }

	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer)
            cam.enabled = false;
        else
            cam.enabled = true;
    }
}
