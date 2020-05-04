using UnityEngine;
using Valve.VR;

public class FindTrackerIndex : MonoBehaviour
{
    void OnEnable() 
    {

        SteamVR_Events.DeviceConnected.Listen(OnDeviceConnected);

    }

    // A SteamVR device got connected/disconnected
    private void OnDeviceConnected(int index, bool connected) {

        if (connected && OpenVR.System != null) 
        {

            //lets figure what type of device got connected
            ETrackedDeviceClass deviceClass = OpenVR.System.GetTrackedDeviceClass((uint)index);

            if (deviceClass == ETrackedDeviceClass.GenericTracker) 
            {

                GetComponent<SteamVR_TrackedObject>().index = (SteamVR_TrackedObject.EIndex)index;

                Debug.Log("[INFO]: Tracker got connected at index = " + index);

            }
        }
    }
}
