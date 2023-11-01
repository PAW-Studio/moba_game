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

    public CollisionDetector collisionDetector;                               //Reference object for attack body part transform
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
        //RaycastHit[] hits = Physics.RaycastAll(playerScript.transform.position,playerScript.transform.forward,8);
        //Debug.DrawRay(new Vector3(playerScript.transform.position.x,playerScript.transform.position.y,playerScript.transform.position.z),playerScript.transform.forward * 5,Color.blue,10);
        //foreach(RaycastHit item in hits)
        //{
        //    Debug.LogError("Hit " + item.collider.gameObject.name);
        //    MinionAIScript hitItem = item.collider.gameObject.GetComponent<MinionAIScript>();
        //    if(hitItem)
        //    {
        //        hitItem.DealDamage(25);
        //        Debug.LogError("*Minion Hit :");
        //    }
        //}

        foreach(MinionAIScript target in HitList)
        {
            Debug.LogError("*Minion Hit :");
            target.DealDamage(25); //Temp 25 for now
        }
        HitList.Clear();

    }

    
    public void SetAttackIndicator() 
    {
        Attacking = true;
    }
    public void ResetAttackIndicator()
    {
        Attacking = false;
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
}
