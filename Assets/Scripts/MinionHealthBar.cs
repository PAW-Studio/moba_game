using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinionHealthBar : MonoBehaviour
{
    // Referencing slider component; rest is straightforward
    public Slider slider;
    Camera cam;
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
    //private void Update()
    //{
    //    if(cam)
    //        slider.gameObject.transform.LookAt(cam.transform);
    //}
}
