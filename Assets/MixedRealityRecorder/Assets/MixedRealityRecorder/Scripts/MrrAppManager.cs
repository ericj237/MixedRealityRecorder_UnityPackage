﻿using UnityEngine;
using MRR.Settings;

namespace MRR.Controller
{

    public class MrrAppManager : MonoBehaviour
    {

        public Vector3 vector3;

        CameraSettings cameraSettings;

        // Start is called before the first frame update
        void Start()
        {

            cameraSettings.focalLenth = 35;

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}