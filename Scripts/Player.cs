using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public float rotationSpeed = 15.0f;
    public float speed = 5.0f;// speed varaible
    public float jumpSpeed = 10.0f;
    private Rigidbody rb;
    private bool jumpDetect = false;

    private Transform cameraObj;


    private CameraManager cameraManager; // creates new obj of CameraManager type
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private float mouseX;
    private float mouseY;


    private Vector3 moveVert;
    private Vector3 moveHorz;


    AnimatorManager animatorManager;
    private float moveAmount;



    //start grapple control fields
    private float maxGrappleDistance = 100f;
    private float minGrappleDistance = 10f;
    // end grapple control fields


    private ReticleController retControl;
    private RadialFillController radialFill;

    private bool fIsClicked = false;


    private PlayerGrapple pGrap;

    public float getMouseX
    {
        get { return mouseX; }
    }

    public float getMouseY
    {
        get { return mouseY; }
    }

    private void Awake()
    {
        cameraManager = FindObjectOfType<CameraManager>();
        cameraObj = cameraManager.transform;
        // links cameraManager to the obj with CameraManager script attatched

        animatorManager = GetComponent<AnimatorManager>();
        retControl = GetComponent<ReticleController>();

        radialFill = FindObjectOfType<RadialFillController>();

        pGrap = GetComponent<PlayerGrapple>();
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))// get jump input
            jumpDetect = true;

        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        // begin input polling
        // movement
        float moveVertical = Input.GetAxis("Vertical");
        moveVert = cameraObj.forward * moveVertical;
        // transform.forward just returns the forward axis of the objct so we can apply movemnt in the direction the object is facing

        float moveHorizontal = Input.GetAxis("Horizontal");
        moveHorz = cameraObj.right * moveHorizontal;

        

        handleAnimate(moveVertical, moveHorizontal);

        if (Input.GetKeyDown(KeyCode.F))
        {
            fIsClicked = true;
            
        }
        else
            fIsClicked = false;

            BuildGRappleQueue();


    }

    private void FixedUpdate()
    {
        //clear angular velocity to prevent rotation from physics interactions
        rb.angularVelocity = Vector3.zero;

        //movement
        Vector3 horzontialInput = moveHorz + moveVert;// adds two input vectors
        horzontialInput.Normalize(); // turns vector into unit vector just for direciton
        Vector3 horizontalVelocity = horzontialInput * speed;// scales unit vector by speed to get velocity
        horizontalVelocity.y = rb.linearVelocity.y; // preserve vertical velocity
        rb.linearVelocity = horizontalVelocity;// sets new rigidbody velocity


        // rotation
        Vector3 targetDirection = Vector3.zero;
        targetDirection = moveHorz + moveVert;// adds two vector3 input to get total vector
        targetDirection.Normalize();// turns vector into unit vector so just direction
        if(targetDirection.sqrMagnitude > 0.001f)// makes sure if no input is made then error won't be thrown
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);// gets angle to rotate to from Vector3 unit vector
            Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);// rotates slowly each frame for smoothing
            Quaternion smoothed = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(smoothed);// sets curr rotation to each frame smoothed rotation
        }
        
        if(jumpDetect)
        {
            rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
            jumpDetect = false;
        }
    }

    private void handleAnimate(float moveVertical, float moveHorizontal)
    {
        moveAmount = Mathf.Clamp01(Mathf.Abs(moveHorizontal) + Mathf.Abs(moveVertical));// look up what this line does
        animatorManager.UpdateAnimatorValues(0, moveAmount);// look up what this line does
    }

    private void BuildGRappleQueue()
    {
        PriorityQueue<GrapplePoint> grapplePointQueue = new PriorityQueue<GrapplePoint>();// create a priortiy queue for all grapple points
        float coneAngle = 80f; // vision cone for rayCast


        foreach (var point in GrappleManager.instance.grapplePoints)
        {
            float distanceFromPlayer = Vector3.Distance(transform.position, point.transform.position);// gets the distance between two points
            if (distanceFromPlayer <= maxGrappleDistance)// if its closer than max distance then it is loaded in
            {
                
                Vector3 dirToPoint = (point.transform.position - cameraObj.position).normalized;// gets the unit vector from cameraObj to point
                float angle = Vector3.Angle(cameraObj.forward, dirToPoint); // gets angle between direction vector and camera.forward
                if (angle <= coneAngle)
                {
                    int layerMask = ~LayerMask.GetMask("Player"); // the ~ inverts the mask so it knows to ignore the layer mask
                    if(Physics.Raycast(cameraObj.position, dirToPoint, out RaycastHit hit, maxGrappleDistance, layerMask))
                    {
                        if(hit.transform == point.transform)
                        {
                            // point is in cone of visibilty
                            grapplePointQueue.Enqueue(point, distanceFromPlayer);// load each grapple point in based on distance
                        }
                    }
                }
               
            }


           
        }

        if (grapplePointQueue.Count > 0)
        {
            GrapplePoint closest = grapplePointQueue.Dequeue();
            float currDistanceFromPlayer = Vector3.Distance(transform.position, closest.transform.position);
            // Debug.Log("Closest Grapple Point Distance: " + currDistanceFromPlayer);
            float fillAmount;

            //Debug.Log($"currDistanceFromPlayer: {currDistanceFromPlayer}, min: {minGrappleDistance}, max: {maxGrappleDistance}");

            if (currDistanceFromPlayer <= minGrappleDistance)
            {
                //Debug.Log("In fjwkf");
                fillAmount = 1f; // fully filled if closer than min

                if (currDistanceFromPlayer <= minGrappleDistance && Input.GetKeyDown(KeyCode.F))
                {
                    Debug.Log("in Here");
                    pGrap.setGrappelTarget(closest.transform);
                }

            }
            else if (currDistanceFromPlayer >= maxGrappleDistance)
            {
                fillAmount = 0f; // empty if farther than max
            }
            else
            {
               // Debug.Log("In 00");
                fillAmount = 1f - ((currDistanceFromPlayer - minGrappleDistance) / (maxGrappleDistance - minGrappleDistance));
                // if in between find how far curr distance is from minGrappleDistance and then
                // divide by the range which is always larger
                // this is bascially giving you a percentage of how far in the range the differnce of (currDist - minGrapp) is between min and max grapple. 
                // we subtract from 1 because we want the oppisite because we want it to grow with proximity and shrink with distance
            }


            retControl.grapplePoint = closest.transform; // sets the grapple point of retControll to the closest one
                                                         // this is so retController knows which graple point to show the reticle for

           // Debug.Log("Fill Amount: " + fillAmount);
            radialFill.SetFill(fillAmount);
            //Debug.Log("It was done");
            // this is where I will add the bubble element


        }
    }

    
}
