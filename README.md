# Unity Portal Mechanic
## Abstract
My plan was to develop a functioning portal game mechanic that displays the view of the other side of the portal in Unity utilizing LinkedLists, Shaders, and Camera RenderTextures to achieve the desired effect.
## Structure
- PlayerMovement, PlayerCamera, and SphereRoll scripts create basic movement
- MovePortalCamera syncs with player movement and captures the target portal
- PortalTextureSetup and the PortalCutoutShader assign the camera's view to the portal texture and size it properly
- PortalTraveller class is assigned to objects that can teleport through the portal
- PortTeleporter finds every PortalTraveller that collides with the portal and adds them to a LinkedList (for faster removal than C#'s List) and iterates through, teleporting each
### Project Link
.zip of the full project, which was too big for github, can be downloaded here: https://drive.google.com/file/d/1LxEHbY7cx6bx3ZP4bLzan4wP5U6vFC-R/view?usp=sharing
