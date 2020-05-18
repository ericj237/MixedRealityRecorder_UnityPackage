using UnityEngine;
using UnityEngine.UI;
using MRR.Model;
using MRR.Controller;
using System.Collections;
using System.Collections.Generic;

namespace MRR.View
{

    public class MrrUiView : MonoBehaviour
    {

        public MrrAppController appController;

        [Header("Screens")]
        public RawImage screenVirtualCamera;
        public RawImage screenForegroundMask;
        public RawImage screenPhysicalCamera;
        public RawImage screenOptional;

        [Header("Physical Camera Input")]
        public Dropdown dPhysicalCameraInputSource;
        [Header("Camera Preset")]
        public Dropdown dCameraPresetDevice;
        [Header("Camera Settings")]
        public InputField[] iCameraResolution = new InputField[2];
        public InputField iCameraFramerate;
        [Header("Lens Setting")]
        public InputField iCameraFocalLenth;
        [Header("Sensor Settings")]
        public InputField[] iSensorSize = new InputField[2];
        public InputField iSensorDynamicRange;
        [Header("Buttons")]
        public Button bApplyA;
        public Button bResetA;

        [Header("Sensor Offset")]
        public InputField[] iSensorOffsetPosition = new InputField[3];
        public InputField[] iSensorOffsetRotation = new InputField[3];
        [Header("Output Settings")]
        public Dropdown dOutputDestination;
        public Dropdown dOutputCodec;
        [Header("Output Settings")]
        public Dropdown dOptionalScreenInputSource;

        [Header("Controls")]
        public Button bRecord;

        [Header("Footer")]
        public Text[] tCameraResolution = new Text[2];
        public Text tCameraFramerate;
        public Text tOutputContainer;
        public Text tOutputCodec;
        public Text tFrameRecordTime;
        public Text tAllocatedMemory;

        // Events
        private InputField.OnChangeEvent eventVirtualCameraOffsetPositionX = new InputField.OnChangeEvent();
        private InputField.OnChangeEvent eventVirtualCameraOffsetPositionY = new InputField.OnChangeEvent();
        private InputField.OnChangeEvent eventVirtualCameraOffsetPositionZ = new InputField.OnChangeEvent();

        private InputField.OnChangeEvent eventVirtualCameraOffsetRotationX = new InputField.OnChangeEvent();
        private InputField.OnChangeEvent eventVirtualCameraOffsetRotationY = new InputField.OnChangeEvent();
        private InputField.OnChangeEvent eventVirtualCameraOffsetRotationZ = new InputField.OnChangeEvent();

        private void Start()
        {
            RegisterEvents();
            StartCoroutine(Init());
        }

        private IEnumerator Init()
        {
            yield return new WaitForSeconds(1.0f);
            SetPhysicalCameraInputSources();
            SetOptionalScreenInputSources();
            SetCameraPresets();
            UpdateValues();
        }

        private void SetCameraPresets()
        {
            dCameraPresetDevice.ClearOptions();

            List<CameraPreset> cameraPresets = appController.GetCameraPresets();

            List<Dropdown.OptionData> optionData = new List<Dropdown.OptionData>();

            foreach (CameraPreset device in cameraPresets)
                optionData.Add(new Dropdown.OptionData(device.presetName));

            dCameraPresetDevice.AddOptions(optionData);
        }

        private void SetPhysicalCameraInputSources()
        {
            dPhysicalCameraInputSource.ClearOptions();

            WebCamDevice[] webCamDevices = appController.GetWebCamDevices();

            List<Dropdown.OptionData> optionData = new List<Dropdown.OptionData>();

            foreach (WebCamDevice source in webCamDevices)
                optionData.Add(new Dropdown.OptionData(source.name));

            dPhysicalCameraInputSource.AddOptions(optionData);
        }

        private void SetOptionalScreenInputSources()
        {
            dOptionalScreenInputSource.ClearOptions();

            WebCamDevice[] webCamDevices = appController.GetWebCamDevices();

            List<Dropdown.OptionData> optionData = new List<Dropdown.OptionData>();

            optionData.Add(new Dropdown.OptionData("None"));
            //optionData.Add(new Dropdown.OptionData("Composite"));

            foreach (WebCamDevice source in webCamDevices)
            {
                if (source.name != dPhysicalCameraInputSource.captionText.text)
                    optionData.Add(new Dropdown.OptionData(source.name));
            }

            dOptionalScreenInputSource.AddOptions(optionData);            
        }

        private void UpdateValues()
        {
            UpdateCameraResolutionWidth();
            UpdateCameraResolutionHeight();
            UpdateCameraFramerate();
            UpdateCameraFocalLength();
            UpdateSensorHeight();
            UpdateSensorWidth();
            UpdateSensorDynamicRange();

            UpdateSensorOffsetPosition();
            UpdateSensorOffsetRotation();

            CameraSettings currCameraSettings = appController.GetCameraSettings();

            SetFooterResolutionWidth(currCameraSettings.resolutionWidth);
            SetFooterResolutionHeight(currCameraSettings.resolutionHeight);
            SetFooterFramerate(currCameraSettings.framerate);
            SetFooterContainer(Container.MP4);
            SetFooterCodec(Codec.H246);
            //SetFooterFrameTime();
            //SetFooterAllocatedMemory();
        }

        private void UpdateCameraResolutionWidth()
        {
            iCameraResolution[0].SetTextWithoutNotify(appController.GetCameraSettings().resolutionWidth.ToString());
        }

        private void UpdateCameraResolutionHeight()
        {
            iCameraResolution[1].SetTextWithoutNotify(appController.GetCameraSettings().resolutionHeight.ToString());
        }

        private void UpdateCameraFramerate()
        {
            iCameraFramerate.SetTextWithoutNotify(appController.GetCameraSettings().framerate.ToString());
        }

        private void UpdateCameraFocalLength()
        {
            iCameraFocalLenth.SetTextWithoutNotify(appController.GetCameraSettings().focalLenth.ToString());
        }

        private void UpdateSensorHeight()
        {
            iSensorSize[0].SetTextWithoutNotify(appController.GetCameraSettings().sensorHeight.ToString());
        }

        private void UpdateSensorWidth()
        {
            iSensorSize[1].SetTextWithoutNotify(appController.GetCameraSettings().sensorWidth.ToString());
        }

        private void UpdateSensorDynamicRange()
        {
            iSensorDynamicRange.SetTextWithoutNotify(appController.GetCameraSettings().dynamicRange.ToString());
        }

        private void UpdateSensorOffsetPosition()
        {
            Vector3 sensorOffsetPosition = appController.GetSensorOffsetPosition();
            iSensorOffsetPosition[0].SetTextWithoutNotify(sensorOffsetPosition.x.ToString());
            iSensorOffsetPosition[1].SetTextWithoutNotify(sensorOffsetPosition.y.ToString());
            iSensorOffsetPosition[2].SetTextWithoutNotify(sensorOffsetPosition.z.ToString());
        }

        private void UpdateSensorOffsetRotation()
        {
            Vector3 sensorOffsetRoation = appController.GetSensorOffsetRotation();
            iSensorOffsetRotation[0].SetTextWithoutNotify(sensorOffsetRoation.x.ToString());
            iSensorOffsetRotation[1].SetTextWithoutNotify(sensorOffsetRoation.y.ToString());
            iSensorOffsetRotation[2].SetTextWithoutNotify(sensorOffsetRoation.z.ToString());
        }

        private void SetFooterResolutionWidth(int resolutionWidth)
        {
            tCameraResolution[0].text = resolutionWidth.ToString();
        }

        private void SetFooterResolutionHeight(int resolutionHeight)
        {
            tCameraResolution[1].text = resolutionHeight.ToString();
        }

        private void SetFooterFramerate(int framerate)
        {
            tCameraFramerate.text = framerate.ToString();
        }

        private void SetFooterContainer(Container container)
        {
            switch(container)
            {
                case Container.MP4:
                    tOutputContainer.text = "MP4";
                    break;
                default:
                    break;
            }
        }

        private void SetFooterCodec(Codec codec)
        {
            switch (codec)
            {
                case Codec.H246:
                    tOutputCodec.text = "H.246";
                    break;
                default:
                    break;
            }
        }

        private void SetFooterFrameTime(int frameTime)
        {
            tFrameRecordTime.text = frameTime.ToString();
        }

        private void SetFooterAllocatedMemory(int memory)
        {
            tAllocatedMemory.text = memory.ToString();
        }

        private void RegisterEvents()
        {
            RegisterEventsSensorOffset();
        }

        private void RegisterEventsSensorOffset()
        {
            // events offset position
            eventVirtualCameraOffsetPositionX.AddListener(SetSensorOffsetPositionX);
            iSensorOffsetPosition[0].onValueChanged = eventVirtualCameraOffsetPositionX;

            eventVirtualCameraOffsetPositionY.AddListener(SetSensorOffsetPositionY);
            iSensorOffsetPosition[1].onValueChanged = eventVirtualCameraOffsetPositionY;

            eventVirtualCameraOffsetPositionZ.AddListener(SetSensorOffsetPositionZ);
            iSensorOffsetPosition[2].onValueChanged = eventVirtualCameraOffsetPositionZ;

            // events offset rotation
            eventVirtualCameraOffsetRotationX.AddListener(SetSensorOffsetRotationX);
            iSensorOffsetRotation[0].onValueChanged = eventVirtualCameraOffsetRotationX;

            eventVirtualCameraOffsetRotationY.AddListener(SetSensorOffsetRotationY);
            iSensorOffsetRotation[1].onValueChanged = eventVirtualCameraOffsetRotationY;

            eventVirtualCameraOffsetRotationZ.AddListener(SetSensorOffsetRotationZ);
            iSensorOffsetRotation[2].onValueChanged = eventVirtualCameraOffsetRotationZ;
        }

        private void SetSensorOffsetPositionX(string x)
        {
            appController.SetSensorOffsetPosition(float.Parse(x), Vector3Component.x);
        }

        private void SetSensorOffsetPositionY(string y)
        {
            appController.SetSensorOffsetPosition(float.Parse(y), Vector3Component.y);
        }

        private void SetSensorOffsetPositionZ(string z)
        {
            appController.SetSensorOffsetPosition(float.Parse(z), Vector3Component.z);
        }

        private void SetSensorOffsetRotationX(string x)
        {
            appController.SetSensorOffsetRotation(float.Parse(x), Vector3Component.x);
            UpdateSensorOffsetRotation();
        }

        private void SetSensorOffsetRotationY(string y)
        {
            appController.SetSensorOffsetRotation(float.Parse(y), Vector3Component.y);
            UpdateSensorOffsetRotation();
        }

        private void SetSensorOffsetRotationZ(string z)
        {
            appController.SetSensorOffsetRotation(float.Parse(z), Vector3Component.z);
            UpdateSensorOffsetRotation();
        }

        public void SetScreenPhysicalCamera(RenderTexture physicalCameraTexture)
        {
            screenPhysicalCamera.texture = physicalCameraTexture;
        }

        public void SetScreenPhysicalCamera(WebCamTexture rawPhysicalCameraTexture)
        {
            screenPhysicalCamera.texture = rawPhysicalCameraTexture;
        }

        public void SetScreenPhysicalCamera(Texture2D physicalCameraTexture)
        {
            screenPhysicalCamera.texture = physicalCameraTexture;
        }

        public void SetScreenVirtualCamera(RenderTexture colorTexture)
        {
            screenVirtualCamera.texture = colorTexture;
        }

        public void SetScreenForegroundMask(RenderTexture foregroundMaskTexture)
        {
            screenForegroundMask.texture = foregroundMaskTexture;
        }
    }
}