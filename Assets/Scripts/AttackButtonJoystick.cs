using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackButtonJoystick : MonoBehaviour,IPointerUpHandler
{
    [SerializeField]
    FixedJoystick fixedJoystick;
    [SerializeField]
    AttackType attackType;
    AttackButton attackButton;
    private void Start()
    {
        attackButton = GameManager.instance.AttackButtons.Find(x => x.attackType == attackType);
    }
    /// <summary>
    /// Initiate attack when joystick is released
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerUp(PointerEventData eventData)
    {
        Character character = GameManager.instance.currentCharacter;
        int index = GameManager.instance.AttackButtons.IndexOf(attackButton);
        Debug.LogError("From Here");
        character.playerScript.InitiateAttack(character.AttackValues[index],attackButton.attackType);
        //Hide joystick
        attackButton.ShowTargetJoystick(false);
    }
    /// <summary>
    /// Rotate character
    /// </summary>
    private void Update()
    {
        if(fixedJoystick)
        GameManager.instance.currentCharacter.playerScript.RotateCharacter(fixedJoystick);
    }
}
