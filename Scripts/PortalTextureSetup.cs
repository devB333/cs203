using UnityEngine;

public class PortalTextureSetup : MonoBehaviour
{
    public Camera world2Cam;
    public Material world2CamMat;
    public Camera world1Cam;
    public Material world1CamMat;

    void Start()
    {
        // removes textures if they're already there
        if(world2Cam.targetTexture != null)
        {
            world2Cam.targetTexture.Release();
        }
        // scales the portal texture based on the game display size
        world2Cam.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        world2CamMat.mainTexture = world2Cam.targetTexture;

        // removes textures if they're already there
        if(world1Cam.targetTexture != null)
        {
            world1Cam.targetTexture.Release();
        }
        // scales the portal texture based on the game display size
        world1Cam.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        world1CamMat.mainTexture = world1Cam.targetTexture;
    }
}

