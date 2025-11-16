using UnityEngine;

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
    }

    private void FixedUpdate()
    {
        // movement
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 moveVert = cameraObj.forward * moveVertical;
        // transform.forward just returns the forward axis of the objct so we can apply movemnt in the direction the object is facing

        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector3 moveHorz = cameraObj.right * moveHorizontal;

        Vector3 sumVector = moveHorz + moveVert;
        sumVector.Normalize(); // normalize the vector to prevent faster diagonal movment 

        sumVector *= speed * Time.fixedDeltaTime; // scale the vector by speed and time
        rb.MovePosition(rb.position + sumVector);


        // rotation
        Vector3 targetDirection = Vector3.zero;
        targetDirection = moveHorz + moveVert;// adds two vector3 input to get total vector
        targetDirection.Normalize();// turns vector into unit vector so just direction
        if(targetDirection.sqrMagnitude > 0.001f)// makes sure if no input is made then error won't be thrown
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);// gets angle to rotate to from Vector3 unit vector
            Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);// rotates slowly each frame for smoothing

            transform.rotation = playerRotation;// sets curr rotation to each frame smoothed rotation
        }
        
        if(jumpDetect)
        {
            rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
            jumpDetect = false;
        }
    }

    
}
