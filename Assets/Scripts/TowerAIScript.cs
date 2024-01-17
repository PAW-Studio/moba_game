using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TowerAIScript : MonoBehaviour
{
    public GameObject targetMinion;
    public bool hasTarget = false;
    public float attackRange = 10f;
    public float attackTimer = 2f;
    public float attackReset = 2f;
    public float damage = 30f;
    public float maxHealth = 100f;
    public float currentHealth;
    public TeamType teamType;
    public MinionHealthBar minionHealthBar;
    public GameObject TargetIndicator;                                      //Target  Inication (ring) object
    [SerializeField]
    public GameObject referenceObject;                                      //Healthbar display reference object for the tower
    Transform healthBarTransform;
    Camera cam;
    public float Gold, Xp;
    public TowerDetails towerDestroyDetails;
    // Start is called before the first frame update
    void Start()
    {
        towerDestroyDetails = GameManager.instance.TowerDestroyDetails;
        maxHealth = towerDestroyDetails.MaxHealth;
        GameObject Healthbar = Instantiate(GameManager.instance.TowerHealthBar,GameManager.instance.MinionHealthbarsParent);
        Healthbar.transform.localScale = Vector3.one;
        Healthbar.name = "Tower";
        minionHealthBar = Healthbar.GetComponent<MinionHealthBar>();
        healthBarTransform = minionHealthBar.transform;
        cam = FindObjectOfType<Camera>();
        //
        currentHealth = maxHealth;
        minionHealthBar.SetMaxHealth(maxHealth);

        // Similar to MinionAIScript
        if (gameObject.tag == "BlueTower")
        {
            this.gameObject.layer = 11;
            teamType = TeamType.Blue;
        }
        
        else if (gameObject.tag == "RedTower")
        {
            this.gameObject.layer = 12;
            teamType = TeamType.Red;
        }
        if(referenceObject)         //Handle exception for null reference 
        {
            healthBarTransform.position = cam.WorldToScreenPoint(referenceObject.transform.position);   //Set position of healthbar continuously at healbar reference position for the minion
        }
    }
    private void FixedUpdate()
    {
        if(referenceObject && healthBarTransform)         //Handle exception for null reference 
        {
            healthBarTransform.position = cam.WorldToScreenPoint(referenceObject.transform.position);   //Set position of healthbar continuously at healbar reference position for the minion
        }
    }
    // Update is called once per frame
    void Update()
    {
        // Similar to MinionAIScript
        if (hasTarget && targetMinion != null)
        {
            attackTimer = attackTimer - Time.deltaTime;

            if (attackTimer <= 0)
            {
                attackTimer = attackReset;
                
                if(targetMinion.GetComponent<MinionAIScript>()) 
                {
                    targetMinion.GetComponent<MinionAIScript>().currentHealth -= damage;
                    DamageDetails damageDetails = new DamageDetails();
                    damageDetails.damageById = -1;
                    damageDetails.damagedItem = DamagedItem.Minion;
                    damageDetails.damagePosition = targetMinion.transform.position;
                    damageDetails.damangeValue = damage;
                    damageDetails.teamType = teamType == TeamType.Red ?  TeamType.Blue : TeamType.Red;
                    targetMinion.GetComponent<MinionAIScript>().minionHealthBar.SetHealth(targetMinion.GetComponent<MinionAIScript>().currentHealth,true,targetMinion.gameObject,damageDetails);
                }
                
            }
        }

        if (targetMinion == null)
        {
            hasTarget = false;
        }

        if (currentHealth <= 0 && !destroyOnce)
        {
            destroyOnce = true;
            DamageDetails damageDetails = new DamageDetails();
            damageDetails.damageById = -1;
            damageDetails.damagedItem = DamagedItem.Tower;
            damageDetails.damagePosition = transform.position;
            damageDetails.damangeValue = damage;
            damageDetails.teamType = teamType;
            minionHealthBar.slider.gameObject.SetActive(false);
            AnimateAndDestoryTower(damageDetails);
            //Destroy(this.gameObject);
        }
    }
    bool destroyOnce = false;
    bool destroyAnimationStarted = false;
    public void AnimateAndDestoryTower(DamageDetails damageDetails) 
    {
        if(destroyAnimationStarted) return;

        destroyAnimationStarted = true; 
        Vector3 position = gameObject.transform.position;

        Vector3 positionRefForLeftRightAnimation = position;
        positionRefForLeftRightAnimation.x += .5f;
        position.y = -15f;
      
      int id=  LeanTween.moveX(gameObject,positionRefForLeftRightAnimation.x,0.1f).setLoopPingPong().id;
        LeanTween.move(gameObject,new Vector3(gameObject.transform.position.x, position.y,position.z),.5f).setDelay(0.5f).setOnComplete(() => 
        {
            //Destroy(gameObject); 
            LeanTween.cancel(id);
            TowerAIScript towerAIScript = gameObject.GetComponent<TowerAIScript>();
            towerAIScript.enabled = false;
            List<Collider> colliders = gameObject.GetComponentsInChildren<Collider>().ToList();
            foreach(Collider item in colliders)
            {
                item.enabled = false;
            }
            TowerAIScript tower = GameManager.instance.GetTargetUIManager().towerTarget;
            
            if(tower && tower== towerAIScript)
            {
                GameManager.instance.ShowTargetDetailsUI(false);
            }
            PopUpsManager.Instance.ShowTowerDestroyPopup();
            GameManager.instance.TriggerGoldRewardForPlayersForTowerDestroy(teamType,transform.position,GameManager.instance.TowerDestroyDetails);
        });
    }
    /// <summary>
    /// Handle damage and update healthbar
    /// </summary>
    /// <param name="damage">damage value</param>
    public void DealDamage(DamageDetails damageDetails = null)
    {
        if(damageDetails.damangeValue <= 0) return;
        Debug.LogError(GameManager.instance.currentCharacter.playerScript.currentAttackType);
        currentHealth -= damageDetails.damangeValue;
        damageDetails.damagePosition = transform.position;
        minionHealthBar.SetHealth(currentHealth,true,null,damageDetails);
        GameManager.instance.UpdateTargetDetailsUI();
    }
    /// <summary>
    /// Show/Hide indicator objects
    /// </summary>
    /// <param name="show">Show indicator</param>
    public void ShowIndicator(bool show)
    {
        if(TargetIndicator)
        TargetIndicator.SetActive(show);
        if(minionHealthBar)
        minionHealthBar.ShowOutline(show);
    }

}
