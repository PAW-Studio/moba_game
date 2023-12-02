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
    public List<Character> HitListChampions = new List<Character>();
    public List<CollisionDetectorObject> collisionDetectorObjects;            //list of collision detector objects for different type of collisions after attack
    public DamageType currentAttackDamangeType;                               //DamageTypeOff current attack
    public List<ProjectileSpawnDetails> projectileSpawnDetails = new List<ProjectileSpawnDetails>(); //Details of projectiles(throwable/shootable) with resepect to attack types
    [Header("Throw/Shootable projectile")]
    public GameObject ArrowPrefab;                                          //Arrow/projectile prefab
    public Transform spawnTrasform;                                         //Arrow/projectile spawn transform
    public Transform spawnTransforSerinaAuto;                               //Serina Autor arrow spawn reference
    public Transform spawnTrasformLeft;                                     //Arrow/projectile spawn transform for left hand
    public Projectile arrowProjectile;                                      //Reference script of arrow projectile
   

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
      Character character=  playerScript.GetComponent<Character>();
        foreach(MinionAIScript target in HitList)
        {
            if(character.characterData.characterModel.characterType == CharacterType.Jahan) 
            {
                if(playerScript.currentAttackType== AttackType.e) 
                {
                  int E_attackLevel = character.attackLevels.Find(x => x.attackType == AttackType.e).level;
                    ScaleConditionsAndFactors scaleConditionsAndFactors = character.characterData.attackScalingConditions.Find(x => x.attackType == AttackType.e).conditions.Find(y => y.Level == E_attackLevel).scaleConditionsAndFactors.Find(x=>x.scalingCondition== ScalingConditionTypes.SlowerForSomeTime);

                    target.SetSlowerSpeedEffect(scaleConditionsAndFactors.effectTime,scaleConditionsAndFactors.percentage);
                }
            }
            else 
            {
                float damage = GameManager.instance.currentCharacter.CalculateDamangeForAttack(playerScript.currentAttackType);
                //target.DealDamage((float)GameManager.instance.GetCurrentAD());  //damage equal to character's current AD
                Debug.LogError("Scale Damage " + damage);
                target.DealDamage(damage);  //damage equal to character's current attack type and level scale conditions
            }
        }

        foreach(Character target in HitListChampions)
        {
            if(character.characterData.characterModel.characterType == CharacterType.Jahan)
            {
                if(playerScript.currentAttackType == AttackType.e)
                {
                    int E_attackLevel = character.attackLevels.Find(x => x.attackType == AttackType.e).level;
                    ScaleConditionsAndFactors scaleConditionsAndFactors = character.characterData.attackScalingConditions.Find(x => x.attackType == AttackType.e).conditions.Find(y => y.Level == E_attackLevel).scaleConditionsAndFactors.Find(x => x.scalingCondition == ScalingConditionTypes.SlowerForSomeTime);

                    target.playerScript.SetSpeedEffect(scaleConditionsAndFactors.effectTime,scaleConditionsAndFactors.percentage,false);
                }
            }
            else
            {
                float damage = GameManager.instance.currentCharacter.CalculateDamangeForAttack(playerScript.currentAttackType);
                //target.DealDamage((float)GameManager.instance.GetCurrentAD());  //damage equal to character's current AD
                Debug.LogError("*"+target.name+  " Target : Scale Damage " + damage );
                target.DealDamage(damage);  //damage equal to character's current attack type and level scale conditions
            }
        }
        playerScript.currentAttackType = AttackType.None;  //Reset attack type
        HitList.Clear();
        HitListChampions.Clear();
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
    /// <summary>
    /// Spawn arrow/other throwable /shootable projectile : This method is called from animation clip 
    /// </summary>
    public void SpawnArrow() 
    {
        Character character = GetComponentInParent<Character>();
        bool SerinaAutoAttack = false;
        //if(character.currentCharacterModel.characterType== CharacterType.Serina && character.playerScript.currentAttackType== AttackType.auto) 
        //{
        //    SerinaAutoAttack = true; 
        //}
        GameObject arrow = Instantiate(ArrowPrefab,SerinaAutoAttack? spawnTransforSerinaAuto: spawnTrasform.parent);
        arrowProjectile = arrow.GetComponent<Projectile>();
        arrowProjectile.character = character ;
        arrowProjectile.attackType = playerScript.currentAttackType;
        Debug.LogError(arrowProjectile.attackType + " From " + arrowProjectile.character.currentCharacterModel.characterType);
    }
    /// <summary>
    /// Spawn arrow/other throwable /shootable projectile for left hand : This method is called from animation clip 
    /// </summary>
    public void SpawnArrowForLeftHand()
    {
        GameObject arrow = Instantiate(ArrowPrefab,spawnTrasformLeft.parent);
        arrowProjectile = arrow.GetComponent<Projectile>();
        arrowProjectile.character = GetComponentInParent<Character>();
        arrowProjectile.attackType = playerScript.currentAttackType;
        Debug.LogError(arrowProjectile.attackType+ " From "+ arrowProjectile.character.currentCharacterModel.characterType);
    }
    /// <summary>
    /// Set shoot boolean in projectile script and shoot : This method is called from animation clip 
    /// </summary>
    public void ShootArrow() 
    {
        if(arrowProjectile) 
        {
            arrowProjectile.transform.SetParent(playerScript.transform.parent);
            arrowProjectile.Shoot();
        }
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
/// <summary>
/// Used to set different spawn positions and projectiles with respect to attack
/// </summary>
public class ProjectileSpawnDetails
{
    public Transform projectileSpawnPosition;  //spawn position of projectile 
    public GameObject projectilePrefab;        //Projectile to  shoot
    public AttackType AttackType;               //damage type on collision
}
[System.Serializable]
/// <summary>
/// Damage tpye : Normal damge ,area damage etc..
/// </summary>
public enum DamageType 
{
   None,Normal, Area,LeftNormal,RightNormal
}