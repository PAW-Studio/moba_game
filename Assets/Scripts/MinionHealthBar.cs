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
    private void OnEnable()
    {
        camera = Camera.main;
    }
    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
    }
    
    public void SetHealth(float health)
    {
        slider.value = health;
    }
    private void Update()
    {
        if(local)
        transform.parent.LookAt(camera.transform);
    }
}
