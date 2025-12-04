using UnityEngine;

public class PlayerGrapple : MonoBehaviour
{
    private GrappleRope grappleRope;
    private float moveSpeed;// speed of the grapple animation
    private float moveDuration = 1.13333333333f;// duration of the grapple animation
    private float elapsedTime;

    private bool isGrappling = false;
    private Vector3 startPos;
    private Vector3 targetPos;

    public float maxArcHeight = 2f;

    public Animator animator;



    public void setGrappelTarget(Transform target)// this is the first method that will run based on a call from Player.cs. It will take the transform of the cloest valid grapple point and then get its position
    {
        targetPos = target.position;// gets the position of the grapple in world space
       
        animator.Play("GrappleAnimation", layer: 0, normalizedTime: 0f);

        grappleRope.getTarget(targetPos);// pass the target position to the grapple rope script
        Debug.Log("it will click");
    }


    // Start 
    public void StartGrapple()
    {
        startPos = transform.position;
        elapsedTime = 0f;
        isGrappling = true;
        animator.SetBool("isGrappling", true); // start grapple
        Debug.Log("Is grappling is true");
        grappleRope.BeginTighten();// start grapple rope and pass targetPos
    }

    
     public void EndGrapple()
    {
        isGrappling = false;
        animator.SetBool("isGrappling", false); // end grapple
        grappleRope.EndRope();// end rope existance
        transform.position = targetPos;
    }

    private void Update()
    {
        if (!isGrappling) 
            return;

        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / moveDuration);// this is the interpolation factor (between 0 - 1)

        //Horizontal linerar movement
        Vector3 horizontalPos = Vector3.Lerp(startPos, targetPos, t);// t is the interpolation factor that determines how far between the startPos and targetPos the lerp point is

        //Vertical arc (sine curve, peaks at t = 0.5)
        float height = Mathf.Sin(Mathf.PI * t) * maxArcHeight;

        // Set new player pos
        transform.position = horizontalPos + Vector3.up * height;

        // Safety fallback: auto-end grapple if t >=1
        if (t >= 1f)
            EndGrapple();
    }
        

    private void Awake()
    {
        grappleRope = GetComponent<GrappleRope>();
    }

}
