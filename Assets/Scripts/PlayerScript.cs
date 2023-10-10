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

    //
    private float lastHorizontal = 0;

    public Animator characterAnimator;
    
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
        //Movement
         if(characterAnimator) 
        {
            if(joyStick.Horizontal == 0 || joyStick.Vertical == 0) 
            {
                characterAnimator.SetBool("run",false);
             //   characterAnimator.SetBool("attack_E",true);
            }
            else if(joyStick.Horizontal>.1f || joyStick.Vertical>0.1f)
            {
                characterAnimator.SetBool("run",true);
                float angle = Mathf.Atan2(joyStick.Horizontal,joyStick.Vertical) * Mathf.Rad2Deg;
                this.transform.rotation = Quaternion.Euler(new Vector3(0,angle,0));
              
            }
        }
       // transform.Translate(new Vector3(joyStick.Horizontal,0,joyStick.Vertical) * _speed);
       var vel = new Vector3(joyStick.Horizontal,0,joyStick.Vertical) * _speed;
      vel.y = _rb.velocity.y;
       _rb.velocity = vel;
     

        //Set facing direction
        //float angle=   Mathf.Atan(joyStick.Horizontal * joyStick.Vertical);
        //Vector3 rotation = transform.eulerAngles;
        //      rotation.y = angle;
        //transform.eulerAngles= rotation;

        if(characterAnimator) 
        {
            if(attack) 
            {
              // attack = false;
                characterAnimator.SetBool("attack_E",true);
                Debug.LogError("SET TRUE");
            }
            if(off) 
            {
                off = true;
             //   characterAnimator.SetBool("attack_E",false);
            }
        }


        //
        //
    }

    private void FixedUpdate()
    {
       
    }
    bool attack = false, off=false;
    public void InitiateAttack(int AttackValue,AttackType attackType)
    {
        characterAnimator.SetBool(attackType.ToString(),true);
        StartCoroutine(SetBoolOff(attackType,0.2f));
    }
    public IEnumerator SetBoolOff(AttackType attackType,float duration=0.2f) 
    {
        yield return new WaitForSeconds(duration);
        characterAnimator.SetBool(attackType.ToString(),false);
    }
    public void SetSpeed(float speed) 
    {
        //Temp
        speed = 30f;
        _speed = speed;
    }
}
public enum AttackType {w, q ,e ,r}
