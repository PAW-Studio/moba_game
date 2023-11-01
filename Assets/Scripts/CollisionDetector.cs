using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    [SerializeField]
    AnimationMovementManager AnimationMovementManager;

    private void Update()
    {
        //When the attack/punch animation is ON detect all colliders that collided with character arm reference object
        if(AnimationMovementManager.Attacking) 
        {
            DetectTarget();
        }   
    }
    /// <summary>
    /// Set list of targets 
    /// </summary>
    public void DetectTarget() 
    {
        Collider[] hits = Physics.OverlapSphere(AnimationMovementManager.collisionDetector.transform.position,5f);
        //Debug.DrawRay(AnimationMovementManager.collisionDetector.transform.position,AnimationMovementManager.collisionDetector.transform.position+Vector3.one,Color.blue,10);
     
        foreach(Collider item in hits)
        {
            MinionAIScript hitItem = item.GetComponent<Collider>().gameObject.GetComponent<MinionAIScript>();          
            if(hitItem && Vector3.Distance(AnimationMovementManager.playerScript.transform.position,hitItem.transform.position)<4f)
            {

                    if(!AnimationMovementManager.HitList.Contains(hitItem))
                    {
                        AnimationMovementManager.HitList.Add(hitItem);
                        // hitItem.DealDamage(25);
                        Debug.LogError("*Minion Hit :");
                        Debug.LogError("Hit " + item.GetComponent<Collider>().gameObject.name + "\n" + "DISTANCE " + Vector3.Distance(AnimationMovementManager.playerScript.transform.position,hitItem.transform.position));
                    }
            }
        }


    }
}
