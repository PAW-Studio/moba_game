using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class CharecterDataScriptable : ScriptableObject
{
    public List<CharecterSelectionData> charecters = new List<CharecterSelectionData>();

    public Sprite GetCharecterSprite(int index)
    {
        return charecters[index].iconSprite;
    }
}
