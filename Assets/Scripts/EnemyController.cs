using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject Player;
    public float enemySpeed;
    public float difficultyCurve;

    public GameManager gameManagerScript;
    private Animator enemyAnimator;

    private Vector3 playerDirection;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        enemyAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveTowardsPlayer();
    }

    private void MoveTowardsPlayer()
    {
        transform.LookAt(Player.transform.position);
        transform.Translate(0, 0, Time.deltaTime * enemySpeed * (difficultyCurve * 10 * gameManagerScript.timeSinceStart));
        enemyAnimator.SetFloat("EnemySpeed", enemySpeed * 0.5f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GetComponent<Rigidbody>().AddForce(-1 * Vector3.forward, ForceMode.Impulse);
        }
    }
}
