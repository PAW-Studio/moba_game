using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCam : MonoBehaviour
{
    Camera cam;
    private void OnEnable()
    {
        cam = Camera.main;
      
    }
    private void Update()
    {
        //  transform.LookAt(cam.transform);
        UpdateRotation();
    }
    public void UpdateRotation()
    {
        transform.rotation = cam.transform.rotation;
    }
}
