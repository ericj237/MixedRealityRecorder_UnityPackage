using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

namespace MRR.Controller
{

    public class MrrOperatorController : MonoBehaviour
    {
        // a reference to the action
        public SteamVR_Action_Boolean a_trigger;
        public SteamVR_Action_Boolean a_toggleRecording;
        public SteamVR_Action_Boolean a_pointerMode;
        public SteamVR_Action_Boolean a_lightMode;
        public SteamVR_Action_Boolean a_webcamMode;
        // a reference to the hand
        private SteamVR_Input_Sources handType = SteamVR_Input_Sources.LeftHand;
        //reference to the sphere
        public GameObject HighlightPrefab;
        public Canvas canvasScreencapture;
        public Text debugText;
        //public GameObject webcamScreen;

        private bool isTriggerDown = false;

        private Mode mode = Mode.pointer;

        private enum Mode
        {
            pointer,
            light,
            webcam
        };

        void Start()
        {
            debugText.text = "Pointer Mode";

            a_pointerMode.AddOnStateDownListener(SelectPointerMode, handType);
            a_lightMode.AddOnStateDownListener(SelectLightMode, handType);
            a_webcamMode.AddOnStateDownListener(SelectWebcamMode, handType);

            a_trigger.AddOnStateDownListener(TriggerDown, handType);
            a_trigger.AddOnStateUpListener(TriggerUp, handType);

            a_toggleRecording.AddOnStateDownListener(OperatorStartRecordingButtonDown, handType);
        }

        public void SelectPointerMode(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            mode = Mode.pointer;
            debugText.text = "Pointer Mode";
        }

        public void SelectLightMode(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            mode = Mode.light;
            debugText.text = "Light Mode";
        }

        public void SelectWebcamMode(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            mode = Mode.webcam;
            debugText.text = "Webcam Mode";
        }

        public void TriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            Debug.Log("Operator Trigger is up!");

            switch(mode)
            {
                case Mode.pointer:
                    {
                        isTriggerDown = false;
                        HighlightPrefab.GetComponent<MeshRenderer>().enabled = false;
                        break;
                    }
                case Mode.light:
                    break;
                case Mode.webcam:
                    break;
                default:
                    break;
            }
        }

        public void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            Debug.Log("Operator Trigger is down!");

            switch (mode)
            {
                case Mode.pointer:
                    {
                        isTriggerDown = true;
                        break;
                    }
                case Mode.light:
                    break;
                case Mode.webcam:
                    break;
                default:
                    break;
            }
        }

        public void OperatorStartRecordingButtonDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            Debug.Log("Operator Button is down!");
            canvasScreencapture.GetComponent<Canvas>().enabled = !canvasScreencapture.GetComponent<Canvas>().enabled;
        }

        private void Update()
        {
            if(isTriggerDown)
            {
                RaycastHit hit;

                if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
                {
                    if(!HighlightPrefab.GetComponent<MeshRenderer>().enabled)
                        HighlightPrefab.GetComponent<MeshRenderer>().enabled = true;

                    HighlightPrefab.transform.position = hit.point + hit.normal * 0.1f;
                }
                else
                {
                    HighlightPrefab.GetComponent<MeshRenderer>().enabled = false;
                }
            }
        }
    }
}
