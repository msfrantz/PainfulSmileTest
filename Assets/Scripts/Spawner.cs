using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject chaserPrefab, shooterPrefab;

    [SerializeField]
    public float spawnTime;
    void Start()
    {
        InvokeRepeating("SpawnEnemy", 1f, spawnTime);
    }

   

    void SpawnEnemy()
    {
        int r = Random.Range(0, 2);

        if(r == 0)
        {
            Instantiate(chaserPrefab, transform.position, Quaternion.Euler(0,0,0));
        }
        else if (r == 1)
        {
            Instantiate(shooterPrefab, transform.position, Quaternion.Euler(0, 0, 0));
        }
        
    }
}
