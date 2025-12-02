using UnityEngine;

public class RadialFillController : MonoBehaviour
{
    private Material mat;
    private SpriteRenderer sr;
    private MaterialPropertyBlock mpb;
    private int fillId;
    private bool usePropertyBlock;

    //connect Inner Shader Render to this Script
    public GameObject spriteRend;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            Debug.LogWarning($"RadialFillController on '{name}' requires a SpriteRenderer.", this);
            enabled = false;
            return;
        }

        mat = sr.material; // instance for this renderer
        fillId = Shader.PropertyToID("_FillAmount");
        mpb = new MaterialPropertyBlock();

        if (mat != null)
        {
            Debug.Log($"RadialFillController: material shader = '{mat.shader.name}' on '{name}'");
            if (mat.HasProperty(fillId))
            {
                usePropertyBlock = false;
                Debug.Log("RadialFillController: using material.SetFloat for _FillAmount");
                return;
            }
        }

        // fallback to property block (works even if material doesn't expose property or using sharedMaterial)
        usePropertyBlock = true;
        Debug.LogWarning($"RadialFillController: material does not expose '_FillAmount'. Falling back to MaterialPropertyBlock for '{name}' (shader: {(mat!=null?mat.shader.name:"null")}).", this);
    }

    public void SetFill(float amount)
    {
       // Debug.Log(spriteRend.name + " scaled to " + amount);


        spriteRend.transform.localScale = new Vector3(amount, amount, 1);
        // Debug.Log($"RadialFillController.SetFill: set MPB _FillAmount={amount} on '{name}'");
    }
}
