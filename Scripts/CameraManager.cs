using UnityEngine;
// Known Issues: Jitter on Camera Collide and Jitter when Jumping
public class CameraManager : MonoBehaviour
{
    public Rigidbody playerRigidbody; //  reference to the player transform to get position
    public Transform CameraPivot; // reference to camera pivot (up/down) (0,Y,0)

    private Vector3 currVelocity = Vector3.zero; // interal ref varaible to hold the curr velocity to be smoothed

    public float followTime = 0.2f;// ammount of time the smoothing process will take
    public float rotateSpeed = 2.0f; // speed that the camera will rotate (right/left)
    public float pivotSpeed = 2.0f; // speed that camera will pivot (up/down)

    private Player cameraMove; //  reference to player script to get mouse input

    private float lookAngle;// Camera look up and Down
    private float pivotAngle; // Camera look left and right

    private float defaultCameraPosition;
    private Transform cameraTransform;
    public LayerMask collisionLayers;// layers that the camera can collide with

    public float cameraCollisionRadius = 0.2f;
    public float cameraCollisionOffset = 0.2f;// how much the camera will jump off of objects it collided with
    public float minCollisionOffset = 0.2f;// minimum distance the camera can jump to prevent clipping

    private Vector3 cameraVectorPosition;
    private void Awake()
    {
        cameraMove = FindFirstObjectByType<Player>();// gets reference to object of type Player
        playerRigidbody = FindFirstObjectByType<Player>().GetComponent<Rigidbody>();

        cameraTransform = Camera.main.transform;
        defaultCameraPosition = cameraTransform.localPosition.z;// whatver the game starts of with in the z position of main camera will be default positoin
    }
    private Vector3 cameraFollowVelocity = Vector3.zero;
    public void FollowPlayer()
    {
        Vector3 newCameraPosition = Vector3.SmoothDamp(
            transform.position, 
            playerRigidbody.position, 
            ref currVelocity, 
            followTime
            );// this will update newCameraPosition from the current position
              // to the player position over the a smoothed time and velocity defined
              // by followTime and currVelocity

        transform.position = newCameraPosition;
        // the position of the camera will move twoard the player
        // at a smoothed rate after every frame has been processed.
    }

    private void FixedUpdate()
    {
        FollowPlayer();
    }

    public void LateUpdate()// use late update because there is no physics or rigidbody updates involed in rotation
    {
        RotateCamera();// rotates camera after all other updates have been processed
        HandleCameraCollision();// checks for camera collision after rotation
    }

    public void RotateCamera()
    {
        lookAngle += cameraMove.getMouseX * rotateSpeed;// scale mouse input by rotate speed and update look angle (Y axis)
        pivotAngle -= cameraMove.getMouseY * pivotSpeed;// scale mouse input by pivot speed and update pivot angle (X axis)
        pivotAngle = Mathf.Clamp(pivotAngle, -35f, 35f);// clamps pivot angle to prevent rotation from rolling over

        Vector3 rotation = Vector3.zero; // converts float angle to vector3 euler angle to then be converted to quaternion for use with Unity.
        rotation.y = lookAngle;// stores look angle in y axis
        Quaternion targetRotation = Quaternion.Euler(rotation);// converts euler Vector 3 to quaternion
        transform.rotation = targetRotation;// sets transform rotation = to quaternion angle (0,y,0)
        

        rotation = Vector3.zero;// resets rotation vector to be used for pivot
        rotation.x = pivotAngle;
        Quaternion targetPivotRotation = Quaternion.Euler(rotation);
        CameraPivot.localRotation = targetPivotRotation;// sets local rotation of pivot to the new pivot rotation


        //the axes need to be isolated to prevent gimbal lock and unwanted roll.
    }

    
    private void HandleCameraCollision()// go over this method tmrw
    {
        float targetPosition = defaultCameraPosition;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - CameraPivot.position; // gets distance from pivot to camera and direction
        float distanceToCamera = direction.magnitude;
        direction.Normalize();// strips magnitude to get only direction

        if (Physics.SphereCast
            (CameraPivot.transform.position, cameraCollisionRadius, direction, out hit, distanceToCamera, collisionLayers))
            {
            float distance = Vector3.Distance(CameraPivot.position, hit.point);
            targetPosition = -(distance - cameraCollisionOffset);
        }

        if (Mathf.Abs(targetPosition) < minCollisionOffset)
            targetPosition = -minCollisionOffset;

        cameraVectorPosition = cameraTransform.localPosition; // get current X/Y/Z
        cameraVectorPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, 0.2f);// smoothly lerps camera to target position
        cameraTransform.localPosition = cameraVectorPosition;
    }

}

