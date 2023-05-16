using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    private void OnCollisionEnter(Collision other) 
    {
        Debug.Log("Collision Detected");
        GetComponent<MeshRenderer>().material.color = Color.red;
    }
}
