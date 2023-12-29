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
    [SerializeField]
    Transform damageTextReference;
    [SerializeField]
    GameObject indicator;
    
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
    
    public void SetHealth(float health,bool damage=true,GameObject objectToDestroy=null)
    {
        float oldValue = slider.value;
        slider.value = health;
        SetHealthText(slider.value);
        if(effectBar && damage)
        {
            StartCoroutine(EffectBar(slider.value,oldValue,objectToDestroy));
        }
        
    }
    /// <summary>
    /// This bar creates effect of decrease healthbar effect 
    /// </summary>
    /// <param name="newVal">already decreased value of red-color main slider</param>
    public IEnumerator EffectBar(float newVal,float oldValue,GameObject objToDestroy) 
    {
       // damageText.transform.position = damageTextReference.transform.position;
        float time = 0, duration = .5f;
        float startVal = effectBar.value;
        damageText.text =  ((int)(oldValue-newVal)).ToString();
        damageText.gameObject.SetActive(true);
        LeanTween.scale(damageText.gameObject,Vector3.one,0.25f);
        LeanTween.scale(damageText.gameObject,Vector3.one * 0.75f,.25f).setDelay(0.25f).setOnComplete(()=> damageText.gameObject.SetActive(false) );
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
            Destroy(objToDestroy);

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
