using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinionHealthBar : MonoBehaviour
{
    // Referencing slider component; rest is straightforward
    public Slider slider;                                       //Healthbar slider
    Camera cam;                                                 //Scene camera referemce   
   
    private void OnEnable()
    {
        cam = FindObjectOfType<Camera>();
    }
    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
    }
    
    public void SetHealth(float health)
    {
        slider.value = health;
    }
}
