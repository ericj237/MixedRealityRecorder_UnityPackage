using UnityEngine;
using UnityEngine.UI;
using MRR.DataStructures;

namespace MRR.Controller
{

    public class MrrUiController : MonoBehaviour
    {

        public MrrAppManager appManager;

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
        public InputField iSensorDepth;
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
        private InputField.SubmitEvent eventVirtualCameraOffsetPositionX = new InputField.SubmitEvent();
        private InputField.SubmitEvent eventVirtualCameraOffsetPositionY = new InputField.SubmitEvent();
        private InputField.SubmitEvent eventVirtualCameraOffsetPositionZ = new InputField.SubmitEvent();

        private InputField.SubmitEvent eventVirtualCameraOffsetRotationX = new InputField.SubmitEvent();
        private InputField.SubmitEvent eventVirtualCameraOffsetRotationY = new InputField.SubmitEvent();
        private InputField.SubmitEvent eventVirtualCameraOffsetRotationZ = new InputField.SubmitEvent();

        private void Start()
        {
            // events offset position
            eventVirtualCameraOffsetPositionX.AddListener(SetSensorOffsetPositionX);
            iSensorOffsetPosition[0].onEndEdit = eventVirtualCameraOffsetPositionX;

            eventVirtualCameraOffsetPositionY.AddListener(SetSensorOffsetPositionY);
            iSensorOffsetPosition[1].onEndEdit = eventVirtualCameraOffsetPositionY;

            eventVirtualCameraOffsetPositionZ.AddListener(SetSensorOffsetPositionZ);
            iSensorOffsetPosition[2].onEndEdit = eventVirtualCameraOffsetPositionZ;

            // events offset rotation
            eventVirtualCameraOffsetRotationX.AddListener(SetSensorOffsetRotationX);
            iSensorOffsetRotation[0].onEndEdit = eventVirtualCameraOffsetRotationX;

            eventVirtualCameraOffsetRotationY.AddListener(SetSensorOffsetRotationY);
            iSensorOffsetRotation[1].onEndEdit = eventVirtualCameraOffsetRotationY;

            eventVirtualCameraOffsetRotationZ.AddListener(SetSensorOffsetRotationZ);
            iSensorOffsetRotation[2].onEndEdit = eventVirtualCameraOffsetRotationZ;
        }

        private void SetSensorOffsetPositionX(string x)
        {
            appManager.SetSensorOffsetPosition(float.Parse(x), Vector3Component.x);
        }

        private void SetSensorOffsetPositionY(string y)
        {
            appManager.SetSensorOffsetPosition(float.Parse(y), Vector3Component.y);
        }

        private void SetSensorOffsetPositionZ(string z)
        {
            appManager.SetSensorOffsetPosition(float.Parse(z), Vector3Component.z);
        }

        private void SetSensorOffsetRotationX(string x)
        {
            appManager.SetSensorOffsetRotation(float.Parse(x), Vector3Component.x);
        }

        private void SetSensorOffsetRotationY(string y)
        {
            appManager.SetSensorOffsetRotation(float.Parse(y), Vector3Component.y);
        }

        private void SetSensorOffsetRotationZ(string z)
        {
            appManager.SetSensorOffsetRotation(float.Parse(z), Vector3Component.z);
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