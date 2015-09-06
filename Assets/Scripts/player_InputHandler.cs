using UnityEngine;
using System.Collections;

public class player_InputHandler : MonoBehaviour {

    private Rigidbody player;
    private bool leftClamped;
    private bool rightClamped;
    private Vector3 leftHitPoint;
    private Vector3 rightHitPoint;
    public Camera fpsCam;
    RaycastHit hit;
    public GameObject RopeFirst;
    public GameObject RopeLast;
    public GameObject[] Rope;
	// Use this for initialization
	void Start () {
        player = GetComponent<Rigidbody>();
        leftClamped = false;
        rightClamped = false;
        leftHitPoint.x = 0;
        leftHitPoint.y = 0;
        leftHitPoint.z = 0;
        rightHitPoint = leftHitPoint;
	}
	
	// Update is called once per frame
	void Update () {

        Cursor.lockState = CursorLockMode.Locked;

        if(Input.GetKey(KeyCode.Space))
        {
            player.AddForce(fpsCam.transform.forward * 5.0f, ForceMode.Impulse);
        }

        if(Input.GetKey(KeyCode.LeftControl))
        {
            player.AddForce(-fpsCam.transform.forward * 3.0f, ForceMode.Impulse);
        }

        // Handle left button click, [ Find the point of collision and store the point ]
        if(Input.GetMouseButtonDown(0))
        {            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                leftClamped = true;
                leftHitPoint = hit.point - transform.position;
                //player.AddForce(leftHitPoint * 5.0f, ForceMode.Impulse);
                //leftHitPoint = hit.point;

                RopeFirst.transform.position = hit.point;// -(leftHitPoint * 2.0f);
                //RopeFirst.gameObject.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
                RopeLast.transform.position = transform.position;// +(leftHitPoint * 0.5f);
                
                HingeJoint HJ;

                Rope[0].gameObject.transform.position = hit.point - (leftHitPoint * 0.1f);
                //Rope[0].gameObject.transform.rotation = new Quaternion(0.0f,0.0f,0.0f,0.0f);
                HJ = Rope[0].gameObject.GetComponent<HingeJoint>();
                HJ.connectedBody = RopeFirst.gameObject.GetComponent<Rigidbody>();
                //HJ.axis = Vector3.Cross(this.transform.up, this.transform.forward);
                HJ.anchor = Rope[0].transform.InverseTransformPoint(RopeFirst.transform.position);

                for (int i = 1; i < 9; i++)
                {
                    Rope[i].gameObject.transform.position = hit.point - (leftHitPoint * 0.1f * (i + 1));
                    //Rope[i].gameObject.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
                    HJ = Rope[i].gameObject.GetComponent<HingeJoint>();
                    HJ.connectedBody = Rope[i-1].gameObject.GetComponent<Rigidbody>();
                    //HJ.axis = Vector3.Cross(this.transform.up, this.transform.forward);
                    HJ.anchor = Rope[i].transform.InverseTransformPoint(Rope[i-1].transform.position);
                }

                this.gameObject.AddComponent<HingeJoint>();
                HJ = this.gameObject.GetComponent<HingeJoint>();
                HJ.connectedBody = RopeLast.gameObject.GetComponent<Rigidbody>() ;
                //HJ.axis = new Vector3(1.0f,1.0f,1.0f);
                HJ.anchor = transform.InverseTransformPoint(RopeLast.transform.position);
            }
        }



        // Handle right button click, [ Find the point of collision and store the point ]
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                rightClamped = true;
                rightHitPoint = Vector3.Normalize(hit.point - transform.position);
                player.AddForce(rightHitPoint * 3.0f, ForceMode.Impulse);
                rightHitPoint = hit.point;
            }
        }


        // Handle left button held Down [ Using the point of collision, compute the direction and add force in that direction ]
        if (Input.GetMouseButton(0) && leftClamped == true)
        {
            Vector3 hitDir = Vector3.Normalize(leftHitPoint - transform.position);
            player.AddForce(hitDir * 3.0f, ForceMode.Impulse);
        }


        // Handle right button held Down [ Using the point of collision, compute the direction and add force in that direction ]
	    if(Input.GetMouseButton(1) && rightClamped == true)
        {
             Vector3 hitDir = Vector3.Normalize(rightHitPoint - transform.position);
             player.AddForce(hitDir * 3.0f, ForceMode.Impulse);
        }


        //Release the clamps for left and right respectively
        if (Input.GetKey(KeyCode.Q))
        {
            leftClamped = false;
            Destroy(this.gameObject.GetComponent<HingeJoint>());
        }
        if(Input.GetKey(KeyCode.E))
        {
            rightClamped = false;
        }
	}

    //void OnCollisionEnter(Collision Col)
    //{
    //    player.velocity = new Vector3(0, 0, 0);
    //}
}
