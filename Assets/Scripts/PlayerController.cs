using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//this script contains the code for player movement, attack, health and animations

public class PlayerController : MonoBehaviour
{
    private float verticalInput;
    private float horizontalInput; //mouse and keyboard inputs
    private float mouseXInput;

    private Rigidbody playerRb;
    private Animator m_Animator;    //player components and objects used for animations and visibility
    private GameObject playerMesh;

    private GameManager gameManagerScript; //used to know when games running etc.

    public GameObject Map;  //out of bounds
    public GameObject MagicPrefab; //magic to throw

    public float speed;
    public float strafeSpeed;
    public float rotationSpeed;     //all variables are public to edit in Unity GUI
    public float jumpStrength;
    public bool isOnGround;
    public int health;
    public float magicCooldownTime;
    public float damageImmunityCooldownTime;

    private bool canUseMagic = true;    //used for cooldowns
    private bool damageImmune = false;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        m_Animator = GetComponent<Animator>();
        playerMesh = transform.Find("Mesh").gameObject; //get necessary components and gameobjects
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManagerScript.isGameActive) //move, animate and attack when games running
        {
            MovePlayerAndAnimate();
            UseWeapon();
        }
        if (gameManagerScript.viewMode == "2") { //third person 
            playerMesh.SetActive(true);
        } else {    //first person 
            playerMesh.SetActive(false);
        }
        //Reset rotation so the player doesnt tip over
        transform.Rotate(0 - transform.rotation.x, 0, 0 - transform.rotation.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) {
            isOnGround = true;
        }
        if (collision.gameObject.CompareTag("Enemy") && !damageImmune) {
            health -= 1;
            gameManagerScript.removeHeart(health);
            gameManagerScript.hurtOverlayCountdown(3);
            StartCoroutine(DamageImmunityCooldown());
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
        if (transform.position.x > Map.transform.localScale.x * 5)
        {
            transform.Translate(transform.position.x - Map.transform.localScale.x * 5, 0, 0);
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

    private void UseWeapon()
    {
        if (Input.GetMouseButton(0) && canUseMagic)
        {
            m_Animator.SetTrigger("Attack");
            StartCoroutine(MagicCooldown());
        }
    }

    private IEnumerator MagicCooldown()
    {
        canUseMagic = false;
        yield return new WaitForSeconds(magicCooldownTime);
        canUseMagic = true;
    }
    private IEnumerator DamageImmunityCooldown()
    {
        damageImmune = true;
        yield return new WaitForSeconds(damageImmunityCooldownTime);
        damageImmune = false;
    }

    public void ThrowMagic(Quaternion rotation) 
    {
        if (Input.GetMouseButton(0) && canUseMagic && gameManagerScript.isGameActive) 
        {
            Instantiate(MagicPrefab, transform.position + new Vector3(0, 1, 0), rotation);

        }

    }
}
