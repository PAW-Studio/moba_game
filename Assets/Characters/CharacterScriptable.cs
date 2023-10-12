using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Character",menuName ="Character Data", order =1)]
public class CharacterScriptable : ScriptableObject
{
    public CharacterType characterType;
    //Base and growth
    public string championName = "Morya";
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
   public double currentHealth;
    public double currentHealthRegen;
    public double currentAD;
    public double currentAS;
    public double currentArmor;
    public double currentMagicResistance;
    public double currentMovementSpeed;
    public double currentRange;
    public double currentLevel;
    public double currentXP;
    public double currentAP;
    //
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

    public void UpdateStatistics()
    {
        //currentHealth = getStatistic(baseHealth,growthHealth,currentLevel);
        //currentHealthRegen = getStatistic(baseHealthRegen,growthHealthRegen,currentLevel);
        //currentAD = getStatistic(baseAD,growthAD,currentLevel);
        //currentAS = getStatistic(baseAS,growthAS,currentLevel);
        //currentArmor = getStatistic(baseArmor,growthAD,currentLevel);
        //currentMagicResistance = getStatistic(baseMagicResistance,growthMagicResistance,currentLevel);
        //currentRange = getStatistic(baseRange,growthRange,currentLevel);
    }
    public void DisplayStats()
    {
        Debug.Log("Champion Name: " + championName + "\n "
            + "Current Health: " + currentHealth + ";"
            + "Current AD: " + currentAD + "\n ");
    }
}
public enum CharacterType { Otrill,Morya,VaRun,Misa,Udara,Sura,Hakka,Dira,Tapani,Moorg}
