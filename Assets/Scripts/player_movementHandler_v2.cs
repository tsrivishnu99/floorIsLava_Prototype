using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class player_movementHandler_v2 : MonoBehaviour
{
    public int playerNumber = 0;
    public float speed = 10.0f;
    public float gravity = 10.0f;
    public float maxVelocityChange = 10.0f;
    public float boost = 0.1f;
    public bool canJump = true;
    public float jumpHeight = 2.0f;
    public float rotationSpeed = 2.0f;
    public bool grounded = false;

    private Rigidbody rigidbody;
    private Vector3 forwardDirection;

    void Awake()
    {
        rigidbody = this.gameObject.GetComponent<Rigidbody>();
        rigidbody.freezeRotation = true;
        rigidbody.useGravity = false;
    }

    void FixedUpdate()
    {
        if (grounded)
        {
            // Calculate how fast we should be moving
            Vector3 targetVelocity = new Vector3(Input.GetAxis("HorizontalL" + playerNumber.ToString()), 0, Input.GetAxis("VerticalL" + playerNumber.ToString()));
            targetVelocity = transform.TransformDirection(targetVelocity);
            targetVelocity *= speed;

            // Apply a force that attempts to reach our target velocity
            Vector3 velocity = rigidbody.velocity;
            Vector3 velocityChange = (targetVelocity - velocity);
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = 0;
            rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);

            // Jump
            if (canJump && Input.GetButton("Jump"+ playerNumber.ToString()))
            {
                rigidbody.velocity = new Vector3(velocity.x, CalculateJumpVerticalSpeed(), velocity.z);
            }
        }
        else
        {
            //For ariel Control
            Vector3 targetVelocity = new Vector3(Input.GetAxis("HorizontalL" + playerNumber.ToString()), 0, Input.GetAxis("VerticalL" + playerNumber.ToString()));
            targetVelocity = transform.TransformDirection(targetVelocity);
            targetVelocity *= boost;
            rigidbody.AddForce(targetVelocity, ForceMode.Impulse);
        }


        // We apply gravity manually for more tuning control
        rigidbody.AddForce(new Vector3(0, -gravity * rigidbody.mass, 0));

        grounded = false;

        //Control Camera (look)
        forwardDirection = this.transform.forward;
        forwardDirection = Quaternion.Euler(new Vector3(0, Input.GetAxis("HorizontalR" + playerNumber.ToString()) * rotationSpeed, 0)) * forwardDirection;   //+(playerNumber+1).ToString()
        Vector3 temp = forwardDirection;
        forwardDirection = Quaternion.Euler(Vector3.Cross(this.transform.forward, this.transform.up) * Input.GetAxis("VerticalR" + playerNumber.ToString()) * rotationSpeed) * forwardDirection;
        
        //To ensure we dont encounter Gimbal lock
        if (Vector3.Angle(forwardDirection, new Vector3(0, 1, 0)) < 10.0f || Vector3.Angle(forwardDirection, new Vector3(0, 1, 0)) >170.0f)
            this.transform.forward = temp;
        else
            this.transform.forward = forwardDirection;
    }

    void OnCollisionStay()
    {
        grounded = true;
    }

    float CalculateJumpVerticalSpeed()
    {
        // From the jump height and gravity we deduce the upwards speed 
        // for the character to reach at the apex.
        return Mathf.Sqrt(2 * jumpHeight * gravity);
    }
}