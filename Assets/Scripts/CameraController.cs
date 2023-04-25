using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float mouseYInput;
    public float rotationSpeed;
    
    public float minRotation = 45f; // Minimum rotation
    public float maxRotation = 135f; // Maximum rotation
    public float sensitivity = 1f;
    public float cameraOffsetY;
    public float cameraOffsetX;

    private float rotationX;

    public GameObject GameManager;
    private GameManager gameManagerScript;
    void Start()
    {
        gameManagerScript = GameManager.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManagerScript.isGameActive)
        {
            MoveCamera();
        }
        if (Input.GetKeyDown("p"))
        {

        }
    }
    private void MoveCamera()
    {
        //Clamp the camera's x rotation between min and max rotation variables
        Vector3 rotation = transform.rotation.eulerAngles;
        rotationX -= Input.GetAxis("Mouse Y") * sensitivity;
        rotationX = Mathf.Clamp(rotationX, minRotation, maxRotation);
        transform.Rotate(rotationX - rotation.x, 0, 0);
    }
}
