using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChampionsAbilityManager : MonoBehaviour
{
    [SerializeField]
    TMPro.TextMeshProUGUI ChampionName, ChampionSubTitle,OverviewText;
    [SerializeField]
    TMPro.TextMeshProUGUI AbiltiyTitle, AblitlityDescription;
    [SerializeField]
    Image AblilityScreenBigImage;
    [SerializeField]
    Button LeftButton, RightButton;
    [SerializeField]
    GameObject abliltyPrefab;
    public List<ChampionAblilities> championAblilities = new List<ChampionAblilities>();
    public Transform Ablitiesparent;
    int currentChampionIndex = 0;
    public static ChampionsAbilityManager Instance;
    public List<AblilityItem> ablilityItems = new List<AblilityItem>();
    //Skin scroll
    public GameObject scrollbar;
    float scroll_pos = 0;
    float[] pos;
    public Transform skincontent;
    private void Awake()
    {
        if(Instance == null) Instance = this;

    }
    // Start is called before the first frame update
    private void OnEnable()
    {
        currentChampionIndex = 0;
        UpdateChampion(championAblilities[currentChampionIndex]);
    }
    public void Next()
    {
        currentChampionIndex += 1;

        if(currentChampionIndex > championAblilities.Count - 1)
        {
            currentChampionIndex = 0;
        }
        UpdateChampion(championAblilities[currentChampionIndex]);
    }
    public void Previous()
    {
        currentChampionIndex -= 1;
        if(currentChampionIndex < 0)
        {
            currentChampionIndex = championAblilities.Count - 1;
        }
        UpdateChampion(championAblilities[currentChampionIndex]);
    }
    public void UpdateChampion(ChampionAblilities championAblilities)
    {
        ChampionName.text = championAblilities.champion.ToString().ToUpper();
        ChampionSubTitle.text = championAblilities.Subtitle.ToString();
        OverviewText.text = championAblilities.OverviewText.ToString();
        foreach(Transform item in Ablitiesparent.transform)
        {
            if(item.CompareTag("child"))
            {
                Destroy(item.gameObject);
            }
        }
        ablilityItems.Clear();
        for(int i = 0 ; i < championAblilities.abilities.Count ; i++)
        {
            GameObject ability = Instantiate(abliltyPrefab,Ablitiesparent);
            AblilityItem ablilityItem = ability.GetComponent<AblilityItem>();
            ablilityItem.ability = championAblilities.abilities[i];
            ablilityItems.Add(ablilityItem);
            if(i == 0)
            {
                ablilityItem.SelectItem();
            }
        }
    }
    public void SelectAbility(Ability ability)
    {
        foreach(AblilityItem item in ablilityItems)
        {
            if(item.ability != ability)
            {
                item.ShowBorder(false);
            }
        }
        AbiltiyTitle.text = ability.AbilityTitle;
        AblitlityDescription.text = ability.AblilityDescription;
        AblilityScreenBigImage.sprite = ability.AblilitySprite;
    }
    //
    // Update is called once per frame
    void Update()
    {
        if(skincontent.childCount > 0)
        {
            pos = new float[skincontent.childCount];
            
            float distance = 1f / (pos.Length - 1f);
            for(int i = 0 ; i < pos.Length ; i++)
            {
                pos[i] = distance * i;
            }

            if(Input.GetMouseButton(0))
            {
                scroll_pos = scrollbar.GetComponent<Scrollbar>().value;
            }
            else
            {
                for(int i = 0 ; i < pos.Length ; i++)
                {
                    if(scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
                    {
                        scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value,pos[i],0.1f);
                    }
                }
            }
            for(int i = 0 ; i < pos.Length ; i++)
            {
                if(scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
                {

                    skincontent.GetChild(i).localScale = Vector2.Lerp(skincontent.GetChild(i).localScale,new Vector2(1f,1f),0.1f);
                    for(int a = 0 ; a < pos.Length ; a++)
                    {
                        if(a != i)
                        {
                            skincontent.GetChild(a).localScale = Vector2.Lerp(skincontent.GetChild(a).localScale,new Vector2(0.8f,0.8f),0.1f);
                        }
                    }
                }
            }
        }
    }

        
    //
}

[System.Serializable]
public class ChampionAblilities 
{
    public CharacterType champion;
    public string Subtitle;
    public List<Ability> abilities = new List<Ability>();
    public string OverviewText;
}
[System.Serializable]
public class Ability
{
    public string AbilityTitle;
    public string AblilityDescription;
    public Sprite AblilitySprite;
}