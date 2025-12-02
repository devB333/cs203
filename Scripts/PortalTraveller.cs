using UnityEngine;
using System.Collections;

public class PortalTraveller : MonoBehaviour
{
    public GameObject cloneObject;

    public virtual void Teleport (Transform currPortal, Transform targetPortal, Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
        Physics.SyncTransforms();
        targetPortal.GetComponent<Collider>().enabled = false;

        // makes sure you have time to leave the portal after teleporting
        StartCoroutine(TurnOnCollision(1f, targetPortal));
    }

    // turn target portal collision back on after a second
    IEnumerator TurnOnCollision(float waitTime, Transform targetPortal)
    {
        yield return new WaitForSeconds(waitTime);
        targetPortal.GetComponent<Collider>().enabled = true;
    }
}

