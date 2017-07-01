using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HPCtrl : NetworkBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    [SyncVar]
    public float HP = 100.0f;

    public void Strike(float dmg)
    {
        if (!isServer)
            return;
        
        HP -= dmg;
        if(HP < 0)
        {
            Die();
        }
    }

    void Die (){

    }

	// Update is called once per frame
	void Update () {
	}
}
