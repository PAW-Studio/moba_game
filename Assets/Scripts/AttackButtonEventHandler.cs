using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class AttackButtonEventHandler : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    bool PointerDown = false;
    [SerializeField]
    AttackType attackType;                  
    AttackButton attackButton;
    private void Start()
    {
        attackButton = GameManager.instance.AttackButtons.Find(x => x.attackType == attackType);
    }
    [SerializeField]
    float Holdtime = 0.5f;
    float timePassed = 0;

    /// <summary>
    /// Calculated time passed from the time pointer is down if attack is allowed 
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        if(GameManager.instance.currentCharacter.playerScript.AttackAllowed(attackType))
        { PointerDown = true; }
    }
    /// <summary>
    /// Initiate attack if player does not hold button to use joystick
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerUp(PointerEventData eventData)
    {
        PointerDown = false;
        if(timePassed < Holdtime)
        {
            Character character = GameManager.instance.currentCharacter;
            int index = GameManager.instance.AttackButtons.IndexOf(attackButton);
            Debug.LogError("From Here 2");
            character.playerScript.InitiateAttack(character.AttackValues[index],attackButton.attackType);
        }
        timePassed = 0;
    }
    /// <summary>
    /// Show joystick on button if player holds button for (0.5 seconds)
    /// </summary>
    void Update()
    {
        if(PointerDown) 
        {
            timePassed += Time.deltaTime;
            if(timePassed > Holdtime) 
            {
                if(GameManager.instance.currentCharacter.playerScript.AttackAllowed(attackType))
                    attackButton.ShowTargetJoystick(true);
            }
        }
        
    }
}
