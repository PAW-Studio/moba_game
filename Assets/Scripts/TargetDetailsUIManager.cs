using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetDetailsUIManager : MonoBehaviour
{
    [SerializeField]
    Image targetIcon;
    [SerializeField]
    Slider targetHealthBar;
    [SerializeField]
    TMPro.TextMeshProUGUI healthText;
    [SerializeField]
    TMPro.TextMeshProUGUI AD, AP, AR, MR;
    MinionAIScript minionTarget;
    TowerAIScript towerTarget;
    Character championTarget;
    /// <summary>
    /// Check for targets and update details 
    /// </summary>
    public void UpdateDetails()
    {
        MinionAIScript targetMinion = GameManager.instance.currentCharacter.targetMinion;
        TowerAIScript targetTower = GameManager.instance.currentCharacter.targetTower;
        UpdateHealthBarDetails(targetMinion,targetTower);
    }
    /// <summary>
    /// Update health bar text and slider
    /// </summary>
    /// <param name="targetMinion">target Minion</param>
    /// <param name="targetTower">target Tower</param>
    private void UpdateHealthBarDetails(MinionAIScript targetMinion,TowerAIScript targetTower)
    {
        string health = "";
        if(targetMinion)
        {
            minionTarget = targetMinion;
            targetHealthBar.maxValue = minionTarget.maxHealth;
            targetHealthBar.value = minionTarget.currentHealth;
            health = System.Convert.ToInt32( targetHealthBar.value).ToString() + "/" + System.Convert.ToInt32(minionTarget.maxHealth).ToString();
            AD.text = System.Convert.ToInt32( minionTarget.damage).ToString();
        }
        if(targetTower)
        {
            towerTarget = targetTower;
            targetHealthBar.maxValue = towerTarget.maxHealth;
            targetHealthBar.value = towerTarget.currentHealth;
            towerTarget = targetTower;
            health = System.Convert.ToInt32(targetHealthBar.value).ToString() + "/" + System.Convert.ToInt32(towerTarget.maxHealth).ToString();
            AD.text = System.Convert.ToInt32(towerTarget.damage).ToString();
        }
        healthText.text = health;
    }
}
