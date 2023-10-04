using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Character",menuName ="Character Data", order =1)]
public class CharacterScriptable : ScriptableObject
{
    public CharacterType characterType;
    public Champions GetCharacter() 
    {
        Otrill o = new Otrill();
        return o;
    }
}
public enum CharacterType { Otrill,Morya,VaRun,Misa,Udara,Sura,Hakka,Dira,Tapani,Moorg}
