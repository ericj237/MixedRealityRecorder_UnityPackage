using MRR.Model;
using System;
using System.IO;
using UnityEngine;

namespace MRR.Video
{
    public class MrrVideoRecorder : MonoBehaviour
    {

        private bool isRecording = false;
        private string outPath;
        private Texture2D outColorFrame;
        private Texture2D outMaskFrame;
        private Vector2Int outResolution;
        private OutputFormat outFormat;
        private long frameCount;

        public void StartRecording(string outPath, OutputFormat outFormat, Vector2Int outResolution)
        {
            this.outFormat = outFormat;            

            this.outPath = outPath;

            this.outResolution = outResolution;

            frameCount = 0;

            outColorFrame = new Texture2D(outResolution.x, outResolution.y, TextureFormat.RGB24, false);
            outMaskFrame = new Texture2D(outResolution.x, outResolution.y, TextureFormat.RGB24, false);

            this.outPath += "/recording_" + System.DateTime.Now.ToString("yyyy.MM.dd_hh.mm.ss");
            if (!System.IO.Directory.Exists(this.outPath))
                System.IO.Directory.CreateDirectory(this.outPath);

            isRecording = true;

            //Debug.Log("Started Recording!");
        }

        public void StopRecording()
        {
            isRecording = false;
            //Debug.Log("Stopped Recording!");
        }

        public void RecordFrame(RenderTexture colorFrame, RenderTexture maskFrame)
        {
            if (isRecording)
            {
                RenderTexture.active = colorFrame;

                outColorFrame.ReadPixels(new Rect(0, 0, outResolution.x, outResolution.y), 0, 0);
                outColorFrame.Apply();

                RenderTexture.active = maskFrame;

                outMaskFrame.ReadPixels(new Rect(0, 0, outResolution.x, outResolution.y), 0, 0);
                outMaskFrame.Apply();

                RenderTexture.active = null;

                SaveFrame(outColorFrame, outMaskFrame);
            }
        }

        public void RecordFrame(Texture2D colorFrame, Texture2D maskFrame)
        {
            if(isRecording)
            {
                SaveFrame(colorFrame, maskFrame);
            }
        }

        private void SaveFrame(Texture2D colorFrame, Texture2D maskFrame)
        {
            switch(outFormat)
            {
                case OutputFormat.BmpImageSequence:
                    SaveFrameAsBmp(colorFrame, maskFrame);
                    break;
                case OutputFormat.TgaImageSequence:
                    SaveFrameAsTga(colorFrame, maskFrame);
                    break;
                default:
                    break;
            }
        }

        private void SaveFrameAsTga(Texture2D colorFrame, Texture2D maskFrame)
        {
            frameCount++;
            byte[] bytesColorFrame = ImageConversion.EncodeToTGA(colorFrame);
            byte[] bytesMaskFrame = ImageConversion.EncodeToTGA(maskFrame);

            new System.Threading.Thread(() =>
            {
                File.WriteAllBytes(outPath + "/colorFrame_" + frameCount + ".tga", bytesColorFrame);
            }).Start();
            new System.Threading.Thread(() =>
            {
                File.WriteAllBytes(outPath + "/maskFrame_" + frameCount + ".tga", bytesMaskFrame);
            }).Start();
        }

        private void SaveFrameAsBmp(Texture2D colorFrame, Texture2D maskFrame)
        {
            frameCount++;
            byte[] bytesColorFrame = colorFrame.GetRawTextureData();
            byte[] bytesMaskFrame = maskFrame.GetRawTextureData();

            new System.Threading.Thread(() =>
            {
                using (FileStream fileStream = new FileStream(outPath + "/colorFrame_" + frameCount + ".bmp", FileMode.Create))
                {
                    using (BinaryWriter bw = new BinaryWriter(fileStream))
                    {

                        // define the bitmap file header
                        bw.Write((UInt16)0x4D42);                                                   // bfType;
                        bw.Write((UInt32)(14 + 40 + (outResolution.x * outResolution.y * 4)));      // bfSize;
                        bw.Write((UInt16)0);                                                        // bfReserved1;
                        bw.Write((UInt16)0);                                                        // bfReserved2;
                        bw.Write((UInt32)14 + 40);                                                  // bfOffBits;

                        // define the bitmap information header
                        bw.Write((UInt32)40);                                                       // biSize;
                        bw.Write((Int32)outResolution.x);                                           // biWidth;
                        bw.Write((Int32)outResolution.y);                                           // biHeight;
                        bw.Write((UInt16)1);                                                        // biPlanes;
                        bw.Write((UInt16)32);                                                       // biBitCount;
                        bw.Write((UInt32)0);                                                        // biCompression;
                        bw.Write((UInt32)(outResolution.x * outResolution.y * 4));                  // biSizeImage;
                        bw.Write((Int32)0);                                                         // biXPelsPerMeter;
                        bw.Write((Int32)0);                                                         // biYPelsPerMeter;
                        bw.Write((UInt32)0);                                                        // biClrUsed;
                        bw.Write((UInt32)0);                                                        // biClrImportant;

                        // switch the image data from RGB to BGR
                        for (int imageIdx = 0; imageIdx < bytesColorFrame.Length; imageIdx += 3)
                        {
                            bw.Write(bytesColorFrame[imageIdx + 2]);
                            bw.Write(bytesColorFrame[imageIdx + 1]);
                            bw.Write(bytesColorFrame[imageIdx + 0]);
                            bw.Write((byte)255);
                        }

                        bw.Close();
                    }
                    fileStream.Close();
                }
            }).Start();
            new System.Threading.Thread(() =>
            {
                using (FileStream fileStream = new FileStream(outPath + "/maskFrame_" + frameCount + ".bmp", FileMode.Create))
                {
                    using (BinaryWriter bw = new BinaryWriter(fileStream))
                    {

                        // define the bitmap file header
                        bw.Write((UInt16)0x4D42);                                                   // bfType;
                        bw.Write((UInt32)(14 + 40 + (outResolution.x * outResolution.y * 4)));      // bfSize;
                        bw.Write((UInt16)0);                                                        // bfReserved1;
                        bw.Write((UInt16)0);                                                        // bfReserved2;
                        bw.Write((UInt32)14 + 40);                                                  // bfOffBits;

                        // define the bitmap information header
                        bw.Write((UInt32)40);                                                       // biSize;
                        bw.Write((Int32)outResolution.x);                                           // biWidth;
                        bw.Write((Int32)outResolution.y);                                           // biHeight;
                        bw.Write((UInt16)1);                                                        // biPlanes;
                        bw.Write((UInt16)32);                                                       // biBitCount;
                        bw.Write((UInt32)0);                                                        // biCompression;
                        bw.Write((UInt32)(outResolution.x * outResolution.y * 4));                  // biSizeImage;
                        bw.Write((Int32)0);                                                         // biXPelsPerMeter;
                        bw.Write((Int32)0);                                                         // biYPelsPerMeter;
                        bw.Write((UInt32)0);                                                        // biClrUsed;
                        bw.Write((UInt32)0);                                                        // biClrImportant;

                        // switch the image data from RGB to BGR
                        for (int imageIdx = 0; imageIdx < bytesMaskFrame.Length; imageIdx += 3)
                        {
                            bw.Write(bytesMaskFrame[imageIdx + 2]);
                            bw.Write(bytesMaskFrame[imageIdx + 1]);
                            bw.Write(bytesMaskFrame[imageIdx + 0]);
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