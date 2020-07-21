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
        public GameObject webcamScreen;

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
                        HighlightPrefab.GetComponent<MeshRenderer>().enabled = false;
                        break;
                    }
                case Mode.light:
                    {
                        currSelectedLight = null;
                        break;
                    }
                case Mode.webcam:
                    {
                        break;
                    }
                default:
                    break;
            }

            isTriggerDown = false;
        }

        private GameObject currSelectedLight;

        public void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            Debug.Log("Operator Trigger is down!");

            switch (mode)
            {
                case Mode.pointer:
                    {
                        break;
                    }
                case Mode.light:
                    {
                        currSelectedLight = CreateLight();
                        break;
                    }
                case Mode.webcam:
                    {
                        break;
                    }
                default:
                    break;
            }

            isTriggerDown = true;

        }

        private GameObject CreateLight()
        {
            GameObject lightObject = new GameObject();
            lightObject.name = "light";

            Light lightComponent = lightObject.AddComponent<Light>();
            lightComponent.type = LightType.Point;
            lightComponent.color = Color.white;
            lightComponent.range = 6.0f;
            lightComponent.intensity = 1.25f;

            lightObject.transform.position = transform.position;

            return lightObject;
        }

        public void OperatorStartRecordingButtonDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            Debug.Log("Operator Button is down!");
            canvasScreencapture.GetComponent<Canvas>().enabled = !canvasScreencapture.GetComponent<Canvas>().enabled;
        }

        private void Update()
        {

            if (isTriggerDown)
            {

                switch (mode)
                {
                    case Mode.pointer:
                        {
                            RaycastHit hit;

                            if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
                            {
                                if (!HighlightPrefab.GetComponent<MeshRenderer>().enabled)
                                    HighlightPrefab.GetComponent<MeshRenderer>().enabled = true;

                                HighlightPrefab.transform.position = hit.point + hit.normal * 0.1f;
                            }
                            else
                            {
                                HighlightPrefab.GetComponent<MeshRenderer>().enabled = false;
                            }

                            break;
                        }
                    case Mode.light:
                        {
                            if(currSelectedLight != null)
                                currSelectedLight.transform.position = transform.position;
                            break;
                        }
                    case Mode.webcam:
                        {
                            RaycastHit hit;

                            if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
                            {
                                float scalar = Mathf.Lerp(0.5f, 4.0f, Vector3.Distance(transform.position, webcamScreen.transform.position) / 10.0f);

                                webcamScreen.transform.position = hit.point + Vector3.up * scalar;
                                webcamScreen.transform.localScale = Vector3.one * scalar;
                            }

                            break;
                        }
                    default:
                        break;
                }            
            }
        }
    }
}
