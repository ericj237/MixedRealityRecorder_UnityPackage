using MRR.View;
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

        private int layerMaskPointer = 1 << 9;

        public MrrUiView uiView;
        public MrrAppController appController;

        // a reference to the action
        public SteamVR_Action_Boolean a_trigger;
        public SteamVR_Action_Boolean a_rotateScene;
        public SteamVR_Action_Boolean a_toggleRecording;
        public SteamVR_Action_Boolean a_cycleMode;
        public SteamVR_Action_Boolean a_dPadUp;
        public SteamVR_Action_Boolean a_dPadDown;
        public SteamVR_Action_Boolean a_dPadLeft;
        public SteamVR_Action_Boolean a_dPadRight;
        // a reference to the hand
        private SteamVR_Input_Sources handType = SteamVR_Input_Sources.LeftHand;
        //reference to the sphere
        public GameObject HighlightPrefab;
        public Canvas canvasScreencapture;
        public Text debugText;
        public GameObject webcamScreen;

        private bool isTriggerDown = false;

        private Mode mode = Mode.pointer;
        private LightModeState lightModeState = LightModeState.none;

        private enum Mode
        {
            pointer,
            light,
            webcam
        };

        private enum LightModeState
        {
            none,
            placement,
            edit
        };

        void Start()
        {          
            debugText.text = "Pointer Mode";

            a_cycleMode.AddOnStateDownListener(CycleMode, handType);

            a_trigger.AddOnStateDownListener(TriggerDown, handType);
            a_trigger.AddOnStateUpListener(TriggerUp, handType);

            a_toggleRecording.AddOnStateDownListener(OperatorStartRecordingButtonDown, handType);
            a_rotateScene.AddOnStateDownListener(OperatorRotateSceneButtonDown, handType);

            a_dPadUp.AddOnStateDownListener(OperatorDPadUpButtonDown, handType);
            a_dPadDown.AddOnStateDownListener(OperatorDPadDownButtonDown, handType);
            a_dPadLeft.AddOnStateDownListener(OperatorDPadLeftButtonDown, handType);
            a_dPadRight.AddOnStateDownListener(OperatorDPadRightButtonDown, handType);
        }

        private void OperatorDPadUpButtonDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            switch (mode)
            {
                case Mode.pointer:
                    {
                        break;
                    }
                case Mode.light:
                    {
                        if (lightModeState == LightModeState.edit)
                            currSelectedLight.GetComponent<Light>().range += 0.1f;
                        break;
                    }
                case Mode.webcam:
                    {
                        break;
                    }
                default:
                    break;
            }
        }

        private void OperatorDPadDownButtonDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            switch (mode)
            {
                case Mode.pointer:
                    {
                        break;
                    }
                case Mode.light:
                    {
                        if (lightModeState == LightModeState.edit)
                            currSelectedLight.GetComponent<Light>().range -= 0.1f;
                        break;
                    }
                case Mode.webcam:
                    {
                        break;
                    }
                default:
                    break;
            }
        }

        private void OperatorDPadLeftButtonDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            switch (mode)
            {
                case Mode.pointer:
                    {
                        break;
                    }
                case Mode.light:
                    {
                        if (lightModeState == LightModeState.edit)
                            currSelectedLight.GetComponent<Light>().intensity -= 0.1f;
                        break;
                    }
                case Mode.webcam:
                    {
                        break;
                    }
                default:
                    break;
            }
        }

        private void OperatorDPadRightButtonDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            switch (mode)
            {
                case Mode.pointer:
                    {
                        break;
                    }
                case Mode.light:
                    {
                        if (lightModeState == LightModeState.edit)
                            currSelectedLight.GetComponent<Light>().intensity += 0.1f;
                        break;
                    }
                case Mode.webcam:
                    {
                        break;
                    }
                default:
                    break;
            }
        }

        public void OperatorRotateSceneButtonDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            appController.RotateScene();
            uiView.UpdateSceneOffsetRotation();
        }

        public void CycleMode(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            if (mode == Mode.webcam)
                mode = Mode.pointer;
            else
                mode++;

            switch(mode)
            {
                case Mode.pointer:
                    debugText.text = "Pointer Mode";
                    break;
                case Mode.light:
                    debugText.text = "Light Mode";
                    break;
                case Mode.webcam:
                    debugText.text = "Webcam Mode";
                    break;
                default:
                    break;
            }
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
                        if (lightModeState == LightModeState.placement)
                            lightModeState = LightModeState.edit;
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
                        if(lightModeState == LightModeState.none)
                        {
                            currSelectedLight = CreateLight();
                            lightModeState = LightModeState.placement;
                        }
                        else if (lightModeState == LightModeState.edit)
                        {
                            lightModeState = LightModeState.none;
                            currSelectedLight = null;
                        }

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
            lightComponent.range = 2.0f;
            lightComponent.intensity = 1.0f;

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
            switch (mode)
            {
                case Mode.pointer:
                    {
                        if (isTriggerDown)
                        {
                            RaycastHit hit;

                            if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, layerMaskPointer))
                            {
                                if (!HighlightPrefab.GetComponent<MeshRenderer>().enabled)
                                    HighlightPrefab.GetComponent<MeshRenderer>().enabled = true;

                                HighlightPrefab.transform.position = hit.point + hit.normal * 0.1f;
                            }
                            else
                            {
                                HighlightPrefab.GetComponent<MeshRenderer>().enabled = false;
                            }
                        }
                        break;
                    }
                case Mode.light:
                    {
                        if (currSelectedLight != null && lightModeState == LightModeState.placement)
                            currSelectedLight.transform.position = transform.position;
                        break;
                    }
                case Mode.webcam:
                    {
                        if (isTriggerDown)
                        {
                            RaycastHit hit;

                            if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
                            {
                                float scalar = Mathf.Lerp(0.5f, 4.0f, Vector3.Distance(transform.position, webcamScreen.transform.position) / 10.0f);

                                webcamScreen.transform.position = hit.point + Vector3.up * scalar;
                                webcamScreen.transform.localScale = Vector3.one * scalar;
                            }
                        }

                        break;
                    }
                default:
                    break;
            }
        }
    }
}
