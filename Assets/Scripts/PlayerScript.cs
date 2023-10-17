using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class PlayerScript : MonoBehaviour
{
    NavMeshAgent agent;

    public float rotateSpeedMovement = 0.1f;
    public Animator characterAnimator;                                                             //Character Animator
    //public CharacterController characterController;                                                //Character Controller 
    float rotateVelocity;

    [SerializeField] private float _speed = 1;                                                     //Movemnt speed
    [SerializeField] private float _jumpForce = 200;
    [SerializeField] private Rigidbody _rb;                                                        //Player rigidbody
    FixedJoystick joyStick;
    [SerializeField]
    float smoothTime = 0.05f;                                                                      //Character rotation smoothness offset
    [SerializeField]
    float currentVelocity;                                                                         //Current velocity
    bool Attack_R_IsAtcitve = false;                                                                 //True if R attack is active 
    [SerializeField]
    float R_Attack_ActiveTime = 5f;                                                                //Duration till the R attack is active
    Character character;
    
    // Set references 
    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();

        //Set reference for joystick and character
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
        //Character movement and rotation
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
        Debug.LogError(_speed);
        //characterController.Move(new Vector3(joyStick.Horizontal,0,joyStick.Vertical) * _speed);
        //transform.Translate(new Vector3(joyStick.Horizontal,0,joyStick.Vertical) * _speed);

        var vel = new Vector3(joyStick.Horizontal,0,joyStick.Vertical) * _speed;
        vel.y = _rb.velocity.y;
        _rb.velocity = vel;
        
        //-

        //if(characterAnimator)
        //{
        //    if(attack)
        //    {
        //        // attack = false;
        //        characterAnimator.SetBool("attack_E",true);
        //    }
        //    if(off)
        //    {
        //        off = true;
        //    }
        //}
    }

    bool attack = false, off = false;
    bool lastAutoAttackWasLeft = true;
    /// <summary>
    /// Attack button functionality: 
    /// </summary>
    /// <param name="AttackValue">Attack amount </param>
    /// <param name="attackType">Attack type of character</param>
    public void InitiateAttack(int AttackValue,AttackType attackType)
    {
        Debug.LogError(attackType + "  Character : " + (character.currentCharacterModel.characterType));
        //Check for special attack instead of animation
        if(character.currentCharacterModel.characterType == CharacterType.Hakka)
        {
            if(attackType == AttackType.auto)
            {
                attackType = lastAutoAttackWasLeft ? AttackType.right : AttackType.left;  //One by one left then right then left -attacks
                lastAutoAttackWasLeft = !lastAutoAttackWasLeft; //toggle value for next attack
            }
        }
        else if(character.currentCharacterModel.characterType == CharacterType.Tapani)
        {
            if(attackType == AttackType.auto)
            {
                attackType = lastAutoAttackWasLeft ? AttackType.right : AttackType.left;  //One by one left then right then left -attacks
                lastAutoAttackWasLeft = !lastAutoAttackWasLeft; //toggle value for next attack
            }
            Debug.LogError(attackType);
        }
        else if(character.currentCharacterModel.characterType == CharacterType.Otrill)
        {
            if(attackType == AttackType.r)
            {
                if(Attack_R_IsAtcitve) return;
                Attack_R_IsAtcitve = true;
                GameManager.instance.TriggerAttackActiveCoroutine(attackType,R_Attack_ActiveTime);
                attackType = lastAutoAttackWasLeft ? AttackType.rRight : AttackType.rLeft;  //One by one left then right then left -attacks
                lastAutoAttackWasLeft = !lastAutoAttackWasLeft; //toggle value for next attack
                Debug.LogError(attackType);
                Invoke(nameof(ResetRAttackIndicator),R_Attack_ActiveTime);  //Reset indicator of R attack after acitve time limit( default 5 seconds)
            }
           else if(attackType == AttackType.auto)
            {
                if(Attack_R_IsAtcitve) 
                {
                    //Auto become R-left and R-right while R is active
                    attackType = lastAutoAttackWasLeft ? AttackType.rRight : AttackType.rLeft;  //One by one left then right then left -attacks
                    lastAutoAttackWasLeft = !lastAutoAttackWasLeft; //toggle value for next attack
                }
            }
        }
        else if(character.currentCharacterModel.characterType == CharacterType.Moorg)
        {
            if(attackType == AttackType.r)
            { //Invisible effect
              // character.ShowModel(false);
                foreach(Transform item in character.gameObject.transform)
                {
                    if(item.GetComponent<MeshRenderer>())
                    {
                        foreach(Material m in item.GetComponent<MeshRenderer>().materials)
                        {
                            Color c = m.color;
                            c.a = 0.5f;
                            m.color = c;
                        }
                    }
                }
            }
        }
        //Trigger attack animation
        characterAnimator.SetBool(attackType.ToString(),true);
        StartCoroutine(SetBoolOff(attackType,0.2f));
    }
    /// <summary>
    /// Set bull parameter of animator false
    /// </summary>
    /// <param name="attackType">animator bool string</param>
    /// <param name="duration">delay before reseting the bool</param>
    /// <returns></returns>
    public IEnumerator SetBoolOff(AttackType attackType,float duration = 0.2f)
    {
        yield return new WaitForSeconds(duration);
        characterAnimator.SetBool(attackType.ToString(),false);
    }
    /// <summary>
    /// Set character walking speed
    /// </summary>
    /// <param name="speed">character movement speed</param>
    public void SetSpeed(float speed)
    {
        //Temp
        speed = 20f;
        _speed = speed;
    }
    //Reset indicator boolean of R attack
    public void ResetRAttackIndicator()
    {
        Attack_R_IsAtcitve = false;
    }
}
//Player attack types 
public enum AttackType { w, q, e, r, auto, left, right, rLeft, rRight }
