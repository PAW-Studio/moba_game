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
                if(item.GetComponent<MinionAIScript>()) 
                {
                    item.GetComponent<MinionAIScript>().DealDamage((float)GameManager.instance.GetCurrentAD()); // Get current charactes AD
                    hit = true;
                    break;
                }
            }
            transform.Translate(transform.forward*speed*Time.deltaTime,Space.World);
            timePassed += Time.deltaTime;
            if(timePassed > lifeTime || hit) 
            {
                Destroy(this.gameObject);
            }
        }
    }
    /// <summary>
    /// Trigger movement of projectile
    /// </summary>
    public void Shoot() 
    {
        shoot = true;
        Debug.DrawRay(transform.position,transform.forward * 130,Color.blue,10f);

    }
}

