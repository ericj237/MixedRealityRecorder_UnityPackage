using UnityEngine;
using UnityEngine.UI;
using MRR.Model;
using MRR.Controller;
using System.Collections.Generic;

namespace MRR.View
{

    public class MrrUiView : MonoBehaviour
    {
        public MrrAppController appController;

        [Header("Physical Camera")]
        public Dropdown dPhysicalCameraSource;
        [Header("Target")]
        public Dropdown dTargetObject;
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
        [Header("Optional Screen")]
        public Dropdown dOptionalScreenSource;
        [Header("Output Settings")]
        public InputField iOutputPath;
        public Dropdown dOutputCodec;
        [Header("Buttons")]
        public Button bApplyA;
        public Button bResetA;

        [Header("Sensor Offset")]
        public InputField[] iSensorOffsetPosition = new InputField[3];
        public InputField[] iSensorOffsetRotation = new InputField[3];

        [Header("Controls")]
        public Button bRecord;

        [Header("Footer")]
        public Text[] tCameraResolution = new Text[2];
        public Text tCameraFramerate;
        public Text tOutputContainer;
        public Text tOutputCodec;
        public Text tFrameTime;
        public Text tAllocatedMemory;

        [Header("Screens")]
        public RawImage screenVirtualCamera;
        public RawImage screenForegroundMask;
        public RawImage screenPhysicalCamera;
        public RawImage screenOptional;

        public void Init()
        {
            //EnableButtonApplyA(false);
            //EnableButtonResetA(false);

            SetPhysicalCameraInputSources();
            SetTargetObjects();
            SetCameraPresets();
            SetCameraValues();
            SetOptionalScreenInputSources();
            SetOutputPath(Application.persistentDataPath);
            SetOutputCodecPresets();
            SetFooterValues(appController.GetVirtualCameraController().GetCameraSettings());

            RegisterEvents();
        }

        private void RegisterEvents()
        {

            dPhysicalCameraSource.onValueChanged.AddListener(delegate
            {
                OnPhysicalCameraChanged(dPhysicalCameraSource.captionText.text);
            });

            dTargetObject.onValueChanged.AddListener(delegate
            {
                OnTargetObjectChanged(dTargetObject.captionText.text);
            });

            dCameraPresetDevice.onValueChanged.AddListener(delegate
            {
                OnCameraPresetChanged(dCameraPresetDevice.captionText.text);
            });

            // HERE CAMERA VALUES CALLBACKS

            iOutputPath.onEndEdit.AddListener(delegate
            {
                OnOutputPathChanged(iOutputPath.text);
            });

            dOutputCodec.onValueChanged.AddListener(delegate
            {
                OnOuputCodecChanged(dOutputCodec.captionText.text);
            });

            // events offset position
            iSensorOffsetPosition[0].onValueChanged.AddListener(delegate
            {
                OnSensorOffsetPositionXChanged(iSensorOffsetPosition[0].text);
            });

            iSensorOffsetPosition[1].onValueChanged.AddListener(delegate
            {
                OnSensorOffsetPositionYChanged(iSensorOffsetPosition[1].text);
            });

            iSensorOffsetPosition[2].onValueChanged.AddListener(delegate
            {
                OnSensorOffsetPositionZChanged(iSensorOffsetPosition[2].text);
            });

            // events offset rotation
            iSensorOffsetRotation[0].onValueChanged.AddListener(delegate
            {
                OnSensorOffsetRotationXChanged(iSensorOffsetRotation[0].text);
            });

            iSensorOffsetRotation[1].onValueChanged.AddListener(delegate
            {
                OnSensorOffsetRotationYChanged(iSensorOffsetRotation[1].text);
            });

            iSensorOffsetRotation[2].onValueChanged.AddListener(delegate
            {
                OnSensorOffsetRotationZChanged(iSensorOffsetRotation[2].text);
            });
        }

        // event callback methods

        private void OnPhysicalCameraChanged(string sourceName)
        {
            Debug.Log("Changed physical camera source!");
        }

        private void OnTargetObjectChanged(string targetName)
        {
            Debug.Log("Changed target object!");
        }

        private void OnCameraPresetChanged(string presetName)
        {
            Debug.Log("Changed camera preset!");
        }
                          
        private void OnOutputPathChanged(string path)
        {
            Debug.Log("Changed output path!");
        } 

        private void OnOuputCodecChanged(string codecName)
        {
            Debug.Log("Changed output codec!");
        } 

        private void OnSensorOffsetPositionXChanged(string x)
        {
            appController.GetVirtualCameraController().SetSensorOffsetPosition(float.Parse(x), Vector3Component.x);
        }

        private void OnSensorOffsetPositionYChanged(string y)
        {
            appController.GetVirtualCameraController().SetSensorOffsetPosition(float.Parse(y), Vector3Component.y);
        }

        private void OnSensorOffsetPositionZChanged(string z)
        {
            appController.GetVirtualCameraController().SetSensorOffsetPosition(float.Parse(z), Vector3Component.z);
        }                    

        private void OnSensorOffsetRotationXChanged(string x)
        {
            appController.GetVirtualCameraController().SetSensorOffsetRotation(float.Parse(x), Vector3Component.x);
            UpdateSensorOffsetRotation();
        }

        private void OnSensorOffsetRotationYChanged(string y)
        {
            appController.GetVirtualCameraController().SetSensorOffsetRotation(float.Parse(y), Vector3Component.y);
            UpdateSensorOffsetRotation();
        }

        private void OnSensorOffsetRotationZChanged(string z)
        {
            appController.GetVirtualCameraController().SetSensorOffsetRotation(float.Parse(z), Vector3Component.z);
            UpdateSensorOffsetRotation();
        }

        // update methods

        private void UpdateCameraResolutionWidth()
        {
            iCameraResolution[0].SetTextWithoutNotify(appController.GetVirtualCameraController().GetCameraSettings().resolutionWidth.ToString());
        }

        private void UpdateCameraResolutionHeight()
        {
            iCameraResolution[1].SetTextWithoutNotify(appController.GetVirtualCameraController().GetCameraSettings().resolutionHeight.ToString());
        }

        private void UpdateCameraFramerate()
        {
            iCameraFramerate.SetTextWithoutNotify(appController.GetVirtualCameraController().GetCameraSettings().framerate.ToString());
        }

        private void UpdateCameraFocalLength()
        {
            iCameraFocalLenth.SetTextWithoutNotify(appController.GetVirtualCameraController().GetCameraSettings().focalLenth.ToString());
        }

        private void UpdateSensorHeight()
        {
            iSensorSize[0].SetTextWithoutNotify(appController.GetVirtualCameraController().GetCameraSettings().sensorHeight.ToString());
        }

        private void UpdateSensorWidth()
        {
            iSensorSize[1].SetTextWithoutNotify(appController.GetVirtualCameraController().GetCameraSettings().sensorWidth.ToString());
        }

        private void UpdateSensorDynamicRange()
        {
            iSensorDynamicRange.SetTextWithoutNotify(appController.GetVirtualCameraController().GetCameraSettings().dynamicRange.ToString());
        }

        private void UpdateSensorOffsetPosition()
        {
            Vector3 sensorOffsetPosition = appController.GetVirtualCameraController().GetSensorOffsetPosition();
            iSensorOffsetPosition[0].SetTextWithoutNotify(sensorOffsetPosition.x.ToString());
            iSensorOffsetPosition[1].SetTextWithoutNotify(sensorOffsetPosition.y.ToString());
            iSensorOffsetPosition[2].SetTextWithoutNotify(sensorOffsetPosition.z.ToString());
        }

        private void UpdateSensorOffsetRotation()
        {
            Vector3 sensorOffsetRoation = appController.GetVirtualCameraController().GetSensorOffsetRotation();
            iSensorOffsetRotation[0].SetTextWithoutNotify(sensorOffsetRoation.x.ToString());
            iSensorOffsetRotation[1].SetTextWithoutNotify(sensorOffsetRoation.y.ToString());
            iSensorOffsetRotation[2].SetTextWithoutNotify(sensorOffsetRoation.z.ToString());
        }

        // setter methods

        private void SetPhysicalCameraInputSources()
        {
            dPhysicalCameraSource.ClearOptions();

            WebCamDevice[] webCamDevices = appController.GetWebCamDevices();

            List<Dropdown.OptionData> optionData = new List<Dropdown.OptionData>();

            foreach (WebCamDevice source in webCamDevices)
                optionData.Add(new Dropdown.OptionData(source.name));

            dPhysicalCameraSource.AddOptions(optionData);
        }

        private void SetTargetObjects()
        {
            dTargetObject.ClearOptions();

            List<GameObject> targetObjects = appController.GetTargetObjects();

            List<Dropdown.OptionData> optionData = new List<Dropdown.OptionData>();

            foreach (GameObject target in targetObjects)
                optionData.Add(new Dropdown.OptionData(target.name));

            dTargetObject.AddOptions(optionData);
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

        private void SetOptionalScreenInputSources()
        {
            dOptionalScreenSource.ClearOptions();

            WebCamDevice[] webCamDevices = appController.GetWebCamDevices();

            List<Dropdown.OptionData> optionData = new List<Dropdown.OptionData>();

            optionData.Add(new Dropdown.OptionData("None"));
            //optionData.Add(new Dropdown.OptionData("Composite"));

            foreach (WebCamDevice source in webCamDevices)
            {
                if (source.name != dPhysicalCameraSource.captionText.text)
                    optionData.Add(new Dropdown.OptionData(source.name));
            }

            dOptionalScreenSource.AddOptions(optionData);
        }

        private void SetOutputCodecPresets()
        {
            dOutputCodec.ClearOptions();

            List<Dropdown.OptionData> optionData = new List<Dropdown.OptionData>();

            foreach (Codec codec in (Codec[])System.Enum.GetValues(typeof(Codec)))
                optionData.Add(new Dropdown.OptionData(GetCodecName(codec)));

            dOutputCodec.AddOptions(optionData);
        }

        private void SetCameraValues()
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
        }

        private void SetFooterValues(CameraSettings cameraSettings)
        {
            SetFooterResolutionWidth(cameraSettings.resolutionWidth);
            SetFooterResolutionHeight(cameraSettings.resolutionHeight);
            SetFooterFramerate(cameraSettings.framerate);
            SetFooterOutputContainer(Container.MP4);
            SetFooterOutputCodec(Codec.H246);
        }

        private void SetOutputPath(string path)
        {
            iOutputPath.SetTextWithoutNotify(path);
        }

        // setter fotter methods

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

        private void SetFooterOutputContainer(Container container)
        {
            switch (container)
            {
                case Container.MP4:
                    tOutputContainer.text = "MP4";
                    break;
                default:
                    break;
            }
        }

        private void SetFooterOutputCodec(Codec codec)
        {
            tOutputCodec.text = GetCodecName(codec);
        }

        public void SetFooterFrameTime(long frameTime)
        {
            if (frameTime < 1)
                frameTime = 1;

            tFrameTime.text = frameTime.ToString();

            double currFps = 0;

            if (frameTime > 0)
                currFps = (double)(1 / (frameTime * 1000));

            double currFramerate = (double)appController.GetVirtualCameraController().GetCameraSettings().framerate;

            if (currFps < (double)(currFramerate * 0.9f))
                tFrameTime.color = new Color(0, 184, 148);
            else if (currFps < currFramerate)
                tFrameTime.color = new Color(225, 112, 85);
            else
                tFrameTime.color = new Color(214, 48, 49);
        }

        public void SetFooterAllocatedMemory(int memory)
        {
            tAllocatedMemory.text = memory.ToString();
        }

        // setter screens methods

        public void SetScreenVirtualCamera(RenderTexture colorTexture)
        {
            screenVirtualCamera.texture = colorTexture;
        }

        public void SetScreenForegroundMask(RenderTexture foregroundMaskTexture)
        {
            screenForegroundMask.texture = foregroundMaskTexture;
        }

        public void SetScreenPhysicalCamera(RenderTexture physicalCameraTexture)
        {
            screenPhysicalCamera.texture = physicalCameraTexture;
        }

        public void SetScreenPhysicalCamera(WebCamTexture rawPhysicalCameraTexture)
        {
            screenPhysicalCamera.texture = rawPhysicalCameraTexture;
        }

        public void SetOptionalScreen(RenderTexture renderTexture)
        {
            screenOptional.texture = renderTexture;
        }

        public void SetOptionalScreen(WebCamTexture webCamTexture)
        {
            screenOptional.texture = webCamTexture;
        }

        public void SetOptionalScreen(Texture2D texture2D)
        {
            screenOptional.texture = texture2D;
        }

        // button util methods

        private void EnableButtonApplyA(bool toggle)
        {
            if (toggle == true)
            {
                bApplyA.targetGraphic.color = new Color(0, 184, 148);
                bApplyA.interactable = true;
            }
            else
            {
                bApplyA.targetGraphic.color = new Color(138, 184, 175);
                bApplyA.interactable = false;
            }
        }

        private void EnableButtonResetA(bool toggle)
        {
            if (toggle == true)
            {
                bResetA.targetGraphic.color = new Color(214, 48, 49);
                bResetA.interactable = true;
            }
            else
            {
                bResetA.targetGraphic.color = new Color(214, 161, 161);
                bResetA.interactable = false;
            }
        }

        private string GetCodecName(Codec codec)
        {
            switch (codec)
            {
                case Codec.H246:
                    return "H.246";
                default:
                    return "None";
            }
        }
    }
}