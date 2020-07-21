using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

namespace MRR.Controller
{

    public class MrrUserController : MonoBehaviour
    {
        // a reference to the action
        public SteamVR_Action_Boolean a_trigger;
        public SteamVR_Action_Boolean a_switchPrompterPositon;
        public SteamVR_Action_Boolean a_nextPrompterPage;
        public SteamVR_Action_Boolean a_pointerMode;
        public SteamVR_Action_Boolean a_markerMode;
        public SteamVR_Action_Boolean a_prompterMode;
        // a reference to the hand
        private SteamVR_Input_Sources handType = SteamVR_Input_Sources.RightHand;
        // reference to the sphere
        public GameObject highlightPrefab;
        public GameObject markerPrefab;
        public Text debugText;
        public GameObject promterScreen;

        private bool isTriggerDown = false;

        private Mode mode = Mode.pointer;

        private enum Mode
        {
            pointer,
            marker,
            prompter
        };

        void Start()
        {
            debugText.text = "Pointer Mode";

            a_pointerMode.AddOnStateDownListener(SelectPointerMode, handType);
            a_markerMode.AddOnStateDownListener(SelectMarkerMode, handType);
            a_prompterMode.AddOnStateDownListener(SelectPrompterMode, handType);

            a_trigger.AddOnStateDownListener(TriggerDown, handType);
            a_trigger.AddOnStateUpListener(TriggerUp, handType);

            a_switchPrompterPositon.AddOnStateDownListener(UserSwitchPrompterPositionButtonDown, handType);
        }

        public void SelectPointerMode(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            mode = Mode.pointer;
            debugText.text = "Pointer Mode";
        }

        public void SelectMarkerMode(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            mode = Mode.marker;
            debugText.text = "Marker Mode";
        }

        public void SelectPrompterMode(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            mode = Mode.prompter;
            debugText.text = "Prompter Mode";
        }

        public void TriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            Debug.Log("Operator Trigger is up!");

            switch(mode)
            {
                case Mode.pointer:
                    {
                        highlightPrefab.GetComponent<MeshRenderer>().enabled = false;
                        break;
                    }
                case Mode.marker:
                    {
                        currSelectedMarker = null;
                        break;
                    }
                case Mode.prompter:
                    {
                        break;
                    }
                default:
                    break;
            }

            isTriggerDown = false;
        }

        private GameObject currSelectedMarker;

        public void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            Debug.Log("Operator Trigger is down!");

            switch (mode)
            {
                case Mode.pointer:
                    {
                        break;
                    }
                case Mode.marker:
                    {
                        currSelectedMarker = Instantiate(markerPrefab);                        
                        break;
                    }
                case Mode.prompter:
                    {
                        break;
                    }
                default:
                    break;
            }

            isTriggerDown = true;

        }

        public void UserSwitchPrompterPositionButtonDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            Debug.Log("User Button is down!");         
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
                                if (!highlightPrefab.GetComponent<MeshRenderer>().enabled)
                                    highlightPrefab.GetComponent<MeshRenderer>().enabled = true;

                                highlightPrefab.transform.position = hit.point + hit.normal * 0.1f;
                            }
                            else
                            {
                                highlightPrefab.GetComponent<MeshRenderer>().enabled = false;
                            }

                            break;
                        }
                    case Mode.marker:
                        {
                            if(currSelectedMarker != null)
                            {
                                RaycastHit hit;

                                if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
                                {
                                    currSelectedMarker.transform.position = hit.point + Vector3.up * currSelectedMarker.transform.localScale.y / 2.0f;
                                }
                            }

                            break;
                        }
                    case Mode.prompter:
                        {
                            RaycastHit hit;

                            if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
                            {
                                float scalar = Mathf.Lerp(0.5f, 4.0f, Vector3.Distance(transform.position, promterScreen.transform.position) / 10.0f);

                                promterScreen.transform.position = hit.point + Vector3.up * scalar;
                                promterScreen.transform.localScale = Vector3.one * scalar;
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
