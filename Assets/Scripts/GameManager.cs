using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public GameObject meleeMinion;
    public GameObject casterMinion;
    public GameObject cannonMinion;
    public Vector3 blueSpawnLocation = new Vector3(-159,1,-152);
    public Vector3 redSpawnLocation = new Vector3(132,1,140);
    public Transform characterSpawnTranform;
    public GameObject MinioinHealthBar;                                                 //Healthbar prefab for minions
    public Transform MinionHealthbarsParent;                                            //Parent transform for minion healthbars
    public Transform MeleeMinionParentContainer;                                        //Parent transform for melee minions
    public Transform CasterMinionParentContainer;                                        //Parent transform for caster minions
    public Transform CannonMinionParentContainer;                                        //Parent transform for cannon minions
    public GameObject characterPrefab;                                                  //Character prefab object for Instantiation


    public List<AttackButton> AttackButtons = new List<AttackButton>();               //Attack buttons reference
    public List<AttackType> AttackTypes = new List<AttackType>();                     //Attack types list
    public Character currentCharacter;                                               //Character script reference for current character
    public bool canSpawnNextWave = true;                                            //Used to stop spawning waves or pause waves
    public static GameManager instance;                                              //Set instance of GameManage script

    //Temp
    public Vector3 CharacterLastPosition;                                           //Last position of character before death

  

    [SerializeField]
    CameraFollow cameraFollow;                                                       //Reference for camerafollow script


    float TimeInterval = 10f;
    float timer = 0f;
    float spawnDelay = 30f;
    int waveCount = 0;
    [SerializeField]
    Image RButton;                                                                  //RButton image reference 
    // Start is called before the first frame update
    [SerializeField]
    TMPro.TMP_Dropdown QalityDropdown;                                              //Graphics quality dropdown 
    int qLevel = 0;                                                                 //Graphics quality level index

    [SerializeField]
    float FirstWaveSpawnDelay=90f;                                                  //Delay time after which first wave should spawn (time in seconds)
    [SerializeField]
    float IntervalBetweenWaves = 30f;                                               //Time interval between waves (in seconds)
    [SerializeField]
    int SpawnCannonAfterWaves = 3;                                                  //Used to decide after how many waves the cannon should spawn
    int WaveCounter=0;                                                              //Counts waves for calculations
    private void Awake()
    {
        if(instance == null) 
        {
            instance = this;
        }
    }
    /// <summary>
    /// Spawn character and set grahics quality level
    /// </summary>
    void Start()
    {
        //spawn character
        SpawnCharacter();
        qLevel = 2;                                         //Default graphics quality level-2 (Medium) : (0->VeryLow,1->Low,2->Medium,3->High,4->VeryHigh,5->Ultra
        QalityDropdown.value = qLevel;                      //Set Temporary dropdown value as per current graphics quality level

        StartCoroutine(MinionSpawnCoroutine());            //Trigger minion waves coroutine
    }
    /// <summary>
    /// Change graphics quality level
    /// </summary>
    public void ChangeQuality() 
    {
        qLevel = QalityDropdown.value;
        QualitySettings.SetQualityLevel(qLevel);
        qLevel += 1;
        Debug.LogError(qLevel);
        if(qLevel == 6) qLevel = 0;
    }  
    // Update is called once per frame
    void Update()
    {
      //  SpawnTime();
    }
    

    void SpawnTime()
    {
        // Starting game timer
        timer += Time.deltaTime;

        if(timer >= TimeInterval)
        {
            MeleeMinionSpawn();
            CasterMinionSpawn();

            // Resetting timer for subsequent waves
            timer -= spawnDelay;
            waveCount++;

            CannonMinionSpawn();
        }
    }

    void MeleeMinionSpawn()
    {
        // Spawning and setting destination for melee minions
        GameObject[] meleeMinions = new GameObject[3];

        for(int i = 0 ; i < 3 ; i++)
        {
            meleeMinions[i] = Instantiate(meleeMinion,blueSpawnLocation,Quaternion.identity,MeleeMinionParentContainer);
            meleeMinions[i].GetComponent<MinionAIScript>().destination = redSpawnLocation;

            // Blue minions
            meleeMinions[i].GetComponent<MinionAIScript>().isBlue = true;

            meleeMinions[i] = Instantiate(meleeMinion,redSpawnLocation,Quaternion.identity,MeleeMinionParentContainer);
            meleeMinions[i].GetComponent<MinionAIScript>().destination = blueSpawnLocation;

            // Red minions
            meleeMinions[i].GetComponent<MinionAIScript>().isBlue = false;
        }
    }

    void CasterMinionSpawn()
    {
        // Spawning and setting destination for caster minions
        GameObject[] casterMinions = new GameObject[3];

        for(int i = 0 ; i < 3 ; i++)
        {
            casterMinions[i] = Instantiate(casterMinion,blueSpawnLocation,Quaternion.identity,CasterMinionParentContainer);
            casterMinions[i].GetComponent<MinionAIScript>().destination = redSpawnLocation;

            // Blue minions
            casterMinions[i].GetComponent<MinionAIScript>().isBlue = true;

            casterMinions[i] = Instantiate(casterMinion,redSpawnLocation,Quaternion.identity,CasterMinionParentContainer);
            casterMinions[i].GetComponent<MinionAIScript>().destination = blueSpawnLocation;

            // Red minions
            casterMinions[i].GetComponent<MinionAIScript>().isBlue = false;

        }
    }

    void CannonMinionSpawn()
    {
        // Every third wave spawns a cannon minion and sets destination as above
        if(waveCount == 3)
        {
            GameObject cannonMinion1;

            cannonMinion1 = Instantiate(cannonMinion,blueSpawnLocation,Quaternion.identity,CannonMinionParentContainer);
            cannonMinion1.GetComponent<MinionAIScript>().destination = redSpawnLocation;
            cannonMinion1.GetComponent<MinionAIScript>().isBlue = true;

            cannonMinion1 = Instantiate(cannonMinion,redSpawnLocation,Quaternion.identity,CannonMinionParentContainer);
            cannonMinion1.GetComponent<MinionAIScript>().destination = blueSpawnLocation;
            cannonMinion1.GetComponent<MinionAIScript>().isBlue = false;

            // Resetting wave count for next cannon minion wave
            waveCount = 0;
        }
    }
    /// <summary>
    /// Spawns character at given transform position and sets parent of character transform, assigns attack click methods for the character
    /// </summary>
    public void SpawnCharacter()
    {
        Vector3 spawnPosition = characterSpawnTranform.position;
        if(CharacterLastPosition != Vector3.zero) 
        {
            spawnPosition = CharacterLastPosition;
        }
        GameObject character = Instantiate(characterPrefab,spawnPosition,Quaternion.identity,characterSpawnTranform.parent); cameraFollow.SetPlayerAndOffset(character.transform);

        Character characterScirpt = character.GetComponent<Character>();
        for(int i = 0 ; i < AttackButtons.Count ; i++)
        {
            int val = characterScirpt.AttackValues[i];
            AttackType type = AttackTypes[i];
            AttackButtons[i].button.onClick.AddListener(() => characterScirpt.playerScript.InitiateAttack(val,type));
        }
        currentCharacter = characterScirpt;
    }
    //Trigger attack active coroutine 
    public void TriggerAttackActiveCoroutine(AttackType attackType,float duration) 
    {
        StartCoroutine(Attack_ActiveCoroutine(attackType,duration));
    }
    /// <summary>
    /// Indicates that attack is active 
    /// </summary>
    /// <param name="attackType">Attack which is initiated</param>
    /// <param name="activeTime">Duration till the attack will be active</param>
    public IEnumerator Attack_ActiveCoroutine(AttackType attackType,float activeTime) 
    {
        AttackButton attackButton = AttackButtons.Find(x => x.attackType == attackType);
      //  attackButton.button.interactable = false;
        float time = 0f,duration=activeTime;
        float start = 0, end = 1;
        if(attackButton.ActiveIndicator)
        {
            while(time < duration)
            {
                attackButton.ActiveIndicator.fillAmount = Mathf.Lerp(start,end,time / duration);
                time += Time.deltaTime;
                yield return null;
            }
            attackButton.ActiveIndicator.fillAmount = 1;
        }
        else 
        {
            yield return new WaitForSeconds(activeTime);
        }
       // attackButton.button.interactable = true;
        attackButton.ActiveIndicator.fillAmount = 0;
    }

    //Temp to change character
    public void ChangeCharacter()
    {
        currentCharacter.ChangeCharacter();
    }
    /// <summary>
    /// Trigger die animation for current player character
    /// </summary>
    public void TriggerDieAnimation()
    {
        currentCharacter.playerScript.TriggerDeathAnimation();
    }
    /// <summary>
    /// Trigger minion spawn after delay
    /// </summary>
    public IEnumerator MinionSpawnCoroutine()
    {
        yield return new WaitForSeconds(FirstWaveSpawnDelay);     //Wait for delay 
       StartCoroutine(SpawnWave(0));  //Start firs wave
    }
    /// <summary>
    /// Spawns minions after the given time interval
    /// </summary>
    public IEnumerator SpawnWave(float delay=-1f)
    {
        yield return new WaitForSeconds(delay!=-1?delay: IntervalBetweenWaves );  //If delay is specified then use it
        if(canSpawnNextWave)
        {
            WaveCounter += 1;
            MeleeMinionSpawn();
            CasterMinionSpawn();

            if(WaveCounter % SpawnCannonAfterWaves == 0)                // checks that if the wave is multiple of the given variable for spawning cannon or not
            {
                CannonMinionSpawn();
            }
            
        }
      StartCoroutine(SpawnWave());                              //Continue spawn coroutine                                             
    }
    /// <summary>
    /// Starts coroutine for attack cooldown timer and image update
    /// </summary>
    /// <param name="activeTime">attack active time</param>
    /// <param name="timerValue">cooldown time</param>
    /// <param name="attackType">current attack type</param>
    public void StartTimeTextUpdate(float activeTime,float timerValue,AttackType attackType) 
    {
       StartCoroutine( AttackButtons.Find(x => x.attackType == attackType).UpdateTimerText(activeTime,timerValue));
    }
    /// <summary>
    /// Stop coroutine on disable
    /// </summary>
    private void OnDisable()
    {
        StopCoroutine(SpawnWave());   
    }
}
//Handle attack buttton UI with this class
[System.Serializable]
public class AttackButton 
{
    public AttackType attackType;                           //Type of attack
    public Button button;                                   //Button object for this attack
    public Image ActiveIndicator;                           //Image that indicates that attack is active    
    public GameObject DeactiveIndicator;                    //Gameobject that covers attack button while the attack is on cooldown
    public TMPro.TextMeshProUGUI coolDownTimer;             //Remaining time display object while cooldown is on
    public TMPro.TextMeshProUGUI attackName;                //Object reference for attack name text 
    /// <summary>
    /// Update remaining time text on button while attack button is on cooldown
    /// </summary>
    /// <param name="val">coolddown time</param>
    public void UpdateTime(float val) 
    {
        string timerText = "";
        if(val > 1)
        {
            timerText =System.Convert.ToInt32(val).ToString();
            Debug.LogError(timerText);
        }
        else
        {          
            timerText = val.ToString("F1");
        }
        coolDownTimer.text = timerText.ToString();
    }
    /// <summary>
    /// Method to trigger coroutine for continuously update remaining time and image fill amount 
    /// </summary>
    /// <param name="activeTime">activet time of attack</param>
    /// <param name="timerValue">cooldown time value</param>
    public void StartTimerUpdateCoroutine(float activeTime,float timerValue) 
    {
        GameManager.instance.StartTimeTextUpdate(activeTime,timerValue,attackType);     
    }
    /// <summary>
    /// Updates timer text and image fill amount of attack button while button is on cooldown mode
    /// </summary>
    /// <param name="activeTime">active time of attack</param>
    /// <param name="delay">coold down time</param>
    /// <returns></returns>
    public IEnumerator UpdateTimerText(float activeTime,float delay)
    {
        yield return new WaitForSeconds(activeTime);
        attackName.gameObject.SetActive(false);
        coolDownTimer.gameObject.SetActive(true);
        Image InactiveImage = DeactiveIndicator.GetComponent<Image>();
        InactiveImage.fillAmount = 1;
        float time = delay;
        while(time > 0)
        {
           // float timeRemaining = Mathf.Lerp(time,0,time / delay);
            float InactiveImageFill = Mathf.Lerp(0,1,time / delay);
            InactiveImage.fillAmount = InactiveImageFill;
            time -= Time.deltaTime;
            UpdateTime(time);
            yield return null;
        }
        UpdateTime(0);
        InactiveImage.fillAmount = 0;
        coolDownTimer.gameObject.SetActive(false);
        attackName.gameObject.SetActive(true);
    }
}