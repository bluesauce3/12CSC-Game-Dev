using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public GameObject GameManager;
    public float difficultyCurve;
    private GameObject LastEnemySpawned;
    public GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        GameManager = GameObject.Find("GameManager");
        StartCoroutine(EnemySpawning());      
    }

    // Update is called once per frame
    void Update()
    {}

    public Vector3 GenerateRandomVector3(Vector3 inputVector3, float minDistance, float maxDistance)
    {
        float offsetX = Random.Range(-1f, 1f);
        float offsetY = 0;
        float offsetZ = Random.Range(-1f, 1f);

        Vector3 randomVector = new Vector3(offsetX, offsetY, offsetZ).normalized;

        float distance = Random.Range(minDistance, maxDistance);

        Vector3 resultVector = inputVector3 + randomVector * distance;

        return resultVector;
    }

    private IEnumerator EnemySpawning()
    {
        while (GameManager.GetComponent<GameManager>().isGameActive)
        {
            yield return new WaitForSeconds(Mathf.Max(-1 * difficultyCurve * GameManager.GetComponent<GameManager>().timeSinceStart + 1.5f, 0.01f));
            LastEnemySpawned = Instantiate(EnemyPrefab, GenerateRandomVector3(Player.transform.position, 4, 8), transform.rotation);
            LastEnemySpawned.GetComponent<EnemyController>().difficultyCurve = difficultyCurve;
        }
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                Destroy(enemy);
            }
        
    }
}
