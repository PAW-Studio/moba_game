using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionAIScript : MonoBehaviour
{
    public Vector3 destination;

    UnityEngine.AI.NavMeshAgent agent;
    
    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.SetDestination(destination);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
