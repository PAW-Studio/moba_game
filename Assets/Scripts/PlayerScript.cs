using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class PlayerScript : MonoBehaviour
{
    NavMeshAgent agent;

    public float rotateSpeedMovement = 0.1f;
    float rotateVelocity;

    [SerializeField] private float _speed = 1;
    [SerializeField] private float _jumpForce = 200;
    [SerializeField] private Rigidbody _rb;
    FixedJoystick joyStick;

    // Start is called before the first frame update
    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        joyStick = FindObjectOfType<FixedJoystick>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;

            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit,Mathf.Infinity))
            {
                agent.SetDestination(hit.point);

                Quaternion rotationToLookAt = Quaternion.LookRotation(hit.point - transform.position);
                float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y,rotationToLookAt.eulerAngles.y,
                ref rotateVelocity,rotateSpeedMovement * (Time.deltaTime * 5));

                transform.eulerAngles = new Vector3(0,rotationY,0);
            }
        }
    }
    private void FixedUpdate()
    {
        var vel = new Vector3(joyStick.Horizontal,0,joyStick.Vertical) * _speed;
        vel.y = _rb.velocity.y;
        _rb.velocity = vel;
    }
    public void InitiateAttack(int AttackValue)
    {
        Debug.Log("Attack: " + AttackValue);
    }
    public void SetSpeed(float speed) 
    {
        _speed = speed;
    }
}
