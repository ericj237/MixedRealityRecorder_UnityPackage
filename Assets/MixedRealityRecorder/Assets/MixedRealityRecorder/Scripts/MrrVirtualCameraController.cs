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
        private Transform target;

        private float targetDepth;

        public void Init(CameraSettings cameraSetting, GameObject targetObject)
        {
            SetTargetObject(targetObject);

            virtualCamera = GetComponent<Camera>();
            virtualCamera.depthTextureMode = DepthTextureMode.Depth;

            SetCameraSettings(cameraSetting);

            depthTexture2D = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
            UpdateInternalTextures();

            virtualCamera.SetTargetBuffers(colorTexture.colorBuffer, rawDepthTexture.depthBuffer);
            matDepthTexture.SetTexture("_RawDepthTex", rawDepthTexture);
        }

        private void SetTargetObject(GameObject targetObject)
        {
            target = targetObject.transform;
        }

        private void UpdateInternalTextures()
        {
            // screen size
            Vector2Int cameraResolution = new Vector2Int(cameraSettings.resolutionWidth, cameraSettings.resolutionHeight);

            // create render textures
            colorTexture = new RenderTexture(cameraResolution.x, cameraResolution.y, 0, RenderTextureFormat.RGB565);
            rawDepthTexture = new RenderTexture(cameraResolution.x, cameraResolution.y, 16, RenderTextureFormat.Depth);
            depthTexture = new RenderTexture(cameraResolution.x, cameraResolution.y, 0, RenderTextureFormat.ARGBHalf);
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
            UpdateCameraSettings();
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

        public float GetTargetDepth()
        {
            return targetDepth;
        }

        public void Render()
        {
            virtualCamera.Render();
            UpdateTargetDepth();
        }
        
        private void UpdateTargetDepth()
        {
            Graphics.Blit(rawDepthTexture, depthTexture, matDepthTexture);

            Vector2 targetScreenPosition = virtualCamera.WorldToScreenPoint(target.position);
            //Debug.Log(targetScreenPosition);

            // screen size
            Vector2Int cameraResolution = new Vector2Int(cameraSettings.resolutionWidth, cameraSettings.resolutionHeight);

            if (targetScreenPosition.x >= 0 && targetScreenPosition.x <= cameraResolution.x && targetScreenPosition.y >= 0 && targetScreenPosition.y <= cameraResolution.y)
            {
                //Debug.Log("HDM Depth = " + hmdDepth);

                Vector3 origin = transform.position + transform.forward * virtualCamera.nearClipPlane;
                Vector3 direction = Vector3.Normalize(target.position - origin);
                float distance = Vector3.Distance(target.position, origin);

                RaycastHit hit;
                if (Physics.Raycast(origin, direction, out hit, virtualCamera.farClipPlane))
                {
                    if (hit.collider.gameObject.name == target.name)
                    {
                        RenderTexture.active = depthTexture;

                        depthTexture2D.ReadPixels(new Rect((int)targetScreenPosition.x, (int)targetScreenPosition.y, 1, 1), 0, 0);
                        depthTexture2D.Apply();

                        RenderTexture.active = null;

                        //Debug.DrawRay(origin, direction * hit.distance, Color.green);
                        targetDepth = depthTexture2D.GetPixel(1, 1).r;
                        //return;
                    }
                    //else
                    //{
                    //    Debug.DrawRay(origin, direction * hit.distance, Color.red);
                    //}
                }
            }
        }
    }
}