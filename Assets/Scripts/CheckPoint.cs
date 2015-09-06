using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour {

	public GameObject[] track;
	private int counter;

	// Use this for initialization
	void Start () {
		counter = 0;
		this.transform.position = new Vector3 (track [counter].transform.position.x, track [counter].transform.position.y, track [counter].transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.name == "RigidBodyFPSController") {
			counter++;
			if (counter == track.Length)
				counter = 0;

			this.transform.position = new Vector3 (track [counter].transform.position.x, track [counter].transform.position.y, track [counter].transform.position.z);
		}
	}

}
