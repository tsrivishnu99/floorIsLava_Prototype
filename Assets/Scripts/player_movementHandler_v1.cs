using UnityEngine;
using System.Collections;

public class player_movementHandler_v1 : MonoBehaviour {

    private CharacterController characterController;
    private Vector3 moveDirection;
    private Vector3 forwardDirection;

    public int playerNumber = 0;
    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public float rotationSpeed = 2.0f;

	// Use this for initialization
	void Start () {
        characterController = this.gameObject.GetComponent<CharacterController>();
        moveDirection = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate()
    {
        if (characterController.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal" + playerNumber.ToString()), 0, Input.GetAxis("Vertical" + playerNumber.ToString()));
            moveDirection = this.transform.TransformDirection(moveDirection);
            moveDirection *= speed;

            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed;
        }

        moveDirection.y -= gravity * Time.deltaTime;
        characterController.Move(moveDirection * Time.deltaTime);

        forwardDirection = this.transform.forward;
        forwardDirection = Quaternion.Euler(new Vector3(Input.GetAxis("VerticalR" + playerNumber.ToString()) * rotationSpeed, 
            Input.GetAxis("HorizontalR" + playerNumber.ToString()) * rotationSpeed,
            0)) * forwardDirection;   //+(playerNumber+1).ToString()

        this.transform.forward = forwardDirection;
    }
}
