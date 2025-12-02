using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public bool readyToJump = true;

    [Header("Ground Checking")]
    // check ground
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask findGround;
    public bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rigBod;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigBod = GetComponent<Rigidbody>();
        rigBod.freezeRotation = true;

    }

    private void input()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // jumps if jump was pressed + player isn't floating
        if(Input.GetButton("Jump") && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            // call jump reset with cooldown as the delay
            Invoke(nameof(jumpReset), jumpCooldown);
        }
    }

    private void movePlayer()
    {
        // calculates move direction so player always walks in the direction 
        // they're facing
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if(grounded)
            rigBod.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        else
            rigBod.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);


    }

    // updates alongside the physics engine
    private void FixedUpdate()
    {
        movePlayer();
    }

    // Update is called once per frame
    void Update()
    {
        // casts a ray below player and checks if the ground is there
        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, findGround);

        input();

        // drag
        if (grounded)
            rigBod.linearDamping = groundDrag;
        else
            rigBod.linearDamping = 0;
    }

    private void SpeedControl()
    {
        Vector3 flatVelocity = new Vector3(rigBod.linearVelocity.x, 0f, rigBod.linearVelocity.z);

        // limit velocity
        if(flatVelocity.magnitude > moveSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
            rigBod.linearVelocity = new Vector3(limitedVelocity.x, rigBod.linearVelocity.y, limitedVelocity.z);
        }
    }

    private void Jump()
    {
        // reset y velocity to make sure you always jump the same height
        rigBod.linearVelocity = new Vector3(rigBod.linearVelocity.x, 0f, rigBod.linearVelocity.z);

        rigBod.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void jumpReset()
    {
        readyToJump = true;
    }
}

