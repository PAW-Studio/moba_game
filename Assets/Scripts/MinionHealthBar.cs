using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MinionHealthBar : MonoBehaviour
{
    [SerializeField]
    bool local = false;
    Camera camera;
    // Referencing slider component; rest is straightforward
    public Slider slider;                                       //Healthbar slider
    public Slider effectBar;                                    //Effect bar to create decrease effect in helatbar     
    public TMPro.TextMeshProUGUI damageText;                    //Damage text
    public TMPro.TextMeshProUGUI HealthText;                    //Damage text
    public GameObject textPrefab;
    [SerializeField]
    Transform damageTextReference;
    [SerializeField]
    GameObject indicator;
    [SerializeField]
    List<Transform> animationPoints = new List<Transform>();
    [SerializeField]
    Color ADDamageColor, APDamageColor;
    private void OnEnable()
    {
        camera = Camera.main;
    }
    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        if(effectBar) 
        {
            effectBar.maxValue = health;
            effectBar.value = float.MaxValue;
        }
        slider.value = health;
        effectBar.value = health;
        SetHealthText(slider.value);
        
    }
    
    public void SetHealth(float health,bool damage=true,GameObject objectToDestroy=null,DamageTypeDetails damageType= DamageTypeDetails.None)
    {
        if(slider.value == 0f) return;
       
        float oldValue = slider.value;
        slider.value = health;
        SetHealthText(slider.value);
        if(effectBar && damage)
        {
            StartCoroutine(EffectBar(slider.value,oldValue,objectToDestroy,damageType));
        }
        
    }
    /// <summary>
    /// This bar creates effect of decrease healthbar effect 
    /// </summary>
    /// <param name="newVal">already decreased value of red-color main slider</param>
    public IEnumerator EffectBar(float newVal,float oldValue,GameObject objToDestroy,DamageTypeDetails damageType) 
    {
       // damageText.transform.position = damageTextReference.transform.position;
        float time = 0, duration = .5f;
        float startVal = effectBar.value;        
        damageText.text =  ((int)(oldValue-newVal)).ToString();
        GameObject textObj = Instantiate(textPrefab,transform.parent);
        LeanTween.scale(textObj.gameObject,Vector3.one * 0.75f,0);
        textObj.transform.position = damageText.transform.position;
        TMPro.TextMeshProUGUI tmpObject = textObj.GetComponent<TMPro.TextMeshProUGUI>();
        
        tmpObject.color = damageType == DamageTypeDetails.AD || damageType == DamageTypeDetails.None ? Color.red : APDamageColor;
        tmpObject.text = damageText.text;
      //  LeanTween.scale(damageText.gameObject,Vector3.one * 0.75f,0);
       // damageText.gameObject.SetActive(true);
      //  LeanTween.scale(damageText.gameObject,Vector3.one,0.25f);
      //  LeanTween.scale(damageText.gameObject,Vector3.one * 0.75f,.25f).setDelay(0.25f).setOnComplete(()=> damageText.gameObject.SetActive(false) );


        
        // damageText.gameObject.SetActive(true);
        textObj.SetActive(true);
        LeanTween.scale(textObj.gameObject,Vector3.one,0.25f);
        int random = UnityEngine.Random.Range(0,2);
        
        float xMovement = random == 0 ? 15 : -15f;
        LeanTween.moveX(textObj,textObj.transform.position.x +xMovement,0.25f);
        LeanTween.moveY(textObj,textObj.transform.position.y + 10f,0.25f);
        LeanTween.scale(textObj.gameObject,Vector3.one * 0.75f,.25f).setDelay(0.25f).setOnComplete(() => Destroy( textObj.gameObject));
        LeanTween.moveY(textObj,textObj.transform.position.y - 10f,0.25f).setDelay(0.25f);

        float delay = 0;
        //foreach(Transform item in animationPoints)
        //{
        //    LeanTween.move(damageText.gameObject,item,0.1f).setDelay(delay);
        //    delay += 0.1f;
        //}
       // LeanTween.move(damageText.gameObject,)
        while(time<duration)
        {
            effectBar.value = Mathf.Lerp(startVal,newVal,time/duration);
            time += Time.deltaTime;
            yield return null;
        }
        effectBar.value = newVal;
        if(newVal <= 0f) 
        {
            if(objToDestroy)
            {
                MinionAIScript minionObject = objToDestroy.GetComponent<MinionAIScript>();
                Character characterObject = objToDestroy.GetComponent<Character>();
                if(minionObject) 
                {
                    MinionAIScript minionTarget = GameManager.instance.GetTargetUIManager().minionTarget;
                    if(minionTarget && minionTarget== minionObject) 
                    {
                        GameManager.instance.ShowTargetDetailsUI(false);
                    }
                }
                else if(characterObject) 
                {
                    Character champ = GameManager.instance.GetTargetUIManager().championTarget;
                    if(champ && champ == characterObject)
                    {
                        GameManager.instance.ShowTargetDetailsUI(false);
                    }
                }
                Destroy(objToDestroy); 
            }

            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        if(local)
        transform.parent.LookAt(camera.transform);
    }
    /// <summary>
    /// Show indicator on healthbar
    /// </summary>
    /// <param name="show">Show outline highlight</param>
    public void ShowOutline(bool show) 
    {
        indicator.gameObject.SetActive(show);
    }
    void SetHealthText(float value) 
    {
        if(HealthText) 
        {
            HealthText.text = Convert.ToInt32(value).ToString();
        }
    }
}
