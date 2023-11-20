using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackTypeReference : MonoBehaviour
{
    public AttackType attackType;
    [SerializeField]
    Button LevelUpButton;
    [SerializeField]
    public TMPro.TextMeshProUGUI currentLevel;

    /// <summary>
    /// Set button interactable true/false
    /// </summary>
    /// <param name="setInteractable">Button interactable :true/false</param>
    public void EnableButton(bool setInteractable) 
    {
        LevelUpButton.interactable = setInteractable;
    }
    /// <summary>
    /// Trigger attack type level up
    /// </summary>
    public void LevelUp() 
    {
        GameManager.instance.currentCharacter.UpdateAttackLevel(attackType);
    }
}
