using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 5f;
    private NetworkCharacterController _controller;
    public Animator characterAnimator;
    public List<CharacterData> characters = new List<CharacterData>();
    public NetworkMecanimAnimator mecanimAnimator;

    public RoomPlayer RoomPlayer;

    private void Awake()
    {
        mecanimAnimator = GetComponent<NetworkMecanimAnimator>();
        _controller = GetComponent<NetworkCharacterController>();
    }

    private void Start()
    {
        if (HasInputAuthority && Runner.IsForward)
        {
            FusionNetwork.GameManager.Instance.cameraFollow.SetPlayerAndOffset(transform);
        }
    }
    public override void FixedUpdateNetwork()
    {
        // Only move own player and not every other player. Each player controls its own player object.
        if (HasStateAuthority == false)
        {
            return;
        }

        if (GetInput(out NetworkInputData data))
        {
            data.direction.Normalize();
            SetDirections(data.direction.normalized, characterAnimator);
           // _controller.MoveNew(data.direction, moveSpeed, characterAnimator);
        }
    }

    private void SetDirections(Vector3 moveVector, Animator characterAnimator)
    {
        _controller.Move(new Vector3(moveVector.x, moveVector.y, moveVector.z), characterAnimator);

       /* if (aimVector.sqrMagnitude > 0)
            _turret.forward = new Vector3(aimVector.x, 0, aimVector.y);
        aimDirection = _turret.rotation.eulerAngles.y;*/
    }

    public override void Spawned()
    {
        /* Debug.Log("Character Selected Index---------------- " + RoomPlayer.KartId);


         if (HasInputAuthority && Runner.IsForward)
         {
             Debug.Log("Character Selected Index " + RoomPlayer.KartId);
         }
         else if (Object.HasStateAuthority)
         {
             // This runs only on the server or the client that controls the state
             // You might send RPCs here to new clients with additional info if needed

         }*/

        Debug.Log("Start Player ID " + RoomPlayer.KartId);
        SetPlayer(RoomPlayer.KartId);
    }

    public void SetPlayer(int i)
    {
        foreach (var item in characters)
        {
            item.character.SetActive(false);
        }

        int index = FusionNetwork.GameManager.Instance.selectedCharacterIndex;
        Debug.Log(" Charecter Game object " + i);
        characters[i].character.SetActive(true);
        characterAnimator = characters[i].character.GetComponent<Animator>();
        mecanimAnimator.Animator = characters[i].character.GetComponent<Animator>();
       
    }
}
