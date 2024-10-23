using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionButton : MonoBehaviour
{
    public GameObject selectionImage;
    public Button button;
    public void SetSelection(bool value)
    {
        selectionImage.SetActive(value);
    }

    public void Awake()
    {
        if (button == null)
            button = GetComponent<Button>();
    }
}
