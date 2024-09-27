using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave_Spawner : MonoBehaviour
{

    [SerializeField] private float countdown;
    //[SerializeField] private GameObject spawnPoint;
    [SerializeField] private Transform[] spawnPoints;

    public Wave[] waves;
    private int currentWaveIndex = 0;
    private bool readyToCountDown;

    // Start is called before the first frame update
    void Start()
    {
        readyToCountDown = true;

        for (int i = 0; i < waves.Length; i++)
        {
            waves[i].enemiesLeft = waves[i].enemies.Length;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWaveIndex >= waves.Length)
        {
            //Debug.Log("All Waves Complete");
            return;
        }

        if (readyToCountDown == true) 
        {
            countdown -= Time.deltaTime;
        }
            
        if (countdown <= 0)            
        {   
            readyToCountDown = false;
            countdown = waves[currentWaveIndex].timeToNextWave;
            StartCoroutine(SpawnWave()) ;            
        }

        if (waves[currentWaveIndex].enemiesLeft == 0)
        {
            readyToCountDown = true;
            currentWaveIndex++;
        }
    }

    IEnumerator SpawnWave()
    {
        if (currentWaveIndex < waves.Length)
        {
            for (int i = 0; i < waves[currentWaveIndex].enemies.Length; i++)
            {
                int spawnPointIndex = i % spawnPoints.Length; // Cycle through spawn points

                // Instantiate the enemy prefab at the current spawn point
                Enemy enemy = Instantiate(waves[currentWaveIndex].enemies[i],
                                           spawnPoints[spawnPointIndex].position,
                                           Quaternion.identity);

                // ... (rest of enemy initialization code)

                yield return new WaitForSeconds(waves[currentWaveIndex].timeToNextEnemy);
            }
        }
    }

    public void EnemyDefeated()
    {
        waves[currentWaveIndex].enemiesLeft--;
        if (waves[currentWaveIndex].enemiesLeft == 0)
        {
            readyToCountDown = true;
        }
    }
}

[System.Serializable]
public class Wave
{
    public Enemy[] enemies;
    public float timeToNextEnemy;
    public float timeToNextWave;

    [HideInInspector] public int enemiesLeft; 
}
