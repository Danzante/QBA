using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReinSwordCtrl : MonoBehaviour {

    GameObject Body;
    GameObject Head;

    void Start()
    {
        Body = this.transform.parent.gameObject;
        Head = this.transform.parent.parent.Find("Head").gameObject;
        startpos = this.transform.localPosition;
        startrot = this.transform.localRotation;
    }

    Vector3 startpos;
    Quaternion startrot;

    public void init(Vector3 point1, Vector3 point2)
    {
        if (!active)
        {
            active = true;
            way = 0;
            this.transform.position = Body.transform.position + point1;
            Vector3 velocity = point2 - point1;
            maxway = velocity.magnitude;
            velocity.Normalize();
            velocity *= 2;
            vmagni = velocity.magnitude;
            this.GetComponent<Rigidbody>().velocity = velocity;
        }
    }

    bool active;
    float way;
    float maxway;
    float vmagni;

    void Play()
    {
        if (active)
        {
            this.transform.LookAt(Head.transform);
            if (way >= maxway)
            {
                this.GetComponent<Rigidbody>().velocity = new Vector3();
                active = false;
            }
            way += vmagni * Time.deltaTime;
        }
        else
        {
            this.transform.localPosition = startpos;
            this.transform.localRotation = startrot;
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
                QBA.QBAEffect eff = new QBA.QBAEffect(90);
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
