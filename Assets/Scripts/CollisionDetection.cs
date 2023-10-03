using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    int collisionNumber = 0;
    private void OnCollisionEnter(Collision other) 
    {
        Debug.Log("Collision Detected");
        collisionNumber ++;
        if (collisionNumber < 2)
        {
            GetComponent<MeshRenderer>().material.color = Color.red;
        }
        else
        {
            GetComponent<MeshRenderer>().material.color = Color.green;
        }
    }
}
