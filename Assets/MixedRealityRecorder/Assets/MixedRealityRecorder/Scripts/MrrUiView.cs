using UnityEngine;
using UnityEngine.UI;
using MRR.Model;
using MRR.Controller;
using System.Collections.Generic;
using MRR.Utility;

namespace MRR.View
{

    public class MrrUiView : MonoBehaviour
    {
        public MrrAppController appController;

        public Canvas canvasMain;

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
        public RawImage imgRecord;
        public RawImage imgStop;

        [Header("Footer")]
        public Text[] tCameraResolution = new Text[2];
        public Text tCameraFramerate;
        public Text tOutputFormat;
        public Text tFrameTime;
        public Text tMaxFrameTime;

        [Header("Screens")]
        public RawImage screenVirtualCamera;
        public RawImage screenForegroundMask;
        public RawImage screenPhysicalCamera;
        public RawImage screenOptional;

        [Header("Manual Screencapture")]
        public Canvas canvasScreencapture;
        public RawImage screenVirtualCameraScreencapture;
        public RawImage screenForegroundMaskScreencapture;
        public RawImage screenPhysicalCameraScreencapture;
        public RawImage screenOptionalScreencapture;

        public void Init()
        {
            EnableButtonsA(false);

            SetPhysicalCameraInputSources();
            SetTargetObjects();
            SetCameraPresets();

            SetCameraValues(appController.GetCameraSettingByPresetName(dCameraPresetDevice.captionText.text));

            SetOptionalScreenInputSources();

            SetOutputPath(Application.persistentDataPath);
            SetOutputFormatPresets();

            UpdateSensorOffsetPosition();
            UpdateSensorOffsetRotation();

            ApplySettings();

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

            iCameraResolution[0].onEndEdit.AddListener(delegate
            {                
                OnCameraResolutionWidthChanged(iCameraResolution[0]);
            });

            iCameraResolution[1].onEndEdit.AddListener(delegate
            {
                OnCameraResolutionHeightChanged(iCameraResolution[1]);
            });

            iCameraFramerate.onEndEdit.AddListener(delegate
            {
                OnCameraFramerateChanged(iCameraFramerate);
            });

            iCameraFocalLenth.onEndEdit.AddListener(delegate
            {
                OnCameraFocalLengthChanged(iCameraFocalLenth);
            });

            iSensorSize[0].onEndEdit.AddListener(delegate
            {
                OnSensorSizeWidthChanged(iSensorSize[0]);
            });

            iSensorSize[1].onEndEdit.AddListener(delegate
            {
                OnSensorSizeHeightChanged(iSensorSize[1]);
            });

            dOptionalScreenSource.onValueChanged.AddListener(delegate
            {
                OnOptionalScreenChanged(dOptionalScreenSource.captionText.text);
            });

            iOutputPath.onEndEdit.AddListener(delegate
            {
                OnOutputPathChanged(iOutputPath.text);
            });

            dOutputCodec.onValueChanged.AddListener(delegate
            {
                OnOutputFormatChanged(dOutputCodec.captionText.text);
            });

            bApplyA.onClick.AddListener(delegate
            {
                OnButtonApplyAClicked(bApplyA);
            });

            bResetA.onClick.AddListener(delegate
            {
                OnButtonResetAClicked(bResetA);
            });

            bRecord.onClick.AddListener(delegate
            {
                OnButtonRecordClicked();
            });

            // events offset position
            iSensorOffsetPosition[0].onEndEdit.AddListener(delegate
            {
                OnSensorOffsetPositionXChanged(Utility.Util.ReturnValidFloatFromString(iSensorOffsetPosition[0].text));
            });

            iSensorOffsetPosition[1].onEndEdit.AddListener(delegate
            {
                OnSensorOffsetPositionYChanged(Utility.Util.ReturnValidFloatFromString(iSensorOffsetPosition[1].text));
            });

            iSensorOffsetPosition[2].onEndEdit.AddListener(delegate
            {
                OnSensorOffsetPositionZChanged(Utility.Util.ReturnValidFloatFromString(iSensorOffsetPosition[2].text));
            });

            // events offset rotation
            iSensorOffsetRotation[0].onEndEdit.AddListener(delegate
            {
                OnSensorOffsetRotationXChanged(Utility.Util.ReturnValidFloatFromString(iSensorOffsetRotation[0].text));
            });

            iSensorOffsetRotation[1].onEndEdit.AddListener(delegate
            {
                OnSensorOffsetRotationYChanged(Utility.Util.ReturnValidFloatFromString(iSensorOffsetRotation[1].text));
            });

            iSensorOffsetRotation[2].onEndEdit.AddListener(delegate
            {
                OnSensorOffsetRotationZChanged(Utility.Util.ReturnValidFloatFromString(iSensorOffsetRotation[2].text));
            });
        }

        // event callback settings

        private void OnPhysicalCameraChanged(string sourceName)
        {
            if(sourceName != appController.GetSettings().physicalCameraSource)
            {
                //Debug.Log("Changed physical camera source!");
                EnableButtonsA(true);
            }
        }

        private void OnTargetObjectChanged(string targetName)
        {
            if (targetName != appController.GetSettings().targetObject)
            {
                //Debug.Log("Changed target object!");
                EnableButtonsA(true);
            }
        }

        private void OnCameraPresetChanged(string presetName)
        {
            //Debug.Log("Changed camera preset!");

            SetCameraValues(appController.GetCameraSettingByPresetName(presetName));

            EnableButtonsA(true);
        }

        private void OnCameraResolutionWidthChanged(InputField iResolutionWidth)
        {
            int width = Utility.Util.ReturnValidIntFromString(iResolutionWidth.text);

            if (width == 0)
                SetCameraResolutionWidth(appController.GetVirtualCameraController().GetCameraSettings().resolutionWidth);
            else
            {
                //Debug.Log("Changed camera resolution width!");
                SetCustomCameraPreset();
                EnableButtonsA(true);
            }
        }

        private void OnCameraResolutionHeightChanged(InputField iResolutionHeight)
        {
            int height = Utility.Util.ReturnValidIntFromString(iResolutionHeight.text);

            if (height == 0)
                SetCameraResolutionHeight(appController.GetVirtualCameraController().GetCameraSettings().resolutionHeight);
            else
            {
                //Debug.Log("Changed camera resolution height!");
                SetCustomCameraPreset();
                EnableButtonsA(true);
            }

        }

        private void OnCameraFramerateChanged(InputField iFramerate)
        {
            int framerate = Utility.Util.ReturnValidIntFromString(iFramerate.text);

            if (framerate == 0)
                SetCameraFramerate(appController.GetVirtualCameraController().GetCameraSettings().framerate);
            else
            {
                //Debug.Log("Changed camera framerate!");
                SetCustomCameraPreset();
                EnableButtonsA(true);
            }
        }

        private void OnCameraFocalLengthChanged(InputField iFocalLength)
        {
            int focalLength = Utility.Util.ReturnValidIntFromString(iFocalLength.text);

            if (focalLength == 0)
                SetCameraFocalLength(appController.GetVirtualCameraController().GetCameraSettings().focalLenth);
            else
            {
                //Debug.Log("Changed camera focal length!");
                SetCustomCameraPreset();
                EnableButtonsA(true);
            }
        }

        private void OnSensorSizeWidthChanged(InputField iSensorWidth)
        {
            int sensorWidth = Utility.Util.ReturnValidIntFromString(iSensorWidth.text);

            if (sensorWidth == 0)
                SetSensorWidth(appController.GetVirtualCameraController().GetCameraSettings().sensorWidth);
            else
            {
                //Debug.Log("Changed sensor size width!");
                SetCustomCameraPreset();
                EnableButtonsA(true);
            }
        }

        private void OnSensorSizeHeightChanged(InputField iSensorHeight)
        {
            int sensorHeight = Utility.Util.ReturnValidIntFromString(iSensorHeight.text);

            if (sensorHeight == 0)
                SetSensorHeight(appController.GetVirtualCameraController().GetCameraSettings().sensorHeight);
            else
            {
                //Debug.Log("Changed sensor size heigth!");
                SetCustomCameraPreset();
                EnableButtonsA(true);
            }
        }

        private void OnOptionalScreenChanged(string sourceName)
        {
            if (sourceName != appController.GetSettings().optionalScreenSource)
            {
                //Debug.Log("Changed optional screen source!");
                EnableButtonsA(true);
            }
        }

        private void OnOutputPathChanged(string path)
        {
            if (path != appController.GetSettings().outputPath && System.IO.Directory.Exists(path))
            {
                //Debug.Log("Changed output path!");
                EnableButtonsA(true);
            }
            else
            {
                SetOutputPath(appController.GetSettings().outputPath);
            }
        } 

        private void OnOutputFormatChanged(string outputFormatName)
        {
            if (outputFormatName != appController.GetSettings().outputFormat)
            {
                //Debug.Log("Changed output codec!");
                EnableButtonsA(true);
            }
        } 

        // event callback sensor offset - REALTIME

        private void OnSensorOffsetPositionXChanged(float x)
        {
            appController.GetVirtualCameraController().SetSensorOffsetPosition(x, Vector3Component.x);
            UpdateSensorOffsetPosition();
        }

        private void OnSensorOffsetPositionYChanged(float y)
        {
            appController.GetVirtualCameraController().SetSensorOffsetPosition(y, Vector3Component.y);
            UpdateSensorOffsetPosition();
        }

        private void OnSensorOffsetPositionZChanged(float z)
        {
            appController.GetVirtualCameraController().SetSensorOffsetPosition(z, Vector3Component.z);
            UpdateSensorOffsetPosition();
        }                    

        private void OnSensorOffsetRotationXChanged(float x)
        {
            appController.GetVirtualCameraController().SetSensorOffsetRotation(x, Vector3Component.x);
            UpdateSensorOffsetRotation();
        }

        private void OnSensorOffsetRotationYChanged(float y)
        {
            appController.GetVirtualCameraController().SetSensorOffsetRotation(y, Vector3Component.y);
            UpdateSensorOffsetRotation();
        }

        private void OnSensorOffsetRotationZChanged(float z)
        {
            appController.GetVirtualCameraController().SetSensorOffsetRotation(z, Vector3Component.z);
            UpdateSensorOffsetRotation();
        }

        // event callback buttons

        private void OnButtonApplyAClicked(Button bApplyA)
        {
            //Debug.Log("Clicked button apply A!");
            EnableButtonsA(false);

            ApplySettings();
        }

        private void ApplySettings()
        {
            Settings settings = new Settings();
            settings.cameraSettings = new CameraSetting();

            settings.physicalCameraSource = GetSelectedPhysicalCamera();
            settings.targetObject = GetSelectedTargetObject();
            settings.cameraPreset = GetSelectedCameraPreset();
            settings.cameraSettings.resolutionWidth = GetSelectedCameraResolutionWidth();
            settings.cameraSettings.resolutionHeight = GetSelectedCameraResolutionHeight();
            settings.cameraSettings.framerate = GetSelectedCameraFramerate();
            settings.cameraSettings.focalLenth = GetSelectedCameraFocalLength();
            settings.cameraSettings.sensorWidth = GetSelectedSensorSizeWidth();
            settings.cameraSettings.sensorHeight = GetSelectedSensorSizeHeight();
            settings.outputPath = GetSelectedOutputPath();
            settings.outputFormat = GetSelectedOuputCodec();

            appController.ApplySettings(settings);
            SetFooterValues(settings);
        }

        private void OnButtonResetAClicked(Button bResetA)
        {
            //Debug.Log("Clicked button reset A!");
            EnableButtonsA(false);

            ResetSettings();
        }

        private void ResetSettings()
        {
            SetPhysicalCameraInputSources();
            SetTargetObjects();
            SetCameraPresets();

            SetCameraValues(appController.GetCameraSettingByPresetName(dCameraPresetDevice.captionText.text));

            SetOptionalScreenInputSources();

            SetOutputPath(Application.persistentDataPath);
            SetOutputFormatPresets();

            SetFooterValues(appController.GetSettings());
        }

        private void OnButtonRecordClicked()
        {
            if (HasSettingsChanged())
            {
                EnableButtonsA(false);
                ResetSettings();
            }

            if (Utility.Util.GetOutputFormat(appController.GetSettings().outputFormat) == OutputFormat.ManualScreencapture)
                canvasScreencapture.enabled = true;
            else
            {
                imgRecord.enabled = !imgRecord.enabled;
                imgStop.enabled = !imgStop.enabled;

                appController.ToggleRecord();                
            }
        }

        private bool HasSettingsChanged()
        {
            return bResetA.enabled;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F10) && canvasScreencapture.enabled)
                canvasScreencapture.enabled = false;
        }

        // update methods

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

        private void SetCameraResolutionWidth(int resolutionWidth)
        {
            iCameraResolution[0].SetTextWithoutNotify(resolutionWidth.ToString());
        }

        private void SetCameraResolutionHeight(int resolutionHeight)
        {
            iCameraResolution[1].SetTextWithoutNotify(resolutionHeight.ToString());
        }

        private void SetCameraFramerate(int framerate)
        {
            iCameraFramerate.SetTextWithoutNotify(framerate.ToString());
        }

        private void SetCameraFocalLength(int focalLenth)
        {
            iCameraFocalLenth.SetTextWithoutNotify(focalLenth.ToString());
        }

        private void SetSensorHeight(int sensorHeight)
        {
            iSensorSize[1].SetTextWithoutNotify(sensorHeight.ToString());
        }

        private void SetSensorWidth(int sensorWidth)
        {
            iSensorSize[0].SetTextWithoutNotify(sensorWidth.ToString());
        }

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

        private void SetCustomCameraPreset()
        {
            dCameraPresetDevice.ClearOptions();

            List<CameraPreset> cameraPresets = appController.GetCameraPresets();

            List<Dropdown.OptionData> optionData = new List<Dropdown.OptionData>();

            optionData.Add(new Dropdown.OptionData("Custom"));

            foreach (CameraPreset device in cameraPresets)
                optionData.Add(new Dropdown.OptionData(device.presetName));

            dCameraPresetDevice.AddOptions(optionData);
        }

        private void SetOptionalScreenInputSources()
        {
            dOptionalScreenSource.ClearOptions();

            WebCamDevice[] webCamDevices = appController.GetWebCamDevices();

            List<Dropdown.OptionData> optionData = new List<Dropdown.OptionData>();

            optionData.Add(new Dropdown.OptionData("Depth Virtual Camera"));

            foreach (WebCamDevice source in webCamDevices)
            {
                if (source.name != dPhysicalCameraSource.captionText.text)
                    optionData.Add(new Dropdown.OptionData(source.name));
            }

            dOptionalScreenSource.AddOptions(optionData);
        }

        private void SetOutputFormatPresets()
        {
            dOutputCodec.ClearOptions();

            List<Dropdown.OptionData> optionData = new List<Dropdown.OptionData>();

            foreach (OutputFormat outputFormat in (OutputFormat[])System.Enum.GetValues(typeof(OutputFormat)))
                optionData.Add(new Dropdown.OptionData(Utility.Util.GetOutputFormatName(outputFormat)));

            dOutputCodec.AddOptions(optionData);
        }

        private void SetCameraValues(CameraSetting cameraSetting)
        {
            SetCameraResolutionWidth(cameraSetting.resolutionWidth);
            SetCameraResolutionHeight(cameraSetting.resolutionHeight);
            SetCameraFramerate(cameraSetting.framerate);
            SetCameraFocalLength(cameraSetting.focalLenth);
            SetSensorHeight(cameraSetting.sensorHeight);
            SetSensorWidth(cameraSetting.sensorWidth);
        }

        private void SetFooterValues(Settings settings)
        {
            SetFooterResolutionWidth(settings.cameraSettings.resolutionWidth);
            SetFooterResolutionHeight(settings.cameraSettings.resolutionHeight);
            SetFooterFramerate(settings.cameraSettings.framerate);
            SetFooterOutputFormat(Utility.Util.GetOutputFormat(settings.outputFormat));
            SetFooterMaxFrameTime(1000 / settings.cameraSettings.framerate);
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

        private void SetFooterOutputFormat(OutputFormat format)
        {
            switch (format)
            {
                case OutputFormat.ManualScreencapture:
                    tOutputFormat.text = "Manual Screencapture";
                    break;
                case OutputFormat.TgaImageSequence:
                    tOutputFormat.text = "TGA Image Sequence";
                    break;
                case OutputFormat.BmpImageSequence:
                    tOutputFormat.text = "BMP Image Sequence";
                    break;
                default:
                    break;
            }
        }

        private void SetFooterMaxFrameTime(int maxFrameTime)
        {
            tMaxFrameTime.text = maxFrameTime.ToString();
        }

        public void SetFooterFrameTime(long frameTime)
        {
            if (frameTime < 1)
                frameTime = 1;

            tFrameTime.text = frameTime.ToString();

            int maxFrameTime = 1000 / appController.GetSettings().cameraSettings.framerate;

            if (frameTime < maxFrameTime * 0.9f)
                tFrameTime.color = new Color(0, 184, 148);
            else if (frameTime < maxFrameTime)
                tFrameTime.color = Color.yellow;
            else
                tFrameTime.color = Color.red;
        }

        // setter screens methods

        public void SetScreenVirtualCamera(RenderTexture colorTexture)
        {
            screenVirtualCamera.texture = colorTexture;
            screenVirtualCameraScreencapture.texture = colorTexture;
        }

        public void SetScreenForegroundMask(RenderTexture foregroundMaskTexture)
        {
            screenForegroundMask.texture = foregroundMaskTexture;
            screenForegroundMaskScreencapture.texture = foregroundMaskTexture;
        }

        public void SetScreenPhysicalCamera(RenderTexture physicalCameraTexture)
        {
            screenPhysicalCamera.texture = physicalCameraTexture;
            screenPhysicalCameraScreencapture.texture = physicalCameraTexture;
        }

        public void SetScreenPhysicalCamera(WebCamTexture rawPhysicalCameraTexture)
        {
            screenPhysicalCamera.texture = rawPhysicalCameraTexture;
            screenPhysicalCameraScreencapture.texture = rawPhysicalCameraTexture;
        }

        public void SetOptionalScreen(RenderTexture renderTexture)
        {
            screenOptional.texture = renderTexture;
            screenOptionalScreencapture.texture = renderTexture;
        }

        public void SetOptionalScreen(WebCamTexture webCamTexture)
        {
            screenOptional.texture = webCamTexture;
            screenOptionalScreencapture.texture = webCamTexture;
        }

        public void SetOptionalScreen(Texture2D texture2D)
        {
            screenOptional.texture = texture2D;
            screenOptionalScreencapture.texture = texture2D;
        }

        // getter methods

        public string GetSelectedPhysicalCamera()
        {
            return dPhysicalCameraSource.captionText.text;
        }

        private string GetSelectedTargetObject()
        {
            return dTargetObject.captionText.text;
        }

        private string GetSelectedCameraPreset()
        {
            return dCameraPresetDevice.captionText.text;
        }

        private int GetSelectedCameraResolutionWidth()
        {
            return int.Parse(iCameraResolution[0].text);
        }

        private int GetSelectedCameraResolutionHeight()
        {
            return int.Parse(iCameraResolution[1].text);
        }

        private int GetSelectedCameraFramerate()
        {
            return int.Parse(iCameraFramerate.text);
        }

        private int GetSelectedCameraFocalLength()
        {
            return int.Parse(iCameraFocalLenth.text);
        }

        private int GetSelectedSensorSizeWidth()
        {
            return int.Parse(iSensorSize[0].text);
        }

        private int GetSelectedSensorSizeHeight()
        {
            return int.Parse(iSensorSize[1].text);
        }

        private string GetSelectedOptionalScreen()
        {
            return dOptionalScreenSource.captionText.text;
        }

        private string GetSelectedOutputPath()
        {
            return iOutputPath.text;
        }

        private string GetSelectedOuputCodec()
        {
            return dOutputCodec.captionText.text;
        }

        // button util methods

        private void EnableButtonApplyA(bool toggle)
        {
            if (toggle == true)
                bApplyA.interactable = true;
            else
                bApplyA.interactable = false;
        }

        private void EnableButtonResetA(bool toggle)
        {
            if (toggle == true)
                bResetA.interactable = true;
            else
                bResetA.interactable = false;
        }

        private void EnableButtonsA(bool toggle)
        {
            EnableButtonApplyA(toggle);
            EnableButtonResetA(toggle);
        }

    }
}