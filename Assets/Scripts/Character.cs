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
    void Start()
    {
        //Temp 
        characterOtrill = new Otrill();
        characterOtrill.CheckLevel();
        characterOtrill.DisplayStats();

        //Set Default character on start
        ChangeCharacter();
        // SelectCharacter((int) SelectedCharacterIndex);
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
        if(characterData)
            characterData.DisplayStats();

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