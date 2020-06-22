using MRR.Model;
using UnityEngine;

namespace MRR.Controller
{

    public class MrrVirtualCameraController : MonoBehaviour
    {

        public MrrAppController appController;

        public Camera virtualCameraColor;
        public Camera virtualCameraDepth;

        public Material matDepthTexture;
        public Material matRemoveAlphaChannel;

        private CameraSetting cameraSettings = new CameraSetting();

        private Transform target;

        private RenderTexture rawColorTextureBackground;
        private RenderTexture colorTextureBackground;
        private RenderTexture colorTextureForeground;
        private RenderTexture rawDepthTexture;
        private RenderTexture depthTexture;

        // init method

        public void Init(CameraSetting cameraSetting, GameObject targetObject)
        {
            SetCameraSettings(cameraSetting);
            SetTargetObject(targetObject);
            
            UpdateInternalTextures();

            virtualCameraColor.targetTexture = rawColorTextureBackground;
            matRemoveAlphaChannel.SetTexture("_ColorTex", rawColorTextureBackground);
            virtualCameraDepth.SetTargetBuffers(colorTextureForeground.colorBuffer, rawDepthTexture.depthBuffer);
            matDepthTexture.SetTexture("_RawDepthTex", rawDepthTexture);
        }

        // render method

        public void Render()
        {
            UpdateFarClipPlane();
            virtualCameraColor.Render();
            Graphics.Blit(rawColorTextureBackground, colorTextureBackground, matRemoveAlphaChannel);
            virtualCameraDepth.Render();
            Graphics.Blit(rawDepthTexture, depthTexture, matDepthTexture);
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
            colorTextureForeground = new RenderTexture(cameraResolution.x, cameraResolution.y, 0, RenderTextureFormat.Default);
            rawDepthTexture = new RenderTexture(cameraResolution.x, cameraResolution.y, 16, RenderTextureFormat.Depth);
            depthTexture = new RenderTexture(cameraResolution.x, cameraResolution.y, 0, RenderTextureFormat.ARGBHalf);
        }

        // getter methods

        public CameraSetting GetCameraSettings()
        {
            return cameraSettings;
        }

        public RenderTexture GetColorBackgroundTexture()
        {
            return colorTextureBackground;
        }

        public RenderTexture GetDepthTexture()
        {
            return depthTexture;
        }

        public RenderTexture GetRawDepthTexture()
        {
            return rawDepthTexture;
        }

        // setter methods

        public void SetCameraSettings(CameraSetting cameraSettings)
        {
            this.cameraSettings = cameraSettings;
            UpdateCameraSettings(virtualCameraColor);
            UpdateCameraSettings(virtualCameraDepth);
        }

        public void SetTargetObject(GameObject targetObject)
        {
            target = targetObject.transform;
        }

        public void UpdateFarClipPlane()
        {
            float distance = Vector3.Distance(virtualCameraDepth.gameObject.transform.position, target.transform.position);

            virtualCameraDepth.farClipPlane = distance;
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
                    this.transform.localPosition = new Vector3(value, this.transform.localPosition.y, this.transform.localPosition.z);
                    break;
                case Vector3Component.y:
                    this.transform.localPosition = new Vector3(this.transform.localPosition.x, value, this.transform.localPosition.z);
                    break;
                case Vector3Component.z:
                    this.transform.localPosition = new Vector3(this.transform.localPosition.y, this.transform.localPosition.y, value);
                    break;
                default:
                    break;
            }
        }

        public Vector3 GetSensorOffsetPosition()
        {
            return this.transform.localPosition;
        }

        public void SetSensorOffsetRotation(float value, Vector3Component component)
        {
            switch (component)
            {
                case Vector3Component.x:
                    this.transform.localEulerAngles = new Vector3(value, this.transform.localEulerAngles.y, this.transform.localEulerAngles.z);
                    break;
                case Vector3Component.y:
                    this.transform.localEulerAngles = new Vector3(this.transform.localEulerAngles.x, value, this.transform.localEulerAngles.z);
                    break;
                case Vector3Component.z:
                    this.transform.localEulerAngles = new Vector3(this.transform.localEulerAngles.y, this.transform.localEulerAngles.y, value);
                    break;
                default:
                    break;
            }
        }

        public Vector3 GetSensorOffsetRotation()
        {
            return this.transform.localEulerAngles;
        }
    }
}