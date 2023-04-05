using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float mouseYInput;
    public float rotationSpeed;
    // Start is called before the first frame update
    void Start()
    {}

    // Update is called once per frame
    void Update()
    {
        //Look up and down using mouse
        mouseYInput = Input.GetAxis("Mouse Y");
        transform.Rotate(-mouseYInput * rotationSpeed, 0, 0);
    }
}
