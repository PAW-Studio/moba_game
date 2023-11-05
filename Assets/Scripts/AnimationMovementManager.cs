using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationMovementManager : MonoBehaviour
{
    [SerializeField]
    public PlayerScript playerScript;                                          //Character's playerscript reference

    Vector3 startPosition;

    public bool Attacking = false;
    public List<MinionAIScript> HitList = new List<MinionAIScript>();    
    public List<CollisionDetectorObject> collisionDetectorObjects;            //list of collision detector objects for different type of collisions after attack
    public DamageType currentAttackDamangeType;                               //DamageTypeOff current attack
    private void OnEnable()
    {
        // startPosition = transform.position;
    }
    /// <summary>
    /// Set the variable for auto movement ON in playerscript
    /// </summary>
    public void SetMovementOn(AttackType attackType)
    {
        playerScript.SetAnimationMovementSpeedModifier(attackType);
        playerScript.moving = true;
    }

    /// <summary>
    /// Set the variable for auto movement OFF in playerscript
    /// </summary>
    public void SetMovementOff()
    {
        playerScript.moving = false;
        playerScript.ResetAnimationMovementSpeedModifier();
        // transform.position = startPosition;
        //DetectHit();

    }
    /// <summary>
    /// Damages the list of targets hit by player
    /// </summary>
    public void DetectHit()
    {
        foreach(MinionAIScript target in HitList)
        {
            Debug.LogError("*Minion Hit :");
            //Get value with respect to attack type from character data 
            int damage = playerScript.GetComponent<Character>().characterData.attackDamageDetails.Find(x => x.attackType == playerScript.currentAttackType).DamageValue;
            target.DealDamage(damage);
        }
        playerScript.currentAttackType = AttackType.None;  //Reset attack type
        HitList.Clear();

    }

    /// <summary>
    /// Set current attack type and bool variable to indicate that player is attacking -This method is called from animation clips 
    /// </summary>
    /// <param name="_attackType">Current attack type</param>
    public void SetAttackIndicator(AttackType _attackType)
    {
        Attacking = true;
        playerScript.currentAttackType = _attackType;
    }
    /// <summary>
    /// Reset attack indicator bool
    /// </summary>
    public void ResetAttackIndicator()
    {
        Attacking = false;
        currentAttackDamangeType = DamageType.None;
    }
    /// <summary>
    /// Set damage type of current attack
    /// </summary>
    /// <param name="damageType">damage type : normal, area etc..</param>
    public void SetDamageType(DamageType damageType)
    {
        Attacking = false;
        currentAttackDamangeType = damageType;
    }
    /// <summary>
    /// Set charactor animators's root motion settings ON
    /// </summary>
    public void SetRootMotionOn()
    {
        playerScript.characterAnimator.applyRootMotion = true;
    }
    /// <summary>
    /// Set charactor animators's root motion settings OFF 
    /// </summary>
    public void SetRootMotionOff()
    {
        playerScript.characterAnimator.applyRootMotion = false;
    }
    /// <summary>
    /// Trigger destroy function in character main script
    /// </summary>
    public void TriggerDestroyCharacter()
    {
        playerScript.CharacterDie();
    }
    /// <summary>
    /// Get collision detector object from list
    /// </summary>
    /// <returns>Collision object</returns>
    public CollisionDetector GetCollionsDetectorObject()
    {
      return  collisionDetectorObjects.Find(x => x.damageType == currentAttackDamangeType).collisionDetector;
    }
}
[System.Serializable]
/// <summary>
/// Used to create instance of that holds collision detector details
/// </summary>
public class CollisionDetectorObject
{
  public CollisionDetector collisionDetector;  //reference script of collision detector object
  public DamageType damageType;               //damage type on collision
}
[System.Serializable]
/// <summary>
/// Damage tpye : Normal damge ,area damage etc..
/// </summary>
public enum DamageType 
{
   None,Normal, Area,LeftNormal,RightNormal
}