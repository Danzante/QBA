using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Player: NetworkBehaviour {
    Ray myray;
    RaycastHit help;
    const float dist = 5;

    public GameObject parrow;
    public GameObject pdisk;
    public GameObject psmoke;

    GameObject Head;
    GameObject Body;
    
    float v = 0;

    // Use this for initialization
    void Start ()
    {
        CheckPlayerName();
        Body = GameObject.Find(this.gameObject.name + "/Body");
        Head = GameObject.Find(this.gameObject.name + "/Head");
    }

    void CheckPlayerName()
    {
        if (this.gameObject.name == "Player(Clone)")
            this.gameObject.name = "Player" + gameController.GetNewID();
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;

        if (!gameController.paused)
        {
            Update2();
        }
    }

    [Command]
    void CmdStartArrow()
    {
        GameObject arrow = GameObject.Instantiate(parrow);
        arrow.transform.position = Head.transform.position + Head.transform.forward * 0.6f;
        arrow.transform.rotation = Head.transform.rotation;
        arrow.transform.Rotate(new Vector3(90, 0, 0));
        arrow.GetComponent<ArrowCtrl>().Init();

        NetworkServer.Spawn(arrow);
    }

    [Command]
    void CmdStartSmoke()
    {
        GameObject smoke = GameObject.Instantiate(psmoke);
        smoke.transform.position = Head.transform.position - Head.transform.up * 2;

        NetworkServer.Spawn(smoke);
    }

    [Command]
    void CmdStartDisk(float vel, bool t)
    {
        GameObject disk = GameObject.Instantiate(pdisk);
        disk.transform.position = Head.transform.position + Head.transform.forward * 1f;
        disk.transform.rotation = Head.transform.rotation;
        disk.GetComponent<DiskCtrl>().Init(vel, t);

        NetworkServer.Spawn(disk);
    }

    // Update is called once per frame
    void Update2()
    {
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    myray = new Ray(transform.position, transform.forward);
        //    Physics.Raycast(myray, out help, dist);
        //    if (help.collider.gameObject.tag == "Activable")
        //    {
        //        help.collider.gameObject.GetComponent<ActivateCtrl>().Activate();
        //    }
        //}
        if (Input.GetKeyDown(KeyCode.M)) //Mouse1))
        {
            CmdStartArrow();
        }
        if (Input.GetKeyDown(KeyCode.B)) //Mouse1))
        {
            CmdStartSmoke();
        }
        if (Input.GetKey(KeyCode.N)) //Mouse1))
        {
            v += 2 * Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.N)) //Mouse1))
        {
            CmdStartDisk(v, true);
            v = 0;
        }
    }
}
