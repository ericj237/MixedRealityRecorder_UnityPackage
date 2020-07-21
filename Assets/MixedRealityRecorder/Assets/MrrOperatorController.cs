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
        public SteamVR_Action_Boolean OperatorPoint;
        // a reference to the hand
        private SteamVR_Input_Sources handType = SteamVR_Input_Sources.LeftHand;
        //reference to the sphere
        public GameObject HighlightPrefab;

        private bool isTriggerDown = false;

        void Start()
        {
            OperatorPoint.AddOnStateDownListener(TriggerDown, handType);
            OperatorPoint.AddOnStateUpListener(TriggerUp, handType);
        }

        public void TriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            Debug.Log("Operator Trigger is up!");
            isTriggerDown = false;
            HighlightPrefab.GetComponent<MeshRenderer>().enabled = false;
        }

        public void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            Debug.Log("Operator Trigger is down!");
            isTriggerDown = true;
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

                    HighlightPrefab.transform.position = hit.point + hit.normal * 0.25f;
                }
                else
                {
                    HighlightPrefab.GetComponent<MeshRenderer>().enabled = false;
                }
            }
        }
    }
}
