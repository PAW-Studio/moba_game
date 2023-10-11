using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
   
    Otrill characterOtrill;
    
    public PlayerScript playerScript;
    public List<int> AttackValues = new List<int>();
    [SerializeField]
    List<GameObject> characterModels=new List<GameObject>();

    //Temp
   
    public int SelectedCharacterIndex=0;
    // Start is called before the first frame update
    
    
    void Start()
    {
        SelectCharacter(SelectedCharacterIndex);
        characterOtrill = new Otrill();
        characterOtrill.CheckLevel();
        characterOtrill.DisplayStats();
        playerScript.characterAnimator = characterModels[SelectedCharacterIndex].GetComponent<Animator>();
        playerScript.SetSpeed(characterOtrill.GetMovementSpeed());
        
    }   
    public void SelectCharacter(int index) 
    {
        for(int i = 0 ; i < characterModels.Count ; i++)
        {
            characterModels[i].SetActive(i == index ? true : false);  
        }    
    }
}
