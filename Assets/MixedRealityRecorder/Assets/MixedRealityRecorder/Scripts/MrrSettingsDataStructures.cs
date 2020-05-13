namespace MRR.Settings
{

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
}