﻿using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    [SerializeField]
    protected List<EnemyAndPoint> toSpawn;
    public List<EnemyAI> enemiesSpawned;

    
    public void Spawn()
    {        
        for (int i = 0; i < toSpawn.Count; i++)
        {
            Vector3 enemyPosition = toSpawn[i].transform.position;
            GameObject go = Instantiate(toSpawn[i].enemy,enemyPosition,Quaternion.identity);
            enemiesSpawned.Add(go.GetComponent<EnemyAI>());
            go.GetComponent<EnemyAI>().target = EnemyManager.instance.player;
            go.GetComponent<EnemyAI>().cam = EnemyManager.instance.cam;
        }
        GetComponent<BoxCollider>().enabled = false;
        Debug.Log("Enemies spawned");
    }

    public void EnemiesInRoomFall()
    {
        for (int i = 0; i < enemiesSpawned.Count; i++)
        {
            enemiesSpawned[i].Fall();
        }
    }
}

[System.Serializable]
public class EnemyAndPoint
{
    [SerializeField]
    public GameObject enemy;
    [SerializeField]
    public Transform transform;
}