using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float mouseYInput; //vertical axis mouse input

    public float rotationSpeed; //rotation speed of players head
    
    public float minRotation; // min and max vertical rotation
    public float maxRotation;

    private Vector3 cameraOffset; //offset of camera

    public Vector3 firstPerson; //first person camera offset
    public Vector3 thirdPerson; //third person camera offset

    private float rotationX;

    private GameManager gameManagerScript; //external scripts to call variables from
    private PlayerController playerControllerScript;
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>(); //get scripts from GameObjects
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        cameraOffset = firstPerson; //set default camera offset to first person
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManagerScript.isGameActive) //move camera and throw magic, if game is runnning
        {
            MoveCamera();
            playerControllerScript.ThrowMagic(transform.rotation); //function from playerController; throw magic from camera rotation
        }
        
        if (gameManagerScript.viewMode == "1") { //press the 1 key to go to first person
            cameraOffset = firstPerson;
        }
        if (gameManagerScript.viewMode == "2") { //press the 2 key to go to third person
            cameraOffset = thirdPerson;
        }
        
    }
    private void MoveCamera()
    {
        //move the camera based on mouse y position
        //but clamp the camera's x rotation between min and max vertical rotation variables
        Vector3 rotation = transform.rotation.eulerAngles;
        rotationX -= Input.GetAxis("Mouse Y");
        rotationX = Mathf.Clamp(rotationX, minRotation, maxRotation);
        transform.Rotate(rotationX - rotation.x, 0, 0);
        transform.localPosition = cameraOffset;
    }
}
