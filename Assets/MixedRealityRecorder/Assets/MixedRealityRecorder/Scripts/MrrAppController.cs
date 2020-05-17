using UnityEngine;
using MRR.Model;
using MRR.View;
using MRR.Video;
using System.Collections;
using System.Collections.Generic;

namespace MRR.Controller
{

    public class MrrAppController : MonoBehaviour
    {

        public MrrUiView uiView;

        public MrrVirtualCameraModel virtualCamera;

        public List<CameraPreset> cameraPresets = new List<CameraPreset>();

        public Material matForegroundMask;
        private RenderTexture foregroundMaskTexture;
        private WebCamTexture rawPhysicalCameraTexture;

        public List<CameraPreset> GetCameraPresets()
        {
            return cameraPresets;
        }

        void Start()
        {
            // TMP
            //ApplyApplicationSettings();

            // screen size
            Vector2Int screenSize = new Vector2Int(1920, 1080);

            foregroundMaskTexture = new RenderTexture(screenSize.x, screenSize.y, 0, RenderTextureFormat.ARGBHalf);
            rawPhysicalCameraTexture = new WebCamTexture();

            StartCoroutine(Init());
        }

        private IEnumerator Init()
        {
            yield return new WaitForSeconds(1.0f);

            virtualCamera.SetCameraSettings(cameraPresets[0].cameraSettings);

            // set foreground shader depth texture
            matForegroundMask.SetTexture("_DepthTex", virtualCamera.GetDepthTexture());

            rawPhysicalCameraTexture.Play();

            // assign the textures to the ui raw image component
            uiView.SetScreenVirtualCamera(virtualCamera.GetColorTexture());
            uiView.SetScreenForegroundMask(foregroundMaskTexture);
            uiView.SetScreenPhysicalCamera(rawPhysicalCameraTexture);
        }

        public void UpdateForegroundMask()
        {
            // we only use this method to receive our processed foreground mask texture
            // all shader properties are already set in Initialize() method
            matForegroundMask.SetFloat("_HmdDepth", virtualCamera.GetHmdDepth());
            Graphics.Blit(virtualCamera.GetDepthTexture(), foregroundMaskTexture, matForegroundMask);
        }

        private void ApplyApplicationSettings()
        {
            // is there a better way to lock the camera rendering framerate?
            // most vr sdks set the application framerate to 75 or 90 
            // we cant control our virtual camera framerate with this settings
            Application.targetFrameRate = (int)virtualCamera.GetCameraSettings().framerate;
        }

        private void SetCameraResolutionWidth(int resolutionWidth)
        {
            CameraSettings cameraSettings = virtualCamera.GetCameraSettings();
            cameraSettings.resolutionWidth = resolutionWidth;
            virtualCamera.SetCameraSettings(cameraSettings);
        }

        private void SetCameraResolutionHeight(int resolutionHeight)
        {
            CameraSettings cameraSettings = virtualCamera.GetCameraSettings();
            cameraSettings.resolutionWidth = resolutionHeight;
            virtualCamera.SetCameraSettings(cameraSettings);
        }

        private void SetCameraFramerate(int framerate)
        {
            CameraSettings cameraSettings = virtualCamera.GetCameraSettings();
            cameraSettings.framerate = framerate;
            virtualCamera.SetCameraSettings(cameraSettings);
        }

        private void SetCameraFocalLength(int focalLength)
        {
            CameraSettings cameraSettings = virtualCamera.GetCameraSettings();
            cameraSettings.focalLenth = focalLength;
            virtualCamera.SetCameraSettings(cameraSettings);
        }

        private void SetCameraSensorHeight(int sensorHeight)
        {
            CameraSettings cameraSettings = virtualCamera.GetCameraSettings();
            cameraSettings.sensorHeight = sensorHeight;
            virtualCamera.SetCameraSettings(cameraSettings);
        }

        private void SetCameraSensorWidth(int sensorWidth)
        {
            CameraSettings cameraSettings = virtualCamera.GetCameraSettings();
            cameraSettings.sensorHeight = sensorWidth;
            virtualCamera.SetCameraSettings(cameraSettings);
        }

        private void SetCameraSensorDynamicRange(int stops)
        {
            CameraSettings cameraSettings = virtualCamera.GetCameraSettings();
            cameraSettings.dynamicRange = stops;
            virtualCamera.SetCameraSettings(cameraSettings);
        }

        public CameraSettings GetCameraSettings()
        {
            return virtualCamera.GetCameraSettings();
        }

        public void SetCameraSettings(CameraSettings cameraSettings)
        {
            virtualCamera.SetCameraSettings(cameraSettings);
        }

        public void SetSensorOffsetPosition(float value, Vector3Component component)
        {
            switch(component)
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