using UnityEngine;

[System.Serializable]
public class CharecterSelectionData
{
    public int index;
    public Sprite iconSprite;
}

[System.Serializable]
public class CharacterData
{
    public int index;
    public string characterName;    
    public GameObject character;
}