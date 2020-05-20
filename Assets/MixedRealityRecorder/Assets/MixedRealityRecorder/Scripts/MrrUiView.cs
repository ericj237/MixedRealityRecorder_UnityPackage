﻿using UnityEngine;
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

            iCameraResolution[0].onEndEdit.AddListener(delegate
            {                
                OnCameraResolutionWidthChanged(ReturnValidIntFromString(iCameraResolution[0].text));
            });

            iCameraResolution[1].onEndEdit.AddListener(delegate
            {
                OnCameraResolutionHeightChanged(ReturnValidIntFromString(iCameraResolution[1].text));
            });

            iCameraFramerate.onEndEdit.AddListener(delegate
            {
                OnCameraFramerateChanged(ReturnValidIntFromString(iCameraFramerate.text));
            });

            iCameraFocalLenth.onEndEdit.AddListener(delegate
            {
                OnCameraFocalLengthChanged(ReturnValidIntFromString(iCameraFocalLenth.text));
            });

            iSensorSize[0].onEndEdit.AddListener(delegate
            {
                OnSensorSizeWidthChanged(ReturnValidIntFromString(iSensorSize[0].text));
            });

            iSensorSize[1].onEndEdit.AddListener(delegate
            {
                OnSensorSizeHeightChanged(ReturnValidIntFromString(iSensorSize[1].text));
            });

            iSensorDynamicRange.onEndEdit.AddListener(delegate
            {
                OnSensorDynamicRangeChanged(ReturnValidIntFromString(iSensorDynamicRange.text));
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
                OnOuputCodecChanged(dOutputCodec.captionText.text);
            });

            // events offset position
            iSensorOffsetPosition[0].onEndEdit.AddListener(delegate
            {
                OnSensorOffsetPositionXChanged(ReturnValidFloatFromString(iSensorOffsetPosition[0].text));
            });

            iSensorOffsetPosition[1].onEndEdit.AddListener(delegate
            {
                OnSensorOffsetPositionYChanged(ReturnValidFloatFromString(iSensorOffsetPosition[1].text));
            });

            iSensorOffsetPosition[2].onEndEdit.AddListener(delegate
            {
                OnSensorOffsetPositionZChanged(ReturnValidFloatFromString(iSensorOffsetPosition[2].text));
            });

            // events offset rotation
            iSensorOffsetRotation[0].onEndEdit.AddListener(delegate
            {
                OnSensorOffsetRotationXChanged(ReturnValidFloatFromString(iSensorOffsetRotation[0].text));
            });

            iSensorOffsetRotation[1].onEndEdit.AddListener(delegate
            {
                OnSensorOffsetRotationYChanged(ReturnValidFloatFromString(iSensorOffsetRotation[1].text));
            });

            iSensorOffsetRotation[2].onEndEdit.AddListener(delegate
            {
                OnSensorOffsetRotationZChanged(ReturnValidFloatFromString(iSensorOffsetRotation[2].text));
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

        private void OnCameraResolutionWidthChanged(int resolutionWidth)
        {
            Debug.Log("Changed camera resolution width!");
        }

        private void OnCameraResolutionHeightChanged(int resolutionHeight)
        {
            Debug.Log("Changed camera resolution height!");
        }

        private void OnCameraFramerateChanged(int framerate)
        {
            Debug.Log("Changed camera framerate!");
        }

        private void OnCameraFocalLengthChanged(int focalLength)
        {
            Debug.Log("Changed camera focal length!");
        }

        private void OnSensorSizeWidthChanged(int sensorWidth)
        {
            Debug.Log("Changed sensor size width!");
        }

        private void OnSensorSizeHeightChanged(int sensorHeight)
        {
            Debug.Log("Changed sensor size heigth!");
        }

        private void OnSensorDynamicRangeChanged(int sensorHeight)
        {
            Debug.Log("Changed sensor dynamic range!");
        }

        private void OnOptionalScreenChanged(string sourceName)
        {
            Debug.Log("Changed optional screen source!");
        }

        private void OnOutputPathChanged(string path)
        {
            Debug.Log("Changed output path!");
        } 

        private void OnOuputCodecChanged(string codecName)
        {
            Debug.Log("Changed output codec!");
        } 

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

            optionData.Add(new Dropdown.OptionData("Depth Virtual Camera"));

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

        // util methods

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

        private float ReturnValidFloatFromString(string input)
        {
            if (input == "" || input == "-")
                return 0.0f;
            else
                return float.Parse(input);
        }

        private int ReturnValidIntFromString(string input)
        {             
            if(input == "" || input[0] == '-')
                return 0;
            else
                return int.Parse(input);
        }
    }
}