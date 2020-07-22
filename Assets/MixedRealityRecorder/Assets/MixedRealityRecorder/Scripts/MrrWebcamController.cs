using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MRR.Controller
{
    public class MrrWebcamController : MonoBehaviour
    {

        private WebCamTexture webCamTexture;

        private void StartAudio()
        {
            AudioSource audioSource = this.gameObject.GetComponent<AudioSource>();
            audioSource.clip = Microphone.Start("Microphone (Realtek(R) Audio)", true, 10, 44100);
            audioSource.loop = true;
            //while (!(Microphone.GetPosition(null) > 0)) { }
            audioSource.Play();
            //foreach (var device in Microphone.devices)
            //{
            //    Debug.Log("Name: " + device);
            //}
        }

        public void StartWebcam(string deviceName)
        {
            webCamTexture = new WebCamTexture(deviceName);
            webCamTexture.Play();
            this.gameObject.GetComponent<MeshRenderer>().material.mainTexture = webCamTexture;
            StartAudio();
        }
    }
}


