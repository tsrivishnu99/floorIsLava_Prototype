using UnityEngine;
using System.Collections;

public class cameraController : MonoBehaviour {

    // Data Members
    private Vector3 playerLeft;
    private float mousePrevPosX, mousePrevPosY;
    Transform camera;

	// Use this for initialization
	void Start () {
        //Cursor.lockState = CursorLockMode.Locked;
        mousePrevPosX = Input.mousePosition.x;
        mousePrevPosY = Input.mousePosition.y;
        camera = transform.FindChild("playerCam");   
	}

    void cameraControl()
    {
        float dispX = 0, dispY = 0;

        dispX = Input.mousePosition.x - mousePrevPosX;
        dispY = Input.mousePosition.y - mousePrevPosY;

        Vector3 forward = transform.forward;
        
        forward = Quaternion.Euler(0.0f, dispX * 0.1f, 0.0f) * forward;
        forward = Quaternion.Euler(playerLeft *dispY* 0.1f) * forward;
        transform.forward = forward;

        camera.forward = Vector3.Normalize(transform.position - camera.position);

        mousePrevPosX = Input.mousePosition.x;
        mousePrevPosY = Input.mousePosition.y;
    }

	// Update is called once per frame
	void Update () {
        playerLeft = Vector3.Cross(transform.forward, transform.up);
        cameraControl();
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.lockState = CursorLockMode.None;
	}
}
