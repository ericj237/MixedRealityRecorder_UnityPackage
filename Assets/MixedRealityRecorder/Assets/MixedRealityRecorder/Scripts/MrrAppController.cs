using UnityEngine;
using MRR.Model;
using MRR.View;
using System.Collections.Generic;
using System.Diagnostics;

namespace MRR.Controller
{

    public class MrrAppController : MonoBehaviour
    {

        public MrrVirtualCameraController virtualCamera;
        public MrrUiView uiView;

        public Material matForegroundMask;

        public List<CameraPreset> cameraPresets = new List<CameraPreset>();
        public List<GameObject> targetObjects = new List<GameObject>();
        private WebCamDevice[] webCamDevices;

        private RenderTexture foregroundMaskTexture;
        private WebCamTexture rawPhysicalCameraTexture;

        // init methods - entry point for MixedRealtiyRecorder

        void Start()
        {
            CacheWebcamDevices();
            CacheTargetObjects();

            virtualCamera.Init(cameraPresets[0].cameraSettings, targetObjects[0]);

            // screen size
            Vector2Int screenSize = new Vector2Int(virtualCamera.GetCameraSettings().resolutionWidth, virtualCamera.GetCameraSettings().resolutionHeight);

            foregroundMaskTexture = new RenderTexture(screenSize.x, screenSize.y, 0, RenderTextureFormat.ARGBHalf);

            rawPhysicalCameraTexture = new WebCamTexture();
            rawPhysicalCameraTexture.Play();

            // set foreground shader depth texture
            matForegroundMask.SetTexture("_DepthTex", virtualCamera.GetDepthTexture());

            // assign the textures to the ui raw image component
            uiView.SetScreenVirtualCamera(virtualCamera.GetColorTexture());
            uiView.SetScreenForegroundMask(foregroundMaskTexture);
            uiView.SetScreenPhysicalCamera(rawPhysicalCameraTexture);
            uiView.SetOptionalScreen(virtualCamera.GetDepthTexture());

            InvokeRepeating("RunCycle", 0.0f, ((float)1 / virtualCamera.GetCameraSettings().framerate));

            uiView.Init();
        }

        // main loop

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

        // caching methods

        private void CacheWebcamDevices()
        {
            webCamDevices = WebCamTexture.devices;
        }

        private void CacheTargetObjects()
        {
            Camera[] cameras = FindObjectsOfType<Camera>();

            foreach (Camera camera in cameras)
                if (camera.gameObject.name != "cam_virtual" && camera.gameObject.name != "cam_ui")
                    targetObjects.Add(camera.gameObject);
        }

        // getter methods

        public WebCamDevice[] GetWebCamDevices()
        {
            return webCamDevices;
        }

        public List<GameObject> GetTargetObjects()
        {
            return targetObjects;
        }

        public List<CameraPreset> GetCameraPresets()
        {
            return cameraPresets;
        }

        public MrrVirtualCameraController GetVirtualCameraController()
        {
            return virtualCamera;
        }
    }
}