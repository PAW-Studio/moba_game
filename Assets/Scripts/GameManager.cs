using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject meleeMinion;
    public GameObject casterMinion;
    public GameObject cannonMinion;
    public Vector3 blueSpawnLocation = new Vector3(-159, 1, -152);
    public Vector3 redSpawnLocation = new Vector3(132, 1, 140);
    
    float TimeInterval = 10f;
    float timer = 0f; 
    float spawnDelay = 30f;
    int waveCount = 0;
    
    // Start is called before the first frame update
    
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {   
        SpawnTime();
    }
    

    void SpawnTime()
    {
        // Starting game timer
        timer += Time.deltaTime;
        
        if (timer >= TimeInterval)
        {
            MeleeMinionSpawn();
            CasterMinionSpawn();
            
            // Resetting timer for subsequent waves
            timer -= spawnDelay;
            waveCount ++;
            
            CannonMinionSpawn();
        }
    }

    void MeleeMinionSpawn()
    {
        // Spawning and setting destination for melee minions
        GameObject[] meleeMinions = new GameObject[3];

        for(int i = 0; i < 3; i++)
            {
                meleeMinions[i] = Instantiate(meleeMinion, blueSpawnLocation, Quaternion.identity);
                meleeMinions[i].GetComponent<MinionAIScript>().destination = redSpawnLocation;
                
                // Blue minions
                meleeMinions[i].GetComponent<MinionAIScript>().isBlue = true;

                meleeMinions[i] = Instantiate(meleeMinion, redSpawnLocation, Quaternion.identity);
                meleeMinions[i].GetComponent<MinionAIScript>().destination = blueSpawnLocation;
                
                // Red minions
                meleeMinions[i].GetComponent<MinionAIScript>().isBlue = false;
            }
    }

    void CasterMinionSpawn()
    {
        // Spawning and setting destination for caster minions
        GameObject[] casterMinions = new GameObject[3];

        for(int i = 0; i < 3; i++)
            { 
                casterMinions[i] = Instantiate(casterMinion, blueSpawnLocation, Quaternion.identity);
                casterMinions[i].GetComponent<MinionAIScript>().destination = redSpawnLocation;
                
                // Blue minions
                casterMinions[i].GetComponent<MinionAIScript>().isBlue = true;
                
                casterMinions[i] = Instantiate(casterMinion, redSpawnLocation, Quaternion.identity);
                casterMinions[i].GetComponent<MinionAIScript>().destination = blueSpawnLocation;
                
                // Red minions
                casterMinions[i].GetComponent<MinionAIScript>().isBlue = false;

            }
    }

    void CannonMinionSpawn()
    {
        // Every third wave spawns a cannon minion and sets destination as above
        if (waveCount == 3)
            {  
                GameObject cannonMinion1;

                cannonMinion1= Instantiate(cannonMinion, blueSpawnLocation, Quaternion.identity);
                cannonMinion1.GetComponent<MinionAIScript>().destination = redSpawnLocation;
                cannonMinion1.GetComponent<MinionAIScript>().isBlue = true;

                cannonMinion1 = Instantiate(cannonMinion, redSpawnLocation, Quaternion.identity);
                cannonMinion1.GetComponent<MinionAIScript>().destination = blueSpawnLocation;
                cannonMinion1.GetComponent<MinionAIScript>().isBlue = false;
                
                // Resetting wave count for next cannon minion wave
                waveCount = 0;
            }
    }

}