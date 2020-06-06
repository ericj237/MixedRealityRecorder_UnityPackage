using UnityEngine;
using MRR.Model;
using MRR.View;
using System.Collections.Generic;
using System.Diagnostics;
using MRR.Video;

namespace MRR.Controller
{

    public class MrrAppController : MonoBehaviour
    {

        public MrrVirtualCameraController virtualCamera;
        public MrrUiView uiView;
        public Camera uiCamera;

        public Material matForegroundMask;

        public List<CameraPreset> cameraPresets = new List<CameraPreset>();
        public List<GameObject> targetObjects = new List<GameObject>();
        private WebCamDevice[] webCamDevices;

        private Settings settings = new Settings();

        private RenderTexture foregroundMaskTexture;
        private WebCamTexture rawPhysicalCameraTexture;

        public MrrVideoRecorder videoRecorder;

        public Mesh targetMesh;
        public Material targetMaterial;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F11))
                uiCamera.enabled = !uiCamera.enabled;
        }

        public Settings GetSettings()
        {
            return settings;
        }

        public void ToggleRecord()
        {
            if (!videoRecorder.IsRecording())
                videoRecorder.StartRecording(settings.outputPath, GetOutputFormat(settings.outputFormat), new Vector2Int(virtualCamera.GetCameraSettings().resolutionWidth, virtualCamera.GetCameraSettings().resolutionHeight));
            else
                videoRecorder.StopRecording();
        }

        public void ApplySettings(Settings settings)
        {
            string oldTargetObjectName = this.settings.targetObject;
            this.settings = settings;

            CancelInvoke();

            SetTargetObject(oldTargetObjectName, settings.targetObject);
            virtualCamera.SetCameraSettings(settings.cameraSettings);
            virtualCamera.SetTargetObject(GetTargetObjectByName(settings.targetObject));
            UpdateInternalTextures();
            StartCycle();

            UnityEngine.Debug.Log("Applyed Settings!");
        }

        private void SetTargetObject(string oldTarget, string newTarget)
        {

            GameObject oldTargetObject = GameObject.Find(oldTarget);

            if(oldTargetObject != null)
            {
                Destroy(oldTargetObject.GetComponent<SphereCollider>());
                Destroy(oldTargetObject.GetComponent<MeshRenderer>());
                Destroy(oldTargetObject.GetComponent<MeshFilter>());
            }

            GameObject newTargetObject = GameObject.Find(newTarget);

            if(newTargetObject.GetComponent<MeshFilter>() == null)
                newTargetObject.AddComponent<MeshFilter>().mesh = targetMesh;

            if (newTargetObject.GetComponent<MeshRenderer>() == null)
                newTargetObject.AddComponent<MeshRenderer>().material = targetMaterial;

            if (newTargetObject.GetComponent<SphereCollider>() == null)
                newTargetObject.AddComponent<SphereCollider>();

        }

        private GameObject GetTargetObjectByName(string name)
        {
            foreach (GameObject target in targetObjects)
                if (target.name == name)
                    return target;
            return null;
        }

        // init methods - entry point for MixedRealtiyRecorder

        void Start()
        {
            CacheWebcamDevices();
            CacheTargetObjects();

            virtualCamera.Init(cameraPresets[0].cameraSettings, targetObjects[0]);

            UpdateInternalTextures();
            uiView.Init();

            StartCycle();
        }

        private void UpdateInternalTextures()
        {
            // screen size
            Vector2Int screenSize = new Vector2Int(virtualCamera.GetCameraSettings().resolutionWidth, virtualCamera.GetCameraSettings().resolutionHeight);

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
            matForegroundMask.SetTexture("_DepthTex", virtualCamera.GetDepthTexture());

            // assign the textures to the ui raw image component
            uiView.SetScreenVirtualCamera(virtualCamera.GetColorTexture());
            uiView.SetScreenForegroundMask(foregroundMaskTexture);
            uiView.SetScreenPhysicalCamera(rawPhysicalCameraTexture);
            uiView.SetOptionalScreen(virtualCamera.GetDepthTexture());
        }

        // main loop

        private void StartCycle()
        {
            InvokeRepeating("RunCycle", 0.0f, ((float)1 / virtualCamera.GetCameraSettings().framerate));
        }

        private void RunCycle()
        {
            var stopWatch = Stopwatch.StartNew();

            virtualCamera.Render();
            UpdateForegroundMask();

            videoRecorder.RecordFrame(virtualCamera.GetColorTexture(), foregroundMaskTexture);

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

            if (GameObject.FindGameObjectsWithTag("Target").Length > 0)
            {
                GameObject[] targets = GameObject.FindGameObjectsWithTag("Target");

                foreach (GameObject target in targets)
                    targetObjects.Add(target);
            }
            else
            {
                GameObject debugTarget = new GameObject();
                debugTarget.name = "debugTarget";
                debugTarget.transform.position = virtualCamera.gameObject.transform.position + virtualCamera.gameObject.transform.forward * 100;
                targetObjects.Add(debugTarget);
            }
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

        public CameraSetting GetCameraSettingByPresetName(string name)
        {
            foreach (CameraPreset cameraPreset in cameraPresets)
                if (cameraPreset.presetName == name)
                    return cameraPreset.cameraSettings;

            return cameraPresets[0].cameraSettings;
        }

        public OutputFormat GetOutputFormat(string input)
        {
            switch (input)
            {
                case "TGA Image Sequence":
                    return OutputFormat.TgaImageSequence;
                case "BMP Image Sequence":
                    return OutputFormat.BmpImageSequence;
                default:
                    return OutputFormat.ManualScreencapture;
            }
        }

        public MrrVirtualCameraController GetVirtualCameraController()
        {
            return virtualCamera;
        }
    }
}