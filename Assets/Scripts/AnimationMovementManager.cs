using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationMovementManager : MonoBehaviour
{
    [SerializeField]
    PlayerScript playerScript;
    public void SetMovementOn() 
    {
        playerScript.moving = true;
        Debug.LogError("SetTrue");
    }
    public void SetMovementOff()
    {
        playerScript.moving = false;
    }
}
