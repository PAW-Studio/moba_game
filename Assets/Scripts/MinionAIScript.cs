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
    public float health = 100f;
    public float attackTimer = 3f;

    UnityEngine.AI.NavMeshAgent agent;
    
    // Start is called before the first frame update
    void Start()
    {
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
            agent.SetDestination(targetMinion.transform.position);
            attackTimer = attackTimer - Time.deltaTime;
        
            if (attackTimer <= 0)
            {
                attackTimer = 3f;
                targetMinion.GetComponent<MinionAIScript>().health -= 30;
                Debug.Log("Health" + health);
            }

       }

       if (targetMinion == null)
       {
        hasTarget = false;
        agent.SetDestination(destination);
       }


       if (health <= 0)
       {
            Destroy(this.gameObject);
       }

    }
}
