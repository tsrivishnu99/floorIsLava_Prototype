using UnityEngine;
using System.Collections;

public class player_movementHandler_v1 : MonoBehaviour {

    private CharacterController characterController;

	// Use this for initialization
	void Start () {
        characterController = this.gameObject.GetComponent<CharacterController>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
