using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAndShieldController : MonoBehaviour
{
    public List <GameObject> CollidingEnemies;
    // Start is called before the first frame update
    void Start()
    {}
    // Update is called once per frame
    void Update()
    {
        Debug.Log(CollidingEnemies.Count);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            CollidingEnemies.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        CollidingEnemies.Remove(other.gameObject);
    }
    
}
