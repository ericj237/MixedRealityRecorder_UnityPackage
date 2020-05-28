using UnityEngine;

namespace MRR.Model
{
    [System.Serializable]
    public struct CameraSetting
    {
        // in px
        public int resolutionWidth, resolutionHeight;
        // in fps
        public int framerate;

        // in mm
        public int sensorWidth, sensorHeight;
        // in stops
        public int dynamicRange;

        // in mm
        public int focalLenth;
    }

    public enum Vector3Component
    {
        x,
        y,
        z
    }

    [CreateAssetMenu(fileName = "CameraPreset", menuName = "MixedRealityRecorder/CameraPreset", order = 1)]
    public class CameraPreset : ScriptableObject
    {
        public string presetName;
        public CameraSetting cameraSettings;
    }

    public enum OutputFormat
    {
        ManualScreencapture,
        TgaImageSequence,
        BmpImageSequence
    }

    public struct Settings
    {
        public string physicalCameraSource;
        public string targetObject;
        public string cameraPreset;
        public CameraSetting cameraSettings;
        public string optionalScreenSource;
        public string outputPath;
        public string outputCodec;
    }
}