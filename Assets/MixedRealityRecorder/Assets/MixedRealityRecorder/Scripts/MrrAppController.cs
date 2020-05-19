using UnityEngine;
using MRR.Model;
using MRR.View;
using System.Collections.Generic;
using System.Diagnostics;

namespace MRR.Controller
{

    public class MrrAppController : MonoBehaviour
    {

        public MrrUiView uiView;

        public MrrVirtualCameraController virtualCamera;

        public List<CameraPreset> cameraPresets = new List<CameraPreset>();
        public List<GameObject> targetObjects = new List<GameObject>();

        public Material matForegroundMask;
        private RenderTexture foregroundMaskTexture;
        private WebCamTexture rawPhysicalCameraTexture;

        private WebCamDevice[] webCamDevices;

        void Start()
        {
            CacheWebcamDevices();
            CacheTargetObjects();

            virtualCamera.Init(cameraPresets[0].cameraSettings, targetObjects[0]);

            // screen size
            Vector2Int screenSize = new Vector2Int(1920, 1080);

            foregroundMaskTexture = new RenderTexture(screenSize.x, screenSize.y, 0, RenderTextureFormat.ARGBHalf);
            rawPhysicalCameraTexture = new WebCamTexture();

            // set foreground shader depth texture
            matForegroundMask.SetTexture("_DepthTex", virtualCamera.GetDepthTexture());

            rawPhysicalCameraTexture.Play();

            // assign the textures to the ui raw image component
            uiView.SetScreenVirtualCamera(virtualCamera.GetColorTexture());
            uiView.SetScreenForegroundMask(foregroundMaskTexture);
            uiView.SetScreenPhysicalCamera(rawPhysicalCameraTexture);
            uiView.SetOptionalScreen(virtualCamera.GetDepthTexture());

            InvokeRepeating("RunCycle", 0.0f, ((float)1 / virtualCamera.GetCameraSettings().framerate));

            uiView.Init();
        }

        private void RunCycle()
        {
            var stopWatch = Stopwatch.StartNew();

            virtualCamera.Render();
            UpdateForegroundMask();

            long frameTime = stopWatch.ElapsedMilliseconds;

            uiView.SetFooterFrameTime(frameTime);
        }

        public void UpdateForegroundMask()
        {
            // we only use this method to receive our processed foreground mask texture
            // all shader properties are already set in Initialize() method
            matForegroundMask.SetFloat("_HmdDepth", virtualCamera.GetTargetDepth());
            Graphics.Blit(virtualCamera.GetDepthTexture(), foregroundMaskTexture, matForegroundMask);
        }

        public List<CameraPreset> GetCameraPresets()
        {
            return cameraPresets;
        }

        public WebCamDevice[] GetWebCamDevices()
        {
            return webCamDevices;
        }

        private void CacheWebcamDevices()
        {
            webCamDevices = WebCamTexture.devices;
        }

        public List<GameObject> GetTargetObjects()
        {
            return targetObjects;
        }

        private void CacheTargetObjects()
        {
            Camera[] cameras = FindObjectsOfType<Camera>();

            foreach (Camera camera in cameras)
                if (camera.gameObject.name != "cam_virtual" && camera.gameObject.name != "cam_ui")
                    targetObjects.Add(camera.gameObject);
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