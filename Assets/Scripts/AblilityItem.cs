using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AblilityItem : MonoBehaviour
{
    public Ability ability;
    [SerializeField]
    public GameObject border;
    [SerializeField]
    Image abilitySprite;
    public void SelectItem() 
    {
        ShowBorder(true);
        ChampionsAbilityManager.Instance.SelectAbility(ability);
        if(ability.AblilitySprite)
        { abilitySprite.sprite = ability.AblilitySprite; }
    }
    public void ShowBorder(bool val) 
    {
        border.gameObject.SetActive(val);
    }
}
