using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void ShowMatchMakingScene() 
    {
        SceneManager.LoadScene(1); //MatchMaking scene
    }
}
