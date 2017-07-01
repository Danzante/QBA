using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ArrowCtrl : NetworkBehaviour {

    Rigidbody rig;

    // Use this for initialization
    void Start () {
        rig = this.GetComponent<Rigidbody>();
    }

    float speed = 40.0f;
    float gravity = 20.0f;
    public float lft;

    Vector3 velocity;

    public void Init()
    {
        rig = this.GetComponent<Rigidbody>();
        velocity = new Vector3(0, speed, 0);
        velocity = transform.TransformDirection(velocity);
        velocity += new Vector3(0, -gravity * Time.deltaTime, 0);
        rig.velocity = velocity;
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

    // Update is called once per frame
    void Play()
    {
        lifetime += Time.deltaTime;
        if (lifetime > 2.5)
        {
            Destroy(this.gameObject);
        }
        if (transform.position.y > 100 || transform.position.y < 0.15)
        {
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        gameController.Update2();
        if (!gameController.paused)
            Play();
    }
}
