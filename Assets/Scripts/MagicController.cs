using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicController : MonoBehaviour
{
    private Rigidbody magicRb;

    public float throwPower;
    public float throwAngleAdjust;
    public float throwPowerCoefficient;

    public float splashRadius;

    private GameObject Player;
    private GameObject Camera;

    private Vector3 playerEuler;
    private Vector3 cameraEuler;
    private Quaternion throwQuaternion;
    
    private float cameraEulerX;

    public GameObject deathParticle;

    public GameManager gameManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        StartCoroutine(DespawnCountdown());
        Player = GameObject.Find("Player");
        Camera = GameObject.Find("Camera");
        magicRb = GetComponent<Rigidbody>();
        playerEuler = Player.transform.rotation.eulerAngles;
        cameraEuler = Camera.transform.rotation.eulerAngles;
        if (cameraEuler.x > 180) {
            cameraEulerX = -(360 - cameraEuler.x);
        } else {
            cameraEulerX = cameraEuler.x;
        }
        transform.Translate(new Vector3(0, 0, 1));
        magicRb.AddForce((transform.forward * throwPowerCoefficient + new Vector3(0, 1, 0)) * throwPower, ForceMode.Impulse);
        magicRb.angularVelocity = Random.onUnitSphere * Random.Range(10.0f, 100.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Ground")) { 
            Collider[] hitEnemies = Physics.OverlapSphere(transform.position, splashRadius);
            gameManagerScript.enemiesKilledScore += (hitEnemies.Length - 2)*(hitEnemies.Length - 2);
            foreach (var hitEnemy in hitEnemies)
            {
                if (hitEnemy.gameObject.CompareTag("Enemy")) {
                    Destroy(hitEnemy.gameObject);
                }
            }
            Instantiate(deathParticle, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
    private IEnumerator DespawnCountdown() {
        yield return new WaitForSeconds(3.0f);
        Destroy(gameObject);
    }
}
