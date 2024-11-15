using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int maxEnemies = 10;
    public float spawnIntervalMin = 1f;
    public float spawnIntervalMax = 5f;
    public Transform[] spawnPoints;

    private int currentEnemyCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        //“Coroutines are useful when we need something to happen gradually or at intervals without 
        //stopping everything else in the game. 
        StartCoroutine(SpawnEnemies());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator SpawnEnemies()
    {
        //This coroutine will run foreva, checking if there's room for another enemy every few seconds.
        while(true) 
        {
            if(currentEnemyCount < maxEnemies)
            {
                SpawnEnemy();
                currentEnemyCount++;
                
            }
           float waitTime = Random.Range(spawnIntervalMin, spawnIntervalMax);
           yield return new WaitForSeconds(waitTime);
        }
    }

    private void SpawnEnemy()
    {
        int spawnSpotIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnSpot = spawnPoints[spawnSpotIndex];

        GameObject enemy = Instantiate(enemyPrefab, spawnSpot.position, spawnSpot.rotation);
        UIManager.Instance.AddEnemyListeners(enemy.GetComponent<EnemyLogic>());
    }
}
