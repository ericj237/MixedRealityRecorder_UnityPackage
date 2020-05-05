using UnityEngine;
using UnityEngine.UI;

public class MrCamManager : MonoBehaviour
{
    
    public Camera virtualCamera;

    public RawImage imgColor;
    public RawImage imgForegroundMask;

    public Material matForegroundMask;

    private RenderTexture colorTexture;
    private RenderTexture depthTexture;
    private RenderTexture foregroundMaskTexture;

    private CameraSettings cameraSettings;

    void Start()
    {

        // create new camera settings and assign framerate
        cameraSettings = new CameraSettings();
        cameraSettings.framerate = 50;

        // TMP
        ApplyApplicationSettings();

        Initialize();

    }

    private void Update()
    {

        // render the virtual camera
        virtualCamera.Render();

        // we only use this method to receive our processed foreground mask texture
        // all shader properties are already set in Initialize() method
        Graphics.Blit(depthTexture, foregroundMaskTexture, matForegroundMask);

    }

    private void ApplyApplicationSettings()
    {

        // is there a better way to lock the camera rendering framerate?
        // most vr sdks set the application framerate to 75 or 90 
        // we cant control our virtual camera framerate with this settings
        Application.targetFrameRate = (int)cameraSettings.framerate;

    }

    private void Initialize()
    {

        // calculate half the screen size
        Vector2Int halfScreenSize = new Vector2Int(Screen.width / 2, Screen.height / 2);

        // create render textures
        colorTexture = new RenderTexture(halfScreenSize.x, halfScreenSize.y, 0, RenderTextureFormat.RGB565);
        depthTexture = new RenderTexture(halfScreenSize.x, halfScreenSize.y, 24, RenderTextureFormat.Depth);
        foregroundMaskTexture = new RenderTexture(halfScreenSize.x, halfScreenSize.y, 0, RenderTextureFormat.RGB565);

        // set camera to render depth
        virtualCamera.depthTextureMode = DepthTextureMode.Depth;

        // set color and depth buffer locations to the render textures
        virtualCamera.SetTargetBuffers(colorTexture.colorBuffer, depthTexture.depthBuffer);

        // set foreground shader depth texture and hmd position for dynamic obstruction
        matForegroundMask.SetTexture("_DepthTex", depthTexture);
        matForegroundMask.SetVector("_HmdPos", Vector4.zero);

        // assign the textures to the ui raw image component
        imgColor.texture = colorTexture;
        imgForegroundMask.texture = foregroundMaskTexture;

    }

    // camera settings data
    private struct CameraSettings
    {

        public uint framerate;

    }
}
