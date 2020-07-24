using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MRR.Controller
{
    public class MrrWebcamController : MonoBehaviour
    {

        private WebCamTexture webCamTexture;

        private void Start()
        {
            webCamTexture = new WebCamTexture("Logitech Webcam C925e");
            webCamTexture.Play();
            this.gameObject.GetComponent<MeshRenderer>().material.mainTexture = webCamTexture;
        }
    }
}


