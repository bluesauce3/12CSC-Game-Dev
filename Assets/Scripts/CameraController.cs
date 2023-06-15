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

    private Vector3 cameraOffset;

    public Vector3 firstPerson;
    public Vector3 thirdPerson;

    private float rotationX;

    public GameObject GameManager;
    private GameManager gameManagerScript;
    public GameObject Player;
    private PlayerController playerControllerScript;
    void Start()
    {
        playerControllerScript = Player.GetComponent<PlayerController>();
        gameManagerScript = GameManager.GetComponent<GameManager>();
        cameraOffset = firstPerson;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManagerScript.isGameActive)
        {
            MoveCamera();
        }
        playerControllerScript.ThrowMagic(transform.rotation);
        if (gameManagerScript.viewMode == "1") {
            cameraOffset = firstPerson;
        }
        if (gameManagerScript.viewMode == "2") {
            cameraOffset = thirdPerson;
        }
        
    }
    private void MoveCamera()
    {
        //Clamp the camera's x rotation between min and max rotation variables
        Vector3 rotation = transform.rotation.eulerAngles;
        rotationX -= Input.GetAxis("Mouse Y") * sensitivity;
        rotationX = Mathf.Clamp(rotationX, minRotation, maxRotation);
        transform.Rotate(rotationX - rotation.x, 0, 0);
        transform.localPosition = cameraOffset;
    }
}
