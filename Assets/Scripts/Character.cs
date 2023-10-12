using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
   
    Otrill characterOtrill;
    
    public PlayerScript playerScript;
    public List<int> AttackValues = new List<int>();
    [SerializeField]
    List<CharacterModels> characterModels=new List<CharacterModels>();

    //Temp
    public CharacterType SelectedCharacterType;
    public int SelectedCharacterIndex=0;
  
    // Start is called before the first frame update
   
   public CharacterModels currentCharacterModel;
    public float invisibleTime = 5f;
    
    void Start()
    {
        characterOtrill = new Otrill();
        characterOtrill.CheckLevel();
        characterOtrill.DisplayStats();
        ChangeCharacter();
       // SelectCharacter((int) SelectedCharacterIndex);
    }   
    public void ChangeCharacter() 
    {
        if(SelectedCharacterIndex < characterModels.Count-1) 
        {
            SelectedCharacterIndex += 1;
        }
        else 
        {
            SelectedCharacterIndex = 0;
        }
        SelectCharacter(SelectedCharacterIndex);
    }
    public void SelectCharacter(int index) 
    {
        for(int i = 0 ; i < characterModels.Count ; i++)
        {
            characterModels[i].characterModel.SetActive(i == index ? true : false);  
        }
        SelectedCharacterType = characterModels[index].characterType;
        currentCharacterModel = characterModels.Find(x => x.characterType == SelectedCharacterType);
        Debug.LogError(currentCharacterModel.characterModel.name);
        playerScript.characterAnimator = characterModels.Find(x=>x.characterType==SelectedCharacterType).characterModel.GetComponent<Animator>();
        playerScript.SetSpeed(characterOtrill.GetMovementSpeed());
    }
    //True: Show character ,False: Hide character
    public void ShowModel(bool val) 
    {
        currentCharacterModel.characterModel.SetActive(val);
        Invoke(nameof( ShowAgain),invisibleTime);
    }
    public void ShowAgain() 
    {
        currentCharacterModel.characterModel.SetActive(true);
    }
}

[System.Serializable]
public class CharacterModels 
{
    public CharacterType characterType;
    public GameObject characterModel;
}