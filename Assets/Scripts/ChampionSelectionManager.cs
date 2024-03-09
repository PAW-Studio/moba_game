using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChampionSelectionManager : MonoBehaviour
{
    public List<ChampionSelectionItem> championSelectionItems = new List<ChampionSelectionItem>();
    public static ChampionSelectionManager Instance;
    private void Awake()
    {
        if(Instance==null)
        Instance = this;
    }
    private void OnEnable()
    {
        ResetSelection();
        championSelectionItems[0].SetSelectionObject(true);
    }
    public void ResetSelection() 
    {
        foreach(ChampionSelectionItem item in championSelectionItems)
        {
            item.SetSelectionObject(false);

        }
    }
}
