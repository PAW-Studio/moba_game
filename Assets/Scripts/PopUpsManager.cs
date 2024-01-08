using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpsManager : MonoBehaviour
{
    public static PopUpsManager Instance;                                    //Instance of script
    public GameObject TowerDestoryPopup;                                          //Popup object
    private void Awake()
    {
        Instance = this;
    }
    /// <summary>
    /// Show tower destroy popup
    /// </summary>
    public void ShowTowerDestroyPopup() 
    {
        TowerDestoryPopup.SetActive(true);
        Invoke(nameof(HideTowerDestroyPopup),2f);
    }
    /// <summary>
    /// Hide tower destroy popup
    /// </summary>
    void HideTowerDestroyPopup() 
    {
        TowerDestoryPopup.gameObject.SetActive(false);
        
        GameManager.instance.ShowTargetDetailsUI(false);//hide target UI
    }
}
