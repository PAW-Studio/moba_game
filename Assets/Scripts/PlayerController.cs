using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{


    [SerializeField] private float _speed = 1;
    [SerializeField] private float _jumpForce = 200;
    [SerializeField] private Rigidbody _rb;
    FixedJoystick joyStick;
    // Update is called once per frame
    private void OnEnable()
    {
        joyStick = GameManager.instance.MovementJoystick; //FindObjectOfType<FixedJoystick>();
       
    }
    void Update()
    {
      //  var vel = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * _speed;

        var vel = new Vector3(joyStick.Horizontal,0,joyStick.Vertical) * _speed;
        vel.y = _rb.velocity.y;
        _rb.velocity = vel;

        if (Input.GetKeyDown(KeyCode.Space)) _rb.AddForce(Vector3.up * _jumpForce);
    }
}
