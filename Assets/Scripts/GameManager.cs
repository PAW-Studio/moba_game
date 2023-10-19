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

    public GameObject characterPrefab;                                                  //Character prefab object for Instantiation


    public List<AttackButton> AttackButtons = new List<AttackButton>();               //Attack buttons reference
    public List<AttackType> AttackTypes = new List<AttackType>();                     //Attack types list
    public Character currentCharacter;                                               //Character script reference for current character
    public static GameManager instance;                                              //Set instance of GameManage script
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
    TMPro.TMP_Dropdown QalityDropdown;                                              //Graphic quality dropdown 
   
   
    private void Awake()
    {
        if(instance == null) 
        {
            instance = this;
        }
    }
    void Start()
    {
        Debug.LogError(QualitySettings.GetQualityLevel());
        //spawn character
        SpawnCharacter();
        qLevel = 2;
        QalityDropdown.value = qLevel;
    }
    int qLevel = 0;
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
        SpawnTime();
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
            meleeMinions[i] = Instantiate(meleeMinion,blueSpawnLocation,Quaternion.identity);
            meleeMinions[i].GetComponent<MinionAIScript>().destination = redSpawnLocation;

            // Blue minions
            meleeMinions[i].GetComponent<MinionAIScript>().isBlue = true;

            meleeMinions[i] = Instantiate(meleeMinion,redSpawnLocation,Quaternion.identity);
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
            casterMinions[i] = Instantiate(casterMinion,blueSpawnLocation,Quaternion.identity);
            casterMinions[i].GetComponent<MinionAIScript>().destination = redSpawnLocation;

            // Blue minions
            casterMinions[i].GetComponent<MinionAIScript>().isBlue = true;

            casterMinions[i] = Instantiate(casterMinion,redSpawnLocation,Quaternion.identity);
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

            cannonMinion1 = Instantiate(cannonMinion,blueSpawnLocation,Quaternion.identity);
            cannonMinion1.GetComponent<MinionAIScript>().destination = redSpawnLocation;
            cannonMinion1.GetComponent<MinionAIScript>().isBlue = true;

            cannonMinion1 = Instantiate(cannonMinion,redSpawnLocation,Quaternion.identity);
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
        GameObject character = Instantiate(characterPrefab,characterSpawnTranform.position,Quaternion.identity,characterSpawnTranform.parent); cameraFollow.SetPlayerAndOffset(character.transform);

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
    
}
//Handle attack buttton UI with this class
[System.Serializable]
public class AttackButton 
{
    public AttackType attackType;
    public Button button;
    public Image ActiveIndicator;
}