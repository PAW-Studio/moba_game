using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JPCMovementSample : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1;
    // Start is called before the first frame update
    void Start()
    {
        PrintInstructions();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    void PrintInstructions()
    {
        Debug.Log("Welcome to the game");
        Debug.Log("Move your player with WASD or arrow keys");
        Debug.Log("Don't hit the walls");
    }

    void MovePlayer()
    {
        float xValue = moveSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
        float zValue = moveSpeed * Time.deltaTime * Input.GetAxis("Vertical");
        transform.Translate(xValue, 0, zValue);
    }
}
