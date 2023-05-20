using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemSets
{
    // create retrun data function
    public void display()
    {
        Console.WriteLine("-------Item Description------");
    }
}

public class BigSword: ItemSets
{
    string itemName = "Big Sword";
    string itemDescription = ""; //To fill in later
    string itemType = "Basic";
    List<Tuple<string, double>> requirements = new List<Tuple<string, double>>()
    {
        Tuple.Create("", 0.0)
    };
    double itemCost = 350.0;
    public List<Tuple<string, string, double>> buffs = new List<Tuple<string, string, double>>()
    {
        Tuple.Create("AD", "additive", 10.0)
    };

}
public class ShortSowrd : ItemSets
{
    string itemName = "Short Sword";
    string itemDescription = ""; //To fill in later
    string itemType = "Basic";
    List<Tuple<string, double>> requirements = new List<Tuple<string, double>>()
    {
        Tuple.Create("", 0.0)
    };
    double itemCost = 300.0;
    public List<Tuple<string, string, double>> buffs = new List<Tuple<string, string, double>>()
    {
        Tuple.Create("AS", "multiplcative", 0.11)
    };

}

public class BigBook: ItemSets
{
    string itemName = "Big Book";
    string itemDescription = ""; //To fill in later
    string itemType = "Basic";
    List<Tuple<string, double>> requirements = new List<Tuple<string, double>>()
    {
        Tuple.Create("", 0.0)
    };
    double itemCost = 450.0;
    List<Tuple<string, string, double>> buffs = new List<Tuple<string, string, double>>()
    {
        Tuple.Create("AH", "additive", 40.0)
    };

}
public class BigRodOfPower : ItemSets
{
    string itemName = "Big Rod of Power";
    string itemDescription = ""; //To fill in later
    string itemType = "Basic";
    List<Tuple<string, double>> requirements = new List<Tuple<string, double>>()
    {
        Tuple.Create("", 0.0)
    };
    double itemCost = 900.0;
    List<Tuple<string, string, double>> buffs = new List<Tuple<string, string, double>>()
    {
        Tuple.Create("AS", "additive", 40.0)
    };

}

public class TrollArmour : ItemSets
{
    string itemName = "Troll Armour";
    string itemDescription = ""; //To fill in later
    string itemType = "Basic";
    List<Tuple<string, double>> requirements = new List<Tuple<string, double>>()
    {
        Tuple.Create("", 0.0)
    };
    double itemCost = 900.0;
    List<Tuple<string, string, double>> buffs = new List<Tuple<string, string, double>>()
    {
        Tuple.Create("armour", "additive", 40.0)
    };

}
public class GreenBubble : ItemSets
{
    string itemName = "Green Bubble";
    string itemDescription = ""; //To fill in later
    string itemType = "Basic";
    List<Tuple<string, double>> requirements = new List<Tuple<string, double>>()
    {
        Tuple.Create("", 0.0)
    };
    double itemCost = 500.0;
    List<Tuple<string, string, double>> buffs = new List<Tuple<string, string, double>>()
    {
        Tuple.Create("health", "additive", 350.0)
    };

}

public class Blocker : ItemSets
{
    string itemName = "Blocker";
    string itemDescription = ""; //To fill in later
    string itemType = "Basic";
    List<Tuple<string, double>> requirements = new List<Tuple<string, double>>()
    {
        Tuple.Create("", 0.0)
    };
    double itemCost = 400.0;
    List<Tuple<string, string, double>> buffs = new List<Tuple<string, string, double>>()
    {
        Tuple.Create("magic resistance", "multiplicative", 0.15)
    };

}
public class TriStarKnife : ItemSets
{
    string itemName = "Tri-Star Knife";
    string itemDescription = ""; //To fill in later
    string itemType = "Basic";
    List<Tuple<string, double>> requirements = new List<Tuple<string, double>>()
    {
        Tuple.Create("", 0.0)
    };
    double itemCost = 800.0;
    List<Tuple<string, string, double>> buffs = new List<Tuple<string, string, double>>()
    {
        Tuple.Create("AD", "additive", 20.0),
        Tuple.Create("AS", "multiplicative", 0.2)
    };

}
public class ShieldOfEternitiy : ItemSets
{
    string itemName = "Shield of Eternity";
    string itemDescription = ""; //To fill in later
    string itemType = "Basic";
    List<Tuple<string, double>> requirements = new List<Tuple<string, double>>()
    {
        Tuple.Create("", 0.0)
    };
    double itemCost = 450;
    List<Tuple<string, string, double>> buffs = new List<Tuple<string, string, double>>()
    {
        Tuple.Create("AP", "additive", 10.0),
        Tuple.Create("health", "additive", 10.0),
        Tuple.Create("gold", "per_second", 0.33)
    };

}

public class ValorFleece : ItemSets
{
    string itemName = "Valor Fleece";
    string itemDescription = ""; //To fill in later
    string itemType = "Basic";
    List<Tuple<string, double>> requirements = new List<Tuple<string, double>>()
    {
        Tuple.Create("", 0.0)
    };
    double itemCost = 450;
    List<Tuple<string, string, double>> buffs = new List<Tuple<string, string, double>>()
    {
        Tuple.Create("AD", "additive", 10.0),
        Tuple.Create("health", "additive", 10.0),
        Tuple.Create("gold", "per_second", 0.33)
    };
}

public class Boots : ItemSets
{
    string itemName = "Boots";
    string itemDescription = ""; //To fill in later
    string itemType = "Basic";
    List<Tuple<string, double>> requirements = new List<Tuple<string, double>>()
    {
        Tuple.Create("", 0.0)
    };
    double itemCost = 350.0;
    List<Tuple<string, string, double>> buffs = new List<Tuple<string, string, double>>()
    {
        Tuple.Create("movement_speed", "multiplicative", 10.0),
    };

}

public class BlessedBladeOfWoe : ItemSets
{
    string itemName = "Blessed Blade of Woe";
    string itemDescription = ""; //To fill in later
    string itemType = "Basic";
    List<Tuple<string, double>> requirements = new List<Tuple<string, double>>()
    {
        Tuple.Create("", 0.0)
    };
    double itemCost = 450.0;
    List<Tuple<string, string, double>> buffs = new List<Tuple<string, string, double>>()
    {
        Tuple.Create("AD", "multiplicative", 0.30),
    };

}

public class HealthCup : ItemSets
{
    string itemName = "HealthCup";
    string itemDescription = ""; //To fill in later
    string itemType = "Basic";
    List<Tuple<string, double>> requirements = new List<Tuple<string, double>>()
    {
        Tuple.Create("", 0.0)
    };
    double itemCost = 50.0;
    List<Tuple<string, string, double>> buffs = new List<Tuple<string, string, double>>()
    {
        Tuple.Create("health", "per_second", 10.0),
        Tuple.Create("duration", "seconds", 10.0)
    };

}

public class RelfectiveCape : ItemSets
{
    string itemName = "Reflective Cape";
    string itemDescription = ""; //To fill in later
    string itemType = "Basic";
    List<Tuple<string, double>> requirements = new List<Tuple<string, double>>()
    {
        Tuple.Create("", 0.0)
    };
    double itemCost = 350.0;
    List<Tuple<string, string, double>> buffs = new List<Tuple<string, string, double>>()
    {
        Tuple.Create("magic_resistance", "additive", 50.0),
    };

}

public class ReflectionAxe : ItemSets
{
    string itemName = "Reflection Axe";
    string itemDescription = ""; //To fill in later
    string itemType = "Basic";
    List<Tuple<string, double>> requirements = new List<Tuple<string, double>>()
    {
        Tuple.Create("", 0.0)
    };
    double itemCost = 350.0;
    List<Tuple<string, string, double>> buffs = new List<Tuple<string, string, double>>()
    {
        Tuple.Create("magic_resistance", "additive", 50.0),
    };

}

public class CarveSword : ItemSets
{
    string itemName = "Carve Sword";
    string itemDescription = ""; //To fill in later
    string itemType = "Epic";
    List<Tuple<string, double>> requirements = new List<Tuple<string, double>>()
    {
        Tuple.Create("BigSword", 250.0),
        Tuple.Create("ShortSword", 200.0),

    };
    double itemCost = 250.0;
    List<Tuple<string, string, double>> buffs = new List<Tuple<string, string, double>>()
    {
        Tuple.Create("AD", "additive", 20.0),
        Tuple.Create("AS", "multiplicative", 0.15),
    };

}

public class LeacherSword : ItemSets
{
    string itemName = "Leacher Sword";
    string itemDescription = ""; //To fill in later
    string itemType = "Epic";
    List<Tuple<string, double>> requirements = new List<Tuple<string, double>>()
    {
        Tuple.Create("BigSword", 250.0),

    };
    double itemCost = 650.0;
    List<Tuple<string, string, double>> buffs = new List<Tuple<string, string, double>>()
    {
        Tuple.Create("AD", "additive", 15.0),
        Tuple.Create("Lifesteel", "multiplicative", 0.08),
    };

}

//removed reduced healing
public class SeptaSpear : ItemSets
{
    string itemName = "Leacher Sword";
    string itemDescription = ""; //To fill in later
    string itemType = "Epic";
    List<Tuple<string, double>> requirements = new List<Tuple<string, double>>()
    {
        Tuple.Create("Tri-Star Knife", 800.0),
        Tuple.Create("Tri-Star Knife", 800.0)

    };
    double itemCost = 200.0;
    List<Tuple<string, string, double>> buffs = new List<Tuple<string, string, double>>()
    {
        Tuple.Create("AD", "additive", 50.0),
        Tuple.Create("AS", "multiplicative", 0.10),
    };

}

public class Kirpi : ItemSets
{
    string itemName = "Kirpi";
    string itemDescription = ""; //To fill in later
    string itemType = "Epic";
    List<Tuple<string, double>> requirements = new List<Tuple<string, double>>()
    {
        Tuple.Create("BigSword", 250.0),
        Tuple.Create("BigSword", 250.0),

    };
    double itemCost = 300.0;
    List<Tuple<string, string, double>> buffs = new List<Tuple<string, string, double>>()
    {
        Tuple.Create("AD", "additive", 35.0),
        Tuple.Create("AH", "additive", 15.0),
    };

}

public class Blinkstrike : ItemSets
{
    string itemName = "Blinkstrike";
    string itemDescription = ""; //To fill in later
    string itemType = "Epic";
    List<Tuple<string, double>> requirements = new List<Tuple<string, double>>()
    {
        Tuple.Create("Tri-Star Knife", 800.0),
        Tuple.Create("Tri-Star Knife", 800.0),
        Tuple.Create("ShortSword", 200.0),

    };
    double itemCost = 600.0;
    List<Tuple<string, string, double>> buffs = new List<Tuple<string, string, double>>()
    {
        Tuple.Create("AS", "multiplicative", 0.40),
        Tuple.Create("AD", "additive", 50.0),
    };

}

public class FlooFlamer : ItemSets
{
    string itemName = "FlooFlamer";
    string itemDescription = ""; //To fill in later
    string itemType = "Epic";
    List<Tuple<string, double>> requirements = new List<Tuple<string, double>>()
    {
        Tuple.Create("BigBook", 450.0),
        Tuple.Create("BigBook", 450.0),
        Tuple.Create("BigRodOfPower", 900.0),

    };
    double itemCost = 200;
    List<Tuple<string, string, double>> buffs = new List<Tuple<string, string, double>>()
    {
        Tuple.Create("AS", "multiplicative", 0.40),
        Tuple.Create("AD", "additive", 50.0),
    };

}

public class ExileChalice : ItemSets
{
    string itemName = "Exile Chalice";
    string itemDescription = ""; //To fill in later
    string itemType = "Epic";
    List<Tuple<string, double>> requirements = new List<Tuple<string, double>>()
    {
        Tuple.Create("BigBook", 450.0),
        Tuple.Create("BigRodOfPower", 900.0),

    };
    double itemCost = 300;
    List<Tuple<string, string, double>> buffs = new List<Tuple<string, string, double>>()
    {
        Tuple.Create("AP", "additive", 50.0),
        Tuple.Create("AH", "additive", 15.0),
    };

}

public class MoonDust : ItemSets
{
    string itemName = "Moon Dust";
    string itemDescription = ""; //To fill in later
    string itemType = "Epic";
    List<Tuple<string, double>> requirements = new List<Tuple<string, double>>()
    {
        Tuple.Create("BigBook", 450.0),

    };
    double itemCost = 700;
    List<Tuple<string, string, double>> buffs = new List<Tuple<string, string, double>>()
    {
        Tuple.Create("AP", "additive", 30.0),
        Tuple.Create("movement_speed", "additive", 10.0),
    };

}

public class GreenCherry : ItemSets
{
    string itemName = "Moon Dust";
    string itemDescription = ""; //To fill in later
    string itemType = "Epic";
    List<Tuple<string, double>> requirements = new List<Tuple<string, double>>()
    {
        Tuple.Create("Green Bubble", 500.0),
        Tuple.Create("Green Bubble", 500.0),

    };
    double itemCost = 200;
    List<Tuple<string, string, double>> buffs = new List<Tuple<string, string, double>>()
    {
        Tuple.Create("health_regen", "multiplicative", 1.00),
        Tuple.Create("health", "additive", 400.0),
    };

}
public class BoulderBarricade : ItemSets
{
    string itemName = "Boulder Barricade";
    string itemDescription = ""; //To fill in later
    string itemType = "Epic";
    List<Tuple<string, double>> requirements = new List<Tuple<string, double>>()
    {
        Tuple.Create("Green Bubble", 500.0),
        Tuple.Create("Troll Armour", 600.0),

    };
    double itemCost = 500;
    List<Tuple<string, string, double>> buffs = new List<Tuple<string, string, double>>()
    {
        Tuple.Create("armour", "additive", 40.0),
        Tuple.Create("health", "additive", 400.0),
    };

}

public class SpiritCall : ItemSets
{
    string itemName = "Spirit Call";
    string itemDescription = ""; //To fill in later
    string itemType = "Epic";
    List<Tuple<string, double>> requirements = new List<Tuple<string, double>>()
    {
        Tuple.Create("Green Bubble", 500.0),
        Tuple.Create("Troll Armour", 600.0),
        Tuple.Create("BigRodOfPower", 900.0),

    };
    double itemCost = 200;
    List<Tuple<string, string, double>> buffs = new List<Tuple<string, string, double>>()
    {
        Tuple.Create("armour", "additive", 20.0),
        Tuple.Create("AP", "additive", 50.0),
        Tuple.Create("health", "additive", 400.0),
    };

}