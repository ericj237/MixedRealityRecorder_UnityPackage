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

        void Start()
        {
            OperatorPoint.AddOnStateDownListener(TriggerDown, handType);
            OperatorPoint.AddOnStateUpListener(TriggerUp, handType);
        }

        public void TriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            Debug.Log("Operator Trigger is up!");
            HighlightPrefab.GetComponent<MeshRenderer>().enabled = false;
        }

        public void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            Debug.Log("Operator Trigger is down!");
            HighlightPrefab.GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
