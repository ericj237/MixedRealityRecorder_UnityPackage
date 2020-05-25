using System.IO;
using UnityEngine;

namespace MRR.Video
{

    public class MrrVideoRecorder : MonoBehaviour
    {

        private bool isRecording = false;
        private string outPath;
        private Texture2D outFrame;
        private Vector2Int outResolution;
        private long frameCount;

        public void StartRecording(string outPath, Vector2Int outResolution)
        {
            this.outPath = outPath;
            this.outResolution = outResolution;
            frameCount = 0;
            outFrame = new Texture2D(outResolution.x, outResolution.y);
            isRecording = true;

            Debug.Log("Started Recording!");
        }

        public void StopRecording()
        {
            isRecording = false;

            Debug.Log("Stopped Recording!");
        }

        public void RecordFrame(RenderTexture frame)
        {
            if (isRecording)
            {

                RenderTexture.active = frame;

                outFrame.ReadPixels(new Rect(0, 0, outResolution.x, outResolution.y), 0, 0);
                outFrame.Apply();

                RenderTexture.active = null;

                SaveFrame(outFrame);

            }
        }

        public void RecordFrame(Texture2D frame)
        {
            if(isRecording)
            {
                SaveFrame(frame);
            }
        }

        private void SaveFrame(Texture2D frame)
        {

            frameCount++;
            byte[] bytes = ImageConversion.EncodeToTGA(frame);
            File.WriteAllBytes(outPath + "/frame_" + frameCount + ".tga", bytes);

        }

        public bool IsRecording()
        {
            return isRecording;
        }
    }
}