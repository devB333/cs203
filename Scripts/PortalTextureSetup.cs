using UnityEngine;

public class PortalTextureSetup : MonoBehaviour
{
    public Camera world2Cam;
    public Material world2CamMat;
    public Camera world1Cam;
    public Material world1CamMat;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(world2Cam.targetTexture != null)
        {
            world2Cam.targetTexture.Release();
        }
        world2Cam.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        world2CamMat.mainTexture = world2Cam.targetTexture;

        if(world1Cam.targetTexture != null)
        {
            world1Cam.targetTexture.Release();
        }
        world1Cam.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        world1CamMat.mainTexture = world1Cam.targetTexture;
    }
}
