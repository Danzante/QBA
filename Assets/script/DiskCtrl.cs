using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DiskCtrl : NetworkBehaviour
{

    Vector3 velocity;
    Vector3 startPos;
    Vector3 force, lastforce;
    Rigidbody rig;

    // Use this for initialization
    void Start () {
        rig = this.GetComponent<Rigidbody>();
        lastforce = new Vector3();
    }

    bool b;

    int c1;
    
    public void Init(float v, bool t)
    {
        rig = this.GetComponent<Rigidbody>();
        velocity = new Vector3(0, 0, v);
        velocity = transform.TransformDirection(velocity);
        lastforce = new Vector3();
        rig.velocity = velocity;
        startPos = this.transform.position;
        b = false;
        if (t)
        {
            c1 = -1;
        }
        else
        {
            c1 = 1;
        }
        lifetime = 0;
    }

    void OnCollisionEnter(Collision collision)
    {
        var hit = collision.gameObject;
        var hitPlayer = hit.GetComponentInParent<HPCtrl>();
        if (hitPlayer != null)
        {
            hitPlayer.Strike(50);
        }
        Destroy(gameObject);
    }

    float lifetime = 0;

    void Play()
    {
        lifetime += Time.deltaTime;
        Vector3 shift = this.transform.position - startPos;
        if (lifetime > 20)
        {
            Destroy(this.gameObject);
        }
        if (shift.magnitude < 0.1f)
        {
            if (b)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            b = true;
        }

        velocity = rig.velocity;

        //было + в ините был c2 = -c1
        //force = new Vector3(c1 * velocity.z, 0, c2 * velocity.x);
        //force.Normalize();    

        //стало
        this.transform.LookAt(this.transform.position + rig.velocity, this.transform.up);
        force = this.transform.right;
        force = c1 * force * rig.mass;
        
        rig.AddForce(force, ForceMode.Impulse);
    }
	
	// Update is called once per frame
	void Update () {
        gameController.Update2();
        if (!gameController.paused)
            Play();
    }
}
