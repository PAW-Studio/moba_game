using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject meleeMinion;
    public GameObject casterMinion;
    public GameObject cannonMinion;
    public List<Transform> LeftLaneMinionSpawnPositions_Blue, MidLaneMinionSpawnPositions_Blue, RightLaneMinionSpawnPositions_Blue, LeftLaneMinionSpawnPositions_Red, MidLaneMinionSpawnPositions_Red, RightLaneMinionSpawnPositions_Red;
    public Vector3 blueSpawnLocation = new Vector3(-159,1,-152);
    public Vector3 redSpawnLocation = new Vector3(132,1,140);
    public Transform blueLeftLaneMidPoint, blueRightLaneMidPoint;                       //Mid points
    public Transform characterSpawnTranform;
    public GameObject MinioinHealthBar;                                                 //Healthbar prefab for minions
    public GameObject TowerHealthBar;                                                 //Healthbar prefab for towers
    public GameObject ChampionHealthBar;                                                 //Healthbar prefab for Champion
    public Transform MinionHealthbarsParent;                                            //Parent transform for minion healthbars
    public Transform TowerHealthbarParent;                                            //Parent transform for tower healthbars
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
    public GameObject QWER_LevelUpPanel;                                            //Panel with "Q" ,"W", "E" and "R" buttons to choose any one for level up
   
    public List<AttackTypeReference> attackTypeReferences = new List<AttackTypeReference>();      //List of buttons with attack types in QWER_LevelUp panel
    public FixedJoystick MovementJoystick;                                          //Main Joystick
    public Toggle Minion_ChampToggle;                                                //Minion champ toggle
    public Toggle Tower_TargetToggle;                                                //Tower taget toggle
    public TMPro.TextMeshProUGUI ToggleButtonText;                                   //Minion toggle text
    public TMPro.TextMeshProUGUI TowerToggleButtonText;                              //Tower toggle text
    [SerializeField]
    public CameraFollow cameraFollow;                                                       //Reference for camerafollow script

    [SerializeField]
    TargetDetailsUIManager targetDetails;
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
    [SerializeField]
    TMPro.TextMeshProUGUI GoldCountsText;                                              //Current team gold count 
    int GoldCount = 0;
    public List<Character> TeamPlayers;                                            //List of all players
    [SerializeField]
    public TowerDetails TowerDestroyDetails=new TowerDetails();                    //Tower damage details to manage gold rewards of players
    
    public GameObject blueCannon, redCannon,blueMelee,redMelee,blueCaster,redCaster;
    public Image XpFill;
    public TMPro.TextMeshProUGUI CurrnetLevel,XpText;                              
    public List<XpData> xpData = new List<XpData>();
    public XpData CurrnetLevelXpData = new XpData();
    public GameObject XpUI;
    public float CurrnetXpValue;
    //Temp dummy
    public GameObject dummyEnemy;
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene=true;
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
       // SpawnCharacter();
        qLevel = 2;                                         //Default graphics quality level-2 (Medium) : (0->VeryLow,1->Low,2->Medium,3->High,4->VeryHigh,5->Ultra
        QalityDropdown.value = qLevel;                      //Set Temporary dropdown value as per current graphics quality level

        if(PhotonNetwork.IsMasterClient)
        {
          //  StartCoroutine(MinionSpawnCoroutine());            //Trigger minion waves coroutine
        }

        TeamPlayers = FindObjectsOfType<Character>().ToList();
        //Temp
        XpFill.fillAmount = 0;
        CurrnetLevel.text = "1".ToString();
        CurrnetLevelXpData = xpData[0];
        XpText.text = "0" + "/" + CurrnetLevelXpData.XpNeeded;
        photonView.RPC("PlayersReady",RpcTarget.MasterClient);
    }
    //public void SpawnCharacter() 
    //{
    //    GameObject playerCharacter = PhotonNetwork.Instantiate("CharacterPrefab",Vector3.zero,Quaternion.identity);
    //    playerCharacter.gameObject.SetActive(false);
    //    playerCharacter.GetComponent<Rigidbody>().useGravity = false;
    //    currentCharacter = playerCharacter.GetComponent<Character>();
    //    playerCharacter.gameObject.SetActive(true);
    //}
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

    public void Recall() 
    {
        List<PlayerScript> playerScripts = FindObjectsOfType<PlayerScript>().ToList();
        if(playerScripts.Count > 0)
        {
            foreach(PlayerScript item in playerScripts)
            {
                if(item.photonView.IsMine) 
                {
                    //Use this code in fusion after finding local player
                    item.Recall();
                    break;
                }
            }
        }
    }

    [SerializeField]
    public GameObject tower;
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
    public IEnumerator LaneSpawn(Lane lane) 
    {
        //Melee spawn
        GameObject[] meleeMinions = new GameObject[3];
       
        Lane lanetype = lane;
       
        for(int i = 0 ; i < 3 ; i++)
        {

            object[] data = new object[4];
            data[0] = 0; //Team type - set 0 =>Blue, 1 =>Red

            Vector3 DestinationPos = GetDestinationPoint(TeamType.Blue,lanetype); ;
            Vector3 spwanLocation = lanetype == Lane.Center ? MidLaneMinionSpawnPositions_Blue[0].position : (lanetype == Lane.Left ? LeftLaneMinionSpawnPositions_Blue[0] : RightLaneMinionSpawnPositions_Blue[0]).position;
            data[1] = DestinationPos.x;
            data[2] = DestinationPos.y;
            data[3] = DestinationPos.z;
            meleeMinions[i] = PhotonNetwork.Instantiate("MeleeMinion",spwanLocation,Quaternion.identity,0,data);
            // meleeMinions[i] = Instantiate(meleeMinion,blueSpawnLocation,Quaternion.identity,MeleeMinionParentContainer);
            // meleeMinions[i].GetComponent<MinionAIScript>().destination = GetDestinationPoint(TeamType.Blue,lanetype);

            // Blue minions
            // meleeMinions[i].GetComponent<MinionAIScript>().isBlue = true;


            //Red
            object[] dataRed = new object[4];
            dataRed[0] = 1; //Team type - set 0 =>Blue, 1 =>Red
            Vector3 DestinationPosRed = GetDestinationPoint(TeamType.Red,lanetype); ;
             spwanLocation = lanetype == Lane.Center ? MidLaneMinionSpawnPositions_Red[0].position : (lanetype == Lane.Left ? LeftLaneMinionSpawnPositions_Red[0] : RightLaneMinionSpawnPositions_Red[0]).position;
            dataRed[1] = DestinationPosRed.x;
            dataRed[2] = DestinationPosRed.y;
            dataRed[3] = DestinationPosRed.z;
            meleeMinions[i] = PhotonNetwork.Instantiate("MeleeMinion",spwanLocation,Quaternion.identity,0,dataRed);
            //meleeMinions[i] = Instantiate(meleeMinion,redSpawnLocation,Quaternion.identity,MeleeMinionParentContainer);
            //meleeMinions[i].GetComponent<MinionAIScript>().destination = GetDestinationPoint(TeamType.Red,lanetype);

            // Red minions
            //  meleeMinions[i].GetComponent<MinionAIScript>().isBlue = false;
            yield return new WaitForSeconds(1f);
        }

        //Caster spawn
        // Spawning and setting destination for caster minions
        GameObject[] casterMinions = new GameObject[3];
       
         //lanetype = Lane.Center;
      
        for(int i = 0 ; i < 3 ; i++)
        {
            object[] data = new object[4];
            data[0] = 0; //Team type - set 0 =>Blue, 1 =>Red

            Vector3 DestinationPos = GetDestinationPoint(TeamType.Blue,lanetype); ;
            Vector3 spwanLocation = lanetype == Lane.Center ? MidLaneMinionSpawnPositions_Blue[0].position : (lanetype == Lane.Left ? LeftLaneMinionSpawnPositions_Blue[0] : RightLaneMinionSpawnPositions_Blue[0]).position;
            data[1] = DestinationPos.x;
            data[2] = DestinationPos.y;
            data[3] = DestinationPos.z;
            casterMinions[i] = PhotonNetwork.Instantiate("casterMinion",spwanLocation,Quaternion.identity,0,data);
            // casterMinions[i] = Instantiate(casterMinion,blueSpawnLocation,Quaternion.identity,CasterMinionParentContainer);
            //casterMinions[i].GetComponent<MinionAIScript>().destination = GetDestinationPoint(TeamType.Blue,lanetype);

            // Blue minions
            // casterMinions[i].GetComponent<MinionAIScript>().isBlue = true;
            // casterMinions[i].GetComponent<MinionAIScript>().teamType = TeamType.Blue;


            object[] dataRed = new object[4];
            dataRed[0] = 1; //Team type - set 0 =>Blue, 1 =>Red
            Vector3 DestinationPosRed = GetDestinationPoint(TeamType.Red,lanetype); ;
            spwanLocation = lanetype == Lane.Center ? MidLaneMinionSpawnPositions_Red[0].position : (lanetype == Lane.Left ? LeftLaneMinionSpawnPositions_Red[0] : RightLaneMinionSpawnPositions_Red[0]).position;
            dataRed[1] = DestinationPosRed.x;
            dataRed[2] = DestinationPosRed.y;
            dataRed[3] = DestinationPosRed.z;
            casterMinions[i] = PhotonNetwork.Instantiate("casterMinion",spwanLocation,Quaternion.identity,0,dataRed);
            // casterMinions[i] = Instantiate(casterMinion,redSpawnLocation,Quaternion.identity,CasterMinionParentContainer);
            //casterMinions[i].GetComponent<MinionAIScript>().destination = GetDestinationPoint(TeamType.Red,lanetype);

            // Red minions
            //casterMinions[i].GetComponent<MinionAIScript>().isBlue = false;
            // casterMinions[i].GetComponent<MinionAIScript>().teamType = TeamType.Red;
            yield return new WaitForSeconds(1f);
        }

        //Cannon Spawn for third wave

        // Every third wave spawns a cannon minion and sets destination as above
        if(WaveCounter % SpawnCannonAfterWaves == 0)
        {
            // lanetype = Lane.Center;
            //switch(i)

            GameObject cannonMinionBlue, cannonMinionRed;
            //CannonMinion
            object[] data = new object[4];
            data[0] = 0; //Team type - set 0 =>Blue, 1 =>Red

            Vector3 DestinationPos = GetDestinationPoint(TeamType.Blue,lanetype); ;
            Vector3 spwanLocation = lanetype == Lane.Center ? MidLaneMinionSpawnPositions_Blue[0].position : (lanetype == Lane.Left ? LeftLaneMinionSpawnPositions_Blue[0] : RightLaneMinionSpawnPositions_Blue[0]).position;
            data[1] = DestinationPos.x;
            data[2] = DestinationPos.y;
            data[3] = DestinationPos.z;
            cannonMinionBlue = PhotonNetwork.Instantiate("CannonMinion",spwanLocation,Quaternion.identity,0,data);
            //cannonMinionBlue = Instantiate(cannonMinion,blueSpawnLocation,Quaternion.identity,CannonMinionParentContainer);
            //cannonMinionBlue.GetComponent<MinionAIScript>().destination = GetDestinationPoint(TeamType.Blue,lanetype);
            //cannonMinionBlue.GetComponent<MinionAIScript>().isBlue = true;
            //cannonMinionBlue.GetComponent<MinionAIScript>().teamType = TeamType.Blue;


            object[] dataRed = new object[4];
            dataRed[0] = 1; //Team type - set 0 =>Blue, 1 =>Red
            Vector3 DestinationPosRed = GetDestinationPoint(TeamType.Red,lanetype); ;
            spwanLocation = lanetype == Lane.Center ? MidLaneMinionSpawnPositions_Red[0].position : (lanetype == Lane.Left ? LeftLaneMinionSpawnPositions_Red[0] : RightLaneMinionSpawnPositions_Red[0]).position;
            dataRed[1] = DestinationPosRed.x;
            dataRed[2] = DestinationPosRed.y;
            dataRed[3] = DestinationPosRed.z;


            cannonMinionRed = PhotonNetwork.Instantiate("CannonMinion",spwanLocation,Quaternion.identity,0,dataRed);
            //cannonMinionRed = Instantiate(cannonMinion,redSpawnLocation,Quaternion.identity,CannonMinionParentContainer);
            //cannonMinionRed.GetComponent<MinionAIScript>().destination = GetDestinationPoint(TeamType.Red,lanetype); ;
            //cannonMinionRed.GetComponent<MinionAIScript>().isBlue = false;
            //cannonMinionRed.GetComponent<MinionAIScript>().teamType = TeamType.Red;
           // yield return new WaitForSeconds(1f);
            // }
            //
        }
    }
    public IEnumerator MeleeMinionSpawn()
    {
       
        // Spawning and setting destination for melee minions
        GameObject[] meleeMinions = new GameObject[3];
       // for(int j = 0 ; j < 3 ; j++)
      //  {
           // yield return new WaitForSeconds(.5f);
            Lane lanetype = Lane.Center;
            //switch(j)
            //{
            //    case 0:
            //        lanetype = Lane.Center;
            //        break;

            //    case 1:
            //        lanetype = Lane.Left;
            //        break;

            //    case 2:
            //        lanetype = Lane.Right;
            //        break;
            //    default:
            //        break;
            //}
            for(int i = 0 ; i < 3 ; i++)
            {
              
                object[] data = new object[4];
                data[0] = 0; //Team type - set 0 =>Blue, 1 =>Red

                Vector3 DestinationPos = GetDestinationPoint(TeamType.Blue,lanetype); ;
                Vector3 spwanLocation = lanetype == Lane.Center ? MidLaneMinionSpawnPositions_Blue[0].position : (lanetype== Lane.Left ? LeftLaneMinionSpawnPositions_Blue[0] : RightLaneMinionSpawnPositions_Blue[0]).position;
                data[1] = DestinationPos.x;
                data[2] = DestinationPos.y;
                data[3] = DestinationPos.z;
                meleeMinions[i] =PhotonNetwork.Instantiate("MeleeMinion",spwanLocation,Quaternion.identity,0,data);
                // meleeMinions[i] = Instantiate(meleeMinion,blueSpawnLocation,Quaternion.identity,MeleeMinionParentContainer);
                // meleeMinions[i].GetComponent<MinionAIScript>().destination = GetDestinationPoint(TeamType.Blue,lanetype);

                // Blue minions
                // meleeMinions[i].GetComponent<MinionAIScript>().isBlue = true;


                //Red
                object[] dataRed = new object[4];
                dataRed[0] = 1; //Team type - set 0 =>Blue, 1 =>Red
                Vector3 DestinationPosRed = GetDestinationPoint(TeamType.Red,lanetype); ;

                dataRed[1] = DestinationPosRed.x;
                dataRed[2] = DestinationPosRed.y;
                dataRed[3] = DestinationPosRed.z;
                meleeMinions[i] = PhotonNetwork.Instantiate("MeleeMinion",redSpawnLocation,Quaternion.identity,0,dataRed);
                //meleeMinions[i] = Instantiate(meleeMinion,redSpawnLocation,Quaternion.identity,MeleeMinionParentContainer);
                //meleeMinions[i].GetComponent<MinionAIScript>().destination = GetDestinationPoint(TeamType.Red,lanetype);

                // Red minions
                //  meleeMinions[i].GetComponent<MinionAIScript>().isBlue = false;
                yield return new WaitForSeconds(1f);
            }
       // }

      //  for(int j = 0 ; j < 3 ; j++)
      //  {
            //yield return new WaitForSeconds(.5f);
            Lane lanetypeL = Lane.Left;
            //switch(j)
            //{
            //    case 0:
            //        lanetype = Lane.Center;
            //        break;

            //    case 1:
            //        lanetype = Lane.Left;
            //        break;

            //    case 2:
            //        lanetype = Lane.Right;
            //        break;
            //    default:
            //        break;
            //}
            for(int i = 0 ; i < 3 ; i++)
            {

                object[] data = new object[4];
                data[0] = 0; //Team type - set 0 =>Blue, 1 =>Red

                Vector3 DestinationPos = GetDestinationPoint(TeamType.Blue,lanetypeL); ;
                Vector3 spwanLocation = lanetypeL == Lane.Center ? MidLaneMinionSpawnPositions_Blue[0].position : (lanetypeL == Lane.Left ? LeftLaneMinionSpawnPositions_Blue[0] : RightLaneMinionSpawnPositions_Blue[0]).position;
                data[1] = DestinationPos.x;
                data[2] = DestinationPos.y;
                data[3] = DestinationPos.z;
                meleeMinions[i] = PhotonNetwork.Instantiate("MeleeMinion",spwanLocation,Quaternion.identity,0,data);
                // meleeMinions[i] = Instantiate(meleeMinion,blueSpawnLocation,Quaternion.identity,MeleeMinionParentContainer);
                // meleeMinions[i].GetComponent<MinionAIScript>().destination = GetDestinationPoint(TeamType.Blue,lanetype);

                // Blue minions
                // meleeMinions[i].GetComponent<MinionAIScript>().isBlue = true;


                //Red
                object[] dataRed = new object[4];
                dataRed[0] = 1; //Team type - set 0 =>Blue, 1 =>Red
                Vector3 DestinationPosRed = GetDestinationPoint(TeamType.Red,lanetype); ;

                dataRed[1] = DestinationPosRed.x;
                dataRed[2] = DestinationPosRed.y;
                dataRed[3] = DestinationPosRed.z;
                meleeMinions[i] = PhotonNetwork.Instantiate("MeleeMinion",redSpawnLocation,Quaternion.identity,0,dataRed);
                //meleeMinions[i] = Instantiate(meleeMinion,redSpawnLocation,Quaternion.identity,MeleeMinionParentContainer);
                //meleeMinions[i].GetComponent<MinionAIScript>().destination = GetDestinationPoint(TeamType.Red,lanetype);

                // Red minions
                //  meleeMinions[i].GetComponent<MinionAIScript>().isBlue = false;
                yield return new WaitForSeconds(1f);
            }
     //   }

      //  for(int j = 0 ; j < 3 ; j++)
      //  {
          //  yield return new WaitForSeconds(.5f);
            Lane lanetypeR = Lane.Right;
            //switch(j)
            //{
            //    case 0:
            //        lanetype = Lane.Center;
            //        break;

            //    case 1:
            //        lanetype = Lane.Left;
            //        break;

            //    case 2:
            //        lanetype = Lane.Right;
            //        break;
            //    default:
            //        break;
            //}
            for(int i = 0 ; i < 3 ; i++)
            {

                object[] data = new object[4];
                data[0] = 0; //Team type - set 0 =>Blue, 1 =>Red

                Vector3 DestinationPos = GetDestinationPoint(TeamType.Blue,lanetypeR); ;
                Vector3 spwanLocation = lanetypeR == Lane.Center ? MidLaneMinionSpawnPositions_Blue[0].position : (lanetypeR == Lane.Left ? LeftLaneMinionSpawnPositions_Blue[0] : RightLaneMinionSpawnPositions_Blue[0]).position;
                data[1] = DestinationPos.x;
                data[2] = DestinationPos.y;
                data[3] = DestinationPos.z;
                meleeMinions[i] = PhotonNetwork.Instantiate("MeleeMinion",spwanLocation,Quaternion.identity,0,data);
                // meleeMinions[i] = Instantiate(meleeMinion,blueSpawnLocation,Quaternion.identity,MeleeMinionParentContainer);
                // meleeMinions[i].GetComponent<MinionAIScript>().destination = GetDestinationPoint(TeamType.Blue,lanetype);

                // Blue minions
                // meleeMinions[i].GetComponent<MinionAIScript>().isBlue = true;


                //Red
                object[] dataRed = new object[4];
                dataRed[0] = 1; //Team type - set 0 =>Blue, 1 =>Red
                Vector3 DestinationPosRed = GetDestinationPoint(TeamType.Red,lanetype); ;

                dataRed[1] = DestinationPosRed.x;
                dataRed[2] = DestinationPosRed.y;
                dataRed[3] = DestinationPosRed.z;
                meleeMinions[i] = PhotonNetwork.Instantiate("MeleeMinion",redSpawnLocation,Quaternion.identity,0,dataRed);
                //meleeMinions[i] = Instantiate(meleeMinion,redSpawnLocation,Quaternion.identity,MeleeMinionParentContainer);
                //meleeMinions[i].GetComponent<MinionAIScript>().destination = GetDestinationPoint(TeamType.Red,lanetype);

                // Red minions
                //  meleeMinions[i].GetComponent<MinionAIScript>().isBlue = false;
                yield return new WaitForSeconds(1f);
            }
       // }

    }

   public IEnumerator CasterMinionSpawn()
    {
        // Spawning and setting destination for caster minions
        GameObject[] casterMinions = new GameObject[3];
       // for(int j = 0 ; j < 3 ; j++)
       // {
           // yield return new WaitForSeconds(.5f);
            //yield return new WaitForSeconds(.25f);
            Lane lanetype = Lane.Center;
            //switch(j)
            //{
            //    case 0:
            //        lanetype = Lane.Center;
            //        break;

            //    case 1:
            //        lanetype = Lane.Left;
            //        break;

            //    case 2:
            //        lanetype = Lane.Right;
            //        break;
            //    default:
            //        break;
            //}
            for(int i = 0 ; i < 3 ; i++)
            {
                object[] data = new object[4];
                data[0] = 0; //Team type - set 0 =>Blue, 1 =>Red

                Vector3 DestinationPos = GetDestinationPoint(TeamType.Blue,lanetype); ;
                Vector3 spwanLocation = lanetype == Lane.Center ? MidLaneMinionSpawnPositions_Blue[0].position : (lanetype == Lane.Left ? LeftLaneMinionSpawnPositions_Blue[0] : RightLaneMinionSpawnPositions_Blue[0]).position;
                data[1] = DestinationPos.x;
                data[2] = DestinationPos.y;
                data[3] = DestinationPos.z;
                casterMinions[i] = PhotonNetwork.Instantiate("casterMinion",spwanLocation,Quaternion.identity,0,data);
                // casterMinions[i] = Instantiate(casterMinion,blueSpawnLocation,Quaternion.identity,CasterMinionParentContainer);
                //casterMinions[i].GetComponent<MinionAIScript>().destination = GetDestinationPoint(TeamType.Blue,lanetype);

                // Blue minions
                // casterMinions[i].GetComponent<MinionAIScript>().isBlue = true;
                // casterMinions[i].GetComponent<MinionAIScript>().teamType = TeamType.Blue;


                object[] dataRed = new object[4];
                dataRed[0] = 1; //Team type - set 0 =>Blue, 1 =>Red
                Vector3 DestinationPosRed = GetDestinationPoint(TeamType.Red,lanetype); ;

                dataRed[1] = DestinationPosRed.x;
                dataRed[2] = DestinationPosRed.y;
                dataRed[3] = DestinationPosRed.z;
                casterMinions[i] = PhotonNetwork.Instantiate("casterMinion",redSpawnLocation,Quaternion.identity,0,dataRed);
                // casterMinions[i] = Instantiate(casterMinion,redSpawnLocation,Quaternion.identity,CasterMinionParentContainer);
                //casterMinions[i].GetComponent<MinionAIScript>().destination = GetDestinationPoint(TeamType.Red,lanetype);

                // Red minions
                //casterMinions[i].GetComponent<MinionAIScript>().isBlue = false;
                // casterMinions[i].GetComponent<MinionAIScript>().teamType = TeamType.Red;
                yield return new WaitForSeconds(1f);
            }
       // }

     //   for(int j = 0 ; j < 3 ; j++)
     //   {
          //  yield return new WaitForSeconds(.5f);
            //yield return new WaitForSeconds(.25f);
            Lane lanetypeL = Lane.Left;
            //switch(j)
            //{
            //    case 0:
            //        lanetype = Lane.Center;
            //        break;

            //    case 1:
            //        lanetype = Lane.Left;
            //        break;

            //    case 2:
            //        lanetype = Lane.Right;
            //        break;
            //    default:
            //        break;
            //}
            for(int i = 0 ; i < 3 ; i++)
            {
                object[] data = new object[4];
                data[0] = 0; //Team type - set 0 =>Blue, 1 =>Red

                Vector3 DestinationPos = GetDestinationPoint(TeamType.Blue,lanetypeL); ;
                Vector3 spwanLocation = lanetypeL == Lane.Center ? MidLaneMinionSpawnPositions_Blue[0].position : (lanetypeL == Lane.Left ? LeftLaneMinionSpawnPositions_Blue[0] : RightLaneMinionSpawnPositions_Blue[0]).position;
                data[1] = DestinationPos.x;
                data[2] = DestinationPos.y;
                data[3] = DestinationPos.z;
                casterMinions[i] = PhotonNetwork.Instantiate("casterMinion",spwanLocation,Quaternion.identity,0,data);
                // casterMinions[i] = Instantiate(casterMinion,blueSpawnLocation,Quaternion.identity,CasterMinionParentContainer);
                //casterMinions[i].GetComponent<MinionAIScript>().destination = GetDestinationPoint(TeamType.Blue,lanetype);

                // Blue minions
                // casterMinions[i].GetComponent<MinionAIScript>().isBlue = true;
                // casterMinions[i].GetComponent<MinionAIScript>().teamType = TeamType.Blue;


                object[] dataRed = new object[4];
                dataRed[0] = 1; //Team type - set 0 =>Blue, 1 =>Red
                Vector3 DestinationPosRed = GetDestinationPoint(TeamType.Red,lanetypeL); ;

                dataRed[1] = DestinationPosRed.x;
                dataRed[2] = DestinationPosRed.y;
                dataRed[3] = DestinationPosRed.z;
                casterMinions[i] = PhotonNetwork.Instantiate("casterMinion",redSpawnLocation,Quaternion.identity,0,dataRed);
                // casterMinions[i] = Instantiate(casterMinion,redSpawnLocation,Quaternion.identity,CasterMinionParentContainer);
                //casterMinions[i].GetComponent<MinionAIScript>().destination = GetDestinationPoint(TeamType.Red,lanetype);

                // Red minions
                //casterMinions[i].GetComponent<MinionAIScript>().isBlue = false;
                // casterMinions[i].GetComponent<MinionAIScript>().teamType = TeamType.Red;
                yield return new WaitForSeconds(1f);
            }
        //}

        //   for(int j = 0 ; j < 3 ; j++)
        //   {
       // yield return new WaitForSeconds(.5f);
        //yield return new WaitForSeconds(.25f);
        Lane lanetypeR = Lane.Right;
        //switch(j)
        //{
        //    case 0:
        //        lanetype = Lane.Center;
        //        break;

        //    case 1:
        //        lanetype = Lane.Left;
        //        break;

        //    case 2:
        //        lanetype = Lane.Right;
        //        break;
        //    default:
        //        break;
        //}
        for(int i = 0 ; i < 3 ; i++)
        {
            object[] data = new object[4];
            data[0] = 0; //Team type - set 0 =>Blue, 1 =>Red

            Vector3 DestinationPos = GetDestinationPoint(TeamType.Blue,lanetypeR); ;
            Vector3 spwanLocation = lanetypeR== Lane.Center ? MidLaneMinionSpawnPositions_Blue[0].position : (lanetypeR == Lane.Left ? LeftLaneMinionSpawnPositions_Blue[0] : RightLaneMinionSpawnPositions_Blue[0]).position;
            data[1] = DestinationPos.x;
            data[2] = DestinationPos.y;
            data[3] = DestinationPos.z;
            casterMinions[i] = PhotonNetwork.Instantiate("casterMinion",spwanLocation,Quaternion.identity,0,data);
            // casterMinions[i] = Instantiate(casterMinion,blueSpawnLocation,Quaternion.identity,CasterMinionParentContainer);
            //casterMinions[i].GetComponent<MinionAIScript>().destination = GetDestinationPoint(TeamType.Blue,lanetype);

            // Blue minions
            // casterMinions[i].GetComponent<MinionAIScript>().isBlue = true;
            // casterMinions[i].GetComponent<MinionAIScript>().teamType = TeamType.Blue;


            object[] dataRed = new object[4];
            dataRed[0] = 1; //Team type - set 0 =>Blue, 1 =>Red
            Vector3 DestinationPosRed = GetDestinationPoint(TeamType.Red,lanetypeL); ;

            dataRed[1] = DestinationPosRed.x;
            dataRed[2] = DestinationPosRed.y;
            dataRed[3] = DestinationPosRed.z;
            casterMinions[i] = PhotonNetwork.Instantiate("casterMinion",redSpawnLocation,Quaternion.identity,0,dataRed);
            // casterMinions[i] = Instantiate(casterMinion,redSpawnLocation,Quaternion.identity,CasterMinionParentContainer);
            //casterMinions[i].GetComponent<MinionAIScript>().destination = GetDestinationPoint(TeamType.Red,lanetype);

            // Red minions
            //casterMinions[i].GetComponent<MinionAIScript>().isBlue = false;
            // casterMinions[i].GetComponent<MinionAIScript>().teamType = TeamType.Red;
            yield return new WaitForSeconds(1f);
        }
        //}

    }

    public IEnumerator  CannonMinionSpawn()
    {
        // Every third wave spawns a cannon minion and sets destination as above
      //  if(waveCount == 3)
      //  {
          //  for(int i = 0 ; i < 3 ; i++)
          //  {
           // yield return new WaitForSeconds(.25f);
            Lane lanetype = Lane.Center;
                //switch(i)
                //{
                //    case 0:
                //        lanetype = Lane.Center;
                //        break;

                //    case 1:
                //        lanetype = Lane.Left;
                //        break;

                //    case 2:
                //        lanetype = Lane.Right;
                //        break;
                //    default:
                //        break;
                //}
                
                GameObject cannonMinionBlue, cannonMinionRed;
            //CannonMinion
            object[] data = new object[4];
            data[0] = 0; //Team type - set 0 =>Blue, 1 =>Red

            Vector3 DestinationPos= GetDestinationPoint(TeamType.Blue,lanetype); ;
            Vector3 spwanLocation = lanetype == Lane.Center ? MidLaneMinionSpawnPositions_Blue[0].position : (lanetype == Lane.Left ? LeftLaneMinionSpawnPositions_Blue[0] : RightLaneMinionSpawnPositions_Blue[0]).position;
            data[1] = DestinationPos.x;
            data[2] = DestinationPos.y;
            data[3] = DestinationPos.z;
            cannonMinionBlue = PhotonNetwork.Instantiate("CannonMinion",spwanLocation,Quaternion.identity,0,data);
            //cannonMinionBlue = Instantiate(cannonMinion,blueSpawnLocation,Quaternion.identity,CannonMinionParentContainer);
            //cannonMinionBlue.GetComponent<MinionAIScript>().destination = GetDestinationPoint(TeamType.Blue,lanetype);
            //cannonMinionBlue.GetComponent<MinionAIScript>().isBlue = true;
            //cannonMinionBlue.GetComponent<MinionAIScript>().teamType = TeamType.Blue;


            object[] dataRed = new object[4];
            dataRed[0] = 1; //Team type - set 0 =>Blue, 1 =>Red
            Vector3 DestinationPosRed = GetDestinationPoint(TeamType.Red,lanetype); ;

            dataRed[1] = DestinationPosRed.x;
            dataRed[2] = DestinationPosRed.y;
            dataRed[3] = DestinationPosRed.z;


            cannonMinionRed = PhotonNetwork.Instantiate("CannonMinionRed",redSpawnLocation,Quaternion.identity,0,dataRed);
            //cannonMinionRed = Instantiate(cannonMinion,redSpawnLocation,Quaternion.identity,CannonMinionParentContainer);
            //cannonMinionRed.GetComponent<MinionAIScript>().destination = GetDestinationPoint(TeamType.Red,lanetype); ;
            //cannonMinionRed.GetComponent<MinionAIScript>().isBlue = false;
            //cannonMinionRed.GetComponent<MinionAIScript>().teamType = TeamType.Red;
            yield return new WaitForSeconds(1f);
       // }


      //  for(int i = 0 ; i < 3 ; i++)
     //   {
            // yield return new WaitForSeconds(.25f);
            Lane lanetypeL = Lane.Left;
            //switch(i)
            //{
            //    case 0:
            //        lanetype = Lane.Center;
            //        break;

            //    case 1:
            //        lanetype = Lane.Left;
            //        break;

            //    case 2:
            //        lanetype = Lane.Right;
            //        break;
            //    default:
            //        break;
            //}

            // cannonMinionBlue, cannonMinionRed;
            //CannonMinion
             data = new object[4];
            data[0] = 0; //Team type - set 0 =>Blue, 1 =>Red

             DestinationPos = GetDestinationPoint(TeamType.Blue,lanetypeL); ;
             spwanLocation = lanetypeL == Lane.Center ? MidLaneMinionSpawnPositions_Blue[0].position : (lanetypeL == Lane.Left ? LeftLaneMinionSpawnPositions_Blue[0] : RightLaneMinionSpawnPositions_Blue[0]).position;
            data[1] = DestinationPos.x;
            data[2] = DestinationPos.y;
            data[3] = DestinationPos.z;
            cannonMinionBlue = PhotonNetwork.Instantiate("CannonMinion",spwanLocation,Quaternion.identity,0,data);
            //cannonMinionBlue = Instantiate(cannonMinion,blueSpawnLocation,Quaternion.identity,CannonMinionParentContainer);
            //cannonMinionBlue.GetComponent<MinionAIScript>().destination = GetDestinationPoint(TeamType.Blue,lanetype);
            //cannonMinionBlue.GetComponent<MinionAIScript>().isBlue = true;
            //cannonMinionBlue.GetComponent<MinionAIScript>().teamType = TeamType.Blue;


            dataRed = new object[4];
            dataRed[0] = 1; //Team type - set 0 =>Blue, 1 =>Red
            DestinationPosRed = GetDestinationPoint(TeamType.Red,lanetype); ;

            dataRed[1] = DestinationPosRed.x;
            dataRed[2] = DestinationPosRed.y;
            dataRed[3] = DestinationPosRed.z;


            cannonMinionRed = PhotonNetwork.Instantiate("CannonMinion",redSpawnLocation,Quaternion.identity,0,dataRed);
            //cannonMinionRed = Instantiate(cannonMinion,redSpawnLocation,Quaternion.identity,CannonMinionParentContainer);
            //cannonMinionRed.GetComponent<MinionAIScript>().destination = GetDestinationPoint(TeamType.Red,lanetype); ;
            //cannonMinionRed.GetComponent<MinionAIScript>().isBlue = false;
            //cannonMinionRed.GetComponent<MinionAIScript>().teamType = TeamType.Red;
            yield return new WaitForSeconds(1f);
      //  }

      //  for(int i = 0 ; i < 3 ; i++)
     //   {
            // yield return new WaitForSeconds(.25f);
            Lane lanetypeR = Lane.Right;
            //switch(i)
            //{
            //    case 0:
            //        lanetype = Lane.Center;
            //        break;

            //    case 1:
            //        lanetype = Lane.Left;
            //        break;

            //    case 2:
            //        lanetype = Lane.Right;
            //        break;
            //    default:
            //        break;
            //}

           // GameObject cannonMinionBlue, cannonMinionRed;
            //CannonMinion
           // object[] data = new object[4];
            data[0] = 0; //Team type - set 0 =>Blue, 1 =>Red

            DestinationPos = GetDestinationPoint(TeamType.Blue,lanetypeR); ;
            spwanLocation = lanetypeR == Lane.Center ? MidLaneMinionSpawnPositions_Blue[0].position : (lanetypeR == Lane.Left ? LeftLaneMinionSpawnPositions_Blue[0] : RightLaneMinionSpawnPositions_Blue[0]).position;
            data[1] = DestinationPos.x;
            data[2] = DestinationPos.y;
            data[3] = DestinationPos.z;
            cannonMinionBlue = PhotonNetwork.Instantiate("CannonMinion",spwanLocation,Quaternion.identity,0,data);
            //cannonMinionBlue = Instantiate(cannonMinion,blueSpawnLocation,Quaternion.identity,CannonMinionParentContainer);
            //cannonMinionBlue.GetComponent<MinionAIScript>().destination = GetDestinationPoint(TeamType.Blue,lanetype);
            //cannonMinionBlue.GetComponent<MinionAIScript>().isBlue = true;
            //cannonMinionBlue.GetComponent<MinionAIScript>().teamType = TeamType.Blue;


          //  object[] dataRed = new object[4];
            dataRed[0] = 1; //Team type - set 0 =>Blue, 1 =>Red
            DestinationPosRed = GetDestinationPoint(TeamType.Red,lanetype); ;

            dataRed[1] = DestinationPosRed.x;
            dataRed[2] = DestinationPosRed.y;
            dataRed[3] = DestinationPosRed.z;


            cannonMinionRed = PhotonNetwork.Instantiate("CannonMinion",redSpawnLocation,Quaternion.identity,0,dataRed);
            //cannonMinionRed = Instantiate(cannonMinion,redSpawnLocation,Quaternion.identity,CannonMinionParentContainer);
            //cannonMinionRed.GetComponent<MinionAIScript>().destination = GetDestinationPoint(TeamType.Red,lanetype); ;
            //cannonMinionRed.GetComponent<MinionAIScript>().isBlue = false;
            //cannonMinionRed.GetComponent<MinionAIScript>().teamType = TeamType.Red;
            yield return new WaitForSeconds(1f);
       // }

        // Resetting wave count for next cannon minion wave
        //  waveCount = 0;
        // }
    }
    int readycount;
    [PunRPC]
    public void PlayersReady() 
    {
        if(PhotonNetwork.IsMasterClient) 
        {
            readycount += 1;
            if(readycount== PhotonNetwork.CurrentRoom.MaxPlayers) 
            {
                photonView.RPC("SpawnCharacter",RpcTarget.All,-1); //-1-Temp value :For future use,team type-0 for Blue,1 for Red 
            }
        }
    }
    [PunRPC]
    /// <summary>
    /// Spawns character at given transform position and sets parent of character transform, assigns attack click methods for the character
    /// </summary>
    public void SpawnCharacter(int team=-1)
    {
            Vector3 spawnPosition =  characterSpawnTranform.position;
            object[] data = new object[1];
            data[0] = PhotonNetwork.IsMasterClient ? 0 : 1;
            if(CharacterLastPosition != Vector3.zero)
            {
                spawnPosition = CharacterLastPosition;
            }
            //GameObject character = Instantiate(characterPrefab,spawnPosition,Quaternion.identity,characterSpawnTranform.parent); cameraFollow.SetPlayerAndOffset(character.transform);

            GameObject character = PhotonNetwork.Instantiate("CharacterPrefab",spawnPosition,Quaternion.identity,0,data); //cameraFollow.SetPlayerAndOffset(character.transform);
            character.gameObject.SetActive(false);
            Character characterScirpt = character.GetComponent<Character>();
            for(int i = 0 ; i < AttackButtons.Count ; i++)
            {
                int val = characterScirpt.AttackValues[i];
                AttackType type = AttackTypes[i];
                AttackButtons[i].button.onClick.AddListener(() => characterScirpt.playerScript.InitiateAttackWrapper(val,type));
            }
            currentCharacter = characterScirpt;
            currentCharacter.gameObject.SetActive(true);
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
            StartCoroutine(LaneSpawn(Lane.Center));
            StartCoroutine(LaneSpawn(Lane.Left));
            StartCoroutine(LaneSpawn(Lane.Right));
            //  StartCoroutine( MeleeMinionSpawn());
            //   StartCoroutine( CasterMinionSpawn());
            if(WaveCounter % SpawnCannonAfterWaves == 0)                // checks that if the wave is multiple of the given variable for spawning cannon or not
            {
               //StartCoroutine( CannonMinionSpawn());
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
    //Stop coroutine
    public void StopTimeTextUpdate(float activeTime,float timerValue,AttackType attackType)
    {
        StopCoroutine(AttackButtons.Find(x => x.attackType == attackType).UpdateTimerText(activeTime,timerValue));
        AttackButtons.Find(x => x.attackType == attackType).ResetCoolDown();
    }
    /// <summary>
    /// Stop coroutine on disable
    /// </summary>
    private void OnDisable()
    {
        StopCoroutine(SpawnWave());   
    }
    /// <summary>
    /// Get AD for the current selected character
    /// </summary>
    public double GetCurrentAD() 
    {
      return  currentCharacter.currentAD;
    }
    /// <summary>
    /// Show level up screen for Q/W/E/R levelup
    /// </summary>
    public void Show_QWER_LevelUpdatePanel()
    {
       // Debug.LogError(currentCharacter.characterData.characterModel.characterType);
        foreach(AttackTypeReference item in attackTypeReferences)
        {
            //Allow button to be interactable only if attack level is less then max(5) attack level
            AttackLevel attackLevel = currentCharacter.attackLevels.Find(x => x.attackType == item.attackType);
            item.currentLevel.text = attackLevel.level.ToString();
            if(attackLevel.level < attackLevel.MaxLevelUpLimit && (attackLevel.LevelUpAllowedForCharacterLevels.Count==0|| attackLevel.LevelUpAllowedForCharacterLevels.Count>0 && attackLevel.LevelUpAllowedForCharacterLevels.Contains((int)currentCharacter.currentLevel)))
            {
                item.EnableButton(true);
            }
            else 
            {
                item.EnableButton(false);
            }
        }
       // QWER_LevelUpPanel.SetActive(true);
    }
    /// <summary>
    /// Hide level up screen for Q/W/E/R levelup
    /// </summary>
    public void Hide_QWER_LevelUpdatePanel()
    {
        foreach(AttackButton item in  AttackButtons)
        {
            if(item.ScaleUpButton)
            { item.ScaleUpButton.gameObject.SetActive(false); }
        }
        //QWER_LevelUpPanel.SetActive(false);
    }
    /// <summary>
    /// Toggle minion target button
    /// </summary>
    public void ToggleMinion() 
    {
        //Minion_ChampToggle.isOn = !Minion_ChampToggle.isOn;        
        ToggleButtonText.text =Minion_ChampToggle.isOn?  "Champ": "Minion";
        if(!Minion_ChampToggle.isOn) 
        {
            if(currentCharacter.targetMinion)
            currentCharacter.targetMinion.ShowIndicator(false);
        }
        
    }
    /// <summary>
    /// Toggle tower target button
    /// </summary>
    public void ToggleTower()
    {
        //Minion_ChampToggle.isOn = !Minion_ChampToggle.isOn;        
        TowerToggleButtonText.text = Tower_TargetToggle.isOn ? "Attack\nTower" : "X";
        if(!Tower_TargetToggle.isOn)
        {
            if(currentCharacter.targetTower)
                currentCharacter.targetTower.ShowIndicator(false);
        }
    }
    /// <summary>
    /// Returns toggle valu of minion_champ toggle
    /// </summary>
    /// <returns></returns>
    public bool Get_Minion_ChampToggleValue()
    {
        return Minion_ChampToggle.isOn;
    }
    /// <summary>
    /// Update details in Target UI
    /// </summary>
    public void UpdateTargetDetailsUI()
    {
        if(targetDetails)
        targetDetails.UpdateDetails();
    }
    /// <summary>
    /// Show / Hide target UI
    /// </summary>
    /// <param name="val"></param>
    public void ShowTargetDetailsUI(bool val)
    {
        if(targetDetails)
        {
            targetDetails.UpdateDetails();
            targetDetails.gameObject.SetActive(val);
        }
    }
    
    /// <summary>
    /// Access target details manager
    /// </summary>
    /// <returns></returns>
    public TargetDetailsUIManager GetTargetUIManager() 
    {
        return targetDetails;
    }
    /// <summary>
    /// Returns destination points with resepect to lane type and team type (for left and right lanes , half way points are set as first destination)
    /// </summary>
    /// <param name="teamType">Team type: Red / Blue</param>
    /// <param name="lane">Left , Right , Center</param>
    /// <returns>Destination position (Vector3)</returns>
    public Vector3 GetDestinationPoint(TeamType teamType,Lane lane) 
    {
        Vector3 destination = teamType == TeamType.Blue ? redSpawnLocation: blueSpawnLocation;
        switch(lane)
        {
            case Lane.Left:
                destination = teamType == TeamType.Blue ? blueLeftLaneMidPoint.transform.position : blueRightLaneMidPoint.transform.position;
                break;
            case Lane.Right:
                destination = teamType == TeamType.Blue ? blueRightLaneMidPoint.transform.position : blueLeftLaneMidPoint.transform.position;
                break;
            default:
                break;
        }
        return destination;
    }
    /// <summary>
    /// Update gold count
    /// </summary>
    /// <param name="gold">amount</param>
    public void UpdateGold(float gold) 
    {
        GoldCount += (int)gold;
        GoldCountsText.text =GoldCount.ToString();
    }
    public void UpdateXp(float Xp,float Id) 
    {
        Character character = TeamPlayers.Find(x => x.Id == Id);
        character.UpdateXp(Xp) ;
    }
    /// <summary>
    /// Give gold rewards to players with in range of current tower whose health is decrease upto  level(1000/2000/3000/4000 )
    /// </summary>
    public void TriggerGoldRewardForPlayerswithInRangeForTowerForLevelDestroy(TeamType towerType,Vector3 towerPostion,TowerDetails towerDetails) 
    {
        float distance = ((towerDetails.Range / 10) / 2);
        List<Character> list = TeamPlayers.FindAll(x => x.teamType != towerType && Vector3.Distance(x.transform.position,towerPostion) < distance );
        int goldForEachPlayer = list.Count>0? towerDetails.LevelGold / list.Count: towerDetails.LevelGold;
        foreach(Character item in list)
        {
            item.UpdateGold(goldForEachPlayer);
            UpdateGold(goldForEachPlayer);
        }
    }
    /// <summary>
    /// Give gold rewards to all team players for tower destroy
    /// </summary>
    public void TriggerGoldRewardForPlayersForTowerDestroy(TeamType towerType,Vector3 towerPostion,TowerDetails towerDetails)
    {
        List<Character> list = TeamPlayers.FindAll(x => x.teamType != towerType);
        int goldForEachPlayer = list.Count>0? towerDetails.FinalGoldWithiInRange / list.Count: towerDetails.FinalGoldWithiInRange;
        foreach(Character item in list)
        {
            item.UpdateGold(goldForEachPlayer);
            UpdateGold(goldForEachPlayer);
        }
        //foreach(Character item in TeamPlayers.FindAll(x=>x.teamType!=towerType && !list.Contains(x)))
        //{
        //    item.UpdateGold(towerDetails.FinalGold_OutOfRange);
        //    UpdateGold(towerDetails.FinalGold_OutOfRange);
        //}
    }
    float xpReminder = 0;
    public void UpdateXpData(float xp) 
    {
        CurrnetXpValue += xp;
     
        float amount= (xp * 100) / CurrnetLevelXpData.XpNeeded;
        amount = amount/ 100;  
        XpFill.fillAmount += amount; //Xp added
        if(XpFill.fillAmount >= 1) 
        {
            XpFill.fillAmount = 1;
            if(CurrnetXpValue > CurrnetLevelXpData.XpNeeded) 
            {
                xpReminder = CurrnetXpValue - CurrnetLevelXpData.XpNeeded;
                CurrnetXpValue -= xpReminder;
            }
        }
        XpText.text = CurrnetXpValue+ "/" + CurrnetLevelXpData.XpNeeded;

        if(XpFill.fillAmount == 1)
        {
            //Xp level increase
            LeanTween.scale(XpUI,Vector3.one * 1.1f,0.25f);
            LeanTween.scale(XpUI,Vector3.one * 1f,0.25f).setDelay(0.25f);

            Invoke(nameof(UpdateXpUIForNextLevel),2f);
        }

    }
   
    bool levelUpdate = false;
    private void UpdateXpUIForNextLevel()
    {
        if(levelUpdate) return;

            levelUpdate = true;
        Debug.LogError(xpReminder);
        XpData _CurrnetLevel, NextLevel;
        _CurrnetLevel = CurrnetLevelXpData;
        NextLevel = xpData.Find(x => x.CurrentLevel == _CurrnetLevel.NextLevel);
        CurrnetLevelXpData = NextLevel;
        CurrnetLevel.text = CurrnetLevelXpData.CurrentLevel.ToString();
        XpFill.fillAmount = 0;
        CurrnetXpValue = 0+xpReminder;
        XpText.text = CurrnetXpValue + "/" + CurrnetLevelXpData.XpNeeded;
        UpdateXpData(xpReminder);
        Invoke("ResetLevelUpdate",0.5f);
    }
    public void ResetLevelUpdate() 
    {
        levelUpdate = false;
    }
}
[System.Serializable]
public enum Lane { Center,Left,Right}
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
    public Button ScaleUpButton;                            //Scale up button for Attack
    public FixedJoystick AttackButtonJoystick;              //AttackButton joystick
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
    /// Method to trigger stop coroutine
    /// </summary>
    /// <param name="activeTime">activet time of attack</param>
    /// <param name="timerValue">cooldown time value</param>
    public void StopTimerUpdateCoroutine(float activeTime,float timerValue)
    {
        GameManager.instance.StopTimeTextUpdate(activeTime,timerValue,attackType);
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
    public void ResetCoolDown() 
    {
        UpdateTime(0);
        Image InactiveImage = DeactiveIndicator.GetComponent<Image>();
        InactiveImage.fillAmount = 0;
        coolDownTimer.gameObject.SetActive(false);
        attackName.gameObject.SetActive(true);
        DeactiveIndicator.gameObject.SetActive(false);
    }
    /// <summary>
    /// Show/Hide attack button target joystick
    /// </summary>
    /// <param name="show">True: show joystick, False : hide joystick</param>
    public void ShowTargetJoystick(bool show) 
    {
        AttackButtonJoystick.transform.parent.gameObject.SetActive(show);
    }
    /// <summary>
    /// Check if joystickis active
    /// </summary>
    /// <returns></returns>
    public bool JoyStickAtcive() 
    {
        return AttackButtonJoystick.transform.parent.gameObject.activeInHierarchy;
    } 
}
[System.Serializable]
public enum TeamType { Blue, Red}
[System.Serializable]
public enum MinionType {Melee,Cannon,Caster }
/// <summary>
/// Tower damage and gold reward details
/// </summary>
[System.Serializable]
public class TowerDetails 
{
    public int MaxHealth = 5000;
    public int Levels = 5;
    public int Range = 1200;
    public int LevelGold = 125; // gold given as reward to team players within the range for tower level destroy
    public int FinalGoldWithiInRange = 250; // gold given as reward to team players within the range for tower level destroy
    public int FinalGold_OutOfRange = 50; // gold given as reward to team players within the range for tower level destroy
}
[System.Serializable]
public class XpData 
{
    public int CurrentLevel;
    public int NextLevel;
    public int XpNeeded;
}