using UnityEngine;
using MRR.Model;
using MRR.View;
using MRR.Utility;
using System.Collections.Generic;
using System.Diagnostics;
using MRR.Video;

namespace MRR.Controller
{

    public class MrrAppController : MonoBehaviour
    {

        public MrrUiView uiView;
        public Camera uiCamera;

        public MrrVirtualCameraController virtualCameraController;
        public Material matForegroundMask;

        private CameraPreset[] cameraPresets;
        private List<GameObject> targetObjects;
        private WebCamDevice[] webCamDevices;

        private Settings settings = new Settings();

        private RenderTexture foregroundMaskTexture;
        private WebCamTexture rawPhysicalCameraTexture;

        public MrrVideoRecorder videoRecorder;

        public CameraPreset CreateWebcamPreset()
        {            
            CameraSetting currCamSetting = new CameraSetting();
            currCamSetting.resolutionWidth = 1280;
            currCamSetting.resolutionHeight = 720;
            currCamSetting.framerate = 30;
            currCamSetting.focalLenth = 4;
            currCamSetting.sensorWidth = 3;
            currCamSetting.sensorHeight = 2;

            CameraPreset currCamPreset = new CameraPreset();
            currCamPreset.presetName = "Webcam";
            currCamPreset.cameraSettings = currCamSetting;

            return currCamPreset;
        }

        private CameraPreset CreateBmpcc4kPreset()
        {
            CameraSetting currCamSetting = new CameraSetting();
            currCamSetting.resolutionWidth = 1920;
            currCamSetting.resolutionHeight = 1080;
            currCamSetting.framerate = 60;
            currCamSetting.focalLenth = 18;
            currCamSetting.sensorWidth = 18;
            currCamSetting.sensorHeight = 14;

            CameraPreset currCamPreset = new CameraPreset();
            currCamPreset.presetName = "Pocket Cinema Camera 4k";
            currCamPreset.cameraSettings = currCamSetting;

            return currCamPreset;
        }

        private void CacheCameraPresets()
        {
            cameraPresets = new CameraPreset[2];
            cameraPresets[0] = CreateBmpcc4kPreset();
            cameraPresets[1] = CreateWebcamPreset();
        }

        // init methods - entry point for MixedRealtiyRecorder

        private void Start()
        {
            CacheCameraPresets();
            CacheWebcamDevices();
            CacheTargetObjects();

            virtualCameraController.Init(cameraPresets[0].cameraSettings, targetObjects[0]);

            UpdateInternalTextures();
            uiView.Init();

            StartCycle();
        }

        private void UpdateInternalTextures()
        {
            // screen size
            Vector2Int screenSize = new Vector2Int(virtualCameraController.GetCameraSettings().resolutionWidth, virtualCameraController.GetCameraSettings().resolutionHeight);

            foregroundMaskTexture = new RenderTexture(screenSize.x, screenSize.y, 0, RenderTextureFormat.ARGBHalf);

            if(rawPhysicalCameraTexture != null && rawPhysicalCameraTexture.isPlaying)
            {
                rawPhysicalCameraTexture.Stop();
            }
            else
            {
                rawPhysicalCameraTexture = new WebCamTexture(settings.physicalCameraSource);
            }

            rawPhysicalCameraTexture.Play();

            // set foreground shader depth texture
            matForegroundMask.SetTexture("_DepthTex", virtualCameraController.GetDepthTexture());

            // assign the textures to the ui raw image component
            uiView.SetScreenVirtualCamera(virtualCameraController.GetColorBackgroundTexture());
            uiView.SetScreenForegroundMask(foregroundMaskTexture);
            uiView.SetScreenPhysicalCamera(rawPhysicalCameraTexture);
            uiView.SetOptionalScreen(virtualCameraController.GetDepthTexture());
        }

        // main loop

        private void StartCycle()
        {
            InvokeRepeating("RunCycle", 0.0f, ((float)1 / virtualCameraController.GetCameraSettings().framerate));
        }

        private void RunCycle()
        {
            var stopWatch = Stopwatch.StartNew();

            virtualCameraController.Render();
            UpdateForegroundMask();

            videoRecorder.RecordFrame(virtualCameraController.GetColorBackgroundTexture(), foregroundMaskTexture);

            long frameTime = stopWatch.ElapsedMilliseconds;

            uiView.SetFooterFrameTime(frameTime);
        }

        public void UpdateForegroundMask()
        {
            // we only use this method to receive our processed foreground mask texture
            // all shader properties are already set in Initialize() method
            Graphics.Blit(virtualCameraController.GetDepthTexture(), foregroundMaskTexture, matForegroundMask);
        }

        // user input

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F11))
                uiCamera.enabled = !uiCamera.enabled;
        }

        public void ToggleRecord()
        {
            if (!videoRecorder.IsRecording())
                videoRecorder.StartRecording(settings.outputPath, Utility.Util.GetOutputFormat(settings.outputFormat), new Vector2Int(virtualCameraController.GetCameraSettings().resolutionWidth, virtualCameraController.GetCameraSettings().resolutionHeight));
            else
                videoRecorder.StopRecording();
        }

        // update methods

        public void ApplySettings(Settings settings)
        {
            //string oldTargetObjectName = this.settings.targetObject;
            this.settings = settings;

            CancelInvoke();

            //SetTargetObject(oldTargetObjectName, settings.targetObject);
            virtualCameraController.SetCameraSettings(settings.cameraSettings);
            virtualCameraController.SetCameraSettings(settings.cameraSettings);
            virtualCameraController.SetTargetObject(GetTargetObjectByName(settings.targetObject));
            UpdateInternalTextures();
            StartCycle();

            //UnityEngine.Debug.Log("Applyed Settings!");
        }

        // caching methods

        private void CacheWebcamDevices()
        {
            webCamDevices = WebCamTexture.devices;
        }

        private void CacheTargetObjects()
        {
            targetObjects = new List<GameObject>();

            if (GameObject.FindGameObjectsWithTag("Target").Length > 0)
            {
                GameObject[] targets = GameObject.FindGameObjectsWithTag("Target");

                foreach (GameObject target in targets)
                    targetObjects.Add(target);
            }
            //else
            //{
            //    GameObject debugTarget = new GameObject();
            //    debugTarget.name = "debugTarget";
            //    debugTarget.transform.position = virtualCameraColor.gameObject.transform.position + virtualCameraColor.gameObject.transform.forward * 100;
            //    targetObjects.Add(debugTarget);
            //}
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

        private GameObject GetTargetObjectByName(string name)
        {
            foreach (GameObject target in targetObjects)
                if (target.name == name)
                    return target;
            return null;
        }

        public CameraPreset[] GetCameraPresets()
        {
            return cameraPresets;
        }

        public Settings GetSettings()
        {
            return settings;
        }

        public CameraSetting GetCameraSettingByPresetName(string name)
        {
            foreach (CameraPreset cameraPreset in cameraPresets)
                if (cameraPreset.presetName == name)
                    return cameraPreset.cameraSettings;

            return cameraPresets[0].cameraSettings;
        }

        public MrrVirtualCameraController GetVirtualCameraController()
        {
            return virtualCameraController;
        }
    }
}