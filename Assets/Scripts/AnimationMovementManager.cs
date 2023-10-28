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
    public void SetMovementOn() 
    {
        playerScript.moving = true;
        Debug.LogError("SetTrue");
    }

    /// <summary>
    /// Set the variable for auto movement OFF in playerscript
    /// </summary>
    public void SetMovementOff()
    {
        playerScript.moving = false;
       
    }
    /// <summary>
    /// Set charactor animators's root motion settings ON
    /// </summary>
    public void SetRootMotionOn()
    {
       // playerScript.characterAnimator.applyRootMotion = true;
    }
    /// <summary>
    /// Set charactor animators's root motion settings OFF 
    /// </summary>
    public void SetRootMotionOff()
    {
       // playerScript.characterAnimator.applyRootMotion = false;
       // transform.position = startPosition;
    }
}
