using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MinionAIScript : MonoBehaviour
{
    public Vector3 destination;

    public Material blueMinionMat;
    public Material redMinionMat;

    public GameObject targetMinion;

    public bool isBlue;
    public bool hasTarget = false;
    public float maxHealth = 100f;
    public float currentHealth;
    public float attackTimer = 2f;
    public float attackReset = 2f;
    public float damage = 30f;
    public float attackRange = 10f;
    public Vector3 offset = new Vector3(5,0,5);
    public TeamType teamType;                    //Minion team type
    UnityEngine.AI.NavMeshAgent agent;
    Renderer renderer;
    [SerializeField]
    public GameObject referenceObject;                                      //Healthbar display reference object for the minion

    public MinionHealthBar minionHealthBar;
    public GameObject TargetIndicator;
    Camera cam;
    Transform healthBarTransform;
    float OriginalSpeed;
    float DecreasedSpeed;
    bool DecreasedSpeedEffect;
    float TimePassed = 0;
    float SlowEffectTime;
    
    // Start is called before the first frame update
    void Start()
    { 
        //Instntiate healthbar for the minion and set it in canvas and set proper scale 
        GameObject Healthbar = Instantiate(GameManager.instance.MinioinHealthBar,GameManager.instance.MinionHealthbarsParent);
        Healthbar.transform.localScale = Vector3.one;
        minionHealthBar = Healthbar.GetComponent<MinionHealthBar>();
        healthBarTransform = minionHealthBar.transform;
        cam = FindObjectOfType<Camera>();
        //

        // Caching references
        renderer = GetComponent<Renderer>();
        agent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
       // Save original speed for reference
          OriginalSpeed = agent.speed ;
       // Decreased speed effect false on start
          DecreasedSpeedEffect = false;
       // Setting minion health bar
        currentHealth = maxHealth;
        minionHealthBar.SetMaxHealth(maxHealth);
        if(referenceObject)         //Handle exception for null reference 
        {
            healthBarTransform.position = cam.WorldToScreenPoint(referenceObject.transform.position);   //Set position of healthbar continuously at healbar reference position for the minion
        }
        Invoke(nameof(ShowHealthBar),0.3f);

        // Setting type of minion based on layer
        if(isBlue)
        {
            renderer.material = blueMinionMat;
            this.gameObject.layer = 9;
            teamType = TeamType.Blue;
        }
        else
        {
            renderer.material = redMinionMat;
            this.gameObject.layer = 10;
            teamType = TeamType.Red;
        }
    
        agent.SetDestination(destination);
    }
    /// <summary>
    /// Set active healthvar object on
    /// </summary>
    public void ShowHealthBar() 
    {
        minionHealthBar.gameObject.SetActive(true);
    }
    /// <summary>
    /// Set original speed and reset decreased speed effect
    /// </summary>
    private void SetOriginalSpeed()
    {
        DecreasedSpeedEffect = false;
        agent.speed = OriginalSpeed;
        SlowEffectTime = 0;
        TimePassed = 0;
        descreasedOne = false;
    }
    bool descreasedOne = false; //To ensure only one time decrease the speed 
    // Update is called once per frame
    void Update()
    {
        if(DecreasedSpeedEffect)
        {
            TimePassed += Time.deltaTime;
            if(TimePassed > SlowEffectTime)
            {
                SetOriginalSpeed();
            }
        }
        if(hasTarget && targetMinion != null)
        {
            if(targetMinion.layer == 11 || targetMinion.layer == 12)
            {
                if(targetMinion.tag == "BlueTower")
                {
                    MoveToBlueTower();
                }

                else if(targetMinion.tag == "RedTower")
                {
                    MoveToRedTower();
                }
            }

            else
            {
                MoveToMinion();
            }

            attackTimer = attackTimer - Time.deltaTime;

            if(attackTimer <= 0)
            {
                attackTimer = attackReset;

                InitiateAttack();
            }
        }

        if(targetMinion == null)
        {
            // No target; resumes pathing towards destination
            hasTarget = false;
            agent.SetDestination(destination);
        }

       
        //if(currentHealth <= 0)
        //{
        //    Destroy(minionHealthBar.gameObject);
        //    Destroy(this.gameObject);
        //}

    }

    private void FixedUpdate()
    {
        if(referenceObject && healthBarTransform)         //Handle exception for null reference 
        {
           healthBarTransform.position = cam.WorldToScreenPoint(referenceObject.transform.position);   //Set position of healthbar continuously at healbar reference position for the minion
        }
    }
    void MoveToMinion()
    {   
        // Calculating distance between this minion and target
        if(Vector3.Distance(transform.position,targetMinion.transform.position) > attackRange)
        {
            agent.SetDestination(targetMinion.transform.position);
            // Minion stops at attackRange distance from target 
            agent.stoppingDistance = attackRange;
            
        }
        else
        {
            // If target minion is less than attackRange distance away, moves towards it
            agent.SetDestination(targetMinion.transform.position);
            agent.velocity = Vector3.zero;   
        }
    }

    void MoveToBlueTower()
    {
        agent.SetDestination(targetMinion.transform.position + offset);
    }

    void MoveToRedTower()
    {
        agent.SetDestination(targetMinion.transform.position - offset);
    }

    void InitiateAttack()
    {
        // Attacks opposite tower
        if(targetMinion.layer == 11 || targetMinion.layer == 12)
        {
            targetMinion.GetComponent<TowerAIScript>().currentHealth -= damage;

            // Reduces tower health from current health bar
            targetMinion.GetComponent<TowerAIScript>().minionHealthBar.SetHealth(targetMinion.GetComponent<TowerAIScript>().currentHealth,true,targetMinion.gameObject);
        }
        else
        {
            if(targetMinion.GetComponent<Character>())
            {
               // targetMinion.GetComponent<Character>().DealDamage(0);
            }
            else 
            {
                // Attacks opposite minion and reduces minion health from current health bar
                //targetMinion.GetComponent<MinionAIScript>().currentHealth -= damage;
                //targetMinion.GetComponent<MinionAIScript>().minionHealthBar.SetHealth(targetMinion.GetComponent<MinionAIScript>().currentHealth,true,targetMinion.gameObject);
            }
        }
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
        minionHealthBar.SetHealth(currentHealth,true,gameObject);
    }
    /// <summary>
    /// Set slower movement speed for the given time
    /// </summary>
    /// <param name="slowerEffectTime">effect time</param>
    /// <param name="percentage">speed decrease percentage with resepect to current speed</param>
    public void SetSlowerSpeedEffect(float slowerEffectTime, float percentage) 
    {
        DecreasedSpeedEffect = true;
        descreasedOne = true;
        DecreasedSpeed = agent.speed - (agent.speed * (percentage / 100));
        SlowEffectTime = slowerEffectTime;
        agent.speed = DecreasedSpeed;
    }
    /// <summary>
    /// Show/Hide indicator objects
    /// </summary>
    /// <param name="show">Show indicator</param>
    public void ShowIndicator(bool show) 
    {
        TargetIndicator.SetActive(show);
        minionHealthBar.ShowOutline(show);
       
    }
    public void NewTarget() { }
}
