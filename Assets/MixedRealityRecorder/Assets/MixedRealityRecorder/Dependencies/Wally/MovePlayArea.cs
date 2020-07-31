using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayArea : MonoBehaviour
{

    [SerializeField]
    float moveSpeed = 2;
    float rotationSpeed = 1.75423f;



    void MoveX(int _dir) {
        transform.position += new Vector3(1, 0, 0) * moveSpeed * Time.deltaTime * _dir;

    }

    void MoveY(int _dir) {
        transform.position += new Vector3(0, 0, 1) * moveSpeed * Time.deltaTime * _dir;
    }

    void RotateY(float _dir) {
        transform.Rotate(new Vector3(0, 1, 0) * rotationSpeed * Time.deltaTime * _dir);
    }

    public void SetPosition(Vector3 _pos)
    {
        transform.position = _pos;
    }

    public void SetRotation(Quaternion _rot)
    {
        transform.rotation = _rot;
    }

    // Update is called once per frame
    void Update()
    {
        MoveX((int)Input.GetAxis("Horizontal"));
        MoveY((int)Input.GetAxis("Vertical"));

        RotateY(Input.GetAxis("Rotation"));
     
        
    }
}
