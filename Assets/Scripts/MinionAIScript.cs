using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionAIScript : MonoBehaviour
{
    public Vector3 destination;

    public Material blueMinionMat;
    public Material redMinionMat;

    public GameObject targetMinion;

    public bool isBlue;
    public bool hasTarget = false;
    public float maxHealth = 100f;
    public float currentHealth;
    public float attackTimer = 2f;
    public float attackRange = 10f;

    UnityEngine.AI.NavMeshAgent agent;
    public MinionHealthBar minionHealthBar;
    
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        minionHealthBar.SetMaxHealth(maxHealth);
        
        if (isBlue)
        {
            this.gameObject.GetComponent<Renderer>().material = blueMinionMat;
            this.gameObject.layer = 9;
        }

        else
        {
            this.gameObject.GetComponent<Renderer>().material = redMinionMat;
            this.gameObject.layer = 10;
        }
        
        agent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.SetDestination(destination);
    }

    // Update is called once per frame
    void Update()
    {
       if (hasTarget && targetMinion != null)
       {
            if (Vector3.Distance(transform.position, targetMinion.transform.position) > attackRange)
            {
                agent.SetDestination(targetMinion.transform.position);
                agent.stoppingDistance = attackRange;
            }

            else
            {
                agent.SetDestination(targetMinion.transform.position);
            }
                
                attackTimer = attackTimer - Time.deltaTime;
        
            if (attackTimer <= 0)
            {
                attackTimer = 2f;
                targetMinion.GetComponent<MinionAIScript>().currentHealth -= 30;
                targetMinion.GetComponent<MinionAIScript>().minionHealthBar.SetHealth(targetMinion.GetComponent<MinionAIScript>().currentHealth);
            }

       }

       if (targetMinion == null)
       {
        hasTarget = false;
        agent.SetDestination(destination);
       }


       if (currentHealth <= 0)
       {
            Destroy(this.gameObject);
       }

    }
}
