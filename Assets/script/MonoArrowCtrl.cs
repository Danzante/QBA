using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using QBA;

public class MonoArrowCtrl : MonoBehaviour {

    Rigidbody rig;

    // Use this for initialization
    void Start () {
        rig = this.GetComponent<Rigidbody>();
    }

    float speed = 40.0f;
    float gravity = 20.0f;
    public float lft;

    Vector3 velocity;

    public void Init(QBAEffect ef)
    {
        rig = this.GetComponent<Rigidbody>();
        velocity = new Vector3(0, speed, 0);
        velocity = transform.TransformDirection(velocity);
        velocity += new Vector3(0, -gravity * Time.deltaTime, 0);
        rig.velocity = velocity;
        lifetime = 0;
        eff = ef;
    }

    public void Init(QBAEffect ef, float spdMult)
    {
        rig = this.GetComponent<Rigidbody>();
        velocity = new Vector3(0, speed * spdMult, 0);
        velocity = transform.TransformDirection(velocity);
        velocity += new Vector3(0, -gravity * Time.deltaTime, 0);
        rig.velocity = velocity;
        lifetime = 0;
        eff = ef;
    }

    QBAEffect eff;

    void OnCollisionEnter(Collision collision)
    {
        var hit = collision.gameObject;
        if (hit.tag != "Shield")
        {
            var hitPlayer = hit.GetComponentInParent<MonoHPCtrl>();
            if (hitPlayer != null)
            {
                hitPlayer.Strike(eff);
            }
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
        MonogameController.Update2();
        if (!MonogameController.paused)
            Play();
    }
}
