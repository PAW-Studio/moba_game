using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;                                            //Camera follow target transform
    private Vector3 cameraOffset;                                       //offset value         
    bool startingOffSetSet = false;                                     //True once the offset between player and camera is set        
    Vector3 StartingOffset;                                             //Reference for starting offset
    [Range(0.01f, 1.0f)]
    public float smoothness = 0.5f;                                     //Camera follow smoothness
    // Start is called before the first frame update
    void Start()
    {
      /*  if (player)
        {
            SetPlayerAndOffset(player);
        }*/
    }
    /// <summary>
    /// Set player and offeset transform
    /// </summary>
    /// <param name="_player">Target player transform</param>
    public void SetPlayerAndOffset(Transform _player) 
    {
        if(_player) 
        {
            player = _player;
            if(!startingOffSetSet)
            {
                cameraOffset = transform.position - player.transform.position;
                StartingOffset = cameraOffset;
                startingOffSetSet = true;
            }
            else 
            {
                cameraOffset = StartingOffset;
            }  
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(player)
        {
            Vector3 newPos = player.position + cameraOffset;
            transform.position = Vector3.Slerp(transform.position,newPos,smoothness);
        }
    }

}
