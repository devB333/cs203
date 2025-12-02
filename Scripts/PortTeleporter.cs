using UnityEngine;
using System.Collections.Generic;

public class PortTeleporter : MonoBehaviour
{
    public Transform player;
    public PortTeleporter targetPortal;
    public LinkedList<PortalTraveller> currTravellers = new LinkedList<PortalTraveller>(); // tracks portal travellers using a linked list for fast removal

    void LateUpdate()
    {
        if(currTravellers.First != null){
            var node = currTravellers.First;
            // uses while loop to allow removal during the loop
            while (node != null)
            {
                var traveller = node.Value;
                node = node.Next;
                Transform travellerTF = traveller.transform;
                // matrices multiplied together which gets the new location while keeping the current rotation
                var matrix = targetPortal.transform.localToWorldMatrix * transform.worldToLocalMatrix * travellerTF.localToWorldMatrix;

                // gets column 3 of matrix (the position)
                traveller.Teleport(transform, targetPortal.transform, matrix.GetColumn(3), matrix.rotation); 

                currTravellers.Remove(traveller);
            }
        }
    }

    void OnTravellerEnterPortal (PortalTraveller traveller)
    {
        if (!currTravellers.Contains(traveller))
        {
            currTravellers.AddLast(traveller);
        }
    }

    // if object that collides is able to go through the portal, allows it to enter
    void OnTriggerEnter (Collider other)
    {
        GameObject obj = other.gameObject;
        PortalTraveller traveller = obj.GetComponentInParent<PortalTraveller>();
        if(traveller)
        {
            OnTravellerEnterPortal(traveller);
        }
    }

    // removes from list when leaving collider
    void OnTriggerExit (Collider other)
    {
        var traveller = other.GetComponent<PortalTraveller>();
        if(traveller && currTravellers.Contains(traveller))
        {
            currTravellers.Remove(traveller);
        }
    }
}

