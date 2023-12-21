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
    public TeamType targetTeamType;

    // Start is called before the first frame update
    void Start()
    {
        // Caching references
        minionScript = this.GetComponentInParent<MinionAIScript>();
        minionAgent = this.GetComponentInParent<NavMeshAgent>();
        isBlue = minionScript.isBlue;
        targetTeamType = isBlue ? TeamType.Red : TeamType.Blue;
    }

    // Update is called once per frame
    void Update()
    {
        if (targetList.Count > 0 && minionScript.hasTarget == false)
        {
            // Working backwards through the target list starting from the highest index
            for (var i = targetList.Count - 1; i > -1; i--)
            {
                if (targetList[i] != null)
                {
                    // Calculating distance between this minion gameobject and target minion/tower in target list
                    float closestDistance = Mathf.Infinity;
                    float distance = Vector3.Distance(gameObject.transform.position, targetList[i].transform.position);
                    
                    // Setting target in target list with the smallest distance as target object 
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestTarget = targetList[i];
                    }
                }

                // Removing null references in target list to avoid nullexception error
                else
                {
                    targetList.RemoveAt(i);
                }
            }
           
              
                if(closestTarget.GetComponent<Character>())
                {
                    Character character = closestTarget.GetComponent<Character>();
                    if(character.teamType != minionScript.teamType)
                    {
                        float distance = Vector3.Distance(gameObject.transform.position,closestTarget.transform.position);
                        if(!character.minionInRange)
                        {
                            character.minionInRange = true;
                            character.distance = distance;
                            character.targetMinion = minionScript;
                            minionScript.ShowIndicator(true);
                        }
                        else
                        {
                            if(distance < character.distance)
                            {
                                character.minionInRange = true;

                                if(character.targetMinion)
                                {
                                    character.targetMinion.ShowIndicator(false);
                                    character.targetMinion.hasTarget = false;
                                    character.targetMinion.GetComponentInChildren<MinionTargetScript>().targetList.Clear();
                                }
                                character.distance = distance;
                                character.targetMinion = minionScript;
                                minionScript.ShowIndicator(true);
                            }
                        }
                    }
                    else
                    {
                        character.minionInRange = false;
                        character.distance = 0;
                        character.targetMinion = null;
                        minionScript.ShowIndicator(false);
                        minionScript.hasTarget = false;
                        targetList.Clear();
                    }
                }
                else
                {
                    minionScript.ShowIndicator(false);
                    minionScript.hasTarget = false;
                    targetList.Clear();
                }
                
            minionScript.targetMinion = closestTarget;
            minionScript.hasTarget = true;
        }
        else if(targetList.Count>0 && minionScript.hasTarget) 
        {
            if(!minionScript.targetMinion.GetComponent<Character>()) 
            {
                minionScript.ShowIndicator(false);
            }
        }
    }

    public void OnTriggerEnter(Collider collider) 
    {
        if (isBlue)
        {
            // Adding red minions and towers to target list
            if (!targetList.Contains(collider.gameObject))
            {
                if (collider.gameObject.layer == 10 || collider.gameObject.layer == 12)
                {
                    targetList.Add(collider.gameObject);
                    Debug.Log("Added" + gameObject.name);
                }
            }
        }
        else
        {
            // Adding blue minions and towers to target list
            if (!targetList.Contains(collider.gameObject))
            {    
                if (collider.gameObject.layer == 9 || collider.gameObject.layer == 11)
                {
                    targetList.Add(collider.gameObject);
                    Debug.Log("Added" + gameObject.name);
                }
            }
        }
    }
    
    public void OnTriggerExit(Collider collider) 
    {
        // Removing minions that exit collider range from the target list
        if (targetList.Contains(collider.gameObject))
        {
            targetList.Remove(collider.gameObject);
        }
    }
}
