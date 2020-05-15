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
        private InputField.OnChangeEvent eventVirtualCameraOffsetPositionX = new InputField.OnChangeEvent();
        private InputField.OnChangeEvent eventVirtualCameraOffsetPositionY = new InputField.OnChangeEvent();
        private InputField.OnChangeEvent eventVirtualCameraOffsetPositionZ = new InputField.OnChangeEvent();

        private InputField.OnChangeEvent eventVirtualCameraOffsetRotationX = new InputField.OnChangeEvent();
        private InputField.OnChangeEvent eventVirtualCameraOffsetRotationY = new InputField.OnChangeEvent();
        private InputField.OnChangeEvent eventVirtualCameraOffsetRotationZ = new InputField.OnChangeEvent();

        private void Start()
        {
            RegisterEventsSensorOffset();
            InitInputFieldsSensorOffset();
        }

        private void InitInputFieldsSensorOffset()
        {
            Vector3 sensorOffsetPosition = appManager.GetSensorOffsetPosition();
            iSensorOffsetPosition[0].SetTextWithoutNotify(sensorOffsetPosition.x.ToString());
            iSensorOffsetPosition[1].SetTextWithoutNotify(sensorOffsetPosition.y.ToString());
            iSensorOffsetPosition[2].SetTextWithoutNotify(sensorOffsetPosition.z.ToString());

            Vector3 sensorOffsetRoation = appManager.GetSensorOffsetRotation();
            iSensorOffsetRotation[0].SetTextWithoutNotify(sensorOffsetRoation.x.ToString());
            iSensorOffsetRotation[1].SetTextWithoutNotify(sensorOffsetRoation.y.ToString());
            iSensorOffsetRotation[2].SetTextWithoutNotify(sensorOffsetRoation.z.ToString());
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