using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionButton : MonoBehaviour
{
    public GameObject selectionImage;

    public void SetSelection(bool value)
    {
        selectionImage.SetActive(value);
    }
}
