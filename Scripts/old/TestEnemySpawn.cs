using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemySpawn : MonoBehaviour
{
    //it works
    public GameObject enemy;
    public Transform spawnPos;

    void Start()
    {
        Instantiate(enemy, spawnPos.position, enemy.transform.rotation);
    }


}
