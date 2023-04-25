using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject Player;
    public float enemySpeed;
    
    private Vector3 playerDirection;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        MoveTowardsPlayer();
    }

    private void MoveTowardsPlayer()
    {
        transform.LookAt(Player.transform.position);
        transform.Translate(0, 0, Time.deltaTime * enemySpeed);
    }
}
