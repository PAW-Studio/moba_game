

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealProjectile : MonoBehaviour
{
    public float speed = 5f;                                                     //Projectile speed
   
    public Character character;                                                //Character who originated this projectile 
    public AttackType attackType;                                              //Attack type from which the projectile is instantiated
    public float healAmount = 0;
    

    [SerializeField]
    GameObject HealOnCharacter, HealVFXWithMovement;
    /// <summary>
    /// Move projectile in forward direction and detect hit, destroy on hit or when life time is over
    /// </summary>
    public void MoveTowardsParent(Character _character,float healPoints)
    {
        character = _character;
        Debug.LogError("---"+character == null);
        move = true;
       // (this.gameObject,character.gameObject.transform.position,1f); //.setOnComplete(ShowHealVFXOnCharacter);
    }
    GameObject healEffect;
    public void ShowHealVFXOnCharacter() 
    {
        healEffect = Instantiate(HealOnCharacter,transform.parent);
        Invoke(nameof(DestroyEffectAndObject),1f);
    }
    public void DestroyEffectAndObject() 
    {
        //
        if(healEffect)
        Destroy(healEffect.gameObject);
        Destroy(this.gameObject);
    }
    float timePassed = 0;
    bool move = false;
    private void Update()
    {
        if(move)
        {
            Vector3 dir = (this.transform.position - character.transform.position).normalized;
           transform.Translate(dir * speed * Time.deltaTime,Space.World);
            timePassed += Time.deltaTime;
            float dist = Vector3.Distance(character.transform.position,transform.position);

            if(dist < 2)
            {
                move = false;
                ShowHealVFXOnCharacter();
            }
        }
    }
}

