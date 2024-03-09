using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChampionSelectionItem : MonoBehaviour
{
    [SerializeField]
    GameObject SelectionObject;
    [SerializeField]
    GameObject outline;
    public void SetSelectionObject(bool val) 
    {
        SelectionObject.SetActive(val);
        outline.SetActive(!val);
    }

    public void SelectItem() 
    {
        ChampionSelectionManager.Instance.ResetSelection();
        SetSelectionObject(true);
      
    }
}
