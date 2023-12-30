using System.Collections;
using System.Collections.Generic;
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

    [SerializeField]
    public GameObject referenceObject;                                      //Healthbar display reference object for the tower
    Transform healthBarTransform;
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
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
                    targetMinion.GetComponent<MinionAIScript>().minionHealthBar.SetHealth(targetMinion.GetComponent<MinionAIScript>().currentHealth,true,targetMinion.gameObject);
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
         
            minionHealthBar.slider.gameObject.SetActive(false);
            AnimateAndDestoryTower();
            //Destroy(this.gameObject);
        }
    }
    bool destroyOnce = false;
    bool destroyAnimationStarted = false;
    public void AnimateAndDestoryTower() 
    {
        if(destroyAnimationStarted) return;

        destroyAnimationStarted = true; 
        Vector3 position = gameObject.transform.position;

        Vector3 positionRefForLeftRightAnimation = position;
        positionRefForLeftRightAnimation.x += .5f;
        position.y = -15f;
      
      int id=  LeanTween.moveX(gameObject,positionRefForLeftRightAnimation.x,0.1f).setLoopPingPong().id;
        LeanTween.move(gameObject,new Vector3(gameObject.transform.position.x, position.y,position.z),.5f).setDelay(0.5f).setOnComplete(() => {
            //Destroy(gameObject); 
            LeanTween.cancel(id);
            gameObject.GetComponent<TowerAIScript>().enabled = false;
            gameObject.GetComponent<Collider>().enabled = false;
        });
    }
    /// <summary>
    /// Handle damage and update healthbar
    /// </summary>
    /// <param name="damage">damage value</param>
    public void DealDamage(float damage)
    {
        if(damage <= 0) return;
        Debug.LogError(GameManager.instance.currentCharacter.playerScript.currentAttackType);
        currentHealth -= damage;
        minionHealthBar.SetHealth(currentHealth,true);
    }

}
