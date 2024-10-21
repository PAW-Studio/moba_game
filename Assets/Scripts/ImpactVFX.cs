using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactVFX : MonoBehaviour
{
    [SerializeField]
    GameObject impactVFX;

    public void ShowImpact(Vector3 position) 
    {
        Instantiate(impactVFX,position,Quaternion.identity);
    }
}
