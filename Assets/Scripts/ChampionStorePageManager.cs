using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChampionStorePageManager : MonoBehaviour
{
    public ScrollRect ChampionScrollRect, SkinScrollRect, GemsScrollRect;
    public Button ChampionsButton, SkinsButton, GemsButton;
    // Start is called before the first frame update

    private void OnEnable()
    {
        ChampionScrollRect.gameObject.SetActive(false);
        ShowChamps();
    }

    public void ShowSkins()
    {
        if(SkinScrollRect.gameObject.activeInHierarchy) return;

        SkinsButton.interactable = false;
        ChampionsButton.interactable = true;
        GemsButton.interactable = true;
        SkinScrollRect.gameObject.SetActive(true);
        GemsScrollRect.gameObject.SetActive(false);
        ChampionScrollRect.gameObject.SetActive(false);
        SetVerticalNormalizedPosition(SkinScrollRect);
    }
    public void ShowGems()
    {

        if(GemsScrollRect.gameObject.activeInHierarchy) return;

        SkinsButton.interactable = true;
        ChampionsButton.interactable = true;
        GemsButton.interactable = false;
        SkinScrollRect.gameObject.SetActive(false);
        GemsScrollRect.gameObject.SetActive(true);
        ChampionScrollRect.gameObject.SetActive(false);
        SetVerticalNormalizedPosition(GemsScrollRect);
    }
    public void ShowChamps()
    {
        if(ChampionScrollRect.gameObject.activeInHierarchy) return;

        ChampionsButton.interactable = false;
        SkinsButton.interactable = true;
        GemsButton.interactable = true;
        ChampionScrollRect.gameObject.SetActive(true);
        SkinScrollRect.gameObject.SetActive(false);
        GemsScrollRect.gameObject.SetActive(false);
        SetVerticalNormalizedPosition(ChampionScrollRect);
    }
    public void SetVerticalNormalizedPosition(ScrollRect scrollRect) 
    {
        scrollRect.verticalNormalizedPosition = 1;
    }

}
