using MRR.Model;
using UnityEngine;

namespace MRR.Controller
{

    public class MrrVirtualCameraController : MonoBehaviour
    {
        public MrrAppController appController;
        public GameObject cameraGroup;

        public Camera backgroundCamera;
        public Camera foregroundCamera;

        public Material matRemoveAlphaChannel;

        private CameraSetting cameraSettings = new CameraSetting();

        private Transform target;

        private RenderTexture rawColorTextureBackground;
        private RenderTexture colorTextureBackground;

        private RenderTexture rawColorTextureForeground;
        private RenderTexture colorTextureForeground;

        // init method

        public void Init(CameraSetting cameraSetting, GameObject targetObject)
        {
            SetCameraSettings(cameraSetting);
            SetTargetObject(targetObject);
            
            UpdateInternalTextures();

            backgroundCamera.targetTexture = rawColorTextureBackground;       
            foregroundCamera.targetTexture = rawColorTextureForeground;
        }

        // render method

        public void Render()
        {
            UpdateFarClipPlane();
            UpdateNearClipPlane();

            backgroundCamera.Render();
            foregroundCamera.Render();

            matRemoveAlphaChannel.SetTexture("_ColorTex", rawColorTextureBackground);
            Graphics.Blit(rawColorTextureBackground, colorTextureBackground, matRemoveAlphaChannel);

            matRemoveAlphaChannel.SetTexture("_ColorTex", rawColorTextureForeground);
            Graphics.Blit(rawColorTextureForeground, colorTextureForeground, matRemoveAlphaChannel);
        }

        // update methods

        private void UpdateCameraSettings(Camera camera)
        {
            camera.focalLength = cameraSettings.focalLenth;
            camera.sensorSize = new Vector2(cameraSettings.sensorWidth, cameraSettings.sensorHeight);
        }

        private void UpdateInternalTextures()
        {
            // screen size
            Vector2Int cameraResolution = new Vector2Int(cameraSettings.resolutionWidth, cameraSettings.resolutionHeight);

            // create render textures
            rawColorTextureBackground = new RenderTexture(cameraResolution.x, cameraResolution.y, 0, RenderTextureFormat.Default);
            colorTextureBackground = new RenderTexture(cameraResolution.x, cameraResolution.y, 0, RenderTextureFormat.Default);
            rawColorTextureForeground = new RenderTexture(cameraResolution.x, cameraResolution.y, 0, RenderTextureFormat.Default);
            colorTextureForeground = new RenderTexture(cameraResolution.x, cameraResolution.y, 0, RenderTextureFormat.Default);
        }

        // getter methods

        public CameraSetting GetSettings()
        {
            return cameraSettings;
        }

        public RenderTexture GetColorTextureBackground()
        {
            return colorTextureBackground;
        }

        public RenderTexture GetRawColorTextureForeground()
        {
            return rawColorTextureForeground;
        }

        public RenderTexture GetColorTextureForeground()
        {
            return colorTextureForeground;
        }

        // setter methods

        public void SetCameraSettings(CameraSetting cameraSettings)
        {
            this.cameraSettings = cameraSettings;
            UpdateCameraSettings(backgroundCamera);
            UpdateCameraSettings(foregroundCamera);
        }

        public void SetTargetObject(GameObject targetObject)
        {
            target = targetObject.transform;
        }

        public void UpdateFarClipPlane()
        {
            float distance = Vector3.Distance(foregroundCamera.gameObject.transform.position, target.transform.position);

            foregroundCamera.farClipPlane = distance;
        }

        public void UpdateNearClipPlane()
        {
            float distance = Vector3.Distance(backgroundCamera.gameObject.transform.position, target.transform.position);

            backgroundCamera.nearClipPlane = distance - 0.1f;
        }

        /*

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

        private void SetSensorHeight(float sensorHeight)
        {
            cameraSettings.sensorHeight = sensorHeight;
        }

        private void SetSensorWidth(float sensorWidth)
        {
            cameraSettings.sensorHeight = sensorWidth;
        }

        */

        public void SetSensorOffsetPosition(float value, Vector3Component component)
        {
            switch (component)
            {
                case Vector3Component.x:
                    cameraGroup.transform.localPosition = new Vector3(value, cameraGroup.transform.localPosition.y, cameraGroup.transform.localPosition.z);
                    break;
                case Vector3Component.y:
                    cameraGroup.transform.localPosition = new Vector3(cameraGroup.transform.localPosition.x, value, cameraGroup.transform.localPosition.z);
                    break;
                case Vector3Component.z:
                    cameraGroup.transform.localPosition = new Vector3(cameraGroup.transform.localPosition.x, cameraGroup.transform.localPosition.y, value);
                    break;
                default:
                    break;
            }
        }

        public Vector3 GetSensorOffsetPosition()
        {
            return cameraGroup.transform.localPosition;
        }

        public void SetSensorOffsetRotation(float value, Vector3Component component)
        {
            switch (component)
            {
                case Vector3Component.x:
                    cameraGroup.transform.localEulerAngles = new Vector3(value, cameraGroup.transform.localEulerAngles.y, cameraGroup.transform.localEulerAngles.z);
                    break;
                case Vector3Component.y:
                    cameraGroup.transform.localEulerAngles = new Vector3(cameraGroup.transform.localEulerAngles.x, value, cameraGroup.transform.localEulerAngles.z);
                    break;
                case Vector3Component.z:
                    cameraGroup.transform.localEulerAngles = new Vector3(cameraGroup.transform.localEulerAngles.x, cameraGroup.transform.localEulerAngles.y, value);
                    break;
                default:
                    break;
            }
        }

        public Vector3 GetSensorOffsetRotation()
        {
            return cameraGroup.transform.localEulerAngles;
        }
    }
}