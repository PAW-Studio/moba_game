using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAIScript : MonoBehaviour
{
    public GameObject targetMinion;
    public bool hasTarget = false;
    public float attackRange = 10f;
    public float attackTimer = 2f;
    public float attackReset = 2f;
    public float damage = 30f;
    public float maxHealth = 100f;
    public float currentHealth;

    public MinionHealthBar minionHealthBar;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        minionHealthBar.SetMaxHealth(maxHealth);

        // Similar to MinionAIScript
        if (gameObject.tag == "BlueTower")
        {
            this.gameObject.layer = 11;
        }
        
        else if (gameObject.tag == "RedTower")
        {
            this.gameObject.layer = 12;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Similar to MinionAIScript
        if (hasTarget && targetMinion != null)
        {
            attackTimer = attackTimer - Time.deltaTime;

            if (attackTimer <= 0)
            {
                attackTimer = attackReset;
                targetMinion.GetComponent<MinionAIScript>().currentHealth -= damage;
                targetMinion.GetComponent<MinionAIScript>().minionHealthBar.SetHealth(targetMinion.GetComponent<MinionAIScript>().currentHealth);
            }
        }

        if (targetMinion == null)
        {
            hasTarget = false;
        }

        if (currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    
}
