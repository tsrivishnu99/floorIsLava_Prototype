using UnityEngine;
using System.Collections;

public class player_InputHandler_v2 : MonoBehaviour {

    private Rigidbody player;
    private bool leftClamped;
    //private bool rightClamped;
    private Vector3 leftHitPoint;
    private Vector3 rightHitPoint;
    public Camera fpsCam;
    RaycastHit hit;
    public GameObject RopeElement;
    private GameObject[] Rope;

	// Use this for initialization
	void Start () {
	        player = GetComponent<Rigidbody>();
        leftClamped = false;
        //rightClamped = false;
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
            player.AddForce(fpsCam.transform.forward * 2.0f, ForceMode.Impulse);
        }

        if(Input.GetKey(KeyCode.LeftControl))
        {
            player.AddForce(-fpsCam.transform.forward * 0.5f, ForceMode.Impulse);
        }


        // Handle left button click, [ Find the point of collision and store the point ]
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit) && leftClamped == false)
            {
                leftClamped = true;
                leftHitPoint = hit.point - transform.position;
                leftHitPoint.Normalize();
                //player.AddForce(leftHitPoint * 5.0f, ForceMode.Impulse);
                //leftHitPoint = hit.point;

                //HingeJoint HJ;
                ConfigurableJoint HJ;

                float ropeLength = Vector3.Distance(hit.point, transform.position);
                int totalRopeUnits = (int)(ropeLength / 0.2f);

                if (totalRopeUnits < 40)
                {
                    Rope = new GameObject[totalRopeUnits + 1];

                    GameObject ropeUnit, LastUnit;

                    ropeUnit = Instantiate(RopeElement);
                    ropeUnit.transform.position = hit.point;
                    ropeUnit.GetComponent<Rigidbody>().isKinematic = true;
                    //ropeUnit.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
                    LastUnit = ropeUnit;
                    Rope[0] = ropeUnit;

                    for (int i = 1; i < totalRopeUnits; i++)
                    {
                        ropeUnit = Instantiate(RopeElement);
                        ropeUnit.name = i.ToString();
                        ropeUnit.transform.position = hit.point - (leftHitPoint * 0.21f * i);
                        //ropeUnit.transform.rotation = new Quaternion(0.0f,0.0f,0.0f,0.0f);
                        //HJ = ropeUnit.gameObject.GetComponent<HingeJoint>();
                        HJ = ropeUnit.gameObject.GetComponent<ConfigurableJoint>();
                        HJ.connectedBody = LastUnit.gameObject.GetComponent<Rigidbody>();
                        ropeUnit.gameObject.GetComponent<Rigidbody>().mass = 2.0f;
                        HJ.anchor = ropeUnit.transform.InverseTransformPoint(LastUnit.transform.position);

                        LastUnit = ropeUnit;
                        Rope[i] = ropeUnit;
                    }

                    this.gameObject.AddComponent<ConfigurableJoint>();
                    HJ = this.gameObject.GetComponent<ConfigurableJoint>();
                    HJ.connectedBody = LastUnit.gameObject.GetComponent<Rigidbody>();
                    HJ.xMotion = ConfigurableJointMotion.Locked;
                    HJ.yMotion = ConfigurableJointMotion.Locked;
                    HJ.zMotion = ConfigurableJointMotion.Locked;
                    //HJ.axis = new Vector3(1.0f,1.0f,1.0f);
                    HJ.anchor = transform.InverseTransformPoint(LastUnit.transform.position);
                }
            }
        }

        //Release the clamps for left and right respectively
        if (Input.GetKey(KeyCode.Q))
        {
            leftClamped = false;
            Destroy(this.gameObject.GetComponent<ConfigurableJoint>());
            for(int i = Rope.Length -1 ; i >= 0; i--)
            {
                DestroyObject(Rope[i]);
            }
        }
       
	}

    void FixedUpdate()
    {
       
    }
}
