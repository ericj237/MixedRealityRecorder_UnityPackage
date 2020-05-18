using UnityEngine;

namespace MRR.Model
{
    [System.Serializable]
    public struct CameraSettings
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
        public CameraSettings cameraSettings;
    }

    public enum Container
    {
        MP4
    }

    public enum Codec
    {
        H246
    }
}