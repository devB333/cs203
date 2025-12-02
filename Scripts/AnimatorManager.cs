using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Animator animator;
    int horz;
    int vert;


    public void Awake()
    {
        animator = GetComponent<Animator>();
        horz = Animator.StringToHash("horizontal");
        vert = Animator.StringToHash("vertical");
    }

    public void UpdateAnimatorValues(float horzMovement, float vertMovement)
    {
        //Animation Snapping

        float snappedHorz;
        float snappedVert;

        // start snapping Horz
        if (horzMovement > 0 && horzMovement < 0.55f)
        {
            snappedHorz = 0.5f;
        }
        else if (horzMovement > 0.55f)
        {
            snappedHorz = 1f;
        }
        else if (horzMovement < 0 && horzMovement > -0.55f)
        {
            snappedHorz = -0.5f;

        }
        else if (horzMovement < -0.55f)
        {
            snappedHorz = -1f;
        }
        else
            snappedHorz = 0f;
        // end snapping Horz

        // start snapping Vert
        if (vertMovement > 0 && vertMovement < 0.55f)
        {
            snappedVert = 0.5f;
        }
        else if (vertMovement > 0.55f)
        {
            snappedVert = 1f;
        }
        else if (vertMovement < 0 && vertMovement > -0.55f)
        {
            snappedVert = -0.5f;

        }
        else if (vertMovement < -0.55f)
        {
            snappedVert = -1f;
        }
        else
            snappedVert = 0f;
        animator.SetFloat(horz, snappedHorz, 0.1f, Time.deltaTime);
        animator.SetFloat(vert, snappedVert, 0.1f, Time.deltaTime);
    }
}
