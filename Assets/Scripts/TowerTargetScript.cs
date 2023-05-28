using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerTargetScript : MonoBehaviour
{
    public List<GameObject> targetList = new List<GameObject>();
    public TowerAIScript towerScript;
    public GameObject closestTarget;

    // Start is called before the first frame update
    void Start()
    {
        towerScript = this.GetComponentInParent<TowerAIScript>();
    }

    // Update is called once per frame
    
    // Very similar to MinionTargetScript
    void Update()
    {
        if (targetList.Count > 0 && towerScript.hasTarget == false)
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

            towerScript.targetMinion = closestTarget;
            towerScript.hasTarget = true;
        }
    }

    // Very similar to MinionTargetScript
    public void OnTriggerEnter(Collider collider) 
    {
        if (gameObject.tag == "BlueTower")
        {
            if (!targetList.Contains(collider.gameObject) && collider.gameObject.layer == 10)
            {
                targetList.Add(collider.gameObject);
                Debug.Log("Added" + gameObject.name);
            }
        }
        
        else
        {
            if (gameObject.tag == "RedTower")
        {
            if (!targetList.Contains(collider.gameObject) && collider.gameObject.layer == 9)
            {
                targetList.Add(collider.gameObject);
                Debug.Log("Added" + gameObject.name);
            }
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
