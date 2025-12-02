using UnityEngine;
using System.Collections.Generic;

public class PortTeleporter : MonoBehaviour
{
    public Transform player;
    public PortTeleporter targetPortal;
    public bool playerColliding = false;
    public LinkedList<PortalTraveller> currTravellers = new LinkedList<PortalTraveller>();

    // Update is called once per frame
    void LateUpdate()
    {
        if(currTravellers.First != null){
            var node = currTravellers.First;
            while (node != null)
            {
                var traveller = node.Value;
                node = node.Next;
                Transform travellerTF = traveller.transform;
                var matrix = targetPortal.transform.localToWorldMatrix * transform.worldToLocalMatrix * travellerTF.localToWorldMatrix;
                
                    var currPosition = travellerTF.position;
                    var currRotation = travellerTF.rotation;
                    traveller.Teleport(transform, targetPortal.transform, matrix.GetColumn(3), matrix.rotation);

                    currTravellers.Remove(traveller);
            }
        }
    }

    void OnTravellerEnterPortal (PortalTraveller traveller)
    {
        if (!currTravellers.Contains(traveller))
        {
            traveller.prevOffsetFromPortal = traveller.transform.position - transform.position;
            currTravellers.AddLast(traveller);
        }
    }

    void OnTriggerEnter (Collider other)
    {
        GameObject obj = other.gameObject;
        PortalTraveller traveller = obj.GetComponentInParent<PortalTraveller>();
        if(traveller)
        {
            playerColliding = true;
            OnTravellerEnterPortal(traveller);
        }
    }

    void OnTriggerExit (Collider other)
    {
        playerColliding = false;
        var traveller = other.GetComponent<PortalTraveller>();
        if(traveller && currTravellers.Contains(traveller))
        {
            currTravellers.Remove(traveller);
        }
    }
}
