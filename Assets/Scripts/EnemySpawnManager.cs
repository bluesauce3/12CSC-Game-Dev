using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public GameObject GameManager;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnemySpawning());        
    }

    // Update is called once per frame
    void Update()
    {}

    private IEnumerator EnemySpawning()
    {
        yield return new WaitUntil(() => GameManager.GetComponent<GameManager>().isGameActive);
        while (GameManager.GetComponent<GameManager>().isGameActive)
        {
            yield return new WaitForSeconds(3);
            Instantiate(EnemyPrefab, transform.position, transform.rotation);
        }
        
    }
}
