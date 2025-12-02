using UnityEngine;

public class MovePortalCamera : MonoBehaviour
{
    public Transform playerCam;
    public Transform farPortal;
    public Transform currPortal;

    void LateUpdate()
    {
        // move cam with player
        Vector3 playerDistanceFromPortal = playerCam.position - currPortal.position;
        transform.position = farPortal.position + playerDistanceFromPortal;

        // rotate cam with player
        float differenceBetweenPortalRotations = Quaternion.Angle(farPortal.rotation, currPortal.rotation);

        Quaternion rotationalDifferences = Quaternion.AngleAxis(differenceBetweenPortalRotations, Vector3.up);
        Vector3 newCamDirection = rotationalDifferences * playerCam.forward;
        transform.rotation = Quaternion.LookRotation(newCamDirection, Vector3.up);
    }
}
