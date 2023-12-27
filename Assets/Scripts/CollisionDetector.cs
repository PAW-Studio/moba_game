using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    [SerializeField]
    AnimationMovementManager AnimationMovementManager;
    [SerializeField]
    float DistanceBetweenPlayerAndHitsMaxAllowed = 4f, AreadDamageDistance = 10f;
    Character character;
    private void OnEnable()
    {
        character = AnimationMovementManager.playerScript.GetComponent<Character>();
        Debug.LogError("Character set");
    }
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
        CollisionDetector collisionDetector = AnimationMovementManager.GetCollionsDetectorObject();
        switch(AnimationMovementManager.currentAttackDamangeType)
        {
            case DamageType.None:
                break;
            case DamageType.Normal:
                hits = Physics.OverlapSphere(collisionDetector.transform.position,5f);
                break;
            case DamageType.Area:
                Debug.LogError("Area " + collisionDetector.AreadDamageDistance);
                hits = Physics.OverlapSphere(collisionDetector.transform.position,collisionDetector.AreadDamageDistance);  // Sphere area covered
                //hits = FilterTargetsWithinArc(hits);
                break;
            case DamageType.LeftNormal:
                hits = Physics.OverlapSphere(collisionDetector.transform.position,3f);
                break;
            case DamageType.RightNormal:
                hits = Physics.OverlapSphere(collisionDetector.transform.position,3f);
                break;
            default:
                break;
        }
        Debug.DrawRay(collisionDetector.transform.position,collisionDetector.transform.forward * collisionDetector.AreadDamageDistance,Color.blue,5f);

        //Debug.DrawRay(AnimationMovementManager.collisionDetector.transform.position,AnimationMovementManager.collisionDetector.transform.position+Vector3.one,Color.blue,10);
        Debug.LogError("** hits null  :" + (hits == null) + " Distance Allowed : " + (AnimationMovementManager.currentAttackDamangeType == DamageType.Normal ? collisionDetector.DistanceBetweenPlayerAndHitsMaxAllowed : collisionDetector.AreadDamageDistance));
        if(hits == null) return;
        DetectTargetsAndAddToHitList(hits,AnimationMovementManager.currentAttackDamangeType == DamageType.Normal ? collisionDetector. DistanceBetweenPlayerAndHitsMaxAllowed : collisionDetector.AreadDamageDistance);

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
            //Detects Minions hit
            MinionAIScript hitItem = item.GetComponent<Collider>().gameObject.GetComponent<MinionAIScript>();
            //Detects Champion hit
            Character champhitItem = item.GetComponent<Collider>().gameObject.GetComponent<Character>();

            TowerAIScript towerItem= item.GetComponent<Collider>().gameObject.GetComponent<TowerAIScript>();

            if(hitItem)
            {
                if(hitItem.teamType != character.teamType)
                {
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
            else if(towerItem)
            {
                if(hitItem.teamType != character.teamType)
                {
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
            else if(champhitItem) 
            {
                //Detects Champions hit
                if(hitItem.teamType != character.teamType)
                {
                    Character hitItemChampions = item.GetComponent<Collider>().gameObject.GetComponent<Character>();
                    //Avoid self hit
                    if(hitItemChampions && hitItemChampions != AnimationMovementManager.GetComponentInParent<Character>() && Vector3.Distance(AnimationMovementManager.playerScript.transform.position,hitItemChampions.transform.position) < distanceAllowed)
                    {
                        if(!AnimationMovementManager.HitListChampions.Contains(hitItemChampions))
                        {
                            AnimationMovementManager.HitListChampions.Add(hitItemChampions);
                            // hitItem.DealDamage(25);
                            Debug.LogError("*Champion Hit :");
                            Debug.LogError("Hit " + item.GetComponent<Collider>().gameObject.name + "\n" + "DISTANCE " + Vector3.Distance(AnimationMovementManager.playerScript.transform.position,hitItemChampions.transform.position));
                        }
                    }
                }

            }

        }
    }
    public Collider[] FilterTargetsWithinArc(Collider[] hits) 
    {
        List< Collider> TargetsWithInArc=new List<Collider>();
        //The Cos of the maximum angle accepted (-1 is 0 degrees 0 is 90 degrees, -0.5 is 45 degrees, etc...)
        float maxAngle=45;
        for(int i = 0 ; i < hits.Length ; i++)
        {
            Vector3 difference = AnimationMovementManager.GetCollionsDetectorObject().transform.position - hits[i].transform.position;

            //Check if it's within range of the arc
            if(difference.magnitude < AreadDamageDistance)
            {
                //Checks to see if the object is within a 90 degree Cone in front of the player.
                if(Vector3.Dot(AnimationMovementManager.GetCollionsDetectorObject().transform.forward,difference.normalized) > maxAngle)
                {
                    //This object is within the Cone
                    Debug.LogError("In Arc" + hits[i].name);
                    TargetsWithInArc.Add(hits[i]);
                }
                else 
                {
                 
                    Debug.LogError("Outside Arc" + hits[i].name);
                }
            }
        }
        return TargetsWithInArc.ToArray();        
    }
}
