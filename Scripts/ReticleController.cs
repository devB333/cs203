using UnityEngine;
using UnityEngine.UI;
public class ReticleController : MonoBehaviour
{
    private Transform player; // player position
    public float offsetFactor = 0.2f;// How much the reticle moves to match the player
    public float smoothSpeed = 10f; // Speed of interpolation

    public Camera mainCamera;
    public SpriteRenderer reticle;
    public Transform grapplePoint;

    // Update is called once per frame
    void Update()
    {
        if(!(grapplePoint == null))
        {
           // Debug.Log("Reticle Does Exist");
            

            Vector3 direction = grapplePoint.position - player.position;
            Vector3 targetPosition = grapplePoint.position - direction.normalized * offsetFactor;
            reticle.transform.position = targetPosition;

            reticle.transform.LookAt(mainCamera.transform);
            reticle.transform.Rotate(0f, 180f, 0f);

            // only show if grapple point is in front of the camera
            if(Vector3.Dot(mainCamera.transform.forward, direction.normalized) > 0)// this takes the dot product to make sure that both vectors are pointing in roughly the same direction  
            {
                reticle.gameObject.SetActive(true);
               // Debug.Log("It should be showing");
            }
            else
            {
                reticle.gameObject.SetActive(false);
            }
        }
        else
        {
            reticle.gameObject.SetActive(false);
        }

        
    }

    private void Awake()
    {
        if(reticle == null)
        {
           
               // Debug.LogWarning("ReticleController: reticle Image is not assigned.", this);
        
        }

        player = FindObjectOfType<Player>().transform;
    }
}
