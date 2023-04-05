using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float verticalInput;
    private float horizontalInput;
    private float mouseXInput;
    private Rigidbody playerRb;

    public float speed;
    public float rotationSpeed;
    public float jumpStrength;
    public bool isOnGround;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Movement and looking code using keyboard and mouse
        Cursor.lockState = CursorLockMode.Locked;

        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
        mouseXInput = Input.GetAxis("Mouse X");

        transform.Translate(new Vector3(horizontalInput, 0, verticalInput) * Time.deltaTime * speed);

        transform.Rotate(new Vector3(0, mouseXInput, 0) * rotationSpeed);
        if (Input.GetKeyDown("space") && isOnGround) {
            playerRb.AddForce(new Vector3(0, jumpStrength), ForceMode.Impulse);
            isOnGround = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) {
            isOnGround = true;
        }
    }
}
