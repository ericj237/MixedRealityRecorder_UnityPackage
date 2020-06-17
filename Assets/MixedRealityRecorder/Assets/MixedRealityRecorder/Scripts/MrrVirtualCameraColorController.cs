using MRR.Model;
using UnityEngine;

namespace MRR.Controller
{

    public class MrrVirtualCameraColorController : MonoBehaviour
    {

        public MrrAppController appController;

        private Camera virtualCamera;
        private CameraSetting cameraSettings = new CameraSetting();

        private RenderTexture colorTexture;
        private RenderTexture rawDepthTexture;

        // init method

        public void Init(CameraSetting cameraSetting)
        {

            virtualCamera = GetComponent<Camera>();

            SetCameraSettings(cameraSetting);

            UpdateInternalTextures();

            virtualCamera.SetTargetBuffers(colorTexture.colorBuffer, rawDepthTexture.depthBuffer);
        }

        // render method

        public void Render()
        {
            virtualCamera.Render();
            //UpdateTargetDepth();
        }

        // update methods

        /*
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
        */

        private void UpdateCameraSettings()
        {
            virtualCamera.focalLength = cameraSettings.focalLenth;
            virtualCamera.sensorSize = new Vector2(cameraSettings.sensorWidth, cameraSettings.sensorHeight);
        }

        private void UpdateInternalTextures()
        {
            // screen size
            Vector2Int cameraResolution = new Vector2Int(cameraSettings.resolutionWidth, cameraSettings.resolutionHeight);

            // create render textures
            colorTexture = new RenderTexture(cameraResolution.x, cameraResolution.y, 0, RenderTextureFormat.Default);
            rawDepthTexture = new RenderTexture(cameraResolution.x, cameraResolution.y, 16, RenderTextureFormat.Depth);
        }

        // getter methods

        public CameraSetting GetCameraSettings()
        {
            return cameraSettings;
        }

        public RenderTexture GetColorTexture()
        {
            return colorTexture;
        }

        public Camera GetVirtualCamera()
        {
            return virtualCamera;
        }

        // setter methods

        public void SetCameraSettings(CameraSetting cameraSettings)
        {
            this.cameraSettings = cameraSettings;
            UpdateCameraSettings();
        }

        private void SetResolutionWidth(int resolutionWidth)
        {
            cameraSettings.resolutionWidth = resolutionWidth;
        }

        private void SetResolutionHeight(int resolutionHeight)
        {
            cameraSettings.resolutionWidth = resolutionHeight;
        }

        private void SetFramerate(int framerate)
        {
            cameraSettings.framerate = framerate;
        }

        private void SetFocalLength(int focalLength)
        {
            cameraSettings.focalLenth = focalLength;
        }

        private void SetSensorHeight(int sensorHeight)
        {
            cameraSettings.sensorHeight = sensorHeight;
        }

        private void SetSensorWidth(int sensorWidth)
        {
            cameraSettings.sensorHeight = sensorWidth;
        }

        public void SetSensorOffsetPosition(float value, Vector3Component component)
        {
            switch (component)
            {
                case Vector3Component.x:
                    virtualCamera.transform.localPosition = new Vector3(value, virtualCamera.transform.localPosition.y, virtualCamera.transform.localPosition.z);
                    break;
                case Vector3Component.y:
                    virtualCamera.transform.localPosition = new Vector3(virtualCamera.transform.localPosition.x, value, virtualCamera.transform.localPosition.z);
                    break;
                case Vector3Component.z:
                    virtualCamera.transform.localPosition = new Vector3(virtualCamera.transform.localPosition.y, virtualCamera.transform.localPosition.y, value);
                    break;
                default:
                    break;
            }
        }

        public Vector3 GetSensorOffsetPosition()
        {
            return virtualCamera.transform.localPosition;
        }

        public void SetSensorOffsetRotation(float value, Vector3Component component)
        {
            switch (component)
            {
                case Vector3Component.x:
                    virtualCamera.transform.localEulerAngles = new Vector3(value, virtualCamera.transform.localEulerAngles.y, virtualCamera.transform.localEulerAngles.z);
                    break;
                case Vector3Component.y:
                    virtualCamera.transform.localEulerAngles = new Vector3(virtualCamera.transform.localEulerAngles.x, value, virtualCamera.transform.localEulerAngles.z);
                    break;
                case Vector3Component.z:
                    virtualCamera.transform.localEulerAngles = new Vector3(virtualCamera.transform.localEulerAngles.y, virtualCamera.transform.localEulerAngles.y, value);
                    break;
                default:
                    break;
            }
        }

        public Vector3 GetSensorOffsetRotation()
        {
            return virtualCamera.transform.localEulerAngles;
        }
    }
}