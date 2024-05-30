using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatcolorChange : MonoBehaviour
{
    [SerializeField]
    SkinnedMeshRenderer meshRenderer;
    Material mat;
    private void OnEnable()
    {
        if(meshRenderer) 
        {
            mat = meshRenderer.materials[0];
           
        }
    }

}
