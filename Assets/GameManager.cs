using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject meleeMinion;
    public GameObject casterMinion;
    public GameObject cannonMinion;
    public Vector3 blueSpawnLocation = new Vector3(0, 0, 0);
    public Vector3 redSpawnLocation = new Vector3(-40, 0, -40);
    float TimeInterval = 10;
    float timer = 0f; 
    float spawnDelay = 5f;
    int waveCount = 0;
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
            GameObject meleeMinion1;
            GameObject meleeMinion2;
            GameObject meleeMinion3;
            GameObject casterMinion1;
            GameObject casterMinion2;
            GameObject casterMinion3;
            
            meleeMinion1 = Instantiate(meleeMinion, blueSpawnLocation, Quaternion.identity);
            meleeMinion2 = Instantiate(meleeMinion, blueSpawnLocation, Quaternion.identity);
            meleeMinion3 = Instantiate(meleeMinion, blueSpawnLocation, Quaternion.identity);
            casterMinion1 = Instantiate(casterMinion, blueSpawnLocation, Quaternion.identity);
            casterMinion2 = Instantiate(casterMinion, blueSpawnLocation, Quaternion.identity);
            casterMinion3 = Instantiate(casterMinion, blueSpawnLocation, Quaternion.identity);
            meleeMinion1.GetComponent<MinionAIScript>().destination = redSpawnLocation;
            meleeMinion2.GetComponent<MinionAIScript>().destination = redSpawnLocation;
            meleeMinion3.GetComponent<MinionAIScript>().destination = redSpawnLocation;
            casterMinion1.GetComponent<MinionAIScript>().destination = redSpawnLocation;
            casterMinion2.GetComponent<MinionAIScript>().destination = redSpawnLocation;
            casterMinion3.GetComponent<MinionAIScript>().destination = redSpawnLocation;
            timer -= spawnDelay;
            waveCount ++;

            if (waveCount == 3)
            {
                GameObject cannonMinion1;
                
                cannonMinion1 = Instantiate(cannonMinion, blueSpawnLocation, Quaternion.identity);
                cannonMinion1.GetComponent<MinionAIScript>().destination = redSpawnLocation;
                waveCount = 0;
            }
        }
    }
}
