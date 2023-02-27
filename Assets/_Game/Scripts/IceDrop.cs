using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceDrop : MonoBehaviour
{
    public GameObject icePrefab;
    public Transform spawnPoint;
    public LayerMask layer;

    public float maxX;
    void Start()
    {
        InvokeRepeating(nameof(SpawnIce), 1f, 4f);
    }
    void SpawnIce()
    {
        float randomX = Random.Range(-maxX, maxX) ;
        Vector2 randomSpawnPos = new Vector2(randomX, spawnPoint.position.y);

        Instantiate(icePrefab, randomSpawnPos, Quaternion.identity);
    }

}
