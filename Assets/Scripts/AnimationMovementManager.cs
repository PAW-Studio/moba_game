using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationMovementManager : MonoBehaviour
{
    [SerializeField]
    PlayerScript playerScript;                                          //Character's playerscript reference

    Vector3 startPosition;
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
        Debug.LogError("SetTrue");
    }

    /// <summary>
    /// Set the variable for auto movement OFF in playerscript
    /// </summary>
    public void SetMovementOff()
    {
        playerScript.moving = false;
        playerScript.ResetAnimationMovementSpeedModifier();
        // transform.position = startPosition;
        DetectHit();

    }
    /// <summary>
    /// Detects the nearby minion hit 
    /// </summary>
    public void DetectHit()
    {
        RaycastHit[] hits = Physics.RaycastAll(playerScript.transform.position,transform.forward,8);
        Debug.DrawRay(new Vector3(playerScript.transform.position.x,5,playerScript.transform.position.z),playerScript.transform.forward * 5,Color.blue,10);
        foreach(RaycastHit item in hits)
        {
            Debug.LogError("Hit " + item.collider.gameObject.name);
            MinionAIScript hitItem = item.collider.gameObject.GetComponent<MinionAIScript>();
            if(hitItem)
            {
                hitItem.DealDamage(25);
                Debug.LogError("*Minion Hit :");
            }
        }
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
