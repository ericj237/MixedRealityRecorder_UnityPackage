using MRR.Controller;
using MRR.Settings;
using UnityEngine;

namespace MRR.Video
{

    public class MrrVirtualCamera : MonoBehaviour
    {

        private Camera virtualCamera;
        private CameraSettings cameraSettings;

        private RenderTexture colorTexture;
        private RenderTexture depthTexture;

        public MrrAppManager appManager;
        public Transform foregroundTarget;

        private Texture2D tmpDepthTexture2D;

        private void Start()
        {

            tmpDepthTexture2D = new Texture2D(1920, 1080, TextureFormat.RGB24, false);

            // create new camera settings and assign framerate
            cameraSettings = new CameraSettings();
            cameraSettings.framerate = 50;

            // screen size
            Vector2Int screenSize = new Vector2Int(1920, 1080);

            // create render textures
            colorTexture = new RenderTexture(screenSize.x, screenSize.y, 0, RenderTextureFormat.RGB565);
            depthTexture = new RenderTexture(screenSize.x, screenSize.y, 24, RenderTextureFormat.Depth);

            virtualCamera = GetComponent<Camera>();

            // set camera to render depth
            virtualCamera.depthTextureMode = DepthTextureMode.Depth;

            // set color and depth buffer locations to the render textures
            virtualCamera.SetTargetBuffers(colorTexture.colorBuffer, depthTexture.depthBuffer);
        }

        public RenderTexture GetDepthTexture()
        {
            return depthTexture;
        }

        public RenderTexture GetColorTexture()
        {
            return colorTexture;
        }

        public CameraSettings GetCameraSettings()
        {
            return cameraSettings;
        }

        public Camera GetVirtualCamera()
        {
            return virtualCamera;
        }

        private void OnPostRender()
        {
            Vector2 targetScreenPosition = virtualCamera.WorldToScreenPoint(foregroundTarget.position);
            //Debug.Log(targetScreenPosition);

            //RenderTexture.active = depthTexture;

            //tmpDepthTexture2D.ReadPixels(new Rect(0, 0, 1920, 1080), 0, 0);
            //tmpDepthTexture2D.Apply();

            //RenderTexture.active = null;

            //float hmdDepth = tmpDepthTexture2D.GetPixel((int)targetScreenPosition.x, (int)targetScreenPosition.y).r;

            //Debug.Log("HDM Depth = " + hmdDepth);

            //if(targetScreenPosition.x >= 0 && targetScreenPosition.x <= 1920 && targetScreenPosition.y >= 0 && targetScreenPosition.y <= 1080)
            //    matForegroundMask.SetVector("_HmdPos", targetScreenPosition);
            //else
            //    matForegroundMask.SetVector("_HmdPos", new Vector2(-1.0f, -1.0f));

            appManager.CreateForegroundMask();
        }

        public Texture2D GetTmpDepthTexture()
        {
            return tmpDepthTexture2D;
        }
    }
}