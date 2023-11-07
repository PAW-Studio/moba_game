using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    [SerializeField]
    AnimationMovementManager AnimationMovementManager;
    [SerializeField]
    float DistanceBetweenPlayerAndHitsMaxAllowed = 4f, AreadDamageDistance = 10f;
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
        Collider[] hits = null;
        switch(AnimationMovementManager.currentAttackDamangeType)
        {
            case DamageType.None:
                break;
            case DamageType.Normal:
                hits = Physics.OverlapSphere(AnimationMovementManager.GetCollionsDetectorObject().transform.position,5f);
                break;
            case DamageType.Area:
                hits = Physics.OverlapSphere(AnimationMovementManager.GetCollionsDetectorObject().transform.position,AreadDamageDistance);  // Sphere area covered
                break;
            case DamageType.LeftNormal:
                hits = Physics.OverlapSphere(AnimationMovementManager.GetCollionsDetectorObject().transform.position,3f);
                break;
            case DamageType.RightNormal:
                hits = Physics.OverlapSphere(AnimationMovementManager.GetCollionsDetectorObject().transform.position,3f);
                break;
            default:
                break;
        }
        Debug.DrawRay(AnimationMovementManager.GetCollionsDetectorObject().transform.position,AnimationMovementManager.GetCollionsDetectorObject().transform.forward * AreadDamageDistance,Color.blue,5f);

        //Debug.DrawRay(AnimationMovementManager.collisionDetector.transform.position,AnimationMovementManager.collisionDetector.transform.position+Vector3.one,Color.blue,10);
        if(hits == null) return;
        DetectTargetsAndAddToHitList(hits,AnimationMovementManager.currentAttackDamangeType == DamageType.Normal ? DistanceBetweenPlayerAndHitsMaxAllowed : AreadDamageDistance);

    }

    /// <summary>
    /// filter hit objects and add actual targets to hit list
    /// </summary>
    /// <param name="hits">colliders list collided with this object</param>
    /// <param name="distanceAllowed">Maximum distance allowed between character and target</param>
    private void DetectTargetsAndAddToHitList(Collider[] hits,float distanceAllowed)
    {
        foreach(Collider item in hits)
        {
            MinionAIScript hitItem = item.GetComponent<Collider>().gameObject.GetComponent<MinionAIScript>();
            if(hitItem && Vector3.Distance(AnimationMovementManager.playerScript.transform.position,hitItem.transform.position) < distanceAllowed)
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
