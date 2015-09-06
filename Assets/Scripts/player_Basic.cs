using UnityEngine;
using System.Collections;

public class player_Basic : MonoBehaviour {

    public float speed;
    const float defaultSpeed = 0.5f;
    private Vector3 playerLeft;
    private Transform camPivot;
    private Vector3 camForward;
   // Use this for initialization
    void Start () {
        if (speed == 0.0f)
        {
            speed = defaultSpeed;
        }
        camPivot = transform.FindChild("cameraPivot");
        camForward = camPivot.forward;
        camForward.y = 0.0f;
	}


    void movement()
    {

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKey(KeyCode.W))
        {
            transform.position += camForward * speed;
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKey(KeyCode.S))
        {
            transform.position -= camForward * speed;
        }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKey(KeyCode.A))
        {
            transform.position += playerLeft * speed;
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.D))
        {
            transform.position -= playerLeft * speed;
        }
    }

	// Update is called once per frame
	void Update () {

        camForward = camPivot.forward;
        camForward.y = 0.0f;
        playerLeft = Vector3.Cross(camForward, transform.up);
        movement();
	}
}
