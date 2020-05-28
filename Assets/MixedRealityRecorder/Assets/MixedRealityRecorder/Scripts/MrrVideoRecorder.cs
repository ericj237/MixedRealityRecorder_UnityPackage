using System;
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
            outFrame = new Texture2D(outResolution.x, outResolution.y, TextureFormat.RGB24, false);
            isRecording = true;

            Debug.Log("Started Recording!");
        }

        public void StopRecording()
        {
            isRecording = false;
            outFrame = null;
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

                SaveFrameAsBmp(outFrame);

            }
        }

        public void RecordFrame(Texture2D frame)
        {
            if(isRecording)
            {
                SaveFrameAsBmp(frame);
            }
        }

        private void SaveFrameAsTga(Texture2D frame)
        {
            frameCount++;
            byte[] bytes = ImageConversion.EncodeToTGA(frame);

            new System.Threading.Thread(() =>
            {
                File.WriteAllBytes(outPath + "/frame_" + frameCount + ".tga", bytes);
            }).Start();
        }

        private void SaveFrameAsBmp(Texture2D frame)
        {
            frameCount++;
            byte[] bytes = frame.GetRawTextureData();

            new System.Threading.Thread(() =>
            {
                using (FileStream fileStream = new FileStream(outPath + "/frame_" + frameCount + ".bmp", FileMode.Create))
                {
                    using (BinaryWriter bw = new BinaryWriter(fileStream))
                    {

                        // define the bitmap file header
                        bw.Write((UInt16)0x4D42);                               // bfType;
                        bw.Write((UInt32)(14 + 40 + (outResolution.x * outResolution.y * 4)));     // bfSize;
                        bw.Write((UInt16)0);                                    // bfReserved1;
                        bw.Write((UInt16)0);                                    // bfReserved2;
                        bw.Write((UInt32)14 + 40);                              // bfOffBits;

                        // define the bitmap information header
                        bw.Write((UInt32)40);                               // biSize;
                        bw.Write((Int32)outResolution.x);                                 // biWidth;
                        bw.Write((Int32)outResolution.y);                                // biHeight;
                        bw.Write((UInt16)1);                                    // biPlanes;
                        bw.Write((UInt16)32);                                   // biBitCount;
                        bw.Write((UInt32)0);                                    // biCompression;
                        bw.Write((UInt32)(outResolution.x * outResolution.y * 4));                 // biSizeImage;
                        bw.Write((Int32)0);                                     // biXPelsPerMeter;
                        bw.Write((Int32)0);                                     // biYPelsPerMeter;
                        bw.Write((UInt32)0);                                    // biClrUsed;
                        bw.Write((UInt32)0);                                    // biClrImportant;

                        // switch the image data from RGB to BGR
                        for (int imageIdx = 0; imageIdx < bytes.Length; imageIdx += 3)
                        {
                            bw.Write(bytes[imageIdx + 2]);
                            bw.Write(bytes[imageIdx + 1]);
                            bw.Write(bytes[imageIdx + 0]);
                            bw.Write((byte)255);
                        }

                        bw.Close();
                    }
                    fileStream.Close();
                }
            }).Start();
        }

        public bool IsRecording()
        {
            return isRecording;
        }
    }
}