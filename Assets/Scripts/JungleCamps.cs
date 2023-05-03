using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class Camps
{
	protected static double getStatistic(double baseStatistic, double growthStatistic)
	{
		double return_value = baseStatistic + growthStatistic;
		return return_value;
	}
}
public class MurkWolf : Camps
{
	string CampName = "MurkWolf";

	// setting base and growth health based on excel
	double baseHealth = 2160;
	double growthHealth = 572;

	// setting base and growth AD based on excel
	double baseAD = 55;
	Dictionary<string, double> growthAD= new Dictionary<string, double>
	{
		{"level3", 11},
		{"level5", 6},
		{"level7", 5},
		{"level8", 6},
		{"level9", 6},
		{"level11", 16},
		{"level12", 11},
		{"level13", 11},
		{"level14", 6},
		{"level15", 11},
		{"level16", 11},
		{"level17", 11}
	};
	Dictionary<string, double> growthXP = new Dictionary<string, double>();

	//TODO - include growthXP

	// setting base AS based on excel
	double baseAS = 0.625;

	// setting base MR based on excel
	double baseMR = 20;

	// setting base AR based on excel
	double baseAR = 20;

	// setting base MS based on excel
	double baseMS = 525;

	// setting base Range based on excel
	double baseRange = 175;

	// setting base Leash based on excel
	double baseLeash = 800;

	// setting base Gold based on excel
	double baseGold = 81;

	// setting base XP and growth XP based on excel
	double baseXP = 80;

	//TODO
	/*Dictionary growthXP = new Dictionary();
	growthXP.add("level3", 2.01);
	growthXP.add("level4", 4);
	growthXP.add("level5", 9.99);
	growthXP.add("level7", 8);
	growthXP.add("level9", 12);*/

	// creating variables to store current level of each statistic
	double currentHealth;
	double currentAD;
	double currentAS;
	double currentAR;
	double currentMR;
	double currentMS;
	double currentRange;
	double currentLevel;
	double currentXP;
	double currentLeash;
	double currentGold;
	double AverageChampLevel;

	// defining constructor
	public MurkWolf()
	{
		currentHealth = baseHealth;
		currentAD = baseAD;
		currentAS = baseAS;
		currentMS = baseMS;
		currentRange = baseRange;
		currentMR = baseMR;
		currentAR = baseAR;
		currentLeash = baseLeash;
		currentGold = baseGold;
		currentXP = baseXP;
		currentLevel = 0;
	}

	// defining function to check level
	public void CheckLevel()
	{
		if (AverageChampLevel < 2 && AverageChampLevel >= 1)
		{
			currentLevel = 1;
		}

		else if (AverageChampLevel < 3 && AverageChampLevel >= 2)
		{
			currentLevel = 2;
		}

		else if (AverageChampLevel < 4 && AverageChampLevel >= 3)
		{
			currentLevel = 3;
			UpdateHealth();
			UpdateXPLevel3();
			UpdateADLevel3();
		}

		else if (AverageChampLevel < 5 && AverageChampLevel >= 4)
		{
			currentLevel = 4;
			UpdateXPLevel4();
		}

		else if (AverageChampLevel < 6 && AverageChampLevel >= 5)
		{
			currentLevel = 5;
			UpdateHealth();
			UpdateADLevel5();
		}

		else if (AverageChampLevel < 7 && AverageChampLevel >= 6)
		{
			currentLevel = 6;
		}

		else if (AverageChampLevel < 8 && AverageChampLevel >= 7)
		{
			currentLevel = 7;
			UpdateHealth();
			UpdateXPLevel7();
			UpdateADLevel7();
		}

		else if (AverageChampLevel < 9 && AverageChampLevel >= 8)
		{
			currentLevel = 8;
			UpdateADLevel8();
		}

		else if (AverageChampLevel < 10 && AverageChampLevel >= 9)
		{
			currentLevel = 9;
			UpdateHealth();
			UpdateXPLevel9();
			UpdateADLevel9();
		}

		else if (AverageChampLevel < 11 && AverageChampLevel >= 10)
		{
			currentLevel = 10;
		}

		else if (AverageChampLevel < 12 && AverageChampLevel >= 11)
		{
			currentLevel = 11;
			UpdateHealth();
			UpdateADLevel11();
		}

		else if (AverageChampLevel < 13 && AverageChampLevel >= 12)
		{
			currentLevel = 12;
			UpdateADLevel12();
		}

		else if (AverageChampLevel < 14 && AverageChampLevel >= 13)
		{
			currentLevel = 13;
			UpdateADLevel13();
		}

		else if (AverageChampLevel < 15 && AverageChampLevel >= 14)
		{
			currentLevel = 14;
			UpdateADLevel14();
		}

		else if (AverageChampLevel < 16 && AverageChampLevel >= 15)
		{
			currentLevel = 15;
			UpdateADLevel15();
		}

		else if (AverageChampLevel < 17 && AverageChampLevel >= 16)
		{
			currentLevel = 16;
			UpdateADLevel16();
		}

		else if (AverageChampLevel < 18 && AverageChampLevel >= 17)
		{
			currentLevel = 17;
			UpdateADLevel17();
		}
	}

	public void UpdateHealth()
	{
		currentHealth = getStatistic(currentHealth, growthHealth);
	}

	public void UpdateXPLevel3()
	{
		currentXP = getStatistic(currentXP, growthXP["level3"]);
	}

	public void UpdateXPLevel4()
	{
		currentXP = getStatistic(currentXP, growthXP["level4"]);
	}

	public void UpdateXPLevel5()
	{
		currentXP = getStatistic(currentXP, growthXP["level5"]);
	}

	public void UpdateXPLevel7()
	{
		currentXP = getStatistic(currentXP, growthXP["level7"]);
	}

	public void UpdateXPLevel9()
	{
		currentXP = getStatistic(currentXP, growthXP["level9"]);
	}

	public void UpdateADLevel3()
	{
		currentAD = getStatistic(currentAD, growthAD["level3"]);
	}

	public void UpdateADLevel5()
	{
		currentAD = getStatistic(currentAD, growthAD["level5"]);
	}

	public void UpdateADLevel7()
	{
		currentAD = getStatistic(currentAD, growthAD["level7"]);
	}

	public void UpdateADLevel8()
	{
		currentAD = getStatistic(currentAD, growthAD["level8"]);
	}

	public void UpdateADLevel9()
	{
		currentAD = getStatistic(currentAD, growthAD["level9"]);
	}

	public void UpdateADLevel11()
	{
		currentAD = getStatistic(currentAD, growthAD["level11"]);
	}

	public void UpdateADLevel12()
	{
		currentAD = getStatistic(currentAD, growthAD["level12"]);
	}

	public void UpdateADLevel13()
	{
		currentAD = getStatistic(currentAD, growthAD["level13"]);
	}

	public void UpdateADLevel14()
	{
		currentAD = getStatistic(currentAD, growthAD["level14"]);
	}

	public void UpdateADLevel15()
	{
		currentAD = getStatistic(currentAD, growthAD["level15"]);
	}

	public void UpdateADLevel16()
	{
		currentAD = getStatistic(currentAD, growthAD["level16"]);
	}

	public void UpdateADLevel17()
	{
		currentAD = getStatistic(currentAD, growthAD["level17"]);
	}

	public void DisplayStats()
    {
		Debug.Log("Camp Name:" + CampName + "\n " +
			"Current Health:" + currentHealth + "\n " +
			"Current AD:" + currentAD + "\n " +
			"Current AS:" + currentAS + "\n " +
			"Current Range:" + currentRange + "\n " +
			"Current Leash:" + currentLeash + "\n " +
			"Current Level:" + currentLevel + "\n ");
    }

}

public class Raptor : Camps
{
	string CampName = "Raptor";
	Dictionary<string, double> growthAD = new Dictionary<string, double>();
	Dictionary<string, double> growthXP = new Dictionary<string, double>();
	// setting base and growth health based on excel
	double baseHealth = 2925;
	double growthHealth = 720;

	// setting base and growth AD based on excel
	double baseAD = 70;

	//TODO
	/*Dictionary growthAD = new Dictionary();
	growthAD.add("level3", 14);
	growthAD.add("level5", 7);
	growthAD.add("level7", 7);
	growthAD.add("level8", 7);
	growthAD.add("level9", 21);
	growthAD.add("level11", 14);
	growthAD.add("level12", 14);
	growthAD.add("level13", 7);
	growthAD.add("level14", 14);
	growthAD.add("level15", 7);
	growthAD.add("level16", 14);
	growthAD.add("level17", 14);*/

	// setting base AS based on excel
	double baseAS = 0.875;

	// setting base MR based on excel
	double baseMR = 20;

	// setting base AR based on excel
	double baseAR = 20;

	// setting base MS based on excel
	double baseMS = 475;

	// setting base Range based on excel
	double baseRange = 200;

	// setting base Leash based on excel
	double baseLeash = 800;

	// setting base Gold based on excel
	double baseGold = 75;

	// setting base XP and growth XP based on excel
	double baseXP = 70;
	/*Dictionary growthXP = new Dictionary();
	growthXP.add("level3", 1.75);
	growthXP.add("level4", 3.5);
	growthXP.add("level5", 8.75);
	growthXP.add("level7", 7);
	growthXP.add("level9", 10.5);*/

	// creating variables to store current level of each statistic
	double currentHealth;
	double currentAD;
	double currentAS;
	double currentAR;
	double currentMR;
	double currentMS;
	double currentRange;
	double currentLevel;
	double currentXP;
	double currentLeash;
	double currentGold;
	double AverageChampLevel;

	// defining constructor
	public Raptor()
	{
		currentHealth = baseHealth;
		currentAD = baseAD;
		currentAS = baseAS;
		currentMS = baseMS;
		currentRange = baseRange;
		currentMR = baseMR;
		currentAR = baseAR;
		currentLeash = baseLeash;
		currentGold = baseGold;
		currentXP = baseXP;
		currentLevel = 0;
	}

	// defining function to check level
	public void CheckLevel()
	{
		if (AverageChampLevel < 2 && AverageChampLevel >= 1)
		{
			currentLevel = 1;
		}

		else if (AverageChampLevel < 3 && AverageChampLevel >= 2)
		{
			currentLevel = 2;
		}

		else if (AverageChampLevel < 4 && AverageChampLevel >= 3)
		{
			currentLevel = 3;
			UpdateHealth();
			UpdateXPLevel3();
			UpdateADLevel3();
		}

		else if (AverageChampLevel < 5 && AverageChampLevel >= 4)
		{
			currentLevel = 4;
			UpdateXPLevel4();
		}

		else if (AverageChampLevel < 6 && AverageChampLevel >= 5)
		{
			currentLevel = 5;
			UpdateHealth();
			UpdateADLevel5();
		}

		else if (AverageChampLevel < 7 && AverageChampLevel >= 6)
		{
			currentLevel = 6;
		}

		else if (AverageChampLevel < 8 && AverageChampLevel >= 7)
		{
			currentLevel = 7;
			UpdateHealth();
			UpdateXPLevel7();
			UpdateADLevel7();
		}

		else if (AverageChampLevel < 9 && AverageChampLevel >= 8)
		{
			currentLevel = 8;
			UpdateADLevel8();
		}

		else if (AverageChampLevel < 10 && AverageChampLevel >= 9)
		{
			currentLevel = 9;
			UpdateHealth();
			UpdateXPLevel9();
			UpdateADLevel9();
		}

		else if (AverageChampLevel < 11 && AverageChampLevel >= 10)
		{
			currentLevel = 10;
		}

		else if (AverageChampLevel < 12 && AverageChampLevel >= 11)
		{
			currentLevel = 11;
			UpdateHealth();
			UpdateADLevel11();
		}

		else if (AverageChampLevel < 13 && AverageChampLevel >= 12)
		{
			currentLevel = 12;
			UpdateADLevel12();
		}

		else if (AverageChampLevel < 14 && AverageChampLevel >= 13)
		{
			currentLevel = 13;
			UpdateADLevel13();
		}

		else if (AverageChampLevel < 15 && AverageChampLevel >= 14)
		{
			currentLevel = 14;
			UpdateADLevel14();
		}

		else if (AverageChampLevel < 16 && AverageChampLevel >= 15)
		{
			currentLevel = 15;
			UpdateADLevel15();
		}

		else if (AverageChampLevel < 17 && AverageChampLevel >= 16)
		{
			currentLevel = 16;
			UpdateADLevel16();
		}

		else if (AverageChampLevel < 18 && AverageChampLevel >= 17)
		{
			currentLevel = 17;
			UpdateADLevel17();
		}
	}

	public void UpdateHealth()
	{
		currentHealth = getStatistic(currentHealth, growthHealth);
	}

	public void UpdateXPLevel3()
	{
		currentXP = getStatistic(currentXP, growthXP["level3"]);
	}

	public void UpdateXPLevel4()
	{
		currentXP = getStatistic(currentXP, growthXP["level4"]);
	}

	public void UpdateXPLevel5()
	{
		currentXP = getStatistic(currentXP, growthXP["level5"]);
	}

	public void UpdateXPLevel7()
	{
		currentXP = getStatistic(currentXP, growthXP["level7"]);
	}

	public void UpdateXPLevel9()
	{
		currentXP = getStatistic(currentXP, growthXP["level9"]);
	}

	public void UpdateADLevel3()
	{
		currentAD = getStatistic(currentAD, growthAD["level3"]);
	}

	public void UpdateADLevel5()
	{
		currentAD = getStatistic(currentAD, growthAD["level5"]);
	}

	public void UpdateADLevel7()
	{
		currentAD = getStatistic(currentAD, growthAD["level7"]);
	}

	public void UpdateADLevel8()
	{
		currentAD = getStatistic(currentAD, growthAD["level8"]);
	}

	public void UpdateADLevel9()
	{
		currentAD = getStatistic(currentAD, growthAD["level9"]);
	}

	public void UpdateADLevel11()
	{
		currentAD = getStatistic(currentAD, growthAD["level11"]);
	}

	public void UpdateADLevel12()
	{
		currentAD = getStatistic(currentAD, growthAD["level12"]);
	}

	public void UpdateADLevel13()
	{
		currentAD = getStatistic(currentAD, growthAD["level13"]);
	}

	public void UpdateADLevel14()
	{
		currentAD = getStatistic(currentAD, growthAD["level14"]);
	}

	public void UpdateADLevel15()
	{
		currentAD = getStatistic(currentAD, growthAD["level15"]);
	}

	public void UpdateADLevel16()
	{
		currentAD = getStatistic(currentAD, growthAD["level16"]);
	}

	public void UpdateADLevel17()
	{
		currentAD = getStatistic(currentAD, growthAD["level17"]);
	}
	public void DisplayStats()
	{
		Debug.Log("Camp Name:" + CampName + "\n " +
			"Current Health:" + currentHealth + "\n " +
			"Current AD:" + currentAD + "\n " +
			"Current AS:" + currentAS + "\n " +
			"Current Range:" + currentRange + "\n " +
			"Current Leash:" + currentLeash + "\n " +
			"Current Level:" + currentLevel + "\n ");
	}

}

public class Krug : Camps
{
	string CampName = "Krug";
	Dictionary<string, double> growthAD = new Dictionary<string, double>();
	Dictionary<string, double> growthXP = new Dictionary<string, double>();
	// setting base and growth health based on excel
	double baseHealth = 2010;
	double growthHealth = 472;

	// setting base and growth AD based on excel
	double baseAD = 172;
		
	//TODO
	/*Dictionary growthAD = new Dictionary();
	growthAD.add("level3", 19);
	growthAD.add("level5", 27);
	growthAD.add("level7", 16);
	growthAD.add("level8", 15);
	growthAD.add("level9", 42);
	growthAD.add("level11", 31);
	growthAD.add("level12", 37);
	growthAD.add("level13", 27);
	growthAD.add("level14", 25);
	growthAD.add("level15", 27);
	growthAD.add("level16", 25);
	growthAD.add("level17", 31);*/

	// setting base AS based on excel
	double baseAS = 0.613;

	// setting base MR based on excel
	double baseMR = 20;

	// setting base AR based on excel
	double baseAR = 20;

	// setting base MS based on excel
	double baseMS = 300;

	// setting base Range based on excel
	double baseRange = 175;

	// setting base Leash based on excel
	double baseLeash = 900;

	// setting base Gold based on excel
	double baseGold = 109;

	// setting base XP and growth XP based on excel
		
	double baseXP = 121;
		
	//TODO
	/*Dictionary growthXP = new Dictionary();
	growthXP.add("level3", 3.03);
	growthXP.add("level4", 6.05);
	growthXP.add("level5", 15.12);
	growthXP.add("level7", 12.1);
	growthXP.add("level9", 18.15);*/

	// creating variables to store current level of each statistic
	double currentHealth;
	double currentAD;
	double currentAS;
	double currentAR;
	double currentMR;
	double currentMS;
	double currentRange;
	double currentLevel;
	double currentXP;
	double currentLeash;
	double currentGold;
	double AverageChampLevel;

	// defining constructor
	public Krug()
	{
		currentHealth = baseHealth;
		currentAD = baseAD;
		currentAS = baseAS;
		currentMS = baseMS;
		currentRange = baseRange;
		currentMR = baseMR;
		currentAR = baseAR;
		currentLeash = baseLeash;
		currentGold = baseGold;
		currentXP = baseXP;
		currentLevel = 0;
	}

	// defining function to check level
	public void CheckLevel()
	{
		if (AverageChampLevel < 2 && AverageChampLevel >= 1)
		{
			currentLevel = 1;
		}

		else if (AverageChampLevel < 3 && AverageChampLevel >= 2)
		{
			currentLevel = 2;
		}

		else if (AverageChampLevel < 4 && AverageChampLevel >= 3)
		{
			currentLevel = 3;
			UpdateHealth();
			UpdateXPLevel3();
			UpdateADLevel3();
		}

		else if (AverageChampLevel < 5 && AverageChampLevel >= 4)
		{
			currentLevel = 4;
			UpdateXPLevel4();
		}

		else if (AverageChampLevel < 6 && AverageChampLevel >= 5)
		{
			currentLevel = 5;
			UpdateHealth();
			UpdateADLevel5();
		}

		else if (AverageChampLevel < 7 && AverageChampLevel >= 6)
		{
			currentLevel = 6;
		}

		else if (AverageChampLevel < 8 && AverageChampLevel >= 7)
		{
			currentLevel = 7;
			UpdateHealth();
			UpdateXPLevel7();
			UpdateADLevel7();
		}

		else if (AverageChampLevel < 9 && AverageChampLevel >= 8)
		{
			currentLevel = 8;
			UpdateADLevel8();
		}

		else if (AverageChampLevel < 10 && AverageChampLevel >= 9)
		{
			currentLevel = 9;
			UpdateHealth();
			UpdateXPLevel9();
			UpdateADLevel9();
		}

		else if (AverageChampLevel < 11 && AverageChampLevel >= 10)
		{
			currentLevel = 10;
		}

		else if (AverageChampLevel < 12 && AverageChampLevel >= 11)
		{
			currentLevel = 11;
			UpdateHealth();
			UpdateADLevel11();
		}

		else if (AverageChampLevel < 13 && AverageChampLevel >= 12)
		{
			currentLevel = 12;
			UpdateADLevel12();
		}

		else if (AverageChampLevel < 14 && AverageChampLevel >= 13)
		{
			currentLevel = 13;
			UpdateADLevel13();
		}

		else if (AverageChampLevel < 15 && AverageChampLevel >= 14)
		{
			currentLevel = 14;
			UpdateADLevel14();
		}

		else if (AverageChampLevel < 16 && AverageChampLevel >= 15)
		{
			currentLevel = 15;
			UpdateADLevel15();
		}

		else if (AverageChampLevel < 17 && AverageChampLevel >= 16)
		{
			currentLevel = 16;
			UpdateADLevel16();
		}

		else if (AverageChampLevel < 18 && AverageChampLevel >= 17)
		{
			currentLevel = 17;
			UpdateADLevel17();
		}
	}

	public void UpdateHealth()
	{
		currentHealth = getStatistic(currentHealth, growthHealth);
	}

	public void UpdateXPLevel3()
	{
		currentXP = getStatistic(currentXP, growthXP["level3"]);
	}

	public void UpdateXPLevel4()
	{
		currentXP = getStatistic(currentXP, growthXP["level4"]);
	}

	public void UpdateXPLevel5()
	{
		currentXP = getStatistic(currentXP, growthXP["level5"]);
	}

	public void UpdateXPLevel7()
	{
		currentXP = getStatistic(currentXP, growthXP["level7"]);
	}

	public void UpdateXPLevel9()
	{
		currentXP = getStatistic(currentXP, growthXP["level9"]);
	}

	public void UpdateADLevel3()
	{
		currentAD = getStatistic(currentAD, growthAD["level3"]);
	}

	public void UpdateADLevel5()
	{
		currentAD = getStatistic(currentAD, growthAD["level5"]);
	}

	public void UpdateADLevel7()
	{
		currentAD = getStatistic(currentAD, growthAD["level7"]);
	}

	public void UpdateADLevel8()
	{
		currentAD = getStatistic(currentAD, growthAD["level8"]);
	}

	public void UpdateADLevel9()
	{
		currentAD = getStatistic(currentAD, growthAD["level9"]);
	}

	public void UpdateADLevel11()
	{
		currentAD = getStatistic(currentAD, growthAD["level11"]);
	}

	public void UpdateADLevel12()
	{
		currentAD = getStatistic(currentAD, growthAD["level12"]);
	}

	public void UpdateADLevel13()
	{
		currentAD = getStatistic(currentAD, growthAD["level13"]);
	}

	public void UpdateADLevel14()
	{
		currentAD = getStatistic(currentAD, growthAD["level14"]);
	}

	public void UpdateADLevel15()
	{
		currentAD = getStatistic(currentAD, growthAD["level15"]);
	}

	public void UpdateADLevel16()
	{
		currentAD = getStatistic(currentAD, growthAD["level16"]);
	}

	public void UpdateADLevel17()
	{
		currentAD = getStatistic(currentAD, growthAD["level17"]);
	}
	public void DisplayStats()
	{
		Debug.Log("Camp Name:" + CampName + "\n " +
			"Current Health:" + currentHealth + "\n " +
			"Current AD:" + currentAD + "\n " +
			"Current AS:" + currentAS + "\n " +
			"Current Range:" + currentRange + "\n " +
			"Current Leash:" + currentLeash + "\n " +
			"Current Level:" + currentLevel + "\n ");
	}

}

public class Gromp : Camps
{
	string CampName = "Gromp";
	Dictionary<string, double> growthAD = new Dictionary<string, double>();
	Dictionary<string, double> growthXP = new Dictionary<string, double>();
	// setting base and growth health based on excel
	double baseHealth = 1650;
	double growthHealth = 410;

	// setting base and growth AD based on excel
	double baseAD = 74;
			
	//TODO
	/*Dictionary growthAD = new Dictionary();
	growthAD.add("level3", 15);
	growthAD.add("level5", 7);
	growthAD.add("level7", 8);
	growthAD.add("level8", 7);
	growthAD.add("level9", 22);
	growthAD.add("level11", 15);
	growthAD.add("level12", 15);
	growthAD.add("level13", 7);
	growthAD.add("level14", 15);
	growthAD.add("level15", 7);
	growthAD.add("level16", 15);
	growthAD.add("level17", 15);*/

	// setting base AS based on excel
	double baseAS = 0.425;

	// setting base MR based on excel
	double baseMR = 20;

	// setting base AR based on excel
	double baseAR = 20;

	// setting base MS based on excel
	double baseMS = 330;

	// setting base Range based on excel
	double baseRange = 250;

	// setting base Leash based on excel
	double baseLeash = 700;

	// setting base Gold based on excel
	double baseGold = 80;

	// setting base XP and growth XP based on excel
	double baseXP = 120;

	//TODO
	/*Dictionary growthXP = new Dictionary();
	growthXP.add("level3", 3);
	growthXP.add("level4", 6);
	growthXP.add("level5", 15);
	growthXP.add("level7", 12);
	growthXP.add("level9", 18);*/

	// creating variables to store current level of each statistic
	double currentHealth;
	double currentAD;
	double currentAS;
	double currentAR;
	double currentMR;
	double currentMS;
	double currentRange;
	double currentLevel;
	double currentXP;
	double currentLeash;
	double currentGold;
	double AverageChampLevel;

	// defining constructor
	public Gromp()
	{
		currentHealth = baseHealth;
		currentAD = baseAD;
		currentAS = baseAS;
		currentMS = baseMS;
		currentRange = baseRange;
		currentMR = baseMR;
		currentAR = baseAR;
		currentLeash = baseLeash;
		currentGold = baseGold;
		currentXP = baseXP;
		currentLevel = 0;
	}

	// defining function to check level
	public void CheckLevel()
	{
		if (AverageChampLevel < 2 && AverageChampLevel >= 1)
		{
			currentLevel = 1;
		}

		else if (AverageChampLevel < 3 && AverageChampLevel >= 2)
		{
			currentLevel = 2;
		}

		else if (AverageChampLevel < 4 && AverageChampLevel >= 3)
		{
			currentLevel = 3;
			UpdateHealth();
			UpdateXPLevel3();
			UpdateADLevel3();
		}

		else if (AverageChampLevel < 5 && AverageChampLevel >= 4)
		{
			currentLevel = 4;
			UpdateXPLevel4();
		}

		else if (AverageChampLevel < 6 && AverageChampLevel >= 5)
		{
			currentLevel = 5;
			UpdateHealth();
			UpdateADLevel5();
		}

		else if (AverageChampLevel < 7 && AverageChampLevel >= 6)
		{
			currentLevel = 6;
		}

		else if (AverageChampLevel < 8 && AverageChampLevel >= 7)
		{
			currentLevel = 7;
			UpdateHealth();
			UpdateXPLevel7();
			UpdateADLevel7();
		}

		else if (AverageChampLevel < 9 && AverageChampLevel >= 8)
		{
			currentLevel = 8;
			UpdateADLevel8();
		}

		else if (AverageChampLevel < 10 && AverageChampLevel >= 9)
		{
			currentLevel = 9;
			UpdateHealth();
			UpdateXPLevel9();
			UpdateADLevel9();
		}

		else if (AverageChampLevel < 11 && AverageChampLevel >= 10)
		{
			currentLevel = 10;
		}

		else if (AverageChampLevel < 12 && AverageChampLevel >= 11)
		{
			currentLevel = 11;
			UpdateHealth();
			UpdateADLevel11();
		}

		else if (AverageChampLevel < 13 && AverageChampLevel >= 12)
		{
			currentLevel = 12;
			UpdateADLevel12();
		}

		else if (AverageChampLevel < 14 && AverageChampLevel >= 13)
		{
			currentLevel = 13;
			UpdateADLevel13();
		}

		else if (AverageChampLevel < 15 && AverageChampLevel >= 14)
		{
			currentLevel = 14;
			UpdateADLevel14();
		}

		else if (AverageChampLevel < 16 && AverageChampLevel >= 15)
		{
			currentLevel = 15;
			UpdateADLevel15();
		}

		else if (AverageChampLevel < 17 && AverageChampLevel >= 16)
		{
			currentLevel = 16;
			UpdateADLevel16();
		}

		else if (AverageChampLevel < 18 && AverageChampLevel >= 17)
		{
			currentLevel = 17;
			UpdateADLevel17();
		}
	}

	public void UpdateHealth()
	{
		currentHealth = getStatistic(currentHealth, growthHealth);
	}

	public void UpdateXPLevel3()
	{
		currentXP = getStatistic(currentXP, growthXP["level3"]);
	}

	public void UpdateXPLevel4()
	{
		currentXP = getStatistic(currentXP, growthXP["level4"]);
	}

	public void UpdateXPLevel5()
	{
		currentXP = getStatistic(currentXP, growthXP["level5"]);
	}

	public void UpdateXPLevel7()
	{
		currentXP = getStatistic(currentXP, growthXP["level7"]);
	}

	public void UpdateXPLevel9()
	{
		currentXP = getStatistic(currentXP, growthXP["level9"]);
	}

	public void UpdateADLevel3()
	{
		currentAD = getStatistic(currentAD, growthAD["level3"]);
	}

	public void UpdateADLevel5()
	{
		currentAD = getStatistic(currentAD, growthAD["level5"]);
	}

	public void UpdateADLevel7()
	{
		currentAD = getStatistic(currentAD, growthAD["level7"]);
	}

	public void UpdateADLevel8()
	{
		currentAD = getStatistic(currentAD, growthAD["level8"]);
	}

	public void UpdateADLevel9()
	{
		currentAD = getStatistic(currentAD, growthAD["level9"]);
	}

	public void UpdateADLevel11()
	{
		currentAD = getStatistic(currentAD, growthAD["level11"]);
	}

	public void UpdateADLevel12()
	{
		currentAD = getStatistic(currentAD, growthAD["level12"]);
	}

	public void UpdateADLevel13()
	{
		currentAD = getStatistic(currentAD, growthAD["level13"]);
	}

	public void UpdateADLevel14()
	{
		currentAD = getStatistic(currentAD, growthAD["level14"]);
	}

	public void UpdateADLevel15()
	{
		currentAD = getStatistic(currentAD, growthAD["level15"]);
	}

	public void UpdateADLevel16()
	{
		currentAD = getStatistic(currentAD, growthAD["level16"]);
	}

	public void UpdateADLevel17()
	{
		currentAD = getStatistic(currentAD, growthAD["level17"]);
	}

	public void DisplayStats()
	{
		Debug.Log("Camp Name:" + CampName + "\n " +
			"Current Health:" + currentHealth + "\n " +
			"Current AD:" + currentAD + "\n " +
			"Current AS:" + currentAS + "\n " +
			"Current Range:" + currentRange + "\n " +
			"Current Leash:" + currentLeash + "\n " +
			"Current Level:" + currentLevel + "\n ");
	}

}

public class RedBrambleback : Camps
{
	string CampName = "RedBrambleback";
	Dictionary<string, double> growthAD = new Dictionary<string, double>();
	Dictionary<string, double> growthXP = new Dictionary<string, double>();
	// setting base and growth health based on excel
	double baseHealth = 1850;
		double growthHealth = 460;

		// setting base and growth AD based on excel
		double baseAD = 78;
		//TODO
		//Dictionary growthAD = new Dictionary();
		//growthAD.add("level3", 16);
		//growthAD.add("level5", 7);
		//growthAD.add("level7", 8);
		//growthAD.add("level8", 8);
		//growthAD.add("level9", 23);
		//growthAD.add("level11", 16);
		//growthAD.add("level12", 16);
		//growthAD.add("level13", 7);
		//growthAD.add("level14", 16);
		//growthAD.add("level15", 8);
		//growthAD.add("level16", 15);
		//growthAD.add("level17", 16);

		// setting base AS based on excel
		double baseAS = 0.493;

		// setting base MR based on excel
		double baseMR = 20;

		// setting base AR based on excel
		double baseAR = 20;

		// setting base MS based on excel
		double baseMS = 275;

		// setting base Range based on excel
		double baseRange = 225;

		// setting base Leash based on excel
		double baseLeash = 1000;

		// setting base Gold based on excel
		double baseGold = 90;

		// setting base XP and growth XP based on excel
		double baseXP = 95;

		//TODO
		//Dictionary growthXP = new Dictionary();
		//growthXP.add("level3", 2.38);
		//growthXP.add("level4", 4.75);
		//growthXP.add("level5", 11.87);
		//growthXP.add("level7", 9.5);
		//growthXP.add("level9", 14.25);

		// creating variables to store current level of each statistic
		double currentHealth;
		double currentAD;
		double currentAS;
		double currentAR;
		double currentMR;
		double currentMS;
		double currentRange;
		double currentLevel;
		double currentXP;
		double currentLeash;
		double currentGold;
		double AverageChampLevel;

		// defining constructor
		public RedBrambleback()
		{
			currentHealth = baseHealth;
			currentAD = baseAD;
			currentAS = baseAS;
			currentMS = baseMS;
			currentRange = baseRange;
			currentMR = baseMR;
			currentAR = baseAR;
			currentLeash = baseLeash;
			currentGold = baseGold;
			currentXP = baseXP;
			currentLevel = 0;
		}

		// defining function to check level
		public void CheckLevel()
		{
			if (AverageChampLevel < 2 && AverageChampLevel >= 1)
			{
				currentLevel = 1;
			}

			else if (AverageChampLevel < 3 && AverageChampLevel >= 2)
			{
				currentLevel = 2;
			}

			else if (AverageChampLevel < 4 && AverageChampLevel >= 3)
			{
				currentLevel = 3;
				UpdateHealth();
				UpdateXPLevel3();
				UpdateADLevel3();
			}

			else if (AverageChampLevel < 5 && AverageChampLevel >= 4)
			{
				currentLevel = 4;
				UpdateXPLevel4();
			}

			else if (AverageChampLevel < 6 && AverageChampLevel >= 5)
			{
				currentLevel = 5;
				UpdateHealth();
				UpdateADLevel5();
			}

			else if (AverageChampLevel < 7 && AverageChampLevel >= 6)
			{
				currentLevel = 6;
			}

			else if (AverageChampLevel < 8 && AverageChampLevel >= 7)
			{
				currentLevel = 7;
				UpdateHealth();
				UpdateXPLevel7();
				UpdateADLevel7();
			}

			else if (AverageChampLevel < 9 && AverageChampLevel >= 8)
			{
				currentLevel = 8;
				UpdateADLevel8();
			}

			else if (AverageChampLevel < 10 && AverageChampLevel >= 9)
			{
				currentLevel = 9;
				UpdateHealth();
				UpdateXPLevel9();
				UpdateADLevel9();
			}

			else if (AverageChampLevel < 11 && AverageChampLevel >= 10)
			{
				currentLevel = 10;
			}

			else if (AverageChampLevel < 12 && AverageChampLevel >= 11)
			{
				currentLevel = 11;
				UpdateHealth();
				UpdateADLevel11();
			}

			else if (AverageChampLevel < 13 && AverageChampLevel >= 12)
			{
				currentLevel = 12;
				UpdateADLevel12();
			}

			else if (AverageChampLevel < 14 && AverageChampLevel >= 13)
			{
				currentLevel = 13;
				UpdateADLevel13();
			}

			else if (AverageChampLevel < 15 && AverageChampLevel >= 14)
			{
				currentLevel = 14;
				UpdateADLevel14();
			}

			else if (AverageChampLevel < 16 && AverageChampLevel >= 15)
			{
				currentLevel = 15;
				UpdateADLevel15();
			}

			else if (AverageChampLevel < 17 && AverageChampLevel >= 16)
			{
				currentLevel = 16;
				UpdateADLevel16();
			}

			else if (AverageChampLevel < 18 && AverageChampLevel >= 17)
			{
				currentLevel = 17;
				UpdateADLevel17();
			}
		}

		public void UpdateHealth()
		{
			currentHealth = getStatistic(currentHealth, growthHealth);
		}

		public void UpdateXPLevel3()
		{
			currentXP = getStatistic(currentXP, growthXP["level3"]);
		}

		public void UpdateXPLevel4()
		{
			currentXP = getStatistic(currentXP, growthXP["level4"]);
		}

		public void UpdateXPLevel5()
		{
			currentXP = getStatistic(currentXP, growthXP["level5"]);
		}

		public void UpdateXPLevel7()
		{
			currentXP = getStatistic(currentXP, growthXP["level7"]);
		}

		public void UpdateXPLevel9()
		{
			currentXP = getStatistic(currentXP, growthXP["level9"]);
		}

		public void UpdateADLevel3()
		{
			currentAD = getStatistic(currentAD, growthAD["level3"]);
		}

		public void UpdateADLevel5()
		{
			currentAD = getStatistic(currentAD, growthAD["level5"]);
		}

		public void UpdateADLevel7()
		{
			currentAD = getStatistic(currentAD, growthAD["level7"]);
		}

		public void UpdateADLevel8()
		{
			currentAD = getStatistic(currentAD, growthAD["level8"]);
		}

		public void UpdateADLevel9()
		{
			currentAD = getStatistic(currentAD, growthAD["level9"]);
		}

		public void UpdateADLevel11()
		{
			currentAD = getStatistic(currentAD, growthAD["level11"]);
		}

		public void UpdateADLevel12()
		{
			currentAD = getStatistic(currentAD, growthAD["level12"]);
		}

		public void UpdateADLevel13()
		{
			currentAD = getStatistic(currentAD, growthAD["level13"]);
		}

		public void UpdateADLevel14()
		{
			currentAD = getStatistic(currentAD, growthAD["level14"]);
		}

		public void UpdateADLevel15()
		{
			currentAD = getStatistic(currentAD, growthAD["level15"]);
		}

		public void UpdateADLevel16()
		{
			currentAD = getStatistic(currentAD, growthAD["level16"]);
		}

		public void UpdateADLevel17()
		{
			currentAD = getStatistic(currentAD, growthAD["level17"]);
		}

	public void DisplayStats()
	{
		Debug.Log("Camp Name:" + CampName + "\n " +
			"Current Health:" + currentHealth + "\n " +
			"Current AD:" + currentAD + "\n " +
			"Current AS:" + currentAS + "\n " +
			"Current Range:" + currentRange + "\n " +
			"Current Leash:" + currentLeash + "\n " +
			"Current Level:" + currentLevel + "\n ");
	}

}

public class BlueSentinel : Camps
	{
	Dictionary<string, double> growthAD = new Dictionary<string, double>();
	Dictionary<string, double> growthXP = new Dictionary<string, double>();
	string CampName = "BlueSentinel";

		// setting base and growth health based on excel
		double baseHealth = 2160;
		double growthHealth = 572;

		// setting base and growth AD based on excel
		double baseAD = 55;

		//TODO
		//Dictionary growthAD = new Dictionary();
		//growthAD.add("level3", 11);
		//growthAD.add("level5", 6);
		//growthAD.add("level7", 5);
		//growthAD.add("level8", 6);
		//growthAD.add("level9", 6);
		//growthAD.add("level11", 16);
		//growthAD.add("level12", 11);
		//growthAD.add("level13", 11);
		//growthAD.add("level14", 6);
		//growthAD.add("level15", 11);
		//growthAD.add("level16", 11);
		//growthAD.add("level17", 11);

		// setting base AS based on excel
		double baseAS = 0.625;

		// setting base MR based on excel
		double baseMR = 20;

		// setting base AR based on excel
		double baseAR = 20;

		// setting base MS based on excel
		double baseMS = 525;

		// setting base Range based on excel
		double baseRange = 175;

		// setting base Leash based on excel
		double baseLeash = 800;

		// setting base Gold based on excel
		double baseGold = 81;

		// setting base XP and growth XP based on excel
		double baseXP = 80;
		//TODO
		//Dictionary growthXP = new Dictionary();
		//growthXP.add("level3", 2.01);
		//growthXP.add("level4", 4);
		//growthXP.add("level5", 9.99);
		//growthXP.add("level7", 8);
		//growthXP.add("level9", 12);

		// creating variables to store current level of each statistic
		double currentHealth;
		double currentAD;
		double currentAS;
		double currentAR;
		double currentMR;
		double currentMS;
		double currentRange;
		double currentLevel;
		double currentXP;
		double currentLeash;
		double currentGold;
		double AverageChampLevel;

		// defining constructor
		public BlueSentinel()
		{
			currentHealth = baseHealth;
			currentAD = baseAD;
			currentAS = baseAS;
			currentMS = baseMS;
			currentRange = baseRange;
			currentMR = baseMR;
			currentAR = baseAR;
			currentLeash = baseLeash;
			currentGold = baseGold;
			currentXP = baseXP;
			currentLevel = 0;
		}

		// defining function to check level
		public void CheckLevel()
		{
			if (AverageChampLevel < 2 && AverageChampLevel >= 1)
			{
				currentLevel = 1;
			}

			else if (AverageChampLevel < 3 && AverageChampLevel >= 2)
			{
				currentLevel = 2;
			}

			else if (AverageChampLevel < 4 && AverageChampLevel >= 3)
			{
				currentLevel = 3;
				UpdateHealth();
				UpdateXPLevel3();
				UpdateADLevel3();
			}

			else if (AverageChampLevel < 5 && AverageChampLevel >= 4)
			{
				currentLevel = 4;
				UpdateXPLevel4();
			}

			else if (AverageChampLevel < 6 && AverageChampLevel >= 5)
			{
				currentLevel = 5;
				UpdateHealth();
				UpdateADLevel5();
			}

			else if (AverageChampLevel < 7 && AverageChampLevel >= 6)
			{
				currentLevel = 6;
			}

			else if (AverageChampLevel < 8 && AverageChampLevel >= 7)
			{
				currentLevel = 7;
				UpdateHealth();
				UpdateXPLevel7();
				UpdateADLevel7();
			}

			else if (AverageChampLevel < 9 && AverageChampLevel >= 8)
			{
				currentLevel = 8;
				UpdateADLevel8();
			}

			else if (AverageChampLevel < 10 && AverageChampLevel >= 9)
			{
				currentLevel = 9;
				UpdateHealth();
				UpdateXPLevel9();
				UpdateADLevel9();
			}

			else if (AverageChampLevel < 11 && AverageChampLevel >= 10)
			{
				currentLevel = 10;
			}

			else if (AverageChampLevel < 12 && AverageChampLevel >= 11)
			{
				currentLevel = 11;
				UpdateHealth();
				UpdateADLevel11();
			}

			else if (AverageChampLevel < 13 && AverageChampLevel >= 12)
			{
				currentLevel = 12;
				UpdateADLevel12();
			}

			else if (AverageChampLevel < 14 && AverageChampLevel >= 13)
			{
				currentLevel = 13;
				UpdateADLevel13();
			}

			else if (AverageChampLevel < 15 && AverageChampLevel >= 14)
			{
				currentLevel = 14;
				UpdateADLevel14();
			}

			else if (AverageChampLevel < 16 && AverageChampLevel >= 15)
			{
				currentLevel = 15;
				UpdateADLevel15();
			}

			else if (AverageChampLevel < 17 && AverageChampLevel >= 16)
			{
				currentLevel = 16;
				UpdateADLevel16();
			}

			else if (AverageChampLevel < 18 && AverageChampLevel >= 17)
			{
				currentLevel = 17;
				UpdateADLevel17();
			}
		}

		public void UpdateHealth()
		{
			currentHealth = getStatistic(currentHealth, growthHealth);
		}

		public void UpdateXPLevel3()
		{
			currentXP = getStatistic(currentXP, growthXP["level3"]);
		}

		public void UpdateXPLevel4()
		{
			currentXP = getStatistic(currentXP, growthXP["level4"]);
		}

		public void UpdateXPLevel5()
		{
			currentXP = getStatistic(currentXP, growthXP["level5"]);
		}

		public void UpdateXPLevel7()
		{
			currentXP = getStatistic(currentXP, growthXP["level7"]);
		}

		public void UpdateXPLevel9()
		{
			currentXP = getStatistic(currentXP, growthXP["level9"]);
		}

		public void UpdateADLevel3()
		{
			currentAD = getStatistic(currentAD, growthAD["level3"]);
		}

		public void UpdateADLevel5()
		{
			currentAD = getStatistic(currentAD, growthAD["level5"]);
		}

		public void UpdateADLevel7()
		{
			currentAD = getStatistic(currentAD, growthAD["level7"]);
		}

		public void UpdateADLevel8()
		{
			currentAD = getStatistic(currentAD, growthAD["level8"]);
		}

		public void UpdateADLevel9()
		{
			currentAD = getStatistic(currentAD, growthAD["level9"]);
		}

		public void UpdateADLevel11()
		{
			currentAD = getStatistic(currentAD, growthAD["level11"]);
		}

		public void UpdateADLevel12()
		{
			currentAD = getStatistic(currentAD, growthAD["level12"]);
		}

		public void UpdateADLevel13()
		{
			currentAD = getStatistic(currentAD, growthAD["level13"]);
		}

		public void UpdateADLevel14()
		{
			currentAD = getStatistic(currentAD, growthAD["level14"]);
		}

		public void UpdateADLevel15()
		{
			currentAD = getStatistic(currentAD, growthAD["level15"]);
		}

		public void UpdateADLevel16()
		{
			currentAD = getStatistic(currentAD, growthAD["level16"]);
		}

		public void UpdateADLevel17()
		{
			currentAD = getStatistic(currentAD, growthAD["level17"]);
		}
	public void DisplayStats()
	{
		Debug.Log("Camp Name:" + CampName + "\n " +
			"Current Health:" + currentHealth + "\n " +
			"Current AD:" + currentAD + "\n " +
			"Current AS:" + currentAS + "\n " +
			"Current Range:" + currentRange + "\n " +
			"Current Leash:" + currentLeash + "\n " +
			"Current Level:" + currentLevel + "\n ");
	}

}

public class ScuttleCrab : Camps
	{

	string CampName = "ScuttleCrab";

		// setting base and growth health based on excel
		double baseHealth = 1050;
		double growthHealth = 93;

		// setting base AD based on excel
		double baseAD = 35;

		// setting base AS based on excel
		double baseAS = 0.6238;

		// setting base MR based on excel
		double baseMR = 20;

		// setting base AR based on excel
		double baseAR = 20;

		// setting base MS based on excel
		double baseMS = 155;

		// setting base Range based on excel
		double baseRange = 0;

		// setting base Leash based on excel
		double baseLeash = 0;

		// setting base XP and growth XP based on excel
		double baseXP = 100;
		double growthXP = 10;

		// setting base Gold and growth Gold based on excel
		double baseGold = 55;
		double growthGold = 5.5;

		// creating variables to store current level of each statistic
		double currentHealth;
		double currentAD;
		double currentAS;
		double currentAR;
		double currentMR;
		double currentMS;
		double currentRange;
		double currentLevel;
		double currentXP;
		double currentLeash;
		double currentGold;
		double AverageChampLevel;

		// defining constructor
		public ScuttleCrab()
		{
			currentHealth = baseHealth;
			currentAD = baseAD;
			currentAS = baseAS;
			currentMS = baseMS;
			currentRange = baseRange;
			currentMR = baseMR;
			currentAR = baseAR;
			currentLeash = baseLeash;
			currentGold = baseGold;
			currentXP = baseXP;
			currentLevel = 0;
		}

		// defining function to check level
		public void CheckLevel()
		{
			if (AverageChampLevel < 2 && AverageChampLevel >= 1)
			{
				currentLevel = 1;
			}

			else if (AverageChampLevel < 3 && AverageChampLevel >= 2)
			{
				currentLevel = 2;
				UpdateHealth();
				UpdateGold();
				UpdateXP();
			}

			else if (AverageChampLevel < 4 && AverageChampLevel >= 3)
			{
				currentLevel = 3;
				UpdateHealth();
				UpdateGold();
				UpdateXP();
			}

			else if (AverageChampLevel < 5 && AverageChampLevel >= 4)
			{
				currentLevel = 4;
				UpdateHealth();
				UpdateGold();
				UpdateXP();
			}

			else if (AverageChampLevel < 6 && AverageChampLevel >= 5)
			{
				currentLevel = 5;
				UpdateHealth();
				UpdateGold();
				UpdateXP();
			}

			else if (AverageChampLevel < 7 && AverageChampLevel >= 6)
			{
				currentLevel = 6;
				UpdateHealth();
				UpdateGold();
				UpdateXP();
			}

			else if (AverageChampLevel < 8 && AverageChampLevel >= 7)
			{
				currentLevel = 7;
				UpdateHealth();
				UpdateGold();
				UpdateXP();
			}

			else if (AverageChampLevel < 9 && AverageChampLevel >= 8)
			{
				currentLevel = 8;
				UpdateHealth();
				UpdateGold();
				UpdateXP();
			}

			else if (AverageChampLevel < 10 && AverageChampLevel >= 9)
			{
				currentLevel = 9;
				UpdateHealth();
				UpdateGold();
				UpdateXP();
			}

			else if (AverageChampLevel < 11 && AverageChampLevel >= 10)
			{
				currentLevel = 10;
				UpdateHealth();
			}

			else if (AverageChampLevel < 12 && AverageChampLevel >= 11)
			{
				currentLevel = 11;
				UpdateHealth();
			}

			else if (AverageChampLevel < 13 && AverageChampLevel >= 12)
			{
				currentLevel = 12;
				UpdateHealth();
			}

			else if (AverageChampLevel < 14 && AverageChampLevel >= 13)
			{
				currentLevel = 13;
				UpdateHealth();
			}

			else if (AverageChampLevel < 15 && AverageChampLevel >= 14)
			{
				currentLevel = 14;
				UpdateHealth();
			}

			else if (AverageChampLevel < 16 && AverageChampLevel >= 15)
			{
				currentLevel = 15;
				UpdateHealth();
			}

			else if (AverageChampLevel < 17 && AverageChampLevel >= 16)
			{
				currentLevel = 16;
				UpdateHealth();
			}

			else if (AverageChampLevel < 18 && AverageChampLevel >= 17)
			{
				currentLevel = 17;
				UpdateHealth();
			}
		}

		public void UpdateHealth()
		{
			currentHealth = getStatistic(currentHealth, growthHealth);
		}

		public void UpdateXP()
		{
			currentXP = getStatistic(currentXP, growthXP);
		}

		public void UpdateGold()
		{
			currentGold = getStatistic(currentGold, growthGold);
		}
	public void DisplayStats()
	{
		Debug.Log("Camp Name:" + CampName + "\n " +
			"Current Health:" + currentHealth + "\n " +
			"Current AD:" + currentAD + "\n " +
			"Current AS:" + currentAS + "\n " +
			"Current Range:" + currentRange + "\n " +
			"Current Leash:" + currentLeash + "\n " +
			"Current Level:" + currentLevel + "\n ");
	}

}

