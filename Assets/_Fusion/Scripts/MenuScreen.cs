using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScreen : MonoBehaviour
{
    #region PUBLIC_VAR
    public GameObject charecterSelectionScreen;
    public GameObject serverClientScreen;
    public GameObject findMatchScreen;
    public BasicSpawner basicSpawner;

    #endregion

    #region PUBLIC_METHOD
    public void PlayButton()
    {
        if (basicSpawner.serverToggle.isOn)
        {
            basicSpawner.ServerClientCreate();
        }
        else
        {
            gameObject.SetActive(false);
            findMatchScreen.SetActive(true);
        }
    }
    #endregion
}
