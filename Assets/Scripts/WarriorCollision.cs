using UnityEngine;
using System.Collections;

public class WarriorCollision : MonoBehaviour {

	private bool collided;
	private float hitDisplacement = 5000.0f;
	private Rigidbody rb;
    private Transform attack;

	// Use this for initialization
	void Start () 
	{
		collided = false;
		rb = gameObject.GetComponent<Rigidbody> ();
	}

	void FixedUpdate()
	{
		if(collided)
		{
			rb.AddForce(attack.forward * hitDisplacement);
			collided = false;
		}
	}
	
	void OnTriggerEnter(Collider other)
	{
		if((other.gameObject.tag == "Player" || other.gameObject.tag == "Sword") && !other.transform.IsChildOf(transform))
		{
			collided = true;
            attack = other.gameObject.transform;
		}
	}
}
