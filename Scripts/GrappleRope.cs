using UnityEngine;

public class GrappleRope : MonoBehaviour
{
    public LineRenderer line;
    public Transform hand;       // Player hand
    public Vector3 target;     // Grapple point
    public int segments = 20;    // Number of points along rope
    public float waveHeight = 1f;
    public float waveFrequency = 2f;
    public float tightenSpeed = 2f;

    private float t = 0f;             // 0 = fully wavy, 1 = fully tight
    private bool isExtending = false;
    private bool tightenRope = false;

    private Vector3[] positions;


    public void getTarget(Vector3 targetPoint)
    {
        target = targetPoint;
    }
    private void Update()
    {
        if (!isExtending || target == null || hand == null)
            return;

        // Optionally increase t automatically if you want timed tightening
        if (tightenRope)
            t = Mathf.Clamp01(t + Time.deltaTime * tightenSpeed);

        DrawRope();
    }

    public void ShootRope()
    {
        
        isExtending = true;
        tightenRope = false;
        t = 0f;

        if (!line.enabled)
            line.enabled = true;

        positions = new Vector3[segments];
    }

    public void BeginTighten() // Call from animation event
    {
        tightenRope = true;
    }

    private void DrawRope()
    {
        Vector3 startPos = hand.position;
        Vector3 endPos = target;

        for (int i = 0; i < segments; i++)
        {
            float segmentT = i / (float)(segments - 1);

            // Linear interpolation along rope
            Vector3 point = Vector3.Lerp(startPos, endPos, segmentT);

            // Add wave effect that diminishes as t -> 1
            float wave = Mathf.Sin(segmentT * Mathf.PI * waveFrequency) * waveHeight * (1 - t);
            point += Vector3.up * wave;

            positions[i] = point;
        }

        line.positionCount = segments;
        line.SetPositions(positions);
    }

    public void EndRope()
    {
        isExtending = false;
        tightenRope = false;
        t = 0f;

        if (line != null)
            line.enabled = false;
    }

}
