using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCtrl : MonoBehaviour {

	// Use this for initialization
	void Start () {
        startpos = this.transform.localPosition;
	}

    Vector3 startpos;

    public void init(Vector3 point)
    {
        if (!active)
        {
            back = false;
            active = true;
            way = 0;
            Vector3 velocity = point - this.transform.position;
            maxway = 2 * velocity.magnitude;
            velocity.Normalize();
            velocity *= 5;
            this.GetComponent<Rigidbody>().velocity = velocity;
        }
    }

    bool active;
    bool back;
    float way;
    float maxway;

    void Play()
    {
        if (active)
        {
            if (way >= 2 * maxway)
            {
                this.GetComponent<Rigidbody>().velocity = new Vector3();
                active = false;
            }
            if (way > maxway && !back)
            {
                back = true;
                this.GetComponent<Rigidbody>().velocity = -this.GetComponent<Rigidbody>().velocity;
            }
            way += this.GetComponent<Rigidbody>().velocity.magnitude * Time.deltaTime;
        }
        else
        {
            this.transform.localPosition = startpos;
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        var hit = collision.gameObject;
        if (hit.tag != "Shield")
        {
            var hitPlayer = hit.GetComponentInParent<MonoHPCtrl>();
            if (hitPlayer != null)
            {
                QBA.QBAEffect eff = new QBA.QBAEffect(60);
                hitPlayer.Strike(eff);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        MonogameController.Update2();
        if (!MonogameController.paused)
            Play();
    }
}
