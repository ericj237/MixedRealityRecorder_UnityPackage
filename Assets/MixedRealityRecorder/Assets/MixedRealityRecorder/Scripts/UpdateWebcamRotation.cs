using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateWebcamRotation : MonoBehaviour
{

    public Transform hmdTransform;

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(hmdTransform);
    }
}
