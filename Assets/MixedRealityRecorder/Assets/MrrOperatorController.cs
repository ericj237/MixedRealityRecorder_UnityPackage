using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

namespace MRR.Controller
{

    public class MrrOperatorController : MonoBehaviour
    {
        // a reference to the action
        public SteamVR_Action_Boolean a_point;
        public SteamVR_Action_Boolean a_toggleRecording;
        // a reference to the hand
        private SteamVR_Input_Sources handType = SteamVR_Input_Sources.LeftHand;
        //reference to the sphere
        public GameObject HighlightPrefab;

        public Canvas canvasScreencapture;

        private bool isTriggerDown = false;

        void Start()
        {
            a_point.AddOnStateDownListener(OperatorPointTriggerDown, handType);
            a_point.AddOnStateUpListener(OperatorPointTriggerUp, handType);

            a_toggleRecording.AddOnStateDownListener(OperatorStartRecordingButtonDown, handType);
        }

        public void OperatorPointTriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            Debug.Log("Operator Trigger is up!");

            isTriggerDown = false;
            HighlightPrefab.GetComponent<MeshRenderer>().enabled = false;
        }

        public void OperatorPointTriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            Debug.Log("Operator Trigger is down!");
            isTriggerDown = true;
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
