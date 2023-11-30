using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

    Otrill characterOtrill;

    public PlayerScript playerScript;                                               //Handles movement ,attack 
    public List<int> AttackValues = new List<int>();                                //Attack amounts
    [SerializeField]
    List<CharacterModels> characterModels = new List<CharacterModels>();              //List of available character models with character type and object

    [SerializeField]
    List<CharacterScriptable> characterScriptables = new List<CharacterScriptable>();   //Character scriptable data



    public CharacterType SelectedCharacterType;                                    //Selected character type
    public int SelectedCharacterIndex = 0;                                         //Selected character index to set the current selected character

    // Start is called before the first frame update

    public CharacterModels currentCharacterModel;                                  //Current selected character set to this reference variable
    public float invisibleTime = 5f;
    public CharacterScriptable characterData;                                      //Scriptable object of character to reference the selected character details


    //CharacterData references
    //Used to avoid any change in base values in scriptable character object
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
    public double currentLevel = 1;
    [HideInInspector]
    public double currentXP;
    [HideInInspector]
    public double currentAP;
    // [HideInInspector]
    public List<AttackLevel> attackLevels = new List<AttackLevel>();
    public List<AttackScalingConditions> attackScalingConditions = new List<AttackScalingConditions>();
    public MinionHealthBar championHealthBar;                                                               //Healthbar reference for character
    public GameObject referenceObject;                                                                      //Position reference for healthbar
    Transform healthBarTransform;                                                                    //Reference transform of healthbar
    Camera cam;
    //
    void Start()
    {
        //Temp 
        characterOtrill = new Otrill();
        characterOtrill.CheckLevel();
        characterOtrill.DisplayStats();


        //Instntiate healthbar for the minion and set it in canvas and set proper scale 
        GameObject Healthbar = Instantiate(GameManager.instance.ChampionHealthBar,GameManager.instance.MinionHealthbarsParent);
        Healthbar.name = "Champion HealthBar";
        Healthbar.transform.localScale = Vector3.one;
        championHealthBar = Healthbar.GetComponent<MinionHealthBar>();
        healthBarTransform = championHealthBar.transform;
        cam = FindObjectOfType<Camera>();
       // Debug.LogError("Object created " + Healthbar.name);
        //
        //Set Default character on start
        ChangeCharacter();
        // SelectCharacter((int) SelectedCharacterIndex);
        // Healthbar.gameObject.SetActive(true);
        if(referenceObject)         //Handle exception for null reference 
        {
            healthBarTransform.position = cam.WorldToScreenPoint(referenceObject.transform.position);   //Set position of healthbar continuously at healbar reference position for the minion
        }
        Invoke(nameof(ShowHealthBar),0.3f);
    }
    /// <summary>
    /// Set active healthvar object on
    /// </summary>
    public void ShowHealthBar()
    {
        championHealthBar.gameObject.SetActive(true);
    }
    /// <summary>
    /// Changes character model and set next model and animator
    /// </summary>
    public void ChangeCharacter()
    {
        if(SelectedCharacterIndex < characterModels.Count - 1)
        {
            SelectedCharacterIndex += 1;
        }
        else
        {
            SelectedCharacterIndex = 0;
        }
        SelectCharacter(SelectedCharacterIndex);
    }
    /// <summary>
    /// Selects character model from list with respect to input index and set current character model
    /// </summary>
    /// <param name="index">selected character index</param>
    public void SelectCharacter(int index)
    {
        for(int i = 0 ; i < characterModels.Count ; i++)
        {
            characterModels[i].characterModel.SetActive(i == index ? true : false);

        }
        SelectedCharacterType = characterModels[index].characterType;
        currentCharacterModel = characterModels.Find(x => x.characterType == SelectedCharacterType);
        Debug.LogError(currentCharacterModel.characterModel.name);
        playerScript.characterAnimator = characterModels.Find(x => x.characterType == SelectedCharacterType).characterModel.GetComponent<Animator>();
        playerScript.SetSpeed(characterOtrill.GetMovementSpeed());
        
        //We can use this data later
        characterData = characterScriptables.Find(x => x.characterModel.characterType == SelectedCharacterType);
        SetUpValues();   //Setup values from base scriptable character
        if(characterData)
            characterData.DisplayStats();

        //Update statistics with respect to charactes level

        UpdateStatistics(2); //Temp: Level 2 set for testing
        UpdateAttackButtons();

        GameManager.instance.Show_QWER_LevelUpdatePanel();

        Debug.LogError("AD " + currentAD);

    }
    /// <summary>
    /// Enable Q/W/E/R attack buttons with level greater then zero
    /// </summary>
    private void UpdateAttackButtons()
    {
        foreach(AttackButton item in GameManager.instance.AttackButtons)
        {
            if(item.attackType != AttackType.auto)
            {
                if(attackLevels.FindAll(x => x.attackType == item.attackType && x.level < 1).Count > 0)
                {
                    item.DeactiveIndicator.SetActive(true);
                }
                else
                {
                    item.DeactiveIndicator.SetActive(false);
                }
            }
        }
    }

    //True: Show character ,False: Hide character
    public void ShowModel(bool val)
    {
        currentCharacterModel.characterModel.SetActive(val);
        Invoke(nameof(ShowAgain),invisibleTime);
    }
    //Show character model
    public void ShowAgain()
    {
        currentCharacterModel.characterModel.SetActive(true);
    }
    /// <summary>
    /// Update statistics with respect to currentlevel
    /// </summary>
    /// <param name="_currentLevel">character's current level</param>
    public void UpdateStatistics(int _currentLevel)
    {
        currentLevel = _currentLevel;

        currentHealth = Champions.getStatistic(characterData.baseHealth,characterData.growthHealth,currentLevel);
        currentHealthRegen = Champions.getStatistic(characterData.baseHealthRegen,characterData.growthHealthRegen,currentLevel);
        currentAD = Champions.getStatistic(characterData.baseAD,characterData.growthAD,currentLevel);
        currentAS = Champions.getStatistic(characterData.baseAS,characterData.growthAS,currentLevel);
        currentArmor = Champions.getStatistic(characterData.baseArmor,characterData.growthAD,currentLevel);
        currentMagicResistance = Champions.getStatistic(characterData.baseMagicResistance,characterData.growthMagicResistance,currentLevel);
        currentRange = Champions.getStatistic(characterData.baseRange,characterData.growthRange,currentLevel);

        //Set scaling conditions
        attackScalingConditions = characterData.attackScalingConditions;
        //Set slider value
        championHealthBar.SetMaxHealth((float)currentHealth);
        championHealthBar.SetHealth((float)currentHealth);

    }
    /// <summary>
    /// Increase Q/W/E/R attack level by one (Max 5)
    /// </summary>
    /// <param name="attackType">attackType to levelup</param>
    public void UpdateAttackLevel(AttackType attackType)
    {
        AttackLevel attackLevel = attackLevels.Find(x => x.attackType == attackType);
        attackLevel.level += 1;
        if(attackLevel.level > 5)
        { attackLevel.level = 5; }
        UpdateAttackButtons();
        //Debug.LogError("Increased " + attackLevel.level + attackType);

        GameManager.instance.Hide_QWER_LevelUpdatePanel();
    }
    //Setup Values
    //Set up values  of character
    public void SetUpValues(int _currentLevel = 1)
    {
        currentHealth = characterData.baseHealth;
        currentHealthRegen = characterData.baseHealthRegen;
        currentAD = characterData.baseAD;
        currentAS = characterData.baseAS;
        currentArmor = characterData.baseArmor;
        currentMagicResistance = characterData.baseMagicResistance;
        currentMovementSpeed = characterData.baseMovementSpeed;
        currentRange = characterData.baseRange;
        currentXP = Globals.level1;
        currentLevel = _currentLevel;
        currentAP = characterData.baseAP;
    }
    /// <summary>
    /// Calculate damage with resepect to attack level
    /// </summary>
    /// <param name="attackType">current attack type</param>
    /// <returns></returns>
    public float CalculateDamangeForAttack(AttackType attackType)
    {
        float damage = 0;
        switch(attackType)
        {
            case AttackType.w:
                break;
            case AttackType.q:
                float damageValue = 0;
                AttackScalingConditions attackScalingCondition = attackScalingConditions.Find(x => x.attackType == attackType);
                List<ConditionsDetails> conditions = attackScalingCondition.conditions.FindAll(x => x.Level == attackLevels.Find(y => y.attackType == attackType).level);
                foreach(ConditionsDetails condition in conditions)
                {
                    List<ScaleConditionsAndFactors> scaleConditionsAndFactors = condition.scaleConditionsAndFactors;
                    foreach(ScaleConditionsAndFactors item in scaleConditionsAndFactors)
                    {
                        switch(item.scalingCondition)
                        {
                            case ScalingConditionTypes.None:
                                break;
                            case ScalingConditionTypes.Value_Plus_Percentage_AD:
                                damageValue += item.baseValue + (float)(currentAD * (item.percentage / 100));
                                if(characterData.characterModel.characterType == CharacterType.Sura)
                                {
                                    //Sura Specific :
                                    //current character champion is "Sura" then check for Ult/R attack level, If R level is greate then 0 then add extra damange to current damange
                                    float extraDamageOfUlt = 0;
                                    int RLevel = attackLevels.Find(x => x.attackType == AttackType.r).level;
                                    if(RLevel > 0)
                                    {
                                        List<ConditionsDetails> conditionsDetailsForUlt = attackScalingCondition.conditions.FindAll(x => x.Level == attackLevels.Find(y => y.attackType == AttackType.r).level);
                                        ScaleConditionsAndFactors scaleConditions = conditionsDetailsForUlt.Find(x => x.Level == RLevel).scaleConditionsAndFactors.Find(x => x.scalingCondition == ScalingConditionTypes.Value_Plus_Percentage_AD);
                                        extraDamageOfUlt = scaleConditions.baseValue + (float)(currentAD * (scaleConditions.percentage / 100));
                                        
                                        Debug.LogError("ExtraDamage from Ult Specific for Sura");
                                        
                                        damageValue += extraDamageOfUlt;
                                    }
                                }
                                Debug.LogError("AD  : Base Value " + item.baseValue + "  PercentageValue " + (float)(currentAD * (item.percentage / 100)));
                                break;
                            //Treat AP and Bonus AP same for now
                            case ScalingConditionTypes.Value_Plus_Percentage_AP:
                            case ScalingConditionTypes.Value_Plus_Percentage_BonusAP:
                                damageValue += item.baseValue + (float)(currentAP * (item.percentage / 100));
                                Debug.LogError("AP  : Base Value " + item.baseValue + "  PercentageValue " + (float)(currentAP * (item.percentage / 100)));
                                break;
                            case ScalingConditionTypes.SlowerForSomeTime:
                                break;
                            case ScalingConditionTypes.Percentage_DamageReduction:
                                break;
                            case ScalingConditionTypes.Percentage_AS_Up:

                                break;
                            case ScalingConditionTypes.Percentage_MS_Up:
                                break;
                            case ScalingConditionTypes.Percentage_Heal:
                                break;
                            case ScalingConditionTypes.AD_Plus_Percentage_AP:
                                damageValue += (float)currentAD + (float)(currentAP * (item.percentage / 100));
                                break;
                            default:
                                break;
                        }
                    }
                }
                damage = damageValue;
                break;
            case AttackType.e:
                break;
            case AttackType.r:
                break;
            case AttackType.auto:
                float damageValueAuto = 0;
                AttackScalingConditions attackScalingConditionAuto = attackScalingConditions.Find(x => x.attackType == attackType);
                //List<ConditionsDetails> conditionsAuto = attackScalingConditionAuto.conditions.FindAll(x => x.Level == attackLevels.Find(y => y.attackType == attackType).level);
                List<ConditionsDetails> conditionsAuto = attackScalingConditionAuto.conditions.FindAll(x => x.Level == 1); // Auto does not have levels so default 1


                foreach(ConditionsDetails condition in conditionsAuto)
                {
                    List<ScaleConditionsAndFactors> scaleConditionsAndFactors = condition.scaleConditionsAndFactors;
                    foreach(ScaleConditionsAndFactors item in scaleConditionsAndFactors)
                    {
                        switch(item.scalingCondition)
                        {
                            case ScalingConditionTypes.None:
                                break;
                            case ScalingConditionTypes.Value_Plus_Percentage_AD:
                                damageValueAuto += (float)currentAD;// //item.baseValue + (float)(currentAD * (item.percentage / 100));
                                break;
                            //Treat AP and Bonus AP same for now
                            case ScalingConditionTypes.Value_Plus_Percentage_AP:
                            case ScalingConditionTypes.Value_Plus_Percentage_BonusAP:
                                //damageValueAuto += item.baseValue + (float)(currentAP * (item.percentage / 100));
                                damageValueAuto +=  (float)currentAP;
                                break;
                            case ScalingConditionTypes.SlowerForSomeTime:
                                break;
                            case ScalingConditionTypes.Percentage_DamageReduction:
                                break;
                            case ScalingConditionTypes.Percentage_AS_Up:

                                break;
                            case ScalingConditionTypes.Percentage_MS_Up:
                                break;
                            case ScalingConditionTypes.Percentage_Heal:
                                break;
                            case ScalingConditionTypes.AD_Plus_Percentage_AP:
                                damageValueAuto += (float)currentAD + (float)(currentAP * (item.percentage / 100));
                                break;
                            default:
                                break;
                        }
                    }
                }
                damage = damageValueAuto;
                break;

            default:
                break;
        }
        //Check if shield is on then reduce the damage 
        if(playerScript.Shield_Effect) 
        {
            Debug.LogError("Shield :" + playerScript.Shield_UpdatedPercentage + "Damange Reduction :"+(damage * (playerScript.Shield_UpdatedPercentage / 100))) ;
            damage -=  (damage* (playerScript.Shield_UpdatedPercentage / 100));  
        }
        return damage;
    }

    /// <summary>
    /// Calculate and apply effect with resepect to attack level
    /// </summary>
    /// <param name="attackType">current attack type</param>
    /// <returns></returns>
    public void ApplyEffectOnPlayerForAttack(AttackType attackType)
    {
        AttackScalingConditions attackScalingCondition = attackScalingConditions.Find(x => x.attackType == attackType);
        List<ConditionsDetails> conditions = attackScalingCondition.conditions.FindAll(x => x.Level == attackLevels.Find(y => y.attackType == attackType).level);
        foreach(ConditionsDetails condition in conditions)
        {
            List<ScaleConditionsAndFactors> scaleConditionsAndFactors = condition.scaleConditionsAndFactors;
            foreach(ScaleConditionsAndFactors item in scaleConditionsAndFactors)
            {
                switch(item.scalingCondition)
                {
                    case ScalingConditionTypes.None:
                        break;
                    case ScalingConditionTypes.Value_Plus_Percentage_AD:
                        break;
                    //Treat AP and Bonus AP same for now
                    case ScalingConditionTypes.Value_Plus_Percentage_AP:
                    case ScalingConditionTypes.Value_Plus_Percentage_BonusAP:                     
                        break;
                    case ScalingConditionTypes.SlowerForSomeTime:
                        break;
                    case ScalingConditionTypes.Percentage_DamageReduction:
                        ScaleConditionsAndFactors scaleConditionsShield = condition.scaleConditionsAndFactors.Find(x => x.scalingCondition == ScalingConditionTypes.Percentage_DamageReduction);
                        playerScript.Set_Shield_Effect(scaleConditionsShield.effectTime,scaleConditionsShield.percentage,true); //True: indicates increase shield
                        break;
                    case ScalingConditionTypes.Percentage_AS_Up:
                        ScaleConditionsAndFactors scaleConditionsAS = condition.scaleConditionsAndFactors.Find(x => x.scalingCondition == ScalingConditionTypes.Percentage_AS_Up);
                        playerScript.Set_AS_SpeedEffect(scaleConditionsAS.effectTime,scaleConditionsAS.percentage,false); //False: indicates decrease time between two auto attacks : AS_UP decrease time between two Auto  attacks
                        break;
                    case ScalingConditionTypes.Percentage_MS_Up:
                         ScaleConditionsAndFactors scaleConditions =condition.scaleConditionsAndFactors.Find(x => x.scalingCondition == ScalingConditionTypes.Percentage_MS_Up);
                        playerScript.SetSpeedEffect(scaleConditions.effectTime,scaleConditions.percentage,true); //True:  Increase speed
                        break;

                    case ScalingConditionTypes.Percentage_AS_Down:
                        ScaleConditionsAndFactors scaleConditionsASDown = condition.scaleConditionsAndFactors.Find(x => x.scalingCondition == ScalingConditionTypes.Percentage_AS_Down);
                        playerScript.Set_AS_SpeedEffect(scaleConditionsASDown.effectTime,scaleConditionsASDown.percentage,true); //True: indicates increase time between two auto attacks : AS_Down increase time between two auto attacks
                        break;
                    case ScalingConditionTypes.Percentage_MS_Down:
                        ScaleConditionsAndFactors scaleConditionsDown = condition.scaleConditionsAndFactors.Find(x => x.scalingCondition == ScalingConditionTypes.Percentage_MS_Down);
                        playerScript.SetSpeedEffect(scaleConditionsDown.effectTime,scaleConditionsDown.percentage,false); //False:  Decrease speed
                        break;
                    case ScalingConditionTypes.Percentage_Heal:
                        break;
                    case ScalingConditionTypes.AD_Plus_Percentage_AP:
                      
                        break;
                    default:
                        break;
                }
            }
        }
    }
    /// <summary>
    /// Handle damage and update healthbar
    /// </summary>
    /// <param name="damage">damage value</param>
    public void DealDamage(float damage)
    {
       currentHealth  -= damage;
        if(currentHealth < 0) 
        {
            currentHealth = 0;
        }
        championHealthBar.SetHealth((float)currentHealth);
    }
    
}
/// <summary>
/// This class is used to hold the character type and character model object in the script
/// </summary>
[System.Serializable]
public class CharacterModels
{
    public CharacterType characterType;
    public GameObject characterModel;
}