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

        public MrrVirtualCameraController cameraController;
        public MrrWebcamController webcamController;
        public MrrUiView uiView;
        public Camera uiCamera;

        public MrrVideoRecorder videoRecorder;

        public Material matForegroundMask;

        private CameraPreset[] cameraPresets;
        private WebCamDevice[] webCamDevices;
        private List<GameObject> targetObjects;
        private List<GameObject> sceneObjects;

        private Settings settings = new Settings();

        private RenderTexture foregroundMaskTexture;
        private WebCamTexture rawPhysicalCameraTexture;        

        // init methods - entry point for MixedRealtiyRecorder

        private void Start()
        {
            CacheCameraPresets();
            CacheWebcamDevices();
            CacheTargetObjects();
            CacheSceneObjects();

            cameraController.Init(cameraPresets[0].cameraSettings, targetObjects[0]);

            UpdateInternalTextures();
            uiView.Init();

            StartCycle();
        }

        public void RotateScene()
        {
            sceneObjects[0].transform.localEulerAngles = new Vector3(sceneObjects[0].transform.localEulerAngles.x, sceneObjects[0].transform.localEulerAngles.y + 180.0f, sceneObjects[0].transform.localEulerAngles.z);           
        }

        private void UpdateInternalTextures()
        {
            // screen size
            Vector2Int screenSize = new Vector2Int(cameraController.GetSettings().resolutionWidth, cameraController.GetSettings().resolutionHeight);

            foregroundMaskTexture = new RenderTexture(screenSize.x, screenSize.y, 0, RenderTextureFormat.ARGBHalf);

            if(rawPhysicalCameraTexture != null && rawPhysicalCameraTexture.isPlaying)
                rawPhysicalCameraTexture.Stop();
            else
            {
                rawPhysicalCameraTexture = new WebCamTexture("Blackmagic Design");
                //foreach (WebCamDevice webCamDevice in GetWebCamDevices())
                //{
                //    if (webCamDevice.name != settings.physicalCameraSource)
                //    {
                //        webcamController.StartWebcam("Logitech Webcam C925e");
                //        break;
                //    }
                //}

                webcamController.StartWebcam("Logitech Webcam C925e");
            }

            rawPhysicalCameraTexture.Play();

            // set foreground shader depth texture
            matForegroundMask.SetTexture("_ColorTex", cameraController.GetRawColorTextureForeground());

            // assign the textures to the ui raw image component
            uiView.SetScreenBackgroundLayer(cameraController.GetColorTextureBackground());
            uiView.SetScreenForegroundLayer(cameraController.GetColorTextureForeground());
            uiView.SetScreenPhysicalCamera(rawPhysicalCameraTexture);
            uiView.SetScreenForegroundMask(foregroundMaskTexture);
        }

        // main loop

        private void StartCycle()
        {
            InvokeRepeating("RunCycle", 0.0f, ((float)1 / cameraController.GetSettings().framerate));
        }

        private void RunCycle()
        {
            var stopWatch = Stopwatch.StartNew();

            cameraController.Render();
            UpdateForegroundMask();

            videoRecorder.RecordFrame(cameraController.GetColorTextureBackground(), foregroundMaskTexture);

            long frameTime = stopWatch.ElapsedMilliseconds;

            uiView.SetFooterFrameTime(frameTime);
        }

        public void UpdateForegroundMask()
        {
            Graphics.Blit(cameraController.GetRawColorTextureForeground(), foregroundMaskTexture, matForegroundMask);
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
                videoRecorder.StartRecording(settings.outputPath, Utility.Util.GetOutputFormat(settings.outputFormat), new Vector2Int(cameraController.GetSettings().resolutionWidth, cameraController.GetSettings().resolutionHeight));
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
            cameraController.SetCameraSettings(settings.cameraSettings);
            cameraController.SetCameraSettings(settings.cameraSettings);
            cameraController.SetTargetObject(GetTargetObjectByName(settings.targetObject));
            UpdateInternalTextures();
            StartCycle();

            //UnityEngine.Debug.Log("Applyed Settings!");
        }

        // caching methods

        private void CacheCameraPresets()
        {
            cameraPresets = new CameraPreset[3];
            cameraPresets[0] = CreateBmpcc4kPreset();
            cameraPresets[1] = CreateWebcamPreset();
            cameraPresets[2] = CreateCanon7dPreset();
        }

        private void CacheWebcamDevices()
        {
            webCamDevices = WebCamTexture.devices;
            foreach (WebCamDevice device in webCamDevices)
                UnityEngine.Debug.Log("WebCamDevice = " + device.name);
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
        }

        private void CacheSceneObjects()
        {
            sceneObjects = new List<GameObject>();

            if (GameObject.FindGameObjectsWithTag("Scene").Length > 0)
            {
                GameObject[] scenes = GameObject.FindGameObjectsWithTag("Scene");

                foreach (GameObject scene in scenes)
                    sceneObjects.Add(scene);
            }
        }

        // getter methods

        public Settings GetSettings()
        {
            return settings;
        }

        public CameraPreset[] GetCameraPresets()
        {
            return cameraPresets;
        }

        public CameraSetting GetCameraSettingByPresetName(string name)
        {
            foreach (CameraPreset cameraPreset in cameraPresets)
                if (cameraPreset.presetName == name)
                    return cameraPreset.cameraSettings;

            return cameraPresets[0].cameraSettings;
        }

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

        public List<GameObject> GetSceneObjects()
        {
            return sceneObjects;
        }

        public MrrVirtualCameraController GetCameraController()
        {
            return cameraController;
        }

        // camera presets

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
            currCamSetting.sensorWidth = 17.31f;
            currCamSetting.sensorHeight = 12.89f;

            CameraPreset currCamPreset = new CameraPreset();
            currCamPreset.presetName = "Pocket Cinema Camera 4k";
            currCamPreset.cameraSettings = currCamSetting;

            return currCamPreset;
        }

        private CameraPreset CreateCanon7dPreset()
        {
            CameraSetting currCamSetting = new CameraSetting();
            currCamSetting.resolutionWidth = 1920;
            currCamSetting.resolutionHeight = 1080;
            currCamSetting.framerate = 60;
            currCamSetting.focalLenth = 16;
            currCamSetting.sensorWidth = 22.3f;
            currCamSetting.sensorHeight = 14.9f;

            CameraPreset currCamPreset = new CameraPreset();
            currCamPreset.presetName = "Canon 7D";
            currCamPreset.cameraSettings = currCamSetting;

            return currCamPreset;
        }

        public void SetSceneOffsetPosition(int indexSceneObject, float value, Vector3Component component)
        {
            switch (component)
            {
                case Vector3Component.x:
                    sceneObjects[indexSceneObject].transform.localPosition = new Vector3(value, sceneObjects[indexSceneObject].transform.localPosition.y, sceneObjects[indexSceneObject].transform.localPosition.z);
                    break;
                case Vector3Component.y:
                    sceneObjects[indexSceneObject].transform.localPosition = new Vector3(sceneObjects[indexSceneObject].transform.localPosition.x, value, sceneObjects[indexSceneObject].transform.localPosition.z);
                    break;
                case Vector3Component.z:
                    sceneObjects[indexSceneObject].transform.localPosition = new Vector3(sceneObjects[indexSceneObject].transform.localPosition.x, sceneObjects[indexSceneObject].transform.localPosition.y, value);
                    break;
                default:
                    break;
            }
        }

        public Vector3 GetSceneOffsetPosition(int indexSceneObject)
        {
            return sceneObjects[indexSceneObject].transform.localPosition;
        }

        public void SetSceneOffsetRotation(int indexSceneObject, float value, Vector3Component component)
        {
            switch (component)
            {
                case Vector3Component.x:
                    sceneObjects[indexSceneObject].transform.localEulerAngles = new Vector3(value, sceneObjects[indexSceneObject].transform.localEulerAngles.y, sceneObjects[indexSceneObject].transform.localEulerAngles.z);
                    break;
                case Vector3Component.y:
                    sceneObjects[indexSceneObject].transform.localEulerAngles = new Vector3(sceneObjects[indexSceneObject].transform.localEulerAngles.x, value, sceneObjects[indexSceneObject].transform.localEulerAngles.z);
                    break;
                case Vector3Component.z:
                    sceneObjects[indexSceneObject].transform.localEulerAngles = new Vector3(sceneObjects[indexSceneObject].transform.localEulerAngles.x, sceneObjects[indexSceneObject].transform.localEulerAngles.y, value);
                    break;
                default:
                    break;
            }
        }

        public Vector3 GetSceneOffsetRotation(int indexSceneObject)
        {
            return sceneObjects[indexSceneObject].transform.localEulerAngles;
        }
    }
}