using UnityEngine;
using System.Collections;

public class PortalTraveller : MonoBehaviour
{
    public GameObject cloneObject;
    public Vector3 prevOffsetFromPortal { get; set; }

    public virtual void Teleport (Transform currPortal, Transform targetPortal, Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
        Physics.SyncTransforms();
        targetPortal.GetComponent<Collider>().enabled = false;

        StartCoroutine(TurnOnCollision(1f, targetPortal));
    }

    IEnumerator TurnOnCollision(float waitTime, Transform targetPortal)
    {
        yield return new WaitForSeconds(waitTime);
        targetPortal.GetComponent<Collider>().enabled = true;
    }
}
