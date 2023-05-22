using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject meleeMinion;
    public GameObject casterMinion;
    public GameObject cannonMinion;
    public Vector3 blueSpawnLocation = new Vector3(33, 0, 57);
    public Vector3 redSpawnLocation = new Vector3(-40, 0, -40);
    float TimeInterval = 10f;
    float timer = 0f; 
    float spawnDelay = 70f;
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
            GameObject[] meleeMinions = new GameObject[3];
            GameObject[] casterMinions = new GameObject[3];
            
            for(int i = 0; i < 3; i++)
            {
                meleeMinions[i] = Instantiate(meleeMinion, blueSpawnLocation, Quaternion.identity);
                meleeMinions[i].GetComponent<MinionAIScript>().destination = redSpawnLocation;
                meleeMinions[i].GetComponent<MinionAIScript>().isBlue = true;

                meleeMinions[i] = Instantiate(meleeMinion, redSpawnLocation, Quaternion.identity);
                meleeMinions[i].GetComponent<MinionAIScript>().destination = blueSpawnLocation;
                meleeMinions[i].GetComponent<MinionAIScript>().isBlue = false;
                
                casterMinions[i] = Instantiate(casterMinion, blueSpawnLocation, Quaternion.identity);
                casterMinions[i].GetComponent<MinionAIScript>().destination = redSpawnLocation;
                casterMinions[i].GetComponent<MinionAIScript>().isBlue = true;

                casterMinions[i] = Instantiate(casterMinion, redSpawnLocation, Quaternion.identity);
                casterMinions[i].GetComponent<MinionAIScript>().destination = blueSpawnLocation;
                casterMinions[i].GetComponent<MinionAIScript>().isBlue = false;
            }

            timer -= spawnDelay;
            waveCount ++;

            if (waveCount == 3)
            {  
                GameObject cannonMinion1;

                cannonMinion1= Instantiate(cannonMinion, blueSpawnLocation, Quaternion.identity);
                cannonMinion1.GetComponent<MinionAIScript>().destination = redSpawnLocation;
                cannonMinion1.GetComponent<MinionAIScript>().isBlue = true;

                cannonMinion1 = Instantiate(cannonMinion, redSpawnLocation, Quaternion.identity);
                cannonMinion1.GetComponent<MinionAIScript>().destination = blueSpawnLocation;
                cannonMinion1.GetComponent<MinionAIScript>().isBlue = false;
                waveCount = 0;
            }
            
        }
    }
}