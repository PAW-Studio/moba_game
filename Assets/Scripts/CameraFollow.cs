using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    private Vector3 cameraOffset;

    [Range(0.01f, 1.0f)]
    public float smoothness = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        if(player) 
        {
            SetPlayerAndOffset(player);
        }
    }
    public void SetPlayerAndOffset(Transform _player) 
    {
        if(_player) 
        {
            player = _player;
            cameraOffset = transform.position - player.transform.position;
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
