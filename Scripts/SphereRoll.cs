using UnityEngine;

public class SphereRoll : MonoBehaviour
{
    void LateUpdate()
    {
        transform.GetComponent<Rigidbody>().AddForce(-Vector3.forward * Time.deltaTime * 5);
    }
}
