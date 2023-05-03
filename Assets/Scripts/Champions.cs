using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Globals
{
    // creating a global variable for equation modifier 1
    public static double modifier1 = 0.7025;

    // creating a global variable for equation modifier 2
    public static double modifier2 = 0.0175;

    //creating global variables for xp level caps
    public static double level1 = 0;
    public static double level2 = 280;
    public static double level3 = 660;
    public static double level4 = 1140;
    public static double level5 = 1720;
    public static double level6 = 2400;
    public static double level7 = 3180;
    public static double level8 = 4060;
    public static double level9 = 5040;
    public static double level10 = 6120;
    public static double level11 = 7300;
    public static double level12 = 8580;
    public static double level13 = 9960;
    public static double level14 = 11440;
    public static double level15 = 13020;
    public static double level16 = 14700;
    public static double level17 = 16480;
    public static double level18 = 18360;
}
public class Champions
{
    protected static double getStatistic(double baseStatistic, double growthStatistic, double currentLevel)
    {
        double return_value = baseStatistic + (growthStatistic * (currentLevel - 1) * (Globals.modifier1 + (Globals.modifier2 * (currentLevel - 1))));
        return return_value;
    }
}
public class Otrill : Champions
{
    string championName = "Otrill";
    string summonerName = "";

    // setting base and growth health based on excel
    double baseHealth = 650;
    double growthHealth = 109;

    // setting base and growth health regen based on excel
    double baseHealthRegen = 8;
    double growthHealthRegen = 0.72;

    // setting base and growth AD based on excel
    double baseAD = 63.5;
    double growthAD = 3.75;

    // setting base and growth AS based on excel
    double baseAS = 0.631;
    double growthAS = 0.02625;

    // setting base and growth armor based on excel
    double baseArmor = 33.5;
    double growthArmor = 4.9;

    // setting base and growth magic resistance based on excel
    double baseMagicResistance = 30.5;
    double growthMagicResistance = 2.05;

    // setting base and growth movement speed base on excel
    double baseMovementSpeed = 340;
    double growthMovementSpeed = 0;

    // setting base and growth range on excel
    double baseRange = 100;
    double growthRange = 0;

    // creating variables to store current levels of each statistic
    double currentHealth;
    double currentHealthRegen;
    double currentAD;
    double currentAS;
    double currentArmor;
    double currentMagicResistance;
    double currentMovementSpeed;
    double currentRange;
    double currentLevel;
    double currentXP;

    //defining constructor
    public Otrill()
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

    }

    //defining function to check champion_level
    public void CheckLevel()
    {
        if (currentXP >= Globals.level2)
        {
            currentLevel = 2;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level3)
        {
            currentLevel = 3;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level4)
        {
            currentLevel = 4;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level5)
        {
            currentLevel = 5;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level6)
        {
            currentLevel = 6;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level7)
        {
            currentLevel = 7;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level8)
        {
            currentLevel = 8;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level9)
        {
            currentLevel = 9;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level10)
        {
            currentLevel = 10;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level11)
        {
            currentLevel = 11;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level12)
        {
            currentLevel = 12;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level13)
        {
            currentLevel = 13;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level14)
        {
            currentLevel = 14;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level15)
        {
            currentLevel = 15;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level16)
        {
            currentLevel = 16;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level17)
        {
            currentLevel = 17;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level18)
        {
            currentLevel = 18;
            UpdateStatistics();
        }
    }

    public void UpdateStatistics()
    {
        currentHealth = getStatistic(baseHealth, growthHealth, currentLevel);
        currentHealthRegen = getStatistic(baseHealthRegen, growthHealthRegen, currentLevel);
        currentAD = getStatistic(baseAD, growthAD, currentLevel);
        currentAS = getStatistic(baseAS, growthAS, currentLevel);
        currentArmor = getStatistic(baseArmor, growthAD, currentLevel);
        currentMagicResistance = getStatistic(baseMagicResistance, growthMagicResistance, currentLevel);
        currentRange = getStatistic(baseRange, growthRange, currentLevel);
    }

    public void DisplayStats()
    {
        Debug.Log("Champion Name: " + championName + "\n " 
            + "Current Health: " + currentHealth + ";" 
            + "Current AD: " + currentAD + "\n ");
    }
}
public class Morya : Champions
{
    string championName = "Morya";
    string summonerName = "";

    // setting base and growth health based on excel
    double baseHealth = 670;
    double growthHealth = 120;

    // setting base and growth health regen based on excel
    double baseHealthRegen = 8.5;
    double growthHealthRegen = 0.85;

    // setting base and growth AD based on excel
    double baseAD = 62;
    double growthAD = 3.75;

    // setting base and growth AS based on excel
    double baseAS = 0.625;
    double growthAS = 0.02125;

    // setting base and growth armor based on excel
    double baseArmor = 44;
    double growthArmor = 4.7;

    // setting base and growth magic resistance based on excel
    double baseMagicResistance = 32;
    double growthMagicResistance = 2.05;

    // setting base and growth movement speed base on excel
    double baseMovementSpeed = 330;
    double growthMovementSpeed = 0;

    // setting base and growth range on excel
    double baseRange = 125;
    double growthRange = 0;

    // creating variables to store current levels of each statistic
    double currentHealth;
    double currentHealthRegen;
    double currentAD;
    double currentAS;
    double currentArmor;
    double currentMagicResistance;
    double currentMovementSpeed;
    double currentRange;
    double currentLevel;
    double currentXP;

    //defining constructor
    public Morya()
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

    }

    //defining function to check champion_level
    public void CheckLevel()
    {
        if (currentXP >= Globals.level2)
        {
            currentLevel = 2;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level3)
        {
            currentLevel = 3;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level4)
        {
            currentLevel = 4;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level5)
        {
            currentLevel = 5;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level6)
        {
            currentLevel = 6;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level7)
        {
            currentLevel = 7;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level8)
        {
            currentLevel = 8;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level9)
        {
            currentLevel = 9;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level10)
        {
            currentLevel = 10;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level11)
        {
            currentLevel = 11;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level12)
        {
            currentLevel = 12;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level13)
        {
            currentLevel = 13;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level14)
        {
            currentLevel = 14;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level15)
        {
            currentLevel = 15;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level16)
        {
            currentLevel = 16;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level17)
        {
            currentLevel = 17;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level18)
        {
            currentLevel = 18;
            UpdateStatistics();
        }
    }

    public void UpdateStatistics()
    {
        currentHealth = getStatistic(baseHealth, growthHealth, currentLevel);
        currentHealthRegen = getStatistic(baseHealthRegen, growthHealthRegen, currentLevel);
        currentAD = getStatistic(baseAD, growthAD, currentLevel);
        currentAS = getStatistic(baseAS, growthAS, currentLevel);
        currentArmor = getStatistic(baseArmor, growthAD, currentLevel);
        currentMagicResistance = getStatistic(baseMagicResistance, growthMagicResistance, currentLevel);
        currentRange = getStatistic(baseRange, growthRange, currentLevel);
    }
    public void DisplayStats()
    {
        Debug.Log("Champion Name: " + championName + "\n "
            + "Current Health: " + currentHealth + ";"
            + "Current AD: " + currentAD + "\n ");
    }

}
public class VaRun : Champions
{
    string championName = "Va-Run";
    string summonerName = "";

    // setting base and growth health based on excel
    double baseHealth = 610;
    double growthHealth = 105;

    // setting base and growth health regen based on excel
    double baseHealthRegen = 7;
    double growthHealthRegen = 0.67;

    // setting base and growth AD based on excel
    double baseAD = 56;
    double growthAD = 3.3;

    // setting base and growth AS based on excel
    double baseAS = 0.658;
    double growthAS = 0.0223;

    // setting base and growth armor based on excel
    double baseArmor = 26.5;
    double growthArmor = 5.3;

    // setting base and growth magic resistance based on excel
    double baseMagicResistance = 31;
    double growthMagicResistance = 1.75;

    // setting base and growth movement speed base on excel
    double baseMovementSpeed = 340;
    double growthMovementSpeed = 0;

    // setting base and growth range on excel
    double baseRange = 460;
    double growthRange = 0;

    // creating variables to store current levels of each statistic
    double currentHealth;
    double currentHealthRegen;
    double currentAD;
    double currentAS;
    double currentArmor;
    double currentMagicResistance;
    double currentMovementSpeed;
    double currentRange;
    double currentLevel;
    double currentXP;

    //defining constructor
    public VaRun()
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

    }

    //defining function to check champion_level
    public void CheckLevel()
    {
        if (currentXP >= Globals.level2)
        {
            currentLevel = 2;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level3)
        {
            currentLevel = 3;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level4)
        {
            currentLevel = 4;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level5)
        {
            currentLevel = 5;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level6)
        {
            currentLevel = 6;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level7)
        {
            currentLevel = 7;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level8)
        {
            currentLevel = 8;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level9)
        {
            currentLevel = 9;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level10)
        {
            currentLevel = 10;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level11)
        {
            currentLevel = 11;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level12)
        {
            currentLevel = 12;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level13)
        {
            currentLevel = 13;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level14)
        {
            currentLevel = 14;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level15)
        {
            currentLevel = 15;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level16)
        {
            currentLevel = 16;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level17)
        {
            currentLevel = 17;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level18)
        {
            currentLevel = 18;
            UpdateStatistics();
        }
    }

    public void UpdateStatistics()
    {
        currentHealth = getStatistic(baseHealth, growthHealth, currentLevel);
        currentHealthRegen = getStatistic(baseHealthRegen, growthHealthRegen, currentLevel);
        currentAD = getStatistic(baseAD, growthAD, currentLevel);
        currentAS = getStatistic(baseAS, growthAS, currentLevel);
        currentArmor = getStatistic(baseArmor, growthAD, currentLevel);
        currentMagicResistance = getStatistic(baseMagicResistance, growthMagicResistance, currentLevel);
        currentRange = getStatistic(baseRange, growthRange, currentLevel);
    }

    public void DisplayStats()
    {
        Debug.Log("Champion Name: " + championName + "\n "
            + "Current Health: " + currentHealth + ";"
            + "Current AD: " + currentAD + "\n ");
    }

}
public class Misa : Champions
{
    string championName = "Misa";
    string summonerName = "";

    // setting base and growth health based on excel
    double baseHealth = 580;
    double growthHealth = 99;

    // setting base and growth health regen based on excel
    double baseHealthRegen = 2.7;
    double growthHealthRegen = 0.5;

    // setting base and growth AD based on excel
    double baseAD = 52;
    double growthAD = 3.5;

    // setting base and growth AS based on excel
    double baseAS = 0.7;
    double growthAS = 0.0185;

    // setting base and growth armor based on excel
    double baseArmor = 23;
    double growthArmor = 4.1;

    // setting base and growth magic resistance based on excel
    double baseMagicResistance = 31;
    double growthMagicResistance = 1.15;

    // setting base and growth movement speed base on excel
    double baseMovementSpeed = 340;
    double growthMovementSpeed = 0;

    // setting base and growth range on excel
    double baseRange = 555;
    double growthRange = 0;

    // creating variables to store current levels of each statistic
    double currentHealth;
    double currentHealthRegen;
    double currentAD;
    double currentAS;
    double currentArmor;
    double currentMagicResistance;
    double currentMovementSpeed;
    double currentRange;
    double currentLevel;
    double currentXP;

    //defining constructor
    public Misa()
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

    }

    //defining function to check champion_level
    public void CheckLevel()
    {
        if (currentXP >= Globals.level2)
        {
            currentLevel = 2;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level3)
        {
            currentLevel = 3;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level4)
        {
            currentLevel = 4;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level5)
        {
            currentLevel = 5;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level6)
        {
            currentLevel = 6;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level7)
        {
            currentLevel = 7;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level8)
        {
            currentLevel = 8;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level9)
        {
            currentLevel = 9;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level10)
        {
            currentLevel = 10;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level11)
        {
            currentLevel = 11;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level12)
        {
            currentLevel = 12;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level13)
        {
            currentLevel = 13;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level14)
        {
            currentLevel = 14;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level15)
        {
            currentLevel = 15;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level16)
        {
            currentLevel = 16;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level17)
        {
            currentLevel = 17;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level18)
        {
            currentLevel = 18;
            UpdateStatistics();
        }
    }

    public void UpdateStatistics()
    {
        currentHealth = getStatistic(baseHealth, growthHealth, currentLevel);
        currentHealthRegen = getStatistic(baseHealthRegen, growthHealthRegen, currentLevel);
        currentAD = getStatistic(baseAD, growthAD, currentLevel);
        currentAS = getStatistic(baseAS, growthAS, currentLevel);
        currentArmor = getStatistic(baseArmor, growthAD, currentLevel);
        currentMagicResistance = getStatistic(baseMagicResistance, growthMagicResistance, currentLevel);
        currentRange = getStatistic(baseRange, growthRange, currentLevel);
    }

    public void DisplayStats()
    {
        Debug.Log("Champion Name: " + championName + "\n "
            + "Current Health: " + currentHealth + ";"
            + "Current AD: " + currentAD + "\n ");
    }

}
public class Udara : Champions
{
    string championName = "Udara";
    string summonerName = "";

    // setting base and growth health based on excel
    double baseHealth = 618;
    double growthHealth = 107;

    // setting base and growth health regen based on excel
    double baseHealthRegen = 6.3;
    double growthHealthRegen = 0.5;

    // setting base and growth AD based on excel
    double baseAD = 55.5;
    double growthAD = 3.25;

    // setting base and growth AS based on excel
    double baseAS = 0.641;
    double growthAS = 0.0179;

    // setting base and growth armor based on excel
    double baseArmor = 26;
    double growthArmor = 4.7;

    // setting base and growth magic resistance based on excel
    double baseMagicResistance = 30.5;
    double growthMagicResistance = 1.2;

    // setting base and growth movement speed base on excel
    double baseMovementSpeed = 340;
    double growthMovementSpeed = 0;

    // setting base and growth range on excel
    double baseRange = 440;
    double growthRange = 0;

    // creating variables to store current levels of each statistic
    double currentHealth;
    double currentHealthRegen;
    double currentAD;
    double currentAS;
    double currentArmor;
    double currentMagicResistance;
    double currentMovementSpeed;
    double currentRange;
    double currentLevel;
    double currentXP;

    //defining constructor
    public Udara()
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

    }

    //defining function to check champion_level
    public void CheckLevel()
    {
        if (currentXP >= Globals.level2)
        {
            currentLevel = 2;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level3)
        {
            currentLevel = 3;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level4)
        {
            currentLevel = 4;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level5)
        {
            currentLevel = 5;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level6)
        {
            currentLevel = 6;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level7)
        {
            currentLevel = 7;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level8)
        {
            currentLevel = 8;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level9)
        {
            currentLevel = 9;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level10)
        {
            currentLevel = 10;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level11)
        {
            currentLevel = 11;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level12)
        {
            currentLevel = 12;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level13)
        {
            currentLevel = 13;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level14)
        {
            currentLevel = 14;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level15)
        {
            currentLevel = 15;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level16)
        {
            currentLevel = 16;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level17)
        {
            currentLevel = 17;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level18)
        {
            currentLevel = 18;
            UpdateStatistics();
        }
    }

    public void UpdateStatistics()
    {
        currentHealth = getStatistic(baseHealth, growthHealth, currentLevel);
        currentHealthRegen = getStatistic(baseHealthRegen, growthHealthRegen, currentLevel);
        currentAD = getStatistic(baseAD, growthAD, currentLevel);
        currentAS = getStatistic(baseAS, growthAS, currentLevel);
        currentArmor = getStatistic(baseArmor, growthAD, currentLevel);
        currentMagicResistance = getStatistic(baseMagicResistance, growthMagicResistance, currentLevel);
        currentRange = getStatistic(baseRange, growthRange, currentLevel);
    }

    public void DisplayStats()
    {
        Debug.Log("Champion Name: " + championName + "\n "
            + "Current Health: " + currentHealth + ";"
            + "Current AD: " + currentAD + "\n ");
    }
}
public class Sura : Champions
{
    string championName = "Sura";
    string summonerName = "";

    // setting base and growth health based on excel
    double baseHealth = 660;
    double growthHealth = 112;

    // setting base and growth health regen based on excel
    double baseHealthRegen = 2;
    double growthHealthRegen = 1.5;

    // setting base and growth AD based on excel
    double baseAD = 61;
    double growthAD = 4.8;

    // setting base and growth AS based on excel
    double baseAS = 0.65;
    double growthAS = 0.028;

    // setting base and growth armor based on excel
    double baseArmor = 36;
    double growthArmor = 4.6;

    // setting base and growth magic resistance based on excel
    double baseMagicResistance = 32;
    double growthMagicResistance = 2.1;

    // setting base and growth movement speed base on excel
    double baseMovementSpeed = 350;
    double growthMovementSpeed = 0;

    // setting base and growth range on excel
    double baseRange = 170;
    double growthRange = 0;

    // creating variables to store current levels of each statistic
    double currentHealth;
    double currentHealthRegen;
    double currentAD;
    double currentAS;
    double currentArmor;
    double currentMagicResistance;
    double currentMovementSpeed;
    double currentRange;
    double currentLevel;
    double currentXP;

    //defining constructor
    public Sura()
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

    }

    //defining function to check champion_level
    public void CheckLevel()
    {
        if (currentXP >= Globals.level2)
        {
            currentLevel = 2;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level3)
        {
            currentLevel = 3;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level4)
        {
            currentLevel = 4;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level5)
        {
            currentLevel = 5;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level6)
        {
            currentLevel = 6;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level7)
        {
            currentLevel = 7;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level8)
        {
            currentLevel = 8;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level9)
        {
            currentLevel = 9;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level10)
        {
            currentLevel = 10;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level11)
        {
            currentLevel = 11;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level12)
        {
            currentLevel = 12;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level13)
        {
            currentLevel = 13;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level14)
        {
            currentLevel = 14;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level15)
        {
            currentLevel = 15;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level16)
        {
            currentLevel = 16;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level17)
        {
            currentLevel = 17;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level18)
        {
            currentLevel = 18;
            UpdateStatistics();
        }
    }

    public void UpdateStatistics()
    {
        currentHealth = getStatistic(baseHealth, growthHealth, currentLevel);
        currentHealthRegen = getStatistic(baseHealthRegen, growthHealthRegen, currentLevel);
        currentAD = getStatistic(baseAD, growthAD, currentLevel);
        currentAS = getStatistic(baseAS, growthAS, currentLevel);
        currentArmor = getStatistic(baseArmor, growthAD, currentLevel);
        currentMagicResistance = getStatistic(baseMagicResistance, growthMagicResistance, currentLevel);
        currentRange = getStatistic(baseRange, growthRange, currentLevel);
    }
    public void DisplayStats()
    {
        Debug.Log("Champion Name: " + championName + "\n "
            + "Current Health: " + currentHealth + ";"
            + "Current AD: " + currentAD + "\n ");
    }

}
public class Hakka : Champions
{
    string championName = "Hakka";
    string summonerName = "";

    // setting base and growth health based on excel
    double baseHealth = 645;
    double growthHealth = 101;

    // setting base and growth health regen based on excel
    double baseHealthRegen = 6.7;
    double growthHealthRegen = 0.5;

    // setting base and growth AD based on excel
    double baseAD = 55;
    double growthAD = 3.4;

    // setting base and growth AS based on excel
    double baseAS = 0.657;
    double growthAS = 0.026;

    // setting base and growth armor based on excel
    double baseArmor = 41;
    double growthArmor = 4.9;

    // setting base and growth magic resistance based on excel
    double baseMagicResistance = 31;
    double growthMagicResistance = 2.1;

    // setting base and growth movement speed base on excel
    double baseMovementSpeed = 330;
    double growthMovementSpeed = 0;

    // setting base and growth range on excel
    double baseRange = 180;
    double growthRange = 0;

    // creating variables to store current levels of each statistic
    double currentHealth;
    double currentHealthRegen;
    double currentAD;
    double currentAS;
    double currentArmor;
    double currentMagicResistance;
    double currentMovementSpeed;
    double currentRange;
    double currentLevel;
    double currentXP;

    //defining constructor
    public Hakka()
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

    }

    //defining function to check champion_level
    public void CheckLevel()
    {
        if (currentXP >= Globals.level2)
        {
            currentLevel = 2;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level3)
        {
            currentLevel = 3;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level4)
        {
            currentLevel = 4;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level5)
        {
            currentLevel = 5;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level6)
        {
            currentLevel = 6;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level7)
        {
            currentLevel = 7;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level8)
        {
            currentLevel = 8;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level9)
        {
            currentLevel = 9;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level10)
        {
            currentLevel = 10;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level11)
        {
            currentLevel = 11;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level12)
        {
            currentLevel = 12;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level13)
        {
            currentLevel = 13;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level14)
        {
            currentLevel = 14;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level15)
        {
            currentLevel = 15;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level16)
        {
            currentLevel = 16;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level17)
        {
            currentLevel = 17;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level18)
        {
            currentLevel = 18;
            UpdateStatistics();
        }
    }

    public void UpdateStatistics()
    {
        currentHealth = getStatistic(baseHealth, growthHealth, currentLevel);
        currentHealthRegen = getStatistic(baseHealthRegen, growthHealthRegen, currentLevel);
        currentAD = getStatistic(baseAD, growthAD, currentLevel);
        currentAS = getStatistic(baseAS, growthAS, currentLevel);
        currentArmor = getStatistic(baseArmor, growthAD, currentLevel);
        currentMagicResistance = getStatistic(baseMagicResistance, growthMagicResistance, currentLevel);
        currentRange = getStatistic(baseRange, growthRange, currentLevel);
    }

    public void DisplayStats()
    {
        Debug.Log("Champion Name: " + championName + "\n "
            + "Current Health: " + currentHealth + ";"
            + "Current AD: " + currentAD + "\n ");
    }
}
public class Dira : Champions
{
    string championName = "Dira";
    string summonerName = "";

    // setting base and growth health based on excel
    double baseHealth = 590;
    double growthHealth = 103;

    // setting base and growth health regen based on excel
    double baseHealthRegen = 6.5;
    double growthHealthRegen = 0.55;

    // setting base and growth AD based on excel
    double baseAD = 54.5;
    double growthAD = 3;

    // setting base and growth AS based on excel
    double baseAS = 0.63;
    double growthAS = 0.021;

    // setting base and growth armor based on excel
    double baseArmor = 27;
    double growthArmor = 4.4;

    // setting base and growth magic resistance based on excel
    double baseMagicResistance = 30.5;
    double growthMagicResistance = 1.7;

    // setting base and growth movement speed base on excel
    double baseMovementSpeed = 330;
    double growthMovementSpeed = 0;

    // setting base and growth range on excel
    double baseRange = 535;
    double growthRange = 0;

    // creating variables to store current levels of each statistic
    double currentHealth;
    double currentHealthRegen;
    double currentAD;
    double currentAS;
    double currentArmor;
    double currentMagicResistance;
    double currentMovementSpeed;
    double currentRange;
    double currentLevel;
    double currentXP;

    //defining constructor
    public Dira()
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

    }

    //defining function to check champion_level
    public void CheckLevel()
    {
        if (currentXP >= Globals.level2)
        {
            currentLevel = 2;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level3)
        {
            currentLevel = 3;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level4)
        {
            currentLevel = 4;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level5)
        {
            currentLevel = 5;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level6)
        {
            currentLevel = 6;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level7)
        {
            currentLevel = 7;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level8)
        {
            currentLevel = 8;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level9)
        {
            currentLevel = 9;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level10)
        {
            currentLevel = 10;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level11)
        {
            currentLevel = 11;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level12)
        {
            currentLevel = 12;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level13)
        {
            currentLevel = 13;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level14)
        {
            currentLevel = 14;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level15)
        {
            currentLevel = 15;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level16)
        {
            currentLevel = 16;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level17)
        {
            currentLevel = 17;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level18)
        {
            currentLevel = 18;
            UpdateStatistics();
        }
    }

    public void UpdateStatistics()
    {
        currentHealth = getStatistic(baseHealth, growthHealth, currentLevel);
        currentHealthRegen = getStatistic(baseHealthRegen, growthHealthRegen, currentLevel);
        currentAD = getStatistic(baseAD, growthAD, currentLevel);
        currentAS = getStatistic(baseAS, growthAS, currentLevel);
        currentArmor = getStatistic(baseArmor, growthAD, currentLevel);
        currentMagicResistance = getStatistic(baseMagicResistance, growthMagicResistance, currentLevel);
        currentRange = getStatistic(baseRange, growthRange, currentLevel);
    }

    public void DisplayStats()
    {
        Debug.Log("Champion Name: " + championName + "\n "
            + "Current Health: " + currentHealth + ";"
            + "Current AD: " + currentAD + "\n ");
    }
}
public class Tapani : Champions
{
    string championName = "Tapani";
    string summonerName = "";

    // setting base and growth health based on excel
    double baseHealth = 590;
    double growthHealth = 102;

    // setting base and growth health regen based on excel
    double baseHealthRegen = 6.6;
    double growthHealthRegen = 0.89;

    // setting base and growth AD based on excel
    double baseAD = 61;
    double growthAD = 2.9;

    // setting base and growth AS based on excel
    double baseAS = 0.7;
    double growthAS = 0.0352;

    // setting base and growth armor based on excel
    double baseArmor = 29;
    double growthArmor = 4.5;

    // setting base and growth magic resistance based on excel
    double baseMagicResistance = 32;
    double growthMagicResistance = 2;

    // setting base and growth movement speed base on excel
    double baseMovementSpeed = 340;
    double growthMovementSpeed = 0;

    // setting base and growth range on excel
    double baseRange = 180;
    double growthRange = 0;

    // creating variables to store current levels of each statistic
    double currentHealth;
    double currentHealthRegen;
    double currentAD;
    double currentAS;
    double currentArmor;
    double currentMagicResistance;
    double currentMovementSpeed;
    double currentRange;
    double currentLevel;
    double currentXP;

    //defining constructor
    public Tapani()
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

    }

    //defining function to check champion_level
    public void CheckLevel()
    {
        if (currentXP >= Globals.level2)
        {
            currentLevel = 2;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level3)
        {
            currentLevel = 3;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level4)
        {
            currentLevel = 4;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level5)
        {
            currentLevel = 5;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level6)
        {
            currentLevel = 6;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level7)
        {
            currentLevel = 7;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level8)
        {
            currentLevel = 8;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level9)
        {
            currentLevel = 9;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level10)
        {
            currentLevel = 10;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level11)
        {
            currentLevel = 11;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level12)
        {
            currentLevel = 12;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level13)
        {
            currentLevel = 13;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level14)
        {
            currentLevel = 14;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level15)
        {
            currentLevel = 15;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level16)
        {
            currentLevel = 16;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level17)
        {
            currentLevel = 17;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level18)
        {
            currentLevel = 18;
            UpdateStatistics();
        }
    }

    public void UpdateStatistics()
    {
        currentHealth = getStatistic(baseHealth, growthHealth, currentLevel);
        currentHealthRegen = getStatistic(baseHealthRegen, growthHealthRegen, currentLevel);
        currentAD = getStatistic(baseAD, growthAD, currentLevel);
        currentAS = getStatistic(baseAS, growthAS, currentLevel);
        currentArmor = getStatistic(baseArmor, growthAD, currentLevel);
        currentMagicResistance = getStatistic(baseMagicResistance, growthMagicResistance, currentLevel);
        currentRange = getStatistic(baseRange, growthRange, currentLevel);
    }

    public void DisplayStats()
    {
        Debug.Log("Champion Name: " + championName + "\n "
            + "Current Health: " + currentHealth + ";"
            + "Current AD: " + currentAD + "\n ");
    }
}
public class Moorg : Champions
{
    string championName = "Moorg";
    string summonerName = "";

    // setting base and growth health based on excel
    double baseHealth = 645;
    double growthHealth = 100;

    // setting base and growth health regen based on excel
    double baseHealthRegen = 6.8;
    double growthHealthRegen = 0.87;

    // setting base and growth AD based on excel
    double baseAD = 52;
    double growthAD = 2.1;

    // setting base and growth AS based on excel
    double baseAS = 0.645;
    double growthAS = 0.034;

    // setting base and growth armor based on excel
    double baseArmor = 28;
    double growthArmor = 4.55;

    // setting base and growth magic resistance based on excel
    double baseMagicResistance = 31;
    double growthMagicResistance = 2.02;

    // setting base and growth movement speed base on excel
    double baseMovementSpeed = 335;
    double growthMovementSpeed = 0;

    // setting base and growth range on excel
    double baseRange = 470;
    double growthRange = 0;

    // creating variables to store current levels of each statistic
    double currentHealth;
    double currentHealthRegen;
    double currentAD;
    double currentAS;
    double currentArmor;
    double currentMagicResistance;
    double currentMovementSpeed;
    double currentRange;
    double currentLevel;
    double currentXP;

    //defining constructor
    public Moorg()
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

    }

    //defining function to check champion_level
    public void CheckLevel()
    {
        if (currentXP >= Globals.level2)
        {
            currentLevel = 2;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level3)
        {
            currentLevel = 3;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level4)
        {
            currentLevel = 4;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level5)
        {
            currentLevel = 5;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level6)
        {
            currentLevel = 6;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level7)
        {
            currentLevel = 7;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level8)
        {
            currentLevel = 8;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level9)
        {
            currentLevel = 9;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level10)
        {
            currentLevel = 10;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level11)
        {
            currentLevel = 11;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level12)
        {
            currentLevel = 12;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level13)
        {
            currentLevel = 13;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level14)
        {
            currentLevel = 14;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level15)
        {
            currentLevel = 15;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level16)
        {
            currentLevel = 16;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level17)
        {
            currentLevel = 17;
            UpdateStatistics();
        }
        else if (currentXP >= Globals.level18)
        {
            currentLevel = 18;
            UpdateStatistics();
        }
    }

    public void UpdateStatistics()
    {
        currentHealth = getStatistic(baseHealth, growthHealth, currentLevel);
        currentHealthRegen = getStatistic(baseHealthRegen, growthHealthRegen, currentLevel);
        currentAD = getStatistic(baseAD, growthAD, currentLevel);
        currentAS = getStatistic(baseAS, growthAS, currentLevel);
        currentArmor = getStatistic(baseArmor, growthAD, currentLevel);
        currentMagicResistance = getStatistic(baseMagicResistance, growthMagicResistance, currentLevel);
        currentRange = getStatistic(baseRange, growthRange, currentLevel);
    }
    public void DisplayStats()
    {
        Debug.Log("Champion Name: " + championName + "\n "
            + "Current Health: " + currentHealth + ";"
            + "Current AD: " + currentAD + "\n ");
    }

}