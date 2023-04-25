using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    private float verticalInput;
    private float horizontalInput;
    private float mouseXInput;

    private Rigidbody playerRb;
    private Animator m_Animator;
 
    public GameObject GameManager;
    private GameManager gameManagerScript;

    public GameObject Ground;

    public GameObject SwordCollision;
    private List <GameObject> CollidedEnemies;

    public float speed;
    public float strafeSpeed;
    public float rotationSpeed;
    public float jumpStrength;
    public bool isOnGround;

    private bool canUseSword = true;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        m_Animator = GetComponent<Animator>();
        gameManagerScript = GameManager.GetComponent<GameManager>();
        Console.Write(GetSignOfNumber(1));
        Console.Write("hello");
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManagerScript.isGameActive)
        {
            MovePlayerAndAnimate();
            UseSwordAndShield();
        }
        //Reset rotation so the player doesnt tip over
        transform.Rotate(0 - transform.rotation.x, 0, 0 - transform.rotation.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) {
            isOnGround = true;
        }
    }

    //Get 1, 0 or -1 based on +- or 0 of number
    private int GetSignOfNumber(float number) 
    {
        if (number == 0)
        {
            return 0;
        } else 
        {
            return (int)(number / Math.Abs(number));
        }
        
    }

    private void MovePlayerAndAnimate()
    {
        //Movement and looking code using keyboard and mouse to move and rotate
        Cursor.lockState = CursorLockMode.Locked;

        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
        mouseXInput = Input.GetAxis("Mouse X");
        if (verticalInput <= 0) 
        {
            //slow player down when strafing
            transform.Translate(new Vector3(horizontalInput, 0, verticalInput) * Time.deltaTime * speed * strafeSpeed);
        } else
        {
            transform.Translate(new Vector3(horizontalInput, 0, verticalInput) * Time.deltaTime * speed);
        }
        //keep player inbounds
        if (transform.position.x > Ground.transform.localScale.x * 5)
        {
            transform.Translate(transform.position.x - Ground.transform.localScale.x * 5, 0, 0);
        }
        

        m_Animator.SetFloat("VerticalVelocity", GetSignOfNumber(verticalInput));
        m_Animator.SetFloat("HorizontalVelocity", GetSignOfNumber(horizontalInput));             

            transform.Rotate(new Vector3(0, mouseXInput, 0) * rotationSpeed);
        if (Input.GetKeyDown("space") && isOnGround && 
        ((horizontalInput == 0) || !(verticalInput == 0))) 
        {
            playerRb.AddForce(new Vector3(0, jumpStrength), ForceMode.Impulse);
            isOnGround = false;
            m_Animator.SetTrigger("Jump");
        } 
    }

    private void UseSwordAndShield()
    {
        if (Input.GetMouseButton(0) && canUseSword)
        {
            m_Animator.SetTrigger("Attack");
            CollidedEnemies = SwordCollision.GetComponent<SwordAndShieldController>().CollidingEnemies;
            for (int i = 0; i < CollidedEnemies.Count; i++)
            {
                Destroy(CollidedEnemies[i]);
            }
            StartCoroutine(SwordCooldown());
        }
    }

    private IEnumerator SwordCooldown()
    {
        canUseSword = false;
        yield return new WaitForSeconds(5);
        canUseSword = true;
    }
}
