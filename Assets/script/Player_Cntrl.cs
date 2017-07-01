using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Player_Cntrl : NetworkBehaviour
{
    GameObject Head;
    GameObject Body;

    // Use this for initialization
    void Start()
    {
        CheckPlayerName();

        gameController.Start2();
        Body = GameObject.Find(this.gameObject.name + "/Body");
        Head = GameObject.Find(this.gameObject.name + "/Head");
    }

    void CheckPlayerName()
    {
        if (this.gameObject.name == "Player(Clone)")
            this.gameObject.name = "Player" + gameController.GetNewID();
    }

    float speed = 6.0f;
    float jumpSpeed = 8.0f;
    float gravity = 20.0f;
    float rotSpeed = 900;

    private Vector3 moveDirection = Vector3.zero;

    public GameObject inventory = null;

    public void Save(GameObject ob)
    {
        if (inventory == null)
        {
            inventory = ob;
            //inventory.GetComponent<MeshCollider>().enabled = false;
            inventory.GetComponent<BoxCollider>().enabled = false;
            inventory.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    public void Load(Vector3 point)
    {
        if (inventory != null)
        {
            inventory.transform.position = point;
            //inventory.GetComponent<MeshCollider>().enabled = false;
            inventory.GetComponent<BoxCollider>().enabled = true;
            inventory.GetComponent<MeshRenderer>().enabled = true;
            inventory = null;
        }
    }

    void Play()
    {
        transform.Rotate(0, Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime, 0);
        float rotationY = Input.GetAxis("Mouse Y") * 10F;
        if (((Mathf.Abs(Vector3.Angle(Head.transform.forward, Body.transform.forward) - rotationY) < 50) && (Vector3.Angle(Head.transform.forward, Body.transform.up) > 90)) || ((Mathf.Abs(Vector3.Angle(Head.transform.forward, Body.transform.forward) + rotationY) < 70) && (Vector3.Angle(Head.transform.forward, Body.transform.up) <= 90)))
            Head.transform.Rotate(new Vector3(-rotationY, 0, 0));

        CharacterController controller = GetComponent<CharacterController>();
        if (controller.isGrounded)
        {
            // We are grounded, so recalculate
            // move direction directly from axes
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0,
                                    Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }

        // Apply gravity
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller
        controller.Move(moveDirection * Time.deltaTime);
    }

    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        gameController.Update2();
        if (!gameController.paused)
            Play();
    }
}