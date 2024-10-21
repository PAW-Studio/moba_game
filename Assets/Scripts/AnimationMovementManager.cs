using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CharacterEventManager;

public class AnimationMovementManager : MonoBehaviour
{
    [SerializeField]
    public PlayerScript playerScript;                                          //Character's playerscript reference

    Vector3 startPosition;

    public bool Attacking = false;
    public List<MinionAIScript> HitList = new List<MinionAIScript>();
    public List<Character> HitListChampions = new List<Character>();
    public List<TowerAIScript> HitListTower = new List<TowerAIScript>();
    public List<CollisionDetectorObject> collisionDetectorObjects;            //list of collision detector objects for different type of collisions after attack
    public DamageType currentAttackDamangeType;                               //DamageTypeOff current attack
    public List<ProjectileSpawnDetails> projectileSpawnDetails = new List<ProjectileSpawnDetails>(); //Details of projectiles(throwable/shootable) with resepect to attack types
    [Header("Throw/Shootable projectile")]
    public GameObject ArrowPrefab, SerinaQArrowPrefab, SerinaRArrowPrefab, E_ProjectilPrefab;                                          //Arrow/projectile prefab
    public Transform spawnTrasform;                                         //Arrow/projectile spawn transform
    public Transform spawnTransforSerinaAuto;                               //Serina Autor arrow spawn reference
    public Transform spawnTrasformLeft;                                     //Arrow/projectile spawn transform for left hand
    public Projectile arrowProjectile;                                      //Reference script of arrow projectile
    public GameObject TargetLine;
    int characterId;
    public GameObject OtrillAutoProjectile, Otrill_E_VFX, Otrill_R_Left, Otrill_R_Right, DefaultAuraVFX, W_AuraVFX, BeamVFX, SerinaDeathVFX, HakkaW, HakkaHammer, HakkaHammerImpact, ImpactTransform, HakkaDeathVFX, HakkaQVFX, HakkaEEffect, RecallVFX, JahanDeathVFX, SuraDeathVFX, SuraEVFX, SuraQVFX, SuraWVFX, Moorg_W_VFX, Moorg_Ult_VFX, Moorg_Q_VFX_ForEnemy, Sura_Q_Enemy, Moorg_Q_Enemy, Moorg_E_SlashVFX;
    public GameObject MoorgUltMesh, MoorgNormalMesh;
    public GameObject SuraBladeBack, SuraBladeInHand;
    public Transform dieAnimationEndPoint;
    public event OnCharacterDeath OnOtrillCharacterDeath;
    public event ON_Serina_W_Attack onSerinaWAttack;
    public Transform MoorgEVFXRef;
    //Temp
    public GameObject dummyEnemy;
    public Transform SpecialHitTransform;

    private void OnEnable()
    {
        // startPosition = transform.position;
        characterId = GetComponentInParent<Character>().Id;
        OnOtrillCharacterDeath += DeathBeamAnimation;

        if(GameManager.instance && GameManager.instance.dummyEnemy)
            dummyEnemy = GameManager.instance.dummyEnemy;

    }
    public void InvokeOtrhillDeath()
    {
        OnOtrillCharacterDeath.Invoke();
    }
    /// <summary>
    /// Set the variable for auto movement ON in playerscript
    /// </summary>
    public void SetMovementOn(AttackType attackType)
    {
        playerScript.SetAnimationMovementSpeedModifier(attackType);
        playerScript.moving = true;
    }

    /// <summary>
    /// Set the variable for auto movement OFF in playerscript
    /// </summary>
    public void SetMovementOff()
    {
        playerScript.moving = false;
        playerScript.ResetAnimationMovementSpeedModifier();
        // transform.position = startPosition;
        //DetectHit();
    }
    /// <summary>
    /// Damages the list of targets hit by player
    /// </summary>
    public void DetectHit()
    {

        Character character = playerScript.GetComponent<Character>();
        foreach(MinionAIScript target in HitList)
        {
            if(character.characterData.characterModel.characterType == CharacterType.Jahan)
            {
                if(playerScript.currentAttackType == AttackType.e)
                {
                    int E_attackLevel = character.attackLevels.Find(x => x.attackType == AttackType.e).level;
                    ScaleConditionsAndFactors scaleConditionsAndFactors = character.characterData.attackScalingConditions.Find(x => x.attackType == AttackType.e).conditions.Find(y => y.Level == E_attackLevel).scaleConditionsAndFactors.Find(x => x.scalingCondition == ScalingConditionTypes.SlowerForSomeTime);

                    target.SetSlowerSpeedEffect(scaleConditionsAndFactors.effectTime,scaleConditionsAndFactors.percentage);
                }
            }
            else if(character.characterData.characterModel.characterType == CharacterType.Dira)
            {
                if(playerScript.currentAttackType == AttackType.r)
                {

                    int R_attackLevel = character.attackLevels.Find(x => x.attackType == AttackType.r).level;
                    ScaleConditionsAndFactors scaleConditionsAndFactors = character.characterData.attackScalingConditions.Find(x => x.attackType == AttackType.r).conditions.Find(y => y.Level == R_attackLevel).scaleConditionsAndFactors.Find(x => x.scalingCondition == ScalingConditionTypes.SlowerForSomeTime);

                    target.SetSlowerSpeedEffect(scaleConditionsAndFactors.effectTime,scaleConditionsAndFactors.percentage);
                    // damange
                    MinionTargetDamage(target);
                    //
                }
                if(playerScript.currentAttackType == AttackType.q)
                {
                    // damange
                    MinionTargetDamage(target);
                    //
                }
                if(playerScript.currentAttackType == AttackType.w)
                {
                    // damange
                    MinionTargetDamage(target);
                    //
                }
                if(playerScript.currentAttackType == AttackType.e)
                {
                    // damange
                    MinionTargetDamage(target);
                    //
                }
            }
            else
            {
                MinionTargetDamage(target);
            }
        }
        foreach(TowerAIScript target in HitListTower)
        {

            TowerTargetDamage(target);
        }
        foreach(Character target in HitListChampions)
        {
            if(character.characterData.characterModel.characterType == CharacterType.Jahan)
            {
                if(playerScript.currentAttackType == AttackType.e)
                {
                    int E_attackLevel = character.attackLevels.Find(x => x.attackType == AttackType.e).level;
                    ScaleConditionsAndFactors scaleConditionsAndFactors = character.characterData.attackScalingConditions.Find(x => x.attackType == AttackType.e).conditions.Find(y => y.Level == E_attackLevel).scaleConditionsAndFactors.Find(x => x.scalingCondition == ScalingConditionTypes.SlowerForSomeTime);

                    target.playerScript.SetSpeedEffect(scaleConditionsAndFactors.effectTime,scaleConditionsAndFactors.percentage,false);
                }
            }
            else if(character.characterData.characterModel.characterType == CharacterType.Dira)
            {
                if(playerScript.currentAttackType == AttackType.r)
                {
                    int E_attackLevel = character.attackLevels.Find(x => x.attackType == AttackType.r).level;
                    ScaleConditionsAndFactors scaleConditionsAndFactors = character.characterData.attackScalingConditions.Find(x => x.attackType == AttackType.r).conditions.Find(y => y.Level == E_attackLevel).scaleConditionsAndFactors.Find(x => x.scalingCondition == ScalingConditionTypes.SlowerForSomeTime);

                    target.playerScript.SetSpeedEffect(scaleConditionsAndFactors.effectTime,scaleConditionsAndFactors.percentage,false);
                }
            }
            else
            {
                MinionChampionDamage(target);
            }
        }
        playerScript.currentAttackType = AttackType.None;  //Reset attack type
        HitList.Clear();
        HitListChampions.Clear();
    }
    /// <summary>
    /// Damage Minion target
    /// </summary>
    /// <param name="target"></param>
    public void MinionTargetDamage(MinionAIScript target)
    {
        DamageDetails damageDetails = GameManager.instance.currentCharacter.CalculateDamangeForAttack(playerScript.currentAttackType);
        damageDetails.damageById = characterId;
        damageDetails.damagedItem = DamagedItem.Minion;
        damageDetails.teamType = GameManager.instance.TeamPlayers.Find(x => x.Id == characterId).teamType == TeamType.Red ? TeamType.Blue : TeamType.Red;     //Set opposite type
        //target.DealDamage((float)GameManager.instance.GetCurrentAD());  //damage equal to character's current AD
        Debug.LogError("Scale Damage " + damageDetails.damangeValue);
        if(target)
            target.DealDamage(damageDetails);  //damage equal to character's current attack type and level scale conditions
    }
    /// <summary>
    /// Damage Tower target
    /// </summary>
    /// <param name="target"></param>
    public void TowerTargetDamage(TowerAIScript target)
    {
        DamageDetails damageDetails = GameManager.instance.currentCharacter.CalculateDamangeForAttack(playerScript.currentAttackType);
        damageDetails.damageById = characterId;
        damageDetails.damagedItem = DamagedItem.Tower;
        damageDetails.teamType = GameManager.instance.TeamPlayers.Find(x => x.Id == characterId).teamType == TeamType.Red ? TeamType.Blue : TeamType.Red;     //Set opposite type
        //target.DealDamage((float)GameManager.instance.GetCurrentAD());  //damage equal to character's current AD
        Debug.LogError("Scale Damage " + damageDetails.damangeValue);
        if(target)
            target.DealDamage(damageDetails);  //damage equal to character's current attack type and level scale conditions
    }
    // <summary>
    /// Damage Chamipon target
    /// </summary>
    /// <param name="target"></param>
    public void MinionChampionDamage(Character target)
    {
        DamageDetails damageDetails = GameManager.instance.currentCharacter.CalculateDamangeForAttack(playerScript.currentAttackType);
        damageDetails.damageById = characterId;
        damageDetails.damagedItem = DamagedItem.Character;
        damageDetails.teamType = target.teamType;     //Set opposite type
        //target.DealDamage((float)GameManager.instance.GetCurrentAD());  //damage equal to character's current AD
        Debug.LogError("*" + target.name + " Target : Scale Damage " + damageDetails.damangeValue);
        target.DealDamage(damageDetails);  //damage equal to character's current attack type and level scale conditions
    }
    /// <summary>
    /// Set current attack type and bool variable to indicate that player is attacking -This method is called from animation clips 
    /// </summary>
    /// <param name="_attackType">Current attack type</param>
    public void SetAttackIndicator(AttackType _attackType)
    {
        Attacking = true;
        playerScript.currentAttackType = _attackType;
    }
    /// <summary>
    /// Reset attack indicator bool
    /// </summary>
    public void ResetAttackIndicator()
    {
        Attacking = false;
        currentAttackDamangeType = DamageType.None;
    }
    /// <summary>
    /// Set damage type of current attack
    /// </summary>
    /// <param name="damageType">damage type : normal, area etc..</param>
    public void SetDamageType(DamageType damageType)
    {
        Attacking = false;
        currentAttackDamangeType = damageType;
    }
    /// <summary>
    /// Set charactor animators's root motion settings ON
    /// </summary>
    public void SetRootMotionOn()
    {
        playerScript.characterAnimator.applyRootMotion = true;
    }
    /// <summary>
    /// Set charactor animators's root motion settings OFF 
    /// </summary>
    public void SetRootMotionOff()
    {
        playerScript.characterAnimator.applyRootMotion = false;
    }
    /// <summary>
    /// Trigger destroy function in character main script
    /// </summary>
    public void TriggerDestroyCharacter()
    {
        //Temp Disabled
        // playerScript.CharacterDie();
    }
    /// <summary>
    /// Get collision detector object from list
    /// </summary>
    /// <returns>Collision object</returns>
    public CollisionDetector GetCollionsDetectorObject()
    {
        Debug.LogError("**Current Attack Type " + playerScript.currentAttackType);
        return collisionDetectorObjects.Find(x => x.damageType == currentAttackDamangeType && x.attackTypeForThisDetector == playerScript.currentAttackType).collisionDetector;
    }
    /// <summary>
    /// Spawn arrow/other throwable /shootable projectile : This method is called from animation clip 
    /// </summary>
    public void SpawnArrow()
    {
        Character character = GetComponentInParent<Character>();
        bool SerinaAutoAttack = false;

        //if(character.currentCharacterModel.characterType== CharacterType.Serina && character.playerScript.currentAttackType== AttackType.auto) 
        //{
        //    SerinaAutoAttack = true; 
        //}
        GameObject arrow = null;

        switch(character.characterData.characterModel.characterType)
        {
            case CharacterType.Otrill:
                arrow = Instantiate(OtrillAutoProjectile,SerinaAutoAttack ? spawnTransforSerinaAuto : spawnTrasform.parent);
                break;
            case CharacterType.Morya:

            case CharacterType.VaRun:

            case CharacterType.Misa:
            case CharacterType.Udara:


            case CharacterType.Sura:

            case CharacterType.Hakka:

            case CharacterType.Dira:

            case CharacterType.Tapani:

            case CharacterType.Moorg:

            case CharacterType.Jahan:
                arrow = Instantiate(SerinaQArrowPrefab,spawnTrasform.parent);
                arrow.transform.eulerAngles = spawnTrasform.eulerAngles;
                break;

            case CharacterType.Ranzeb:


            case CharacterType.Serina:
                arrow = Instantiate(playerScript.currentAttackType == AttackType.auto ? ArrowPrefab : playerScript.currentAttackType == AttackType.r
                    ? SerinaRArrowPrefab : SerinaQArrowPrefab,SerinaAutoAttack ? spawnTransforSerinaAuto : spawnTrasform.parent);
                break;
            default:
                break;
        }
        arrowProjectile = arrow.GetComponent<Projectile>();
        arrowProjectile.character = character;
        arrowProjectile.attackType = playerScript.currentAttackType;
        Debug.LogError(arrowProjectile.attackType + " From " + arrowProjectile.character.currentCharacterModel.characterType);
    }
    /// <summary>
    /// Spawn arrow/other throwable /shootable projectile : This method is called from animation clip 
    /// </summary>
    public void SpawnArrowForE()
    {
        Character character = GetComponentInParent<Character>();

        GameObject arrow = null;

        switch(character.characterData.characterModel.characterType)
        {
            case CharacterType.Otrill:
                break;
            case CharacterType.Morya:

            case CharacterType.VaRun:

            case CharacterType.Misa:
            case CharacterType.Udara:


            case CharacterType.Sura:

            case CharacterType.Hakka:

            case CharacterType.Dira:

            case CharacterType.Tapani:

            case CharacterType.Moorg:

            case CharacterType.Jahan:
                arrow = Instantiate(E_ProjectilPrefab,spawnTrasform.parent);
                break;

            case CharacterType.Ranzeb:


            case CharacterType.Serina:

                break;
            default:
                break;
        }
        arrowProjectile = arrow.GetComponent<Projectile>();
        arrowProjectile.character = character;
        arrowProjectile.attackType = AttackType.e;

        Debug.LogError(arrowProjectile.attackType + " From " + arrowProjectile.character.currentCharacterModel.characterType);
    }
    /// <summary>
    /// Spawn arrow/other throwable /shootable projectile for left hand : This method is called from animation clip 
    /// </summary>
    public void SpawnArrowForLeftHand()
    {
        Character currentChar = GameManager.instance.currentCharacter;
        GameObject arrow = null;
        switch(currentChar.characterData.characterModel.characterType)
        {
            case CharacterType.Otrill:

                arrow = Instantiate(ArrowPrefab,spawnTrasformLeft.parent);
                break;
            case CharacterType.Morya:
                break;
            case CharacterType.VaRun:
                break;
            case CharacterType.Misa:
                break;
            case CharacterType.Udara:
                break;
            case CharacterType.Sura:
                break;
            case CharacterType.Hakka:
                break;
            case CharacterType.Dira:
                break;
            case CharacterType.Tapani:
                break;
            case CharacterType.Moorg:
                break;
            case CharacterType.Jahan:
                arrow = Instantiate(ArrowPrefab,spawnTrasformLeft);
                arrow.transform.position = spawnTrasformLeft.position;
                arrow.transform.eulerAngles = spawnTrasformLeft.transform.eulerAngles;
                break;
            case CharacterType.Ranzeb:
                break;
            case CharacterType.Serina:
                break;
            default:
                break;
        }
        arrowProjectile = arrow.GetComponent<Projectile>();
        arrowProjectile.character = GetComponentInParent<Character>();
        arrowProjectile.attackType = playerScript.currentAttackType;
        Debug.LogError(arrowProjectile.attackType + " From " + arrowProjectile.character.currentCharacterModel.characterType);
    }
    /// <summary>
    /// Set shoot boolean in projectile script and shoot : This method is called from animation clip 
    /// </summary>
    public void ShootArrow()
    {
        if(arrowProjectile)
        {
            arrowProjectile.transform.SetParent(playerScript.transform.parent);
            arrowProjectile.Shoot();

        }
    }
    public void ShowHakkaHammer()
    {
        if(HakkaHammer)
        {

            HakkaHammerImpact.gameObject.SetActive(false);
            HakkaHammer.gameObject.SetActive(true);
        }
    }
    public void HideHakkaHammer()
    {
        if(HakkaHammer)
        {
            // HakkaHammerImpact.transform.position = ImpactTransform.transform.position;
            HakkaHammerImpact.gameObject.SetActive(true);
            HakkaHammer.gameObject.SetActive(false);
            Invoke(nameof(HideImpact),2f);
        }
    }
    public void ShowHakkaEVFX()
    {
        HakkaEEffect.SetActive(true);
    }
    public void HideHakkaE()
    {
        HakkaEEffect.SetActive(false);
    }
    public void HideImpact()
    {
        HakkaHammerImpact.SetActive(false);
    }
    public void SetOtrill_EVFXOn()
    {
        Otrill_E_VFX.gameObject.SetActive(true);
    }
    public void SetOtrill_EVFXOff()
    {
        Otrill_E_VFX.gameObject.SetActive(false);
    }
    public void EnableOtrill_R_VFX()
    {
        Otrill_R_Left.SetActive(true);
        Otrill_R_Right.SetActive(true);
    }
    public void DisableOtrill_R_VFX()
    {
        Otrill_R_Left.SetActive(false);
        Otrill_R_Right.SetActive(false);
    }
    public void EnableOtrill_W_AuraVFX()
    {
        if(DefaultAuraVFX)
        { DefaultAuraVFX.SetActive(false); }
        if(W_AuraVFX)
        { W_AuraVFX.SetActive(true); }
        if(HakkaW)
        {
            HakkaW.SetActive(true);
            LeanTween.scale(HakkaW,new Vector3(110,110,110),.1f);
        }
    }
    public void EnableMoorg_W_VFX()
    {
        if(Moorg_W_VFX)
        {
            Moorg_W_VFX.SetActive(true);
        }
    }
    public void DisableMoorg_W_VFX()
    {
        if(Moorg_W_VFX)
        {
            Moorg_W_VFX.SetActive(false);
        }
    }
    public void Enable_Sura_E_HealVFX()
    {
        if(SuraEVFX)
        {
            SuraEVFX.SetActive(true);
        }
    }
    public void Disable_Sura_E_HealVFX()
    {
        if(SuraEVFX)
        {
            SuraEVFX.SetActive(false);
        }
    }
    public void Enable_Sura_Q_VFX()
    {
        if(SuraEVFX)
        {
            SuraQVFX.SetActive(true);
        }
    }
    public void Trigger_Sura_Q_OnEnenmy()
    {
        DummyEnemy enemy = GameManager.instance.dummyEnemy.GetComponent<DummyEnemy>();
        GameObject vfxOnEnemy = Instantiate(Sura_Q_Enemy,enemy.Sura_Q_EnemyReferencePosition.position,Quaternion.identity,enemy.VFXParent);
        Destroy(vfxOnEnemy,4f);
    }
    public void Trigger_Moorg_Q_OnEnenmy()
    {
        DummyEnemy enemy = GameManager.instance.dummyEnemy.GetComponent<DummyEnemy>();
        GameObject vfxOnEnemy = Instantiate(Moorg_Q_Enemy,enemy.Moorg_Q_EnemyReferencePosition.position,enemy.Moorg_Q_EnemyReferencePosition.rotation,enemy.VFXParent);
        Destroy(vfxOnEnemy,4f);
    }

    public void Disable_Sura_Q_VFX()
    {
        if(SuraEVFX)
        {
            SuraQVFX.SetActive(false);
        }
    }
    public void TriggerSlowDownEffectOnTargets()
    {

    }
    public void ShowSura_W_VFX()
    {
        SuraWVFX.SetActive(true);
        Invoke(nameof(HideSura_W_VFX),4.5f);
    }
    public void HideSura_W_VFX()
    {
        SuraWVFX.SetActive(false);
    }
    public void DisableOtrill_W_AuraVFX()
    {
        if(DefaultAuraVFX)
        { DefaultAuraVFX.SetActive(true); }
        if(W_AuraVFX)
        { W_AuraVFX.SetActive(false); }
        if(HakkaW)
        {
            LeanTween.scale(HakkaW,new Vector3(110,110,0),.1f).setOnComplete(() => {
                HakkaW.SetActive(false);
            });
        }
    }
    //Otrill Death VFX
    public void DeathBeamAnimation()
    {
        BeamVFX.SetActive(true);
        GameManager.instance.currentCharacter.championHealthBar.gameObject.SetActive(false);
        CastShadowsOff();
        SetOtrill_EVFXOff();
        LeanTween.move(gameObject,dieAnimationEndPoint,1.0f).setDelay(1f);
    }
    //Serina death animation
    public void SerinaDeathAnimation()
    {
        SerinaDeathVFX.SetActive(true);
        GameManager.instance.currentCharacter.championHealthBar.gameObject.SetActive(false);
        CastShadowsOff();
    }
    //Jahan death animation
    public void JahanDeathAnimation()
    {
        JahanDeathVFX.SetActive(true);
        GameManager.instance.currentCharacter.championHealthBar.gameObject.SetActive(false);
        CastShadowsOff();
        if(DefaultAuraVFX)
        {
            DefaultAuraVFX.gameObject.SetActive(false);
        }
    }
    //Sura death animation
    public void SuraDeathAnimation()
    {
        SuraDeathVFX.SetActive(true);
        GameManager.instance.currentCharacter.championHealthBar.gameObject.SetActive(false);
        CastShadowsOff();
        if(DefaultAuraVFX)
        {
            DefaultAuraVFX.gameObject.SetActive(false);
        }
    }
    public void DisableBeam()
    {
        BeamVFX.SetActive(false);
    }
    public void CastShadowsOff()
    {
        foreach(SkinnedMeshRenderer item in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            item.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }
    }
    public void HideCharacter()
    {
        foreach(SkinnedMeshRenderer item in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            item.enabled = false;
        }
        Character currentChar = GameManager.instance.currentCharacter;
        if(currentChar.currentCharacterModel.characterType == CharacterType.Hakka)
        {
            Invoke(nameof(HideDeathVFX),2f);
        }
        if(currentChar.currentCharacterModel.characterType == CharacterType.Jahan)
        {
            Invoke(nameof(HideDeathVFX),2f);
        }
        if(currentChar.currentCharacterModel.characterType == CharacterType.Sura)
        {
            Invoke(nameof(HideDeathVFX),1f);
        }
    }
    public void HideCharacterOnly()
    {
        HideCharacterModel();
        GameManager.instance.cameraFollow.enabled = false;
        playerScript.transform.position = playerScript.GetComponent<Character>().teamType == TeamType.Blue ? GameManager.instance.blueSpawnLocation : GameManager.instance.redSpawnLocation;

        Invoke(nameof(ShowCharacter),1.5f);
    }

    private void ShowCharacter()
    {
        foreach(SkinnedMeshRenderer item in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            item.enabled = true;
        }
        GameManager.instance.cameraFollow.enabled = true;
    }
    private void HideCharacterModel()
    {
        foreach(SkinnedMeshRenderer item in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            item.enabled = false;
        }
    }

    public void ShowHakkaQ()
    {
        if(HakkaQVFX)
        {
            HakkaQVFX.SetActive(true);
            Invoke(nameof(HideHakkaQ),2f);
        }
    }
    public void HideHakkaQ()
    {
        if(HakkaQVFX)
        {
            HakkaQVFX.SetActive(false);
        }
    }
    public void SpawnTargetLine()
    {
        if(TargetLine)
        {
            GameObject line = Instantiate(TargetLine,playerScript.transform.parent);

        }
    }
    public void ShowHakkaDeathVFX()
    {
        HakkaDeathVFX.SetActive(true);
        GameManager.instance.currentCharacter.championHealthBar.gameObject.SetActive(false);
        CastShadowsOff();
    }
    public void HideDeathVFX()
    {
        if(HakkaDeathVFX)
            HakkaDeathVFX.SetActive(false);
        if(JahanDeathVFX)
            JahanDeathVFX.SetActive(false);
        if(SuraDeathVFX)
        {
            SuraDeathVFX.SetActive(false);
        }
        if(DefaultAuraVFX)
        {
            DefaultAuraVFX.SetActive(false);
        }
    }
    public void ShowMoorgUltMesh()
    {
        MoorgUltMesh.SetActive(true);
        MoorgNormalMesh.SetActive(false);
        Invoke(nameof(HideMoorgUltMesh),4f);
    }
    public void HideMoorgUltMesh()
    {
        MoorgNormalMesh.SetActive(true);
        MoorgUltMesh.SetActive(false);
        Moorg_Ult_VFX.SetActive(false);
    }
    public void RecallProcess()
    {
        Invoke(nameof(HideCharacterOnly),5.5f);

    }
    public void EnableMoorg_UltVFX()
    {
        Moorg_Ult_VFX.SetActive(true);
        Invoke(nameof(ShowMoorgUltMesh),1.5f);
    }
    public void EnableSuraHandBladeForUlt()
    {
        SuraBladeBack.gameObject.SetActive(false);
        SuraBladeInHand.gameObject.SetActive(true);
    }
    public void DisableSuraHandBlade()
    {
        SuraBladeBack.gameObject.SetActive(true);
        SuraBladeInHand.gameObject.SetActive(false);
    }
    public void EnableMoorg_E_SlashVFx()
    {
        Instantiate(Moorg_E_SlashVFX ,MoorgEVFXRef.position,MoorgEVFXRef.rotation,transform.parent.parent.parent);
      //  Moorg_E_SlashVFX.SetActive(true);
    }
    public void DisableMoorg_E_SlashVFx()
    {
      //  Moorg_E_SlashVFX.SetActive(false);
    }
}
[System.Serializable]
/// <summary>
/// Used to create instance of that holds collision detector details
/// </summary>
public class CollisionDetectorObject
{
  public CollisionDetector collisionDetector;  //reference script of collision detector object
  public DamageType damageType;               //damage type on collision
  public AttackType attackTypeForThisDetector; //Attack type for which this object should be selected
    public OverlapType collisionDetectionShapeType; //Shape type to detect collisions
}
public enum OverlapType 
{
    Box,Sphere
}
/// <summary>
/// Used to set different spawn positions and projectiles with respect to attack
/// </summary>
public class ProjectileSpawnDetails
{
    public Transform projectileSpawnPosition;  //spawn position of projectile 
    public GameObject projectilePrefab;        //Projectile to  shoot
    public AttackType AttackType;               //damage type on collision
}
[System.Serializable]
/// <summary>
/// Damage tpye : Normal damge ,area damage etc..
/// </summary>
public enum DamageType 
{
   None,Normal, Area,LeftNormal,RightNormal
}