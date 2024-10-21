using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScreen : MonoBehaviour
{
    #region PUBLIC_VAR
    public GameObject charecterSelectionScreen;
    public GameObject serverClientScreen;
    public GameObject findMatchScreen;

    #endregion

    #region PUBLIC_METHOD
    public void PlayButton()
    {
        if (BasicSpawner.Instance.serverToggle.isOn)
        {
            BasicSpawner.Instance.ServerClientCreate();
        }
        else
        {
            gameObject.SetActive(false);
            findMatchScreen.SetActive(true);
        }
    }
    #endregion
}
