using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed=5f;                                                     //Projectile speed
    public bool shoot;                                                         //Set true to shoot this projectile
    public float lifeTime=2f;                                                  //Life time of projectile
    float timePassed = 0;                                                      //Time passed from shooting time
    public GameObject projectileColliderOjbect;                                //Collider object of this projectile to detect hit    
    public float collisionRadius=5f;
    public Character character;                                                //Character who originated this projectile 
    public AttackType attackType;                                              //Attack type from which the projectile is instantiated
    public float distance_Range=75f;
    [SerializeField]
    GameObject DestroyVFXPrefab;                                              //Projectile destroy vfx prefab
    [SerializeField]
    GameObject InHandProjectileObject, ShootObject;                           //In hand object and shoot object if applicable
    [SerializeField]
    GameObject HealOnCharacter, HealVFXWithMovement;
    /// <summary>
    /// Move projectile in forward direction and detect hit, destroy on hit or when life time is over
    /// </summary>
    void Update()
    {
        if(shoot)
        {
           Collider[] hits = Physics.OverlapSphere(projectileColliderOjbect.transform.position,collisionRadius);
            bool hit = false;
            foreach(Collider item in hits)
            {
                MinionAIScript minionAIScript = item.GetComponent<MinionAIScript>();
                if(minionAIScript && character.teamType != minionAIScript.teamType) 
                {
                  DamageDetails damageDetails=  GameManager.instance.currentCharacter.CalculateDamangeForAttack(attackType);
                  damageDetails.damageById = character.Id; //Owner of projectile
                    damageDetails.damagedItem = DamagedItem.Minion;
                    damageDetails.teamType = character.teamType == TeamType.Red ? TeamType.Blue : TeamType.Red;     //Set opposite type
                    //  item.GetComponent<MinionAIScript>().DealDamage((float)GameManager.instance.GetCurrentAD()); // Get current charactes AD
                    item.GetComponent<MinionAIScript>().DealDamage(damageDetails); // Get current charactes AD
                    Debug.LogError(damageDetails.damangeValue);
                    hit = true;
                    break;
                }
                if(item.GetComponent<TowerAIScript>())
                {
                    if(attackType == AttackType.auto)
                    {
                        if(character.teamType != item.GetComponent<TowerAIScript>().teamType)
                        {
                            DamageDetails damageDetails = GameManager.instance.currentCharacter.CalculateDamangeForAttack(attackType);
                            damageDetails.damageById = character.Id;
                            damageDetails.damagedItem = DamagedItem.Tower;
                            damageDetails.teamType = character.teamType== TeamType.Red?TeamType.Blue: TeamType.Red;     //Set opposite type
                            //  item.GetComponent<MinionAIScript>().DealDamage((float)GameManager.instance.GetCurrentAD()); // Get current charactes AD
                            item.GetComponent<TowerAIScript>().DealDamage(damageDetails); // Get current charactes AD
                            Debug.LogError(damageDetails.damangeValue);
                            hit = true;
                        }
                    }
                        break;
                }
                else if(item.GetComponent<Character>() && character.gameObject!= item.gameObject) // Avoid self hit
                {
                    DamageDetails damageDetails = GameManager.instance.currentCharacter.CalculateDamangeForAttack(attackType);
                    damageDetails.damageById = character.Id;
                    damageDetails.damagedItem = DamagedItem.Character;
                    damageDetails.teamType = character.teamType == TeamType.Red ? TeamType.Blue : TeamType.Red;     //Set opposite type
                    //  item.GetComponent<MinionAIScript>().DealDamage((float)GameManager.instance.GetCurrentAD()); // Get current charactes AD
                    item.GetComponent<Character>().DealDamage(damageDetails); // Get current charactes AD
                    Debug.LogError(damageDetails.damangeValue);
                    hit = true;
                    break;
                }
            }
            transform.Translate(transform.forward*speed*Time.deltaTime,Space.World);
            timePassed += Time.deltaTime;
            float dist = Vector3.Distance(character.transform.position,transform.position);
            
            if(dist>distance_Range || hit)
            {
                
                DestoryVFX();
                //Heal effects
                if(hit|| 1==1) //For testing  second condition set
                {
                    if(HealVFXWithMovement) 
                    {
                        SpawnHealEffect();
                    }
                }
                //
                Destroy(this.gameObject);
            }
            //if(timePassed > lifeTime / 100 || hit)
            //{
            //    Destroy(this.gameObject);
            //}
        }
    }   
    /// <summary>
    /// Trigger movement of projectile
    /// </summary>
    public void Shoot() 
    {
        shoot = true;
        if(ShootObject) 
        {
            InHandProjectileObject.SetActive(false);
            ShootObject.SetActive(true);
        }
        Debug.DrawRay(transform.position,transform.forward * 130,Color.blue,10f);
    }
    public void SpawnHealEffect() 
    {
        Character character = GetComponentInParent<Character>();
        GameObject healProjectile = Instantiate(HealVFXWithMovement,transform.parent);
        healProjectile.transform.position = transform.position;
        healProjectile.SetActive(true);
        HealProjectile _heal = healProjectile.GetComponent<HealProjectile>();
        _heal.MoveTowardsParent(character,0);
    }
    public void DestoryVFX() 
    {
        if(DestroyVFXPrefab) 
        {
            Instantiate(DestroyVFXPrefab,transform.position,Quaternion.identity,transform.parent);
        }
    }
}

