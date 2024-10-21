using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchMakingScreen : MonoBehaviour
{
    #region PUBLIC_VAR
    public GameObject charecterScreen;
    public GameObject findMatchScreen;
    #endregion


    #region PUBLIC_METHOD
    
    public void BackButton()
    {
        gameObject.SetActive(false);
        charecterScreen.SetActive(true);
    }

    public void ConfirmButton()
    {
        gameObject.SetActive(false);
        
    }
    #endregion
}
