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
    [SerializeField]
    float smoothTime = 0.05f;
    [SerializeField]
    float currentVelocity;
    //
    private float lastHorizontal = 0;

    public Animator characterAnimator;
    Character character;
    // Start is called before the first frame update
    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        joyStick = FindObjectOfType<FixedJoystick>();
        character = GetComponent<Character>();
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
            if(joyStick.Direction.sqrMagnitude == 0)
            {
                characterAnimator.SetBool("run",false);
            }
            else
            {
                characterAnimator.SetBool("run",true);
                float angle = Mathf.Atan2(joyStick.Horizontal,joyStick.Vertical) * Mathf.Rad2Deg;
                float FinaleAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y,angle,ref currentVelocity,smoothTime);
                this.transform.eulerAngles = new Vector3(0,FinaleAngle,0);

            }
        }
        
        //transform.Translate(new Vector3(joyStick.Horizontal,0,joyStick.Vertical) * _speed);
    
        var vel = new Vector3(joyStick.Horizontal,0,joyStick.Vertical) * _speed;
        vel.y = _rb.velocity.y;
        _rb.velocity = vel;


        if(characterAnimator)
        {
            if(attack)
            {
                // attack = false;
                characterAnimator.SetBool("attack_E",true);
            }
            if(off)
            {
                off = true;
            }
        }
    }

    bool attack = false, off = false;
    bool lastAutoAttackWasLeft = true;
    public void InitiateAttack(int AttackValue,AttackType attackType)
    {
        //Exception conditions for special attack instead of animation
        if(character.currentCharacterModel.characterType== CharacterType.Hakka )
        {
            if(attackType== AttackType.auto)    
            {
                attackType = lastAutoAttackWasLeft? AttackType.right: AttackType.left;  //One by one left then right then left -attacks
                lastAutoAttackWasLeft = !lastAutoAttackWasLeft; //toggle value for next attack
            }
        }
        else if(character.currentCharacterModel.characterType == CharacterType.Moorg) 
        {
            if(attackType == AttackType.r)
            { //Invisible effect
                character.ShowModel(false);
            }
        }
        characterAnimator.SetBool(attackType.ToString(),true);
        StartCoroutine(SetBoolOff(attackType,0.2f));
    }
    
    public IEnumerator SetBoolOff(AttackType attackType,float duration = 0.2f)
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
public enum AttackType { w, q, e, r, auto,left,right }
