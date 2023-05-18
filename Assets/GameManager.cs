using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject minionPrefab;
    public Vector3 blueSpawnLocation = new Vector3(0, 0, 0);
    public Vector3 redSpawnLocation = new Vector3(-40, 0, -40);
    float TimeInterval = 10;
    float timer = 0f; 
    float spawnDelay = 5f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        SpawnTime();
        Debug.Log(Time.time);
    }

    void SpawnTime()
    {
        timer += Time.deltaTime;
        
        if (timer >= TimeInterval)
        {
            GameObject minionSpawned1;
            GameObject minionSpawned2;
            
            minionSpawned1 = Instantiate(minionPrefab, blueSpawnLocation, Quaternion.identity);
            minionSpawned2 = Instantiate(minionPrefab, blueSpawnLocation, Quaternion.identity);

            minionSpawned1.GetComponent<MinionAIScript>().destination = redSpawnLocation;
            minionSpawned2.GetComponent<MinionAIScript>().destination = redSpawnLocation;
            timer -= spawnDelay;
        }
    }
}
