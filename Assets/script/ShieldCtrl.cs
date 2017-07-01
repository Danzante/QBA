using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldCtrl : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        startpos = this.transform.localPosition;
        startrot = this.transform.localRotation;
    }

    Vector3 startpos;
    Quaternion startrot;

    public void init()
    {
        if (!active)
        {
            back = false;
            active = true;
            way = 0;
            Vector3 velocity = new Vector3(1, 0, 0);
            velocity = transform.TransformDirection(velocity);
            this.GetComponent<Rigidbody>().velocity = velocity;
        }
    }

    public bool active;
    bool back;
    float way;
    const float maxway = 1;

    void Play()
    {
        if (active)
        {
            this.GetComponent<Rigidbody>().isKinematic = false;
            if (way >= 2 * maxway)
            {
                this.GetComponent<Rigidbody>().velocity = new Vector3();
                this.gameObject.SetActive(false);
                active = false;
            }
            if (way > maxway && !back)
            {
                back = true;
                this.GetComponent<Rigidbody>().velocity = -this.GetComponent<Rigidbody>().velocity;
            }
            way += Time.deltaTime;
        }
        else
        {
            this.GetComponent<Rigidbody>().isKinematic = true;
            this.transform.localPosition = startpos;
            this.transform.localRotation = startrot;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        var hit = collision.gameObject;
        var hitPlayer = hit.GetComponentInParent<MonoHPCtrl>();
        if (hitPlayer != null)
        {
            QBA.QBAEffect eff = new QBA.QBAEffect(60);
            hitPlayer.Strike(eff);
        }
    }

    // Update is called once per frame
    void Update () {
        MonogameController.Update2();
        if (!MonogameController.paused)
            Play();
    }
}
