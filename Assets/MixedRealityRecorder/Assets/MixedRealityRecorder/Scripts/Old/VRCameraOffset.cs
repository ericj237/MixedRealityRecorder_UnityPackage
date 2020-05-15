using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRCameraOffset : MonoBehaviour
{

    public InputField inputField_vrCameraOffsetX, inputField_vrCameraOffsetY, inputField_vrCameraOffsetZ, inputField_vrCameraFov;
    InputField.SubmitEvent submitEvent_vrCameraOffsetX, submitEvent_vrCameraOffsetY, submitEvent_vrCameraOffsetZ, submitEvent_vrCameraFov;

    public Camera vrCameraForeground, vrCameraBackground, vrCameraMask;

    private Vector3 vrCameraOffset;
    private Transform vrCameraTransform; 

    // Start is called before the first frame update
    void Start()
    {
        vrCameraTransform = this.gameObject.GetComponent<Transform>();

        vrCameraOffset = vrCameraTransform.position;

        inputField_vrCameraOffsetX.text = vrCameraOffset.x.ToString();

        submitEvent_vrCameraOffsetX = new InputField.SubmitEvent();
        submitEvent_vrCameraOffsetX.AddListener(setVRCameraOffsetX);
        inputField_vrCameraOffsetX.onEndEdit = submitEvent_vrCameraOffsetX;

        inputField_vrCameraOffsetY.text = vrCameraOffset.y.ToString();

        submitEvent_vrCameraOffsetY = new InputField.SubmitEvent();
        submitEvent_vrCameraOffsetY.AddListener(setVRCameraOffsetY);
        inputField_vrCameraOffsetY.onEndEdit = submitEvent_vrCameraOffsetY;

        inputField_vrCameraOffsetZ.text = vrCameraOffset.z.ToString();

        submitEvent_vrCameraOffsetZ = new InputField.SubmitEvent();
        submitEvent_vrCameraOffsetZ.AddListener(setVRCameraOffsetZ);
        inputField_vrCameraOffsetZ.onEndEdit = submitEvent_vrCameraOffsetZ;

        inputField_vrCameraFov.text = vrCameraForeground.focalLength.ToString();

        submitEvent_vrCameraFov = new InputField.SubmitEvent();
        submitEvent_vrCameraFov.AddListener(setVRCameraOffsetZ);
        inputField_vrCameraFov.onEndEdit = submitEvent_vrCameraOffsetZ;
    }

    public void setVRCameraOffsetX(string x) 
    {
        vrCameraTransform.position = new Vector3(float.Parse(x), vrCameraTransform.position.y, vrCameraTransform.position.z);
    }

    public void setVRCameraOffsetY(string y) 
    {
        vrCameraTransform.position = new Vector3(vrCameraTransform.position.x, float.Parse(y), vrCameraTransform.position.z);
    }

    public void setVRCameraOffsetZ(string z) 
    {
        vrCameraTransform.position = new Vector3(vrCameraTransform.position.x, vrCameraTransform.position.y, float.Parse(z));
    }

    public void setVRCameraFov(string fov) 
    {
        vrCameraForeground.focalLength = float.Parse(fov);
        vrCameraBackground.focalLength = float.Parse(fov);
        vrCameraMask.focalLength = float.Parse(fov);
    }
}
