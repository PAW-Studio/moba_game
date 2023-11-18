using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This scirptable object can be created from Menu->Asset->Create->CharacterData. It can used to store the details of characters like base health, speed, name, character image, etc and we can use this data in game 

[CreateAssetMenu(fileName ="Character",menuName ="Character Data", order =1)]
public class CharacterScriptable : ScriptableObject
{
    [Tooltip("Character FBX model -Idle and character type")]
    public CharacterModels characterModel;                               //Character FBX model (Idle -fbx model)

    public List<AttackAnimationDetails> attackAnimationDetails = new List<AttackAnimationDetails>();  //Attack type and movement speed details -used to set movement speed character as well as attack type wise dynamically 
    public List<AttackDamageDetails> attackDamageDetails = new List<AttackDamageDetails>();   //List of attack types and respective damages values that can be done
    //Base and growth
    public List<AttackCoolDownDetails> attackCoolDownDetails = new List<AttackCoolDownDetails>(); //Cooldown timings with resepect to attack type

    public string championName = "";                                    //Name of character
    public string summonerName = "";

    // setting base and growth health based on excel
    public double baseHealth = 670;
    public double growthHealth = 120;

    // setting base and growth health regen based on excel
    public double baseHealthRegen = 8.5;
    public double growthHealthRegen = 0.85;

    // setting base and growth AD based on excel
    public double baseAD = 62;
    public double growthAD = 3.75;

    // setting base, growth and current AP
    public double baseAP = 0;
    public double growthAP = 0;

    // setting base and growth AS based on excel
    public double baseAS = 0.625;
    public double growthAS = 0.02125;

    // setting base and growth armor based on excel
    public double baseArmor = 44;
    public double growthArmor = 4.7;

    // setting base and growth magic resistance based on excel
    public double baseMagicResistance = 32;
    public double growthMagicResistance = 2.05;

    // setting base and growth movement speed base on excel
    public double baseMovementSpeed = 330;
    public double growthMovementSpeed = 0;

    // setting base and growth range on excel
    public double baseRange = 125;
    public double growthRange = 0;
    //

    // creating variables to store current levels of each statistic
    [HideInInspector]
    public double currentHealth;
    [HideInInspector]
    public double currentHealthRegen;
    [HideInInspector]
    public double currentAD;
    [HideInInspector]
    public double currentAS;
    [HideInInspector]
    public double currentArmor;
    [HideInInspector]
    public double currentMagicResistance;
    [HideInInspector]
    public double currentMovementSpeed;
    [HideInInspector]
    public double currentRange;
    //[HideInInspector]
    public double currentLevel=1;
    [HideInInspector]
    public double currentXP;
    [HideInInspector]
    public double currentAP;
    //

    //Character Details
    //Animator name in resource folder;
    [Tooltip("Animator name in Resources/CharacterAnimators folder")]
    public string CharacterAnimatorName;                                       //Can be used later to load the animator of character at runtime from resource folder
  
    //Set up values  of character
    public void SetUpValues() 
    {
        currentHealth = baseHealth;
        currentHealthRegen = baseHealthRegen;
        currentAD = baseAD;
        currentAS = baseAS;
        currentArmor = baseArmor;
        currentMagicResistance = baseMagicResistance;
        currentMovementSpeed = baseMovementSpeed;
        currentRange = baseRange;
        currentXP = Globals.level1;
        currentLevel = 0;
        currentAP = baseAP;
    }

    /// <summary>
    /// Check character level : method is taken from champion script
    /// </summary>
    public void CheckLevel()
    {
        if(currentXP >= Globals.level2)
        {
            currentLevel = 2;
            UpdateStatistics();
        }
        else if(currentXP >= Globals.level3)
        {
            currentLevel = 3;
            UpdateStatistics();
        }
        else if(currentXP >= Globals.level4)
        {
            currentLevel = 4;
            UpdateStatistics();
        }
        else if(currentXP >= Globals.level5)
        {
            currentLevel = 5;
            UpdateStatistics();
        }
        else if(currentXP >= Globals.level6)
        {
            currentLevel = 6;
            UpdateStatistics();
        }
        else if(currentXP >= Globals.level7)
        {
            currentLevel = 7;
            UpdateStatistics();
        }
        else if(currentXP >= Globals.level8)
        {
            currentLevel = 8;
            UpdateStatistics();
        }
        else if(currentXP >= Globals.level9)
        {
            currentLevel = 9;
            UpdateStatistics();
        }
        else if(currentXP >= Globals.level10)
        {
            currentLevel = 10;
            UpdateStatistics();
        }
        else if(currentXP >= Globals.level11)
        {
            currentLevel = 11;
            UpdateStatistics();
        }
        else if(currentXP >= Globals.level12)
        {
            currentLevel = 12;
            UpdateStatistics();
        }
        else if(currentXP >= Globals.level13)
        {
            currentLevel = 13;
            UpdateStatistics();
        }
        else if(currentXP >= Globals.level14)
        {
            currentLevel = 14;
            UpdateStatistics();
        }
        else if(currentXP >= Globals.level15)
        {
            currentLevel = 15;
            UpdateStatistics();
        }
        else if(currentXP >= Globals.level16)
        {
            currentLevel = 16;
            UpdateStatistics();
        }
        else if(currentXP >= Globals.level17)
        {
            currentLevel = 17;
            UpdateStatistics();
        }
        else if(currentXP >= Globals.level18)
        {
            currentLevel = 18;
            UpdateStatistics();
        }
    }

    /// <summary>
    /// Update character statistics
    /// </summary>
    public void UpdateStatistics()
    {
        currentHealth = Champions.getStatistic(baseHealth,growthHealth,currentLevel);
        currentHealthRegen = Champions.getStatistic(baseHealthRegen,growthHealthRegen,currentLevel);
        currentAD = Champions.getStatistic(baseAD,growthAD,currentLevel);
        currentAS = Champions.getStatistic(baseAS,growthAS,currentLevel);
        currentArmor = Champions.getStatistic(baseArmor,growthAD,currentLevel);
        currentMagicResistance = Champions.getStatistic(baseMagicResistance,growthMagicResistance,currentLevel);
        currentRange = Champions.getStatistic(baseRange,growthRange,currentLevel);
    }
    /// <summary>
    /// Display states: temporary for displaying states 
    /// </summary>
    public void DisplayStats()
    {
        Debug.Log("Champion Name: " + championName + "\n "
            + "Current Health: " + currentHealth + ";"
            + "Current AD: " + currentAD + "\n ");
    }
    /// <summary>
    /// Get cooldown time for the attack
    /// </summary>
    /// <param name="attackType">attack type</param>
    /// <returns>cooldown time for the attack</returns>
    public float GetCoolDownTime(AttackType attackType)
    {
        float cooldowntime = 0;
        cooldowntime = attackCoolDownDetails.Find(x => x.attackType == attackType).GetCoolDowntime(currentLevel);
        return cooldowntime;
    }
    /// <summary>
    /// Get actvie time of the attack
    /// </summary>
    /// <param name="attackType">attack type</param>
    /// <returns>Active time for the attack</returns>
    public float GetActiveTime(AttackType attackType)
    {
        float ActiveTime = 1f;
        ActiveTime = attackCoolDownDetails.Find(x => x.attackType == attackType).ActiveTime;
        return ActiveTime;
    }
}
/// <summary>
/// characters : this enum is used to decide the character and model is set based on this type
/// </summary>
public enum CharacterType { Otrill,Morya,VaRun,Misa,Udara,Sura,Hakka,Dira,Tapani,Moorg,Jahan,Ranzeb,Serina}

/// <summary>
/// Used to hold the values of attack type and movement speed modifier for the attack
/// </summary>
[System.Serializable]
public class AttackAnimationDetails 
{
    public AttackType attackType;
    public float movementSpeedModifier = 1f;
   // public int DamageValue=25;
}
/// <summary>
/// Used to hold the values of attack type and attack subType(AD,AP etc) 
/// </summary>
[System.Serializable]
public class AttackDamageDetails
{
    public AttackType attackType;
    public AttackSubType attackSubType;
    //public int DamageValue = 25;
}
/// <summary>
/// Used to hold the values of attack type and cooldownFormula
/// </summary>
[System.Serializable]
public class AttackCoolDownDetails
{
    public AttackType attackType;
    public float ActiveTime = 2f;
    public List<LevelCoolDownValue> levelCoolDownValues = new List<LevelCoolDownValue>();   //List of cooldown values with respect to levels   
    /// <summary>
    /// Get cooldown timing for current level of chracter 
    /// </summary>
    /// <param name="characterLevel">character level</param>
    /// <returns>cooldown time for attack</returns>
    public float GetCoolDowntime(double characterLevel) 
    {
        if(characterLevel == 0) 
        {
            characterLevel = 1;
        }
        float cooldownValue = 1;
        if(levelCoolDownValues.Find(x=>x.level== characterLevel) != null) 
        {
            cooldownValue = levelCoolDownValues.Find(x => x.level == characterLevel).cooldownValue;  //Find cooldown value with respect to input level
        }
        return  cooldownValue <0 ?0:cooldownValue;     //return zero if less then zero
    }
}
/// <summary>
/// Cooldown value and level
/// </summary>
[System.Serializable]
public class LevelCoolDownValue
{
    public int level;
    public float cooldownValue;
}

