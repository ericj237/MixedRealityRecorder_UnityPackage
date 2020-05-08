using UnityEngine;
using MRR.Settings;

namespace MRR.Controller
{

    public class MrrAppManager : MonoBehaviour
    {

        public MrrUiController uiController;

        public Camera virtualCamera;
        public Transform foregroundTarget;

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

            Vector2 targetScreenPosition = virtualCamera.WorldToScreenPoint(foregroundTarget.position);
            Debug.Log(targetScreenPosition);

            if(targetScreenPosition.x >= 0 && targetScreenPosition.x <= 1920 && targetScreenPosition.y >= 0 && targetScreenPosition.y <= 1080)
                matForegroundMask.SetVector("_HmdPos", targetScreenPosition);
            else
                matForegroundMask.SetVector("_HmdPos", new Vector2(-1.0f, -1.0f));

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
            Vector2Int screenSize = new Vector2Int(1920, 1080);

            // create render textures
            colorTexture = new RenderTexture(screenSize.x, screenSize.y, 0, RenderTextureFormat.RGB565);
            depthTexture = new RenderTexture(screenSize.x, screenSize.y, 24, RenderTextureFormat.Depth);
            foregroundMaskTexture = new RenderTexture(screenSize.x, screenSize.y, 0, RenderTextureFormat.RGB565);

            // set camera to render depth
            virtualCamera.depthTextureMode = DepthTextureMode.Depth;

            // set color and depth buffer locations to the render textures
            virtualCamera.SetTargetBuffers(colorTexture.colorBuffer, depthTexture.depthBuffer);

            // set foreground shader depth texture and hmd position for dynamic obstruction
            matForegroundMask.SetTexture("_DepthTex", depthTexture);

            // assign the textures to the ui raw image component
            uiController.SetScreenVirtualCamera(colorTexture);
            uiController.SetScreenForegroundMask(foregroundMaskTexture);

        }
    }
}