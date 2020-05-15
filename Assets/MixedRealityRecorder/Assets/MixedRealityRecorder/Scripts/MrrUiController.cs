using UnityEngine;
using UnityEngine.UI;

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