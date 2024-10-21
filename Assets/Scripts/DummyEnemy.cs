using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyEnemy : MonoBehaviour
{
    public Transform Sura_Q_EnemyReferencePosition;
    public Transform Moorg_Q_EnemyReferencePosition;
    public Transform VFXParent;
    public void DestroyVFX() 
    {
        foreach(Transform item in VFXParent)
        {
            if(item.gameObject.name != VFXParent.gameObject.name) 
            {
                Destroy(item.gameObject);
            }
        }
    }
}
