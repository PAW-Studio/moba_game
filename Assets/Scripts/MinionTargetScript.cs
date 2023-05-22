using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinionTargetScript : MonoBehaviour
{
    public List<GameObject> targetList = new List<GameObject>();
    public NavMeshAgent minionAgent;
    public MinionAIScript minionScript;
    public GameObject closestTarget;

    public bool isBlue;

    
    // Start is called before the first frame update
    void Start()
    {
        minionScript = this.GetComponentInParent<MinionAIScript>();
        minionAgent = this.GetComponentInParent<NavMeshAgent>();
        isBlue = minionScript.isBlue;
    }

    // Update is called once per frame
    void Update()
    {
        if (targetList.Count > 0 && minionScript.hasTarget == false)
        {
            for (var i = targetList.Count - 1; i > -1; i--)
            {
                if (targetList[i] != null)
                {
                    float closestDistance = Mathf.Infinity;
                    float distance = Vector3.Distance(gameObject.transform.position, targetList[i].transform.position);
                    
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestTarget = targetList[i];
                    }
                }

                else
                {
                    targetList.RemoveAt(i);
                }
            }

            minionScript.targetMinion = closestTarget;
            minionScript.hasTarget = true;
        }
    }

    public void OnTriggerEnter(Collider collider) 
    {
        if (isBlue)
        {
            if (!targetList.Contains(collider.gameObject) && collider.gameObject.layer == 10)
        {
            targetList.Add(collider.gameObject);
            Debug.Log("Added" + gameObject.name);
        }
        }
        
        else
        {
             if (!targetList.Contains(collider.gameObject) && collider.gameObject.layer == 9)
        {
            targetList.Add(collider.gameObject);
            Debug.Log("Added" + gameObject.name);
        }
        }
    }

    public void OnTriggerExit(Collider collider) 
    {
        if (targetList.Contains(collider.gameObject))
        {
            targetList.Remove(collider.gameObject);
        }
    }
}
