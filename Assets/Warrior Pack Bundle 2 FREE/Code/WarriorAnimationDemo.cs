using UnityEngine;
using System.Collections;

public class WarriorAnimationDemo : MonoBehaviour {

	public Animator animator;

	float rotationSpeed = 30;
	Vector3 inputVec;

	bool isMoving;
	bool isStunned;
	
	//Warrior types
	public enum Warrior{Karate, Ninja, Brute, Sorceress, Knight, Mage, Archer, TwoHanded};
	public Warrior warrior;

    string[] joyStickNum = { "0", "1", "2", "3" };
    public string joyStickKeyFlag;
	
	void Update()
	{
        float x = -1.0f;
        float z = -1.0f;

        switch(warrior)
        {
            case Warrior.Karate:
                break;

            case Warrior.Ninja:
                break;

            case Warrior.Brute:
                break;

            case Warrior.Sorceress:
                break;

            case Warrior.Knight:
                x = Input.GetAxisRaw("Horizontal" + joyStickNum[0]);
                z = Input.GetAxisRaw("Vertical" + joyStickNum[0]);
                joyStickKeyFlag = joyStickNum[0];
                break;

            case Warrior.Mage:
                x = Input.GetAxisRaw("Horizontal" + joyStickNum[3]);
                z = Input.GetAxisRaw("Vertical" + joyStickNum[3]);
                joyStickKeyFlag = joyStickNum[3];
                break;

            case Warrior.Archer:
                x = Input.GetAxisRaw("Horizontal" + joyStickNum[2]);
                z = Input.GetAxisRaw("Vertical" + joyStickNum[2]);
                joyStickKeyFlag = joyStickNum[2];
                break;

            case Warrior.TwoHanded:
                x = Input.GetAxisRaw("Horizontal" + joyStickNum[1]);
                z = Input.GetAxisRaw("Vertical" + joyStickNum[1]);
                joyStickKeyFlag = joyStickNum[1];
                break;
        }

		//Get input from controls
		inputVec = new Vector3(x, 0, z);

		//Apply inputs to animator
		animator.SetFloat("Input X", x);
		animator.SetFloat("Input Z", z);

		if (x != 0 || z != 0 )  //if there is some input
		{
			//set that character is moving
			animator.SetBool("Moving", true);
			isMoving = true;
			animator.SetBool("Running", true);
		}
		else
		{
			//character is not moving
			animator.SetBool("Moving", false);
			animator.SetBool("Running", false);
			isMoving = false;
		}

        if (Input.GetButtonDown("Fire1" + joyStickKeyFlag))
		{
			animator.SetTrigger("Attack1Trigger");
			if (warrior == Warrior.Brute)
				StartCoroutine (COStunPause(1.2f));
			else if (warrior == Warrior.Sorceress)
				StartCoroutine (COStunPause(1.2f));
			else
				StartCoroutine (COStunPause(.6f));
		}

        if(Input.GetButtonDown("Jump"))
        {

        }

		UpdateMovement();  //update character position and facing
	}

	public IEnumerator COStunPause(float pauseTime)
	{
		isStunned = true;
		yield return new WaitForSeconds(pauseTime);
		isStunned = false;
	}
	
	void RotateTowardsMovementDir()  //face character along input direction
	{
		if (inputVec != Vector3.zero)
		{
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(inputVec), Time.deltaTime * rotationSpeed);
		}
	}

	float UpdateMovement()
	{
		Vector3 motion = inputVec;  //get movement input from controls

		//reduce input for diagonal movement
		motion *= (Mathf.Abs(inputVec.x) == 1 && Mathf.Abs(inputVec.z) == 1)?.7f:1;
		
		RotateTowardsMovementDir();  //if not strafing, face character along input direction

		return inputVec.magnitude;  //return a movement value for the animator, not currently used
	}

	void OnGUI () 
	{
		if (GUI.Button (new Rect (25, 85, 100, 30), "Attack1")) 
		{
			animator.SetTrigger("Attack1Trigger");

			if (warrior == Warrior.Brute || warrior == Warrior.Sorceress)  //if character is Brute or Sorceress
				StartCoroutine (COStunPause(1.2f));
			else
				StartCoroutine (COStunPause(.6f));
		}
	}
}