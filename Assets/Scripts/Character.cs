using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
   
    Otrill characterOtrill;
    [SerializeField]
    PlayerScript playerScript;
    // Start is called before the first frame update
    
    void Start()
    {
        
        characterOtrill = new Otrill();
        characterOtrill.CheckLevel();
        characterOtrill.DisplayStats();
        playerScript.SetSpeed(characterOtrill.GetMovementSpeed());
        
    }   
}
