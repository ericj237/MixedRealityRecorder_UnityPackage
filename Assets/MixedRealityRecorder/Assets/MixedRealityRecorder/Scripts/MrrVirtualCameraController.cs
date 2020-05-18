using MRR.Model;
using UnityEngine;

namespace MRR.Controller
{

    public class MrrVirtualCameraController : MonoBehaviour
    {

        private Camera virtualCamera;
        private CameraSettings cameraSettings = new CameraSettings();

        private RenderTexture colorTexture;
        private RenderTexture rawDepthTexture;
        private RenderTexture depthTexture;
        private Texture2D depthTexture2D;

        public Material matDepthTexture;

        public MrrAppController appController;
        public Transform foregroundTarget;

        private float hmdDepth;

        private void Start()
        {
            // create new camera settings and assign framerate
            // cameraSettings = CreateDefaultCameraSettings();

            // screen size
            Vector2Int screenSize = new Vector2Int(1920, 1080);

            // create render textures
            colorTexture = new RenderTexture(screenSize.x, screenSize.y, 0, RenderTextureFormat.RGB565);
            rawDepthTexture = new RenderTexture(screenSize.x, screenSize.y, 24, RenderTextureFormat.Depth);
            depthTexture = new RenderTexture(screenSize.x, screenSize.y, 0, RenderTextureFormat.ARGBHalf);
            depthTexture2D = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);

            virtualCamera = GetComponent<Camera>();

            // set camera to render depth
            virtualCamera.depthTextureMode = DepthTextureMode.Depth;

            // set color and depth buffer locations to the render textures
            virtualCamera.SetTargetBuffers(colorTexture.colorBuffer, rawDepthTexture.depthBuffer);

            matDepthTexture.SetTexture("_RawDepthTex", rawDepthTexture);
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

        public void SetCameraSettings(CameraSettings cameraSettings)
        {
            this.cameraSettings = cameraSettings;

            // UpdateCameraSettings();
        }

        private void UpdateCameraSettings()
        {
            virtualCamera.focalLength = cameraSettings.focalLenth;
            virtualCamera.sensorSize = new Vector2(cameraSettings.sensorWidth, cameraSettings.sensorHeight);
        }

        public Camera GetVirtualCamera()
        {
            return virtualCamera;
        }

        public float GetHmdDepth()
        {
            return hmdDepth;
        }

        public void Render()
        {
            virtualCamera.Render();

            Graphics.Blit(rawDepthTexture, depthTexture, matDepthTexture);

            Vector2 targetScreenPosition = virtualCamera.WorldToScreenPoint(foregroundTarget.position);
            //Debug.Log(targetScreenPosition);

            RenderTexture.active = depthTexture;

            depthTexture2D.ReadPixels(new Rect((int)targetScreenPosition.x, (int)targetScreenPosition.y, 1, 1), 0, 0);
            depthTexture2D.Apply();

            RenderTexture.active = null;

            //Debug.Log("HDM Depth = " + hmdDepth);

            if (targetScreenPosition.x >= 0 && targetScreenPosition.x <= 1920 && targetScreenPosition.y >= 0 && targetScreenPosition.y <= 1080)
            {
                Vector3 origin = transform.position + transform.forward * virtualCamera.nearClipPlane;
                Vector3 direction = Vector3.Normalize(foregroundTarget.transform.position - origin);

                RaycastHit hit;
                if (Physics.Raycast(origin, direction, out hit, virtualCamera.farClipPlane))
                {
                    if (hit.collider.gameObject.name == foregroundTarget.name)
                    {
                        Debug.DrawRay(origin, direction * hit.distance, Color.green);
                        hmdDepth = depthTexture2D.GetPixel(1, 1).r;
                        return;
                    }
                    else
                    {
                        Debug.DrawRay(origin, direction * hit.distance, Color.red);
                    }
                }
            }

            hmdDepth = 0.0f;
        }
    }
}