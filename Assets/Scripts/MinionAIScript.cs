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
    public float attackReset = 2f;
    public float damage = 30f;
    public float attackRange = 10f;

    UnityEngine.AI.NavMeshAgent agent;
    Renderer renderer;

    public MinionHealthBar minionHealthBar;
    
    // Start is called before the first frame update
    void Start()
    {   
        // Caching references
        renderer = GetComponent<Renderer>();
        agent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();

        // Setting minion health bar
        currentHealth = maxHealth;
        minionHealthBar.SetMaxHealth(maxHealth);

        // Setting type of minion based on layer
        if (isBlue)
        {
            renderer.material = blueMinionMat;
            this.gameObject.layer = 9;
        }

        else
        {
            renderer.material = redMinionMat;
            this.gameObject.layer = 10;
        }
        
        agent.SetDestination(destination);
    }

    // Update is called once per frame
    void Update()
    {
       
       if (hasTarget && targetMinion != null)
       {
            MoveToTarget();
                
            attackTimer = attackTimer - Time.deltaTime;
        
            if (attackTimer <= 0)
            {
                attackTimer = attackReset;
                
                InitiateAttack();
            }
       }

       if (targetMinion == null)
       {
        // No target; resumes pathing towards destination
        hasTarget = false;
        agent.SetDestination(destination);
       }


       if (currentHealth <= 0)
       {
            Destroy(this.gameObject);
       }
    }

    void MoveToTarget()
    {
        // Calculating distance between this minion and target
        if (Vector3.Distance(transform.position, targetMinion.transform.position) > attackRange)
        {
            agent.SetDestination(targetMinion.transform.position);
            
            // Minion stops at attackRange distance from target 
            agent.stoppingDistance = attackRange;
        }

        else
        {
            // If target minion is less than attackRange distance away, moves towards it
            agent.SetDestination(targetMinion.transform.position);
        }
    }

    void InitiateAttack()
    {
        // Attacks opposite tower
        if (targetMinion.layer == 11 || targetMinion.layer == 12)
        {    
            targetMinion.GetComponent<TowerAIScript>().currentHealth -= damage;
            
            // Reduces tower health from current health bar
            targetMinion.GetComponent<TowerAIScript>().minionHealthBar.SetHealth(targetMinion.GetComponent<TowerAIScript>().currentHealth);
        }

        else
        {
            // Attacks opposite minion and reduces minion health from current health bar
            targetMinion.GetComponent<MinionAIScript>().currentHealth -= damage;
            targetMinion.GetComponent<MinionAIScript>().minionHealthBar.SetHealth(targetMinion.GetComponent<MinionAIScript>().currentHealth);
        }
    }
}