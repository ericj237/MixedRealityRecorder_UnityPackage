using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MRR.Controller
{
    public class MrrWebcamController : MonoBehaviour
    {

        private WebCamTexture webCamTexture;

        public void StartWebcam(string deviceName)
        {
            webCamTexture = new WebCamTexture(deviceName);
            webCamTexture.Play();
            this.gameObject.GetComponent<MeshRenderer>().material.mainTexture = webCamTexture;
        }
    }
}


