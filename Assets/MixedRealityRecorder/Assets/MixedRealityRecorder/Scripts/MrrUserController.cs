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
        public MrrPromterController prompter;

        // a reference to the action
        public SteamVR_Action_Boolean a_trigger;
        public SteamVR_Action_Boolean a_nextPrompterPage;
        public SteamVR_Action_Boolean a_cycleMode;
        public SteamVR_Action_Boolean a_dPadUp;
        public SteamVR_Action_Boolean a_dPadDown;
        public SteamVR_Action_Boolean a_dPadLeft;
        public SteamVR_Action_Boolean a_dPadRight;
        public Transform userTransform;
        // a reference to the hand
        private SteamVR_Input_Sources handType = SteamVR_Input_Sources.RightHand;
        // reference to the sphere
        public GameObject highlightPrefab;
        public GameObject markerPrefab;
        public Text debugText;
        public GameObject promterScreen;
        public GameObject groupMarker;

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

            a_cycleMode.AddOnStateDownListener(CycleMode, handType);

            a_trigger.AddOnStateDownListener(TriggerDown, handType);
            a_trigger.AddOnStateUpListener(TriggerUp, handType);

            a_nextPrompterPage.AddOnStateDownListener(UserNextPrompterPage, handType);

            a_dPadUp.AddOnStateDownListener(UserDPadUpButtonDown, handType);
            a_dPadDown.AddOnStateDownListener(UserDPadDownButtonDown, handType);
            a_dPadLeft.AddOnStateDownListener(UserDPadLeftButtonDown, handType);
            a_dPadRight.AddOnStateDownListener(UserDPadRightButtonDown, handType);
        }

        private void UserDPadUpButtonDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            switch (mode)
            {
                case Mode.pointer:
                    {
                        break;
                    }
                case Mode.marker:
                    {                

                        foreach (Transform marker in groupMarker.transform)
                            Destroy(marker.gameObject);

                        break;
                    }
                case Mode.prompter:
                    {
                        break;
                    }
                default:
                    break;
            }
        }

        private void UserDPadDownButtonDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            switch (mode)
            {
                case Mode.pointer:
                    {
                        break;
                    }
                case Mode.marker:
                    {
                        if (currSelectedMarker != null)
                            Destroy(currSelectedMarker);
                        break;
                    }
                case Mode.prompter:
                    {
                        break;
                    }
                default:
                    break;
            }
        }

        private void UserDPadLeftButtonDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            switch (mode)
            {
                case Mode.pointer:
                    {
                        break;
                    }
                case Mode.marker:
                    {
                        break;
                    }
                case Mode.prompter:
                    {
                        break;
                    }
                default:
                    break;
            }
        }

        private void UserDPadRightButtonDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            switch (mode)
            {
                case Mode.pointer:
                    {
                        break;
                    }
                case Mode.marker:
                    {
                        break;
                    }
                case Mode.prompter:
                    {
                        break;
                    }
                default:
                    break;
            }
        }

        public void UserNextPrompterPage(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            prompter.DisplayNextPage();
        }

        public void CycleMode(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            if (mode == Mode.prompter)
                mode = Mode.pointer;
            else
                mode++;

            switch (mode)
            {
                case Mode.pointer:
                    debugText.text = "Pointer Mode";
                    break;
                case Mode.marker:
                    debugText.text = "Marker Mode";
                    break;
                case Mode.prompter:
                    debugText.text = "Promter Mode";
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
                        highlightPrefab.GetComponent<MeshRenderer>().enabled = false;
                        break;
                    }
                case Mode.marker:
                    {
                        if(currSelectedMarker != null)
                        {
                            currSelectedMarker.GetComponent<Renderer>().material = matCurrSelectedMarker;
                            currSelectedMarker.GetComponent<BoxCollider>().enabled = true;
                            currSelectedMarker = null;
                        }
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
        public Material[] matMarker = new Material[3];
        public Material matHoverMarker;
        private int markerColorIndex = 0;

        public void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            Debug.Log("User Trigger is down!");

            switch (mode)
            {
                case Mode.pointer:
                    {
                        break;
                    }
                case Mode.marker:
                    {
                        if(currSelectedMarker == null)
                        {
                            RaycastHit hit;

                            if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
                            {
                                if (hit.transform.gameObject.tag == "Marker")
                                {
                                    currSelectedMarker = hit.transform.gameObject;
                                    currSelectedMarker.GetComponent<BoxCollider>().enabled = false;
                                }
                                else if (Vector3.Dot(hit.normal, Vector3.up) > 0.75f)
                                {
                                    if (markerColorIndex == 2)
                                        markerColorIndex = 0;
                                    else
                                        markerColorIndex++;

                                    matCurrSelectedMarker = matMarker[markerColorIndex];

                                    currSelectedMarker = Instantiate(markerPrefab);
                                    currSelectedMarker.transform.parent = groupMarker.transform;
                                    currSelectedMarker.GetComponent<Renderer>().material = matHoverMarker;
                                    currSelectedMarker.GetComponent<BoxCollider>().enabled = false;
                                }
                            }
                        }                       

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

        private GameObject hoverMarker = null;
        private Material matCurrSelectedMarker;

        private void Update()
        {

            switch (mode)
            {
                case Mode.pointer:
                    {
                        if (isTriggerDown)
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
                        }
                        break;
                    }
                case Mode.marker:
                    {
                        RaycastHit hit;

                        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
                        {
                            if (currSelectedMarker != null && isTriggerDown && Vector3.Dot(hit.normal, Vector3.up) > 0.75f)
                            {
                                currSelectedMarker.transform.position = hit.point + Vector3.up * currSelectedMarker.transform.localScale.y / 2.0f;
                                currSelectedMarker.transform.LookAt(new Vector3(userTransform.position.x, currSelectedMarker.transform.position.y, userTransform.position.z));
                            }
                            else if (hoverMarker == null && hit.transform.gameObject.tag == "Marker")
                            {
                                hoverMarker = hit.transform.gameObject;
                                matCurrSelectedMarker = hoverMarker.GetComponent<Renderer>().material;
                                hoverMarker.GetComponent<Renderer>().material = matHoverMarker;
                            }
                            else if(hoverMarker != null && hit.transform.gameObject.tag != "Marker")
                            {
                                hoverMarker.GetComponent<Renderer>().material = matCurrSelectedMarker;
                                hoverMarker = null;
                            }
                        }

                        break;
                    }
                case Mode.prompter:
                    {
                        if(isTriggerDown)
                        {
                            RaycastHit hit;

                            if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
                            {
                                float scalar = Mathf.Lerp(0.5f, 4.0f, Vector3.Distance(transform.position, promterScreen.transform.position) / 10.0f);

                                promterScreen.transform.position = hit.point + Vector3.up * scalar;
                                promterScreen.transform.localScale = Vector3.one * scalar;
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
