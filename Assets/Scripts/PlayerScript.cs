using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    NavMeshAgent agent;

    public float rotateSpeedMovement = 0.1f;
    public Animator characterAnimator;                                                             //Character Animator
    //public CharacterController characterController;                                                //Character Controller 
    public AttackType currentActiveAnimation = AttackType.None;                                       //Current Active attack animation
    public AttackType currentAttackType;                                                             //track current attack type
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
    bool Attack_R_CoolDown = false;                                                                 //True if waiting for cool down after "R" attack

    bool Attack_Q_IsAtcitve = false;                                                                 //True if Q attack is active 
    bool Attack_Q_CoolDown = false;                                                                 //True if waiting for cool down after "Q" attack

    bool Attack_W_IsAtcitve = false;                                                                 //True if W attack is active 
    bool Attack_W_CoolDown = false;                                                                 //True if waiting for cool down after "W" attack

    bool Attack_E_IsAtcitve = false;                                                                 //True if E attack is active 
    bool Attack_E_CoolDown = false;                                                                 //True if waiting for cool down after "E" attack

    bool Attack_Auto_Allowed = true;                                                                //True if AS time passed after last auto attack

    [SerializeField]
    float R_Attack_ActiveTime = 5f;                                                                //Duration till the R attack is active
    Character character;
    float R_Attack_CooldownTime = 10f;                                                             //Cooldown time after "R" is deactivated

    //Used to decide what should be the next attack in case of left and right attacks
    bool lastAutoAttackWasLeft = true;

    //Animation movement speed will be increased using this modifier :Speed can be set from character scriptable object ,default speed can be adjusted from here
    float AnimationMovementSpeedModifier = 1.5f;
    float defaultModifierValue = 1.5f;
    [SerializeField]
    float AS_CapValue = 2.5f;                                                                       //Default cap value for AS
    // Set references 
    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();

        //Set reference for joystick and character
        joyStick = FindObjectOfType<FixedJoystick>();
        character = GetComponent<Character>();
    }

    public bool moving = false;
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

        //characterController.Move(new Vector3(joyStick.Horizontal,0,joyStick.Vertical) * _speed);
        //transform.Translate(new Vector3(joyStick.Horizontal,0,joyStick.Vertical) * _speed);


        if(moving) //If animation with movement then auto move rigidbody
        {
            var vel2 = transform.forward * _speed * AnimationMovementSpeedModifier;  //Increased speed -can be variable with respect to character and animations
            vel2.y = _rb.velocity.y;
            _rb.velocity = vel2;
        }
        else //Move character with respect to josystick
        {
            var vel = new Vector3(joyStick.Horizontal,0,joyStick.Vertical) * _speed;
            vel.y = _rb.velocity.y;
            _rb.velocity = vel;
        }
    }

    /// <summary>
    /// Attack button functionality: 
    /// </summary>
    /// <param name="AttackValue">Attack amount </param>
    /// <param name="attackType">Attack type of character</param>
    public void InitiateAttack(int AttackValue,AttackType attackType)
    {
        bool OtrillRActivated = false; //Used to detect "R" click
        //Check for special attack instead of animation
        //if(character.currentCharacterModel.characterType == CharacterType.Hakka)
        //{
        //    if(attackType == AttackType.auto)
        //    {
        //        attackType = lastAutoAttackWasLeft ? AttackType.right : AttackType.left;  //One by one left then right then left -attacks
        //        lastAutoAttackWasLeft = !lastAutoAttackWasLeft; //toggle value for next attack
        //    }
        //}
        //else if(character.currentCharacterModel.characterType == CharacterType.Tapani)
        //{
        //    if(attackType == AttackType.auto)
        //    {
        //        attackType = lastAutoAttackWasLeft ? AttackType.right : AttackType.left;  //One by one left then right then left -attacks
        //        lastAutoAttackWasLeft = !lastAutoAttackWasLeft; //toggle value for next attack
        //    }
        //    Debug.LogError(attackType);
        //}
        if(character.currentCharacterModel.characterType == CharacterType.Otrill)
        {
            if(attackType == AttackType.r)
            {
                if(Attack_Q_IsAtcitve || Attack_E_IsAtcitve || Attack_W_IsAtcitve || Attack_R_IsAtcitve) return;
                if(Attack_R_IsAtcitve || Attack_R_CoolDown) return;
                Attack_R_IsAtcitve = true;
                Attack_R_CoolDown = true;
                
                EnableDeactiveIndicator(attackType);
                R_Attack_CooldownTime = character.characterData.GetCoolDownTime(attackType);
                GameManager.instance.TriggerAttackActiveCoroutine(attackType,R_Attack_ActiveTime);
                attackType = lastAutoAttackWasLeft ? AttackType.rRight : AttackType.rLeft;  //One by one left then right then left -attacks
                lastAutoAttackWasLeft = !lastAutoAttackWasLeft; //toggle value for next attack
                Debug.LogError(attackType);
                Invoke(nameof(ResetRAttackIndicator),R_Attack_ActiveTime);  //Reset indicator of R attack after acitve time limit( default 5 seconds)
                Invoke(nameof(ResetRAttackCoolDownIndicator),R_Attack_ActiveTime + R_Attack_CooldownTime);  //Reset indicator for R attack cool down after acitve time limit( default 5 seconds)
                OtrillRActivated = true;
                GameManager.instance.AttackButtons.Find(x => x.attackType == AttackType.auto).button.gameObject.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Mele";
            }
            else if(attackType == AttackType.auto)
            {
                
                if(!Attack_R_IsAtcitve) 
                {
                    if(!Attack_Auto_Allowed) return;
                    Check_AutoAttackAllowed(); //Allow auto attack if AS time is passed from last auto attack
                }

                if(Attack_R_IsAtcitve)
                {
                    //Auto become R-left and R-right while R is active
                    attackType = lastAutoAttackWasLeft ? AttackType.rRight : AttackType.rLeft;  //One by one left then right then left -attacks
                    lastAutoAttackWasLeft = !lastAutoAttackWasLeft; //toggle value for next attack
                }               

            }
            if(attackType == AttackType.q)
            {
                if(Attack_Q_IsAtcitve || Attack_E_IsAtcitve || Attack_W_IsAtcitve || Attack_R_IsAtcitve) return;
                if(Attack_Q_IsAtcitve || Attack_Q_CoolDown) return;
                Attack_Q_IsAtcitve = true;
                Attack_Q_CoolDown = true;
                EnableDeactiveIndicator(attackType);
                R_Attack_CooldownTime = character.characterData.GetCoolDownTime(attackType);
                R_Attack_ActiveTime = character.characterData.GetActiveTime(attackType);
                GameManager.instance.TriggerAttackActiveCoroutine(attackType,R_Attack_ActiveTime);


                StartCoroutine(ResetAttackIndicator(R_Attack_ActiveTime,attackType));  //Reset indicator of R attack after acitve time limit( default 5 seconds)
                StartCoroutine(ResetCoolDownAttackIndicator(R_Attack_ActiveTime,R_Attack_CooldownTime,attackType));  //Reset indicator for R attack cool down after acitve time limit( default 5 seconds)
            }
            if(attackType == AttackType.w)
            {
                if(Attack_Q_IsAtcitve || Attack_E_IsAtcitve || Attack_W_IsAtcitve || Attack_R_IsAtcitve) return;
                if(Attack_W_IsAtcitve || Attack_W_CoolDown) return;
                Attack_W_IsAtcitve = true;
                Attack_W_CoolDown = true;
                
                EnableDeactiveIndicator(attackType);
                R_Attack_CooldownTime = character.characterData.GetCoolDownTime(attackType);
                R_Attack_ActiveTime = character.characterData.GetActiveTime(attackType);
                GameManager.instance.TriggerAttackActiveCoroutine(attackType,R_Attack_ActiveTime);


                StartCoroutine(ResetAttackIndicator(R_Attack_ActiveTime,attackType));  //Reset indicator of R attack after acitve time limit( default 5 seconds)
                StartCoroutine(ResetCoolDownAttackIndicator(R_Attack_ActiveTime,R_Attack_CooldownTime,attackType));  //Reset indicator for R attack cool down after acitve time limit( default 5 seconds)
            }
            else if(attackType == AttackType.e)
            {
                if(Attack_Q_IsAtcitve || Attack_E_IsAtcitve || Attack_W_IsAtcitve || Attack_R_IsAtcitve) return;
                if(Attack_E_IsAtcitve || Attack_E_CoolDown) return;
                Attack_E_IsAtcitve = true;
                Attack_E_CoolDown = true;
                
                EnableDeactiveIndicator(attackType);
                R_Attack_CooldownTime = character.characterData.GetCoolDownTime(attackType);
                R_Attack_ActiveTime = character.characterData.GetActiveTime(attackType);
                GameManager.instance.TriggerAttackActiveCoroutine(attackType,R_Attack_ActiveTime);


                StartCoroutine(ResetAttackIndicator(R_Attack_ActiveTime,attackType));  //Reset indicator of R attack after acitve time limit( default 5 seconds)
                StartCoroutine(ResetCoolDownAttackIndicator(R_Attack_ActiveTime,R_Attack_CooldownTime,attackType));  //Reset indicator for R attack cool down after acitve time limit( default 5 seconds)
            }
        }
        else if(character.currentCharacterModel.characterType == CharacterType.Moorg)
        {
            if(attackType == AttackType.auto) 
            {
                if(!Attack_Auto_Allowed) return;
                Check_AutoAttackAllowed(); //Allow auto attack if AS time is passed from last auto attack
            }
            if(attackType == AttackType.q)
            {
                if(Attack_Q_IsAtcitve || Attack_E_IsAtcitve || Attack_W_IsAtcitve || Attack_R_IsAtcitve) return;
                if(Attack_Q_IsAtcitve || Attack_Q_CoolDown) return;
                Attack_Q_IsAtcitve = true;
                Attack_Q_CoolDown = true;
                 EnableDeactiveIndicator(attackType);
                R_Attack_CooldownTime = character.characterData.GetCoolDownTime(attackType);
                R_Attack_ActiveTime = character.characterData.GetActiveTime(attackType);
                GameManager.instance.TriggerAttackActiveCoroutine(attackType,R_Attack_ActiveTime);


                StartCoroutine(ResetAttackIndicator(R_Attack_ActiveTime,attackType));  //Reset indicator of R attack after acitve time limit( default 5 seconds)
                StartCoroutine(ResetCoolDownAttackIndicator(R_Attack_ActiveTime,R_Attack_CooldownTime,attackType));  //Reset indicator for R attack cool down after acitve time limit( default 5 seconds)
            }
            if(attackType == AttackType.w)
            {
                if(Attack_Q_IsAtcitve || Attack_E_IsAtcitve || Attack_W_IsAtcitve || Attack_R_IsAtcitve) return;
                if(Attack_W_IsAtcitve || Attack_W_CoolDown) return;
                Attack_W_IsAtcitve = true;
                Attack_W_CoolDown = true;
                
                 EnableDeactiveIndicator(attackType);
                R_Attack_CooldownTime = character.characterData.GetCoolDownTime(attackType);
                R_Attack_ActiveTime = character.characterData.GetActiveTime(attackType);
                GameManager.instance.TriggerAttackActiveCoroutine(attackType,R_Attack_ActiveTime);


                StartCoroutine(ResetAttackIndicator(R_Attack_ActiveTime,attackType));  //Reset indicator of R attack after acitve time limit( default 5 seconds)
                StartCoroutine(ResetCoolDownAttackIndicator(R_Attack_ActiveTime,R_Attack_CooldownTime,attackType));  //Reset indicator for R attack cool down after acitve time limit( default 5 seconds)
            }
            if(attackType == AttackType.e)
            {
                if(Attack_Q_IsAtcitve || Attack_E_IsAtcitve || Attack_W_IsAtcitve || Attack_R_IsAtcitve) return;
                if(Attack_E_IsAtcitve || Attack_E_CoolDown) return;
                Attack_E_IsAtcitve = true;
                Attack_E_CoolDown = true;
                
                 EnableDeactiveIndicator(attackType);
                R_Attack_CooldownTime = character.characterData.GetCoolDownTime(attackType);
                R_Attack_ActiveTime = character.characterData.GetActiveTime(attackType);
                GameManager.instance.TriggerAttackActiveCoroutine(attackType,R_Attack_ActiveTime);


                StartCoroutine(ResetAttackIndicator(R_Attack_ActiveTime,attackType));  //Reset indicator of R attack after acitve time limit( default 5 seconds)
                StartCoroutine(ResetCoolDownAttackIndicator(R_Attack_ActiveTime,R_Attack_CooldownTime,attackType));  //Reset indicator for R attack cool down after acitve time limit( default 5 seconds)
            }
            if(attackType == AttackType.r)
            {
                if(Attack_Q_IsAtcitve || Attack_E_IsAtcitve || Attack_W_IsAtcitve || Attack_R_IsAtcitve) return;
                if(Attack_R_IsAtcitve || Attack_R_CoolDown) return;
                Attack_R_IsAtcitve = true;
                Attack_R_CoolDown = true;
                
                 EnableDeactiveIndicator(attackType);
                R_Attack_CooldownTime = character.characterData.GetCoolDownTime(attackType);
                R_Attack_ActiveTime = character.characterData.GetActiveTime(attackType);
                GameManager.instance.TriggerAttackActiveCoroutine(attackType,R_Attack_ActiveTime);


                StartCoroutine(ResetAttackIndicator(R_Attack_ActiveTime,attackType));  //Reset indicator of R attack after acitve time limit( default 5 seconds)
                StartCoroutine(ResetCoolDownAttackIndicator(R_Attack_ActiveTime,R_Attack_CooldownTime,attackType));  //Reset indicator for R attack cool down after acitve time limit( default 5 seconds)
            }
        }
        else if(character.currentCharacterModel.characterType == CharacterType.Hakka)
        {

            if(attackType == AttackType.auto)
            {
                if(!Attack_Auto_Allowed) return;
                Check_AutoAttackAllowed(); //Allow auto attack if AS time is passed from last auto attack

                attackType = lastAutoAttackWasLeft ? AttackType.right : AttackType.left;  //One by one left then right then left -attacks
                lastAutoAttackWasLeft = !lastAutoAttackWasLeft; //toggle value for next attack
            }
            if(attackType == AttackType.q)
            {
                if(Attack_Q_IsAtcitve || Attack_E_IsAtcitve || Attack_W_IsAtcitve || Attack_R_IsAtcitve) return;
                if(Attack_Q_IsAtcitve || Attack_Q_CoolDown) return;
                Attack_Q_IsAtcitve = true;
                Attack_Q_CoolDown = true;
                 EnableDeactiveIndicator(attackType);
                R_Attack_CooldownTime = character.characterData.GetCoolDownTime(attackType);
                R_Attack_ActiveTime = character.characterData.GetActiveTime(attackType);
                GameManager.instance.TriggerAttackActiveCoroutine(attackType,R_Attack_ActiveTime);


                StartCoroutine(ResetAttackIndicator(R_Attack_ActiveTime,attackType));  //Reset indicator of R attack after acitve time limit( default 5 seconds)
                StartCoroutine(ResetCoolDownAttackIndicator(R_Attack_ActiveTime,R_Attack_CooldownTime,attackType));  //Reset indicator for R attack cool down after acitve time limit( default 5 seconds)
            }
            if(attackType == AttackType.w)
            {
                if(Attack_Q_IsAtcitve || Attack_E_IsAtcitve || Attack_W_IsAtcitve || Attack_R_IsAtcitve) return;
                if(Attack_W_IsAtcitve || Attack_W_CoolDown) return;
                Attack_W_IsAtcitve = true;
                Attack_W_CoolDown = true;
                 EnableDeactiveIndicator(attackType);
                R_Attack_CooldownTime = character.characterData.GetCoolDownTime(attackType);
                R_Attack_ActiveTime = character.characterData.GetActiveTime(attackType);
                GameManager.instance.TriggerAttackActiveCoroutine(attackType,R_Attack_ActiveTime);


                StartCoroutine(ResetAttackIndicator(R_Attack_ActiveTime,attackType));  //Reset indicator of R attack after acitve time limit( default 5 seconds)
                StartCoroutine(ResetCoolDownAttackIndicator(R_Attack_ActiveTime,R_Attack_CooldownTime,attackType));  //Reset indicator for R attack cool down after acitve time limit( default 5 seconds)
            }
            if(attackType == AttackType.e)
            {
                if(Attack_Q_IsAtcitve || Attack_E_IsAtcitve || Attack_W_IsAtcitve || Attack_R_IsAtcitve) return;
                if(Attack_E_IsAtcitve || Attack_E_CoolDown) return;
                Attack_E_IsAtcitve = true;
                Attack_E_CoolDown = true;
                
                 EnableDeactiveIndicator(attackType);
                R_Attack_CooldownTime = character.characterData.GetCoolDownTime(attackType);
                R_Attack_ActiveTime = character.characterData.GetActiveTime(attackType);
                GameManager.instance.TriggerAttackActiveCoroutine(attackType,R_Attack_ActiveTime);


                StartCoroutine(ResetAttackIndicator(R_Attack_ActiveTime,attackType));  //Reset indicator of R attack after acitve time limit( default 5 seconds)
                StartCoroutine(ResetCoolDownAttackIndicator(R_Attack_ActiveTime,R_Attack_CooldownTime,attackType));  //Reset indicator for R attack cool down after acitve time limit( default 5 seconds)
            }
            if(attackType == AttackType.r)
            {
                if(Attack_Q_IsAtcitve || Attack_E_IsAtcitve || Attack_W_IsAtcitve || Attack_R_IsAtcitve) return;
                if(Attack_R_IsAtcitve || Attack_R_CoolDown) return;
                Attack_R_IsAtcitve = true;
                Attack_R_CoolDown = true;
                
                 EnableDeactiveIndicator(attackType);
                R_Attack_CooldownTime = character.characterData.GetCoolDownTime(attackType);
                R_Attack_ActiveTime = character.characterData.GetActiveTime(attackType);
                GameManager.instance.TriggerAttackActiveCoroutine(attackType,R_Attack_ActiveTime);


                StartCoroutine(ResetAttackIndicator(R_Attack_ActiveTime,attackType));  //Reset indicator of R attack after acitve time limit( default 5 seconds)
                StartCoroutine(ResetCoolDownAttackIndicator(R_Attack_ActiveTime,R_Attack_CooldownTime,attackType));  //Reset indicator for R attack cool down after acitve time limit( default 5 seconds)
            }
        }
        else if(character.currentCharacterModel.characterType == CharacterType.Dira)
        {
            if(attackType== AttackType.auto) 
            {
                if(!Attack_Auto_Allowed) return;
                Check_AutoAttackAllowed(); //Allow auto attack if AS time is passed from last auto attack
            }
            if(attackType == AttackType.q)
            {
                if(Attack_Q_IsAtcitve || Attack_E_IsAtcitve || Attack_W_IsAtcitve || Attack_R_IsAtcitve) return;
                if(Attack_Q_IsAtcitve || Attack_Q_CoolDown) return;
                Attack_Q_IsAtcitve = true;
                Attack_Q_CoolDown = true;
                EnableDeactiveIndicator(attackType);
                R_Attack_CooldownTime = character.characterData.GetCoolDownTime(attackType);
                R_Attack_ActiveTime = character.characterData.GetActiveTime(attackType);
                GameManager.instance.TriggerAttackActiveCoroutine(attackType,R_Attack_ActiveTime);


                StartCoroutine(ResetAttackIndicator(R_Attack_ActiveTime,attackType));  //Reset indicator of R attack after acitve time limit( default 5 seconds)
                StartCoroutine(ResetCoolDownAttackIndicator(R_Attack_ActiveTime,R_Attack_CooldownTime,attackType));  //Reset indicator for R attack cool down after acitve time limit( default 5 seconds)
            }
            if(attackType == AttackType.w)
            {
                if(Attack_Q_IsAtcitve || Attack_E_IsAtcitve || Attack_W_IsAtcitve || Attack_R_IsAtcitve) return;
                if(Attack_W_IsAtcitve || Attack_W_CoolDown) return;
                Attack_W_IsAtcitve = true;
                Attack_W_CoolDown = true;
                
                EnableDeactiveIndicator(attackType);
                R_Attack_CooldownTime = character.characterData.GetCoolDownTime(attackType);
                R_Attack_ActiveTime = character.characterData.GetActiveTime(attackType);
                GameManager.instance.TriggerAttackActiveCoroutine(attackType,R_Attack_ActiveTime);


                StartCoroutine(ResetAttackIndicator(R_Attack_ActiveTime,attackType));  //Reset indicator of R attack after acitve time limit( default 5 seconds)
                StartCoroutine(ResetCoolDownAttackIndicator(R_Attack_ActiveTime,R_Attack_CooldownTime,attackType));  //Reset indicator for R attack cool down after acitve time limit( default 5 seconds)
            }
            if(attackType == AttackType.e)
            {
                if(Attack_Q_IsAtcitve || Attack_E_IsAtcitve || Attack_W_IsAtcitve || Attack_R_IsAtcitve) return;
                if(Attack_E_IsAtcitve || Attack_E_CoolDown) return;
                Attack_E_IsAtcitve = true;
                Attack_E_CoolDown = true;
                
                 EnableDeactiveIndicator(attackType);
                R_Attack_CooldownTime = character.characterData.GetCoolDownTime(attackType);
                R_Attack_ActiveTime = character.characterData.GetActiveTime(attackType);
                GameManager.instance.TriggerAttackActiveCoroutine(attackType,R_Attack_ActiveTime);


                StartCoroutine(ResetAttackIndicator(R_Attack_ActiveTime,attackType));  //Reset indicator of R attack after acitve time limit( default 5 seconds)
                StartCoroutine(ResetCoolDownAttackIndicator(R_Attack_ActiveTime,R_Attack_CooldownTime,attackType));  //Reset indicator for R attack cool down after acitve time limit( default 5 seconds)
            }
            if(attackType == AttackType.r)
            {
                if(Attack_Q_IsAtcitve || Attack_E_IsAtcitve || Attack_W_IsAtcitve || Attack_R_IsAtcitve) return;
                if(Attack_R_IsAtcitve || Attack_R_CoolDown) return;
                Attack_R_IsAtcitve = true;
                Attack_R_CoolDown = true;
                
                 EnableDeactiveIndicator(attackType);
                R_Attack_CooldownTime = character.characterData.GetCoolDownTime(attackType);
                R_Attack_ActiveTime = character.characterData.GetActiveTime(attackType);
                GameManager.instance.TriggerAttackActiveCoroutine(attackType,R_Attack_ActiveTime);


                StartCoroutine(ResetAttackIndicator(R_Attack_ActiveTime,attackType));  //Reset indicator of R attack after acitve time limit( default 5 seconds)
                StartCoroutine(ResetCoolDownAttackIndicator(R_Attack_ActiveTime,R_Attack_CooldownTime,attackType));  //Reset indicator for R attack cool down after acitve time limit( default 5 seconds)
            }
        }
        else if(character.currentCharacterModel.characterType == CharacterType.Sura)
        {
            if(attackType == AttackType.auto)
            {
                if(!Attack_Auto_Allowed) return;
                Check_AutoAttackAllowed(); //Allow auto attack if AS time is passed from last auto attack
            }
            if(attackType == AttackType.q)
            {
                if(Attack_Q_IsAtcitve || Attack_E_IsAtcitve || Attack_W_IsAtcitve || Attack_R_IsAtcitve) return;
                if(Attack_Q_IsAtcitve || Attack_Q_CoolDown) return;
                Attack_Q_IsAtcitve = true;
                Attack_Q_CoolDown = true;
                 EnableDeactiveIndicator(attackType);
                R_Attack_CooldownTime = character.characterData.GetCoolDownTime(attackType);
                R_Attack_ActiveTime = character.characterData.GetActiveTime(attackType);
                GameManager.instance.TriggerAttackActiveCoroutine(attackType,R_Attack_ActiveTime);


                StartCoroutine(ResetAttackIndicator(R_Attack_ActiveTime,attackType));  //Reset indicator of R attack after acitve time limit( default 5 seconds)
                StartCoroutine(ResetCoolDownAttackIndicator(R_Attack_ActiveTime,R_Attack_CooldownTime,attackType));  //Reset indicator for R attack cool down after acitve time limit( default 5 seconds)
            }
            if(attackType == AttackType.w)
            {
                if(Attack_Q_IsAtcitve || Attack_E_IsAtcitve || Attack_W_IsAtcitve || Attack_R_IsAtcitve) return;
                if(Attack_W_IsAtcitve || Attack_W_CoolDown) return;
                Attack_W_IsAtcitve = true;
                Attack_W_CoolDown = true;
                
                 EnableDeactiveIndicator(attackType);
                R_Attack_CooldownTime = character.characterData.GetCoolDownTime(attackType);
                R_Attack_ActiveTime = character.characterData.GetActiveTime(attackType);
                GameManager.instance.TriggerAttackActiveCoroutine(attackType,R_Attack_ActiveTime);


                StartCoroutine(ResetAttackIndicator(R_Attack_ActiveTime,attackType));  //Reset indicator of R attack after acitve time limit( default 5 seconds)
                StartCoroutine(ResetCoolDownAttackIndicator(R_Attack_ActiveTime,R_Attack_CooldownTime,attackType));  //Reset indicator for R attack cool down after acitve time limit( default 5 seconds)
            }
            if(attackType == AttackType.e)
            {
                if(Attack_Q_IsAtcitve || Attack_E_IsAtcitve || Attack_W_IsAtcitve || Attack_R_IsAtcitve) return;
                if(Attack_E_IsAtcitve || Attack_E_CoolDown) return;
                Attack_E_IsAtcitve = true;
                Attack_E_CoolDown = true;
                
                 EnableDeactiveIndicator(attackType);
                R_Attack_CooldownTime = character.characterData.GetCoolDownTime(attackType);
                R_Attack_ActiveTime = character.characterData.GetActiveTime(attackType);
                GameManager.instance.TriggerAttackActiveCoroutine(attackType,R_Attack_ActiveTime);


                StartCoroutine(ResetAttackIndicator(R_Attack_ActiveTime,attackType));  //Reset indicator of R attack after acitve time limit( default 5 seconds)
                StartCoroutine(ResetCoolDownAttackIndicator(R_Attack_ActiveTime,R_Attack_CooldownTime,attackType));  //Reset indicator for R attack cool down after acitve time limit( default 5 seconds)
            }
            if(attackType == AttackType.r)
            {
                if(Attack_Q_IsAtcitve || Attack_E_IsAtcitve || Attack_W_IsAtcitve || Attack_R_IsAtcitve) return;
                if(Attack_R_IsAtcitve || Attack_R_CoolDown) return;
                Attack_R_IsAtcitve = true;
                Attack_R_CoolDown = true;
                
                 EnableDeactiveIndicator(attackType);
                R_Attack_CooldownTime = character.characterData.GetCoolDownTime(attackType);
                R_Attack_ActiveTime = character.characterData.GetActiveTime(attackType);
                GameManager.instance.TriggerAttackActiveCoroutine(attackType,R_Attack_ActiveTime);


                StartCoroutine(ResetAttackIndicator(R_Attack_ActiveTime,attackType));  //Reset indicator of R attack after acitve time limit( default 5 seconds)
                StartCoroutine(ResetCoolDownAttackIndicator(R_Attack_ActiveTime,R_Attack_CooldownTime,attackType));  //Reset indicator for R attack cool down after acitve time limit( default 5 seconds)
            }
        }
        else if(character.currentCharacterModel.characterType == CharacterType.Ranzeb)
        {
            if(attackType == AttackType.auto)
            {
                if(!Attack_Auto_Allowed) return;
                Check_AutoAttackAllowed(); //Allow auto attack if AS time is passed from last auto attack
            }
            if(attackType == AttackType.q)
            {
                if(Attack_Q_IsAtcitve || Attack_E_IsAtcitve || Attack_W_IsAtcitve || Attack_R_IsAtcitve) return;
                if(Attack_Q_IsAtcitve || Attack_Q_CoolDown) return;
                Attack_Q_IsAtcitve = true;
                Attack_Q_CoolDown = true;
                 EnableDeactiveIndicator(attackType);
                R_Attack_CooldownTime = character.characterData.GetCoolDownTime(attackType);
                R_Attack_ActiveTime = character.characterData.GetActiveTime(attackType);
                GameManager.instance.TriggerAttackActiveCoroutine(attackType,R_Attack_ActiveTime);


                StartCoroutine(ResetAttackIndicator(R_Attack_ActiveTime,attackType));  //Reset indicator of R attack after acitve time limit( default 5 seconds)
                StartCoroutine(ResetCoolDownAttackIndicator(R_Attack_ActiveTime,R_Attack_CooldownTime,attackType));  //Reset indicator for R attack cool down after acitve time limit( default 5 seconds)
            }
            if(attackType == AttackType.w)
            {
                if(Attack_Q_IsAtcitve || Attack_E_IsAtcitve || Attack_W_IsAtcitve || Attack_R_IsAtcitve) return;
                if(Attack_W_IsAtcitve || Attack_W_CoolDown) return;
                Attack_W_IsAtcitve = true;
                Attack_W_CoolDown = true;
                
                 EnableDeactiveIndicator(attackType);
                R_Attack_CooldownTime = character.characterData.GetCoolDownTime(attackType);
                R_Attack_ActiveTime = character.characterData.GetActiveTime(attackType);
                GameManager.instance.TriggerAttackActiveCoroutine(attackType,R_Attack_ActiveTime);


                StartCoroutine(ResetAttackIndicator(R_Attack_ActiveTime,attackType));  //Reset indicator of R attack after acitve time limit( default 5 seconds)
                StartCoroutine(ResetCoolDownAttackIndicator(R_Attack_ActiveTime,R_Attack_CooldownTime,attackType));  //Reset indicator for R attack cool down after acitve time limit( default 5 seconds)
            }
            if(attackType == AttackType.e)
            {
                if(Attack_Q_IsAtcitve || Attack_E_IsAtcitve || Attack_W_IsAtcitve || Attack_R_IsAtcitve) return;
                if(Attack_E_IsAtcitve || Attack_E_CoolDown) return;
                Attack_E_IsAtcitve = true;
                Attack_E_CoolDown = true;
                
                 EnableDeactiveIndicator(attackType);
                R_Attack_CooldownTime = character.characterData.GetCoolDownTime(attackType);
                R_Attack_ActiveTime = character.characterData.GetActiveTime(attackType);
                GameManager.instance.TriggerAttackActiveCoroutine(attackType,R_Attack_ActiveTime);


                StartCoroutine(ResetAttackIndicator(R_Attack_ActiveTime,attackType));  //Reset indicator of R attack after acitve time limit( default 5 seconds)
                StartCoroutine(ResetCoolDownAttackIndicator(R_Attack_ActiveTime,R_Attack_CooldownTime,attackType));  //Reset indicator for R attack cool down after acitve time limit( default 5 seconds)
            }
            if(attackType == AttackType.r)
            {
                if(Attack_Q_IsAtcitve || Attack_E_IsAtcitve || Attack_W_IsAtcitve || Attack_R_IsAtcitve) return;
                if(Attack_R_IsAtcitve || Attack_R_CoolDown) return;
                Attack_R_IsAtcitve = true;
                Attack_R_CoolDown = true;
                
                 EnableDeactiveIndicator(attackType);
                R_Attack_CooldownTime = character.characterData.GetCoolDownTime(attackType);
                R_Attack_ActiveTime = character.characterData.GetActiveTime(attackType);
                GameManager.instance.TriggerAttackActiveCoroutine(attackType,R_Attack_ActiveTime);


                StartCoroutine(ResetAttackIndicator(R_Attack_ActiveTime,attackType));  //Reset indicator of R attack after acitve time limit( default 5 seconds)
                StartCoroutine(ResetCoolDownAttackIndicator(R_Attack_ActiveTime,R_Attack_CooldownTime,attackType));  //Reset indicator for R attack cool down after acitve time limit( default 5 seconds)
            }
        }
        else if(character.currentCharacterModel.characterType == CharacterType.Jahan)
        {
            if(attackType == AttackType.auto)
            {
                if(!Attack_Auto_Allowed) return;
                Check_AutoAttackAllowed(); //Allow auto attack if AS time is passed from last auto attack
            }
            if(attackType == AttackType.q)
            {
                if(Attack_Q_IsAtcitve || Attack_E_IsAtcitve || Attack_W_IsAtcitve || Attack_R_IsAtcitve) return;
                if(Attack_Q_IsAtcitve || Attack_Q_CoolDown) return;
                Attack_Q_IsAtcitve = true;
                Attack_Q_CoolDown = true;
                 EnableDeactiveIndicator(attackType);
                R_Attack_CooldownTime = character.characterData.GetCoolDownTime(attackType);
                R_Attack_ActiveTime = character.characterData.GetActiveTime(attackType);
                GameManager.instance.TriggerAttackActiveCoroutine(attackType,R_Attack_ActiveTime);


                StartCoroutine(ResetAttackIndicator(R_Attack_ActiveTime,attackType));  //Reset indicator of R attack after acitve time limit( default 5 seconds)
                StartCoroutine(ResetCoolDownAttackIndicator(R_Attack_ActiveTime,R_Attack_CooldownTime,attackType));  //Reset indicator for R attack cool down after acitve time limit( default 5 seconds)
            }
            if(attackType == AttackType.w)
            {
                if(Attack_Q_IsAtcitve || Attack_E_IsAtcitve || Attack_W_IsAtcitve || Attack_R_IsAtcitve) return;
                if(Attack_W_IsAtcitve || Attack_W_CoolDown) return;
                Attack_W_IsAtcitve = true;
                Attack_W_CoolDown = true;
                
                 EnableDeactiveIndicator(attackType);
                R_Attack_CooldownTime = character.characterData.GetCoolDownTime(attackType);
                R_Attack_ActiveTime = character.characterData.GetActiveTime(attackType);
                GameManager.instance.TriggerAttackActiveCoroutine(attackType,R_Attack_ActiveTime);


                StartCoroutine(ResetAttackIndicator(R_Attack_ActiveTime,attackType));  //Reset indicator of R attack after acitve time limit( default 5 seconds)
                StartCoroutine(ResetCoolDownAttackIndicator(R_Attack_ActiveTime,R_Attack_CooldownTime,attackType));  //Reset indicator for R attack cool down after acitve time limit( default 5 seconds)
            }
            if(attackType == AttackType.e)
            {
                if(Attack_Q_IsAtcitve || Attack_E_IsAtcitve || Attack_W_IsAtcitve || Attack_R_IsAtcitve) return;
                if(Attack_E_IsAtcitve || Attack_E_CoolDown) return;
                Attack_E_IsAtcitve = true;
                Attack_E_CoolDown = true;
                
                 EnableDeactiveIndicator(attackType);
                R_Attack_CooldownTime = character.characterData.GetCoolDownTime(attackType);
                R_Attack_ActiveTime = character.characterData.GetActiveTime(attackType);
                GameManager.instance.TriggerAttackActiveCoroutine(attackType,R_Attack_ActiveTime);


                StartCoroutine(ResetAttackIndicator(R_Attack_ActiveTime,attackType));  //Reset indicator of R attack after acitve time limit( default 5 seconds)
                StartCoroutine(ResetCoolDownAttackIndicator(R_Attack_ActiveTime,R_Attack_CooldownTime,attackType));  //Reset indicator for R attack cool down after acitve time limit( default 5 seconds)
            }
            if(attackType == AttackType.r)
            {
                if(Attack_Q_IsAtcitve || Attack_E_IsAtcitve || Attack_W_IsAtcitve || Attack_R_IsAtcitve) return;
                if(Attack_R_IsAtcitve || Attack_R_CoolDown) return;
                Attack_R_IsAtcitve = true;
                Attack_R_CoolDown = true;
                
                 EnableDeactiveIndicator(attackType);
                R_Attack_CooldownTime = character.characterData.GetCoolDownTime(attackType);
                R_Attack_ActiveTime = character.characterData.GetActiveTime(attackType);
                GameManager.instance.TriggerAttackActiveCoroutine(attackType,R_Attack_ActiveTime);


                StartCoroutine(ResetAttackIndicator(R_Attack_ActiveTime,attackType));  //Reset indicator of R attack after acitve time limit( default 5 seconds)
                StartCoroutine(ResetCoolDownAttackIndicator(R_Attack_ActiveTime,R_Attack_CooldownTime,attackType));  //Reset indicator for R attack cool down after acitve time limit( default 5 seconds)
            }
        }
        else if(character.currentCharacterModel.characterType == CharacterType.Tapani)
        {
            if(attackType == AttackType.auto)
            {
                if(!Attack_Auto_Allowed) return;

                Check_AutoAttackAllowed(); //Allow auto attack if AS time is passed from last auto attack
                attackType = lastAutoAttackWasLeft ? AttackType.right : AttackType.left;  //One by one left then right then left -attacks
                lastAutoAttackWasLeft = !lastAutoAttackWasLeft; //toggle value for next attack
            }

            if(attackType == AttackType.q)
            {
                if(Attack_Q_IsAtcitve || Attack_E_IsAtcitve || Attack_W_IsAtcitve || Attack_R_IsAtcitve) return;
                if(Attack_Q_IsAtcitve || Attack_Q_CoolDown) return;
                Attack_Q_IsAtcitve = true;
                Attack_Q_CoolDown = true;
                EnableDeactiveIndicator(attackType);
                R_Attack_CooldownTime = character.characterData.GetCoolDownTime(attackType);
                R_Attack_ActiveTime = character.characterData.GetActiveTime(attackType);
                GameManager.instance.TriggerAttackActiveCoroutine(attackType,R_Attack_ActiveTime);


                StartCoroutine(ResetAttackIndicator(R_Attack_ActiveTime,attackType));  //Reset indicator of R attack after acitve time limit( default 5 seconds)
                StartCoroutine(ResetCoolDownAttackIndicator(R_Attack_ActiveTime,R_Attack_CooldownTime,attackType));  //Reset indicator for R attack cool down after acitve time limit( default 5 seconds)
            }
            if(attackType == AttackType.w)
            {
                if(Attack_Q_IsAtcitve || Attack_E_IsAtcitve || Attack_W_IsAtcitve || Attack_R_IsAtcitve) return;
                if(Attack_W_IsAtcitve || Attack_W_CoolDown) return;
                Attack_W_IsAtcitve = true;
                Attack_W_CoolDown = true;
                
                EnableDeactiveIndicator(attackType);
                R_Attack_CooldownTime = character.characterData.GetCoolDownTime(attackType);
                R_Attack_ActiveTime = character.characterData.GetActiveTime(attackType);
                GameManager.instance.TriggerAttackActiveCoroutine(attackType,R_Attack_ActiveTime);


                StartCoroutine(ResetAttackIndicator(R_Attack_ActiveTime,attackType));  //Reset indicator of R attack after acitve time limit( default 5 seconds)
                StartCoroutine(ResetCoolDownAttackIndicator(R_Attack_ActiveTime,R_Attack_CooldownTime,attackType));  //Reset indicator for R attack cool down after acitve time limit( default 5 seconds)
            }
            if(attackType == AttackType.e)
            {
                if(Attack_Q_IsAtcitve || Attack_E_IsAtcitve || Attack_W_IsAtcitve || Attack_R_IsAtcitve) return;
                if(Attack_E_IsAtcitve || Attack_E_CoolDown) return;
                Attack_E_IsAtcitve = true;
                Attack_E_CoolDown = true;
                
                EnableDeactiveIndicator(attackType);
                R_Attack_CooldownTime = character.characterData.GetCoolDownTime(attackType);
                R_Attack_ActiveTime = character.characterData.GetActiveTime(attackType);
                GameManager.instance.TriggerAttackActiveCoroutine(attackType,R_Attack_ActiveTime);


                StartCoroutine(ResetAttackIndicator(R_Attack_ActiveTime,attackType));  //Reset indicator of R attack after acitve time limit( default 5 seconds)
                StartCoroutine(ResetCoolDownAttackIndicator(R_Attack_ActiveTime,R_Attack_CooldownTime,attackType));  //Reset indicator for R attack cool down after acitve time limit( default 5 seconds)
            }
            if(attackType == AttackType.r)
            {
                if(Attack_Q_IsAtcitve || Attack_E_IsAtcitve || Attack_W_IsAtcitve || Attack_R_IsAtcitve) return;
                if(Attack_R_IsAtcitve || Attack_R_CoolDown) return;
                Attack_R_IsAtcitve = true;
                Attack_R_CoolDown = true;
                
                EnableDeactiveIndicator(attackType);
                R_Attack_CooldownTime = character.characterData.GetCoolDownTime(attackType);
                R_Attack_ActiveTime = character.characterData.GetActiveTime(attackType);
                GameManager.instance.TriggerAttackActiveCoroutine(attackType,R_Attack_ActiveTime);


                StartCoroutine(ResetAttackIndicator(R_Attack_ActiveTime,attackType));  //Reset indicator of R attack after acitve time limit( default 5 seconds)
                StartCoroutine(ResetCoolDownAttackIndicator(R_Attack_ActiveTime,R_Attack_CooldownTime,attackType));  //Reset indicator for R attack cool down after acitve time limit( default 5 seconds)
            }
        }
        else if(character.currentCharacterModel.characterType == CharacterType.Serina)
        {
            if(attackType == AttackType.auto)
            {
                if(!Attack_Auto_Allowed) return;

                Check_AutoAttackAllowed(); //Allow auto attack if AS time is passed from last auto attack
            }
            if(attackType == AttackType.q)
            {
                if(Attack_Q_IsAtcitve || Attack_E_IsAtcitve || Attack_W_IsAtcitve || Attack_R_IsAtcitve) return;
                if(Attack_Q_IsAtcitve || Attack_Q_CoolDown) return;
                Attack_Q_IsAtcitve = true;
                Attack_Q_CoolDown = true;


                EnableDeactiveIndicator(attackType);

                R_Attack_CooldownTime = character.characterData.GetCoolDownTime(attackType);
                R_Attack_ActiveTime = character.characterData.GetActiveTime(attackType);
                GameManager.instance.TriggerAttackActiveCoroutine(attackType,R_Attack_ActiveTime);


                StartCoroutine(ResetAttackIndicator(R_Attack_ActiveTime,attackType));  //Reset indicator of R attack after acitve time limit( default 5 seconds)
                StartCoroutine(ResetCoolDownAttackIndicator(R_Attack_ActiveTime,R_Attack_CooldownTime,attackType));  //Reset indicator for R attack cool down after acitve time limit( default 5 seconds)
            }
            if(attackType == AttackType.w)
            {
                if(Attack_Q_IsAtcitve || Attack_E_IsAtcitve || Attack_W_IsAtcitve || Attack_R_IsAtcitve) return;
                if(Attack_W_IsAtcitve || Attack_W_CoolDown) return;
                Attack_W_IsAtcitve = true;
                Attack_W_CoolDown = true;
                
                EnableDeactiveIndicator(attackType);
                R_Attack_CooldownTime = character.characterData.GetCoolDownTime(attackType);
                R_Attack_ActiveTime = character.characterData.GetActiveTime(attackType);
                GameManager.instance.TriggerAttackActiveCoroutine(attackType,R_Attack_ActiveTime);


                StartCoroutine(ResetAttackIndicator(R_Attack_ActiveTime,attackType));  //Reset indicator of R attack after acitve time limit( default 5 seconds)
                StartCoroutine(ResetCoolDownAttackIndicator(R_Attack_ActiveTime,R_Attack_CooldownTime,attackType));  //Reset indicator for R attack cool down after acitve time limit( default 5 seconds)
            }
            if(attackType == AttackType.e)
            {
                if(Attack_Q_IsAtcitve || Attack_E_IsAtcitve || Attack_W_IsAtcitve || Attack_R_IsAtcitve) return;
                if(Attack_E_IsAtcitve || Attack_E_CoolDown) return;
                Attack_E_IsAtcitve = true;
                Attack_E_CoolDown = true;
                
                EnableDeactiveIndicator(attackType);
                R_Attack_CooldownTime = character.characterData.GetCoolDownTime(attackType);
                R_Attack_ActiveTime = character.characterData.GetActiveTime(attackType);
                GameManager.instance.TriggerAttackActiveCoroutine(attackType,R_Attack_ActiveTime);


                StartCoroutine(ResetAttackIndicator(R_Attack_ActiveTime,attackType));  //Reset indicator of R attack after acitve time limit( default 5 seconds)
                StartCoroutine(ResetCoolDownAttackIndicator(R_Attack_ActiveTime,R_Attack_CooldownTime,attackType));  //Reset indicator for R attack cool down after acitve time limit( default 5 seconds)
            }
            if(attackType == AttackType.r)
            {
                if(Attack_Q_IsAtcitve || Attack_E_IsAtcitve || Attack_W_IsAtcitve || Attack_R_IsAtcitve) return;
                if(Attack_R_IsAtcitve || Attack_R_CoolDown) return;
                Attack_R_IsAtcitve = true;
                Attack_R_CoolDown = true;
                
                EnableDeactiveIndicator(attackType);
                R_Attack_CooldownTime = character.characterData.GetCoolDownTime(attackType);
                R_Attack_ActiveTime = character.characterData.GetActiveTime(attackType);
                GameManager.instance.TriggerAttackActiveCoroutine(attackType,R_Attack_ActiveTime);


                StartCoroutine(ResetAttackIndicator(R_Attack_ActiveTime,attackType));  //Reset indicator of R attack after acitve time limit( default 5 seconds)
                StartCoroutine(ResetCoolDownAttackIndicator(R_Attack_ActiveTime,R_Attack_CooldownTime,attackType));  //Reset indicator for R attack cool down after acitve time limit( default 5 seconds)
            }
        }
        else if(character.currentCharacterModel.characterType == CharacterType.Udara)
        {
            if(attackType == AttackType.auto)
            {
                Check_AutoAttackAllowed(); //Allow auto attack if AS time is passed from last auto attack
            }
            if(attackType == AttackType.q)
            {
                if(Attack_Q_IsAtcitve || Attack_E_IsAtcitve || Attack_W_IsAtcitve || Attack_R_IsAtcitve) return;
                if(Attack_Q_IsAtcitve || Attack_Q_CoolDown) return;
                Attack_Q_IsAtcitve = true;
                Attack_Q_CoolDown = true;
                EnableDeactiveIndicator(attackType);
                R_Attack_CooldownTime = character.characterData.GetCoolDownTime(attackType);
                R_Attack_ActiveTime = character.characterData.GetActiveTime(attackType);
                GameManager.instance.TriggerAttackActiveCoroutine(attackType,R_Attack_ActiveTime);


                StartCoroutine(ResetAttackIndicator(R_Attack_ActiveTime,attackType));  //Reset indicator of R attack after acitve time limit( default 5 seconds)
                StartCoroutine(ResetCoolDownAttackIndicator(R_Attack_ActiveTime,R_Attack_CooldownTime,attackType));  //Reset indicator for R attack cool down after acitve time limit( default 5 seconds)
            }
            if(attackType == AttackType.w)
            {
                if(Attack_Q_IsAtcitve || Attack_E_IsAtcitve || Attack_W_IsAtcitve || Attack_R_IsAtcitve) return;
                if(Attack_W_IsAtcitve || Attack_W_CoolDown) return;
                Attack_W_IsAtcitve = true;
                Attack_W_CoolDown = true;
                
                EnableDeactiveIndicator(attackType);
                R_Attack_CooldownTime = character.characterData.GetCoolDownTime(attackType);
                R_Attack_ActiveTime = character.characterData.GetActiveTime(attackType);
                GameManager.instance.TriggerAttackActiveCoroutine(attackType,R_Attack_ActiveTime);


                StartCoroutine(ResetAttackIndicator(R_Attack_ActiveTime,attackType));  //Reset indicator of R attack after acitve time limit( default 5 seconds)
                StartCoroutine(ResetCoolDownAttackIndicator(R_Attack_ActiveTime,R_Attack_CooldownTime,attackType));  //Reset indicator for R attack cool down after acitve time limit( default 5 seconds)
            }
            if(attackType == AttackType.e)
            {
                if(Attack_Q_IsAtcitve || Attack_E_IsAtcitve || Attack_W_IsAtcitve || Attack_R_IsAtcitve) return;
                if(Attack_E_IsAtcitve || Attack_E_CoolDown) return;
                Attack_E_IsAtcitve = true;
                Attack_E_CoolDown = true;
                
                EnableDeactiveIndicator(attackType);
                R_Attack_CooldownTime = character.characterData.GetCoolDownTime(attackType);
                R_Attack_ActiveTime = character.characterData.GetActiveTime(attackType);
                GameManager.instance.TriggerAttackActiveCoroutine(attackType,R_Attack_ActiveTime);


                StartCoroutine(ResetAttackIndicator(R_Attack_ActiveTime,attackType));  //Reset indicator of R attack after acitve time limit( default 5 seconds)
                StartCoroutine(ResetCoolDownAttackIndicator(R_Attack_ActiveTime,R_Attack_CooldownTime,attackType));  //Reset indicator for R attack cool down after acitve time limit( default 5 seconds)
            }
            if(attackType == AttackType.r)
            {
                if(Attack_Q_IsAtcitve || Attack_E_IsAtcitve || Attack_W_IsAtcitve || Attack_R_IsAtcitve) return;
                if(Attack_R_IsAtcitve || Attack_R_CoolDown) return;
                Attack_R_IsAtcitve = true;
                Attack_R_CoolDown = true;
                
                 EnableDeactiveIndicator(attackType);
                R_Attack_CooldownTime = character.characterData.GetCoolDownTime(attackType);
                R_Attack_ActiveTime = character.characterData.GetActiveTime(attackType);
                GameManager.instance.TriggerAttackActiveCoroutine(attackType,R_Attack_ActiveTime);


                StartCoroutine(ResetAttackIndicator(R_Attack_ActiveTime,attackType));  //Reset indicator of R attack after acitve time limit( default 5 seconds)
                StartCoroutine(ResetCoolDownAttackIndicator(R_Attack_ActiveTime,R_Attack_CooldownTime,attackType));  //Reset indicator for R attack cool down after acitve time limit( default 5 seconds)
            }
        }

        if(character.currentCharacterModel.characterType == CharacterType.Otrill && OtrillRActivated)
        {
            OtrillRActivated = false;
        }
        else
        {
            if(characterAnimator)
            { //Trigger attack animation
                characterAnimator.SetBool(attackType.ToString(),true);
                StartCoroutine(SetBoolOff(attackType,0.2f));
            }
        }

    }
    /// <summary>
    /// Enable cover image that indicates that button is on cooldown
    /// </summary>
    /// <param name="attackType">current attack type</param>
    private  void EnableDeactiveIndicator(AttackType attackType)
    {
        AttackButton attackButton = GameManager.instance.AttackButtons.Find(x => x.attackType == attackType);
        Image InactiveImage = attackButton.DeactiveIndicator.GetComponent<Image>();
        InactiveImage.fillAmount = 1;
        attackButton.DeactiveIndicator.SetActive(true);
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
        GameManager.instance.AttackButtons.Find(x => x.attackType == AttackType.auto).button.gameObject.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Auto";
    }
    //Reset after cool down time
    public void ResetRAttackCoolDownIndicator()
    {
        Attack_R_CoolDown = false;
        Debug.LogError("Stop");
    }

    //Reset indicator boolean of attack
    public IEnumerator ResetAttackIndicator(float delay,AttackType attackType)
    {
        yield return new WaitForSeconds(delay);
        switch(attackType)
        {
            case AttackType.w:
                Attack_W_IsAtcitve = false;
                break;
            case AttackType.q:
                Attack_Q_IsAtcitve = false;
                break;
            case AttackType.e:
                Attack_E_IsAtcitve = false;
                break;
            case AttackType.r:
                Attack_R_IsAtcitve = false;
                break;
            case AttackType.auto:
                break;
            case AttackType.left:
                break;
            case AttackType.right:
                break;
            case AttackType.rLeft:
                break;
            case AttackType.rRight:
                break;
            case AttackType.None:
                break;
            case AttackType.DieAnimation:
                break;
            default:
                break;
        }
       
    }

    //Reset cooldown boolean of attack
    public IEnumerator ResetCoolDownAttackIndicator(float delayActiveTime,float delayCoolDownTime,AttackType attackType)
    {
        float delay = delayActiveTime + delayCoolDownTime;
        //Start cooldown timer
        GameManager.instance.AttackButtons.Find(x => x.attackType == attackType).StartTimerUpdateCoroutine(delayActiveTime,delayCoolDownTime);  
        yield return new WaitForSeconds(delay);
        switch(attackType)
        {
            case AttackType.w:
               Attack_W_CoolDown = false;
                break;
            case AttackType.q:
                Attack_Q_CoolDown = false;
                break;
            case AttackType.e:
                 Attack_E_CoolDown = false;
                break;
            case AttackType.r:
                 Attack_R_CoolDown = false;
                break;
            case AttackType.auto:
                break;
            case AttackType.left:
                break;
            case AttackType.right:
                break;
            case AttackType.rLeft:
                break;
            case AttackType.rRight:
                break;
            case AttackType.None:
                break;
            case AttackType.DieAnimation:
                break;
            default:
                break;
        }
        GameManager.instance.AttackButtons.Find(x => x.attackType == attackType).DeactiveIndicator.SetActive(false);
    }
    //Cancel all invokes when object is destroyed
    private void OnDestroy()
    {
        CancelInvoke();
    }
    /// <summary>
    /// Triggers death animation of character 
    /// </summary>
    public void TriggerDeathAnimation()
    {
        characterAnimator.SetBool("die",true);
        Invoke("SetDeathBoolOff",0.3f);
    }
    /// <summary>
    /// Reset death animation bool to avoid death animation loop
    /// </summary>
    public void SetDeathBoolOff()
    {
        characterAnimator.SetBool("die",false);
    }
    /// <summary>
    /// Destory character gameobject 
    /// </summary>
    public void CharacterDie()
    {
        GameManager.instance.CharacterLastPosition = transform.position;
        GameManager.instance.SpawnCharacter();      //Temp for development spawn character after destroy
        Destroy(this.gameObject);
    }
    /// <summary>
    /// Set animation movement modifier speed with respect to character and animation type
    /// </summary>
    /// <param name="_currentActiveAnimation">current animation type</param>
    public void SetAnimationMovementSpeedModifier(AttackType _currentActiveAnimation)
    {
        currentActiveAnimation = _currentActiveAnimation;
        AnimationMovementSpeedModifier = character.characterData != null && currentActiveAnimation != AttackType.None ? character.characterData.attackAnimationDetails.Find(x => x.attackType == currentActiveAnimation).movementSpeedModifier : defaultModifierValue;

    }
    /// <summary>
    /// Reset animation movement modifier value to default
    /// </summary>
    public void ResetAnimationMovementSpeedModifier()
    {
        currentActiveAnimation = AttackType.None;
        AnimationMovementSpeedModifier = defaultModifierValue;
    }
    /// <summary>
    /// Cooldown like timer for AutoAttack
    /// </summary>
    /// <param name="AS">Attack speed</param>
    /// <returns></returns>
    public IEnumerator AutoAttack_ASCoroutine(float AS)
    {
        Attack_Auto_Allowed = false;
        yield return new  WaitForSeconds(AS);
        Attack_Auto_Allowed = true;
    }
    /// <summary>
    /// Check if auto attack is allowed,  if not allowed then return 
    /// </summary>
    public void Check_AutoAttackAllowed() 
    {
            //if(!Attack_Auto_Allowed)
            //{
            //    return;            //Wait while next attack is allowed
            //}
            StartCoroutine(AutoAttack_ASCoroutine(AS_CapValue));  //Wait for time equal to AS-2.5 seconds before next auto attack
    }
}
/// <summary>
/// Character attack types
/// </summary>
[System.Serializable]
public enum AttackType { w, q, e, r, auto, left, right, rLeft, rRight, None, DieAnimation }

/// <summary>
/// Character attack types
/// </summary>
[System.Serializable]
public enum AttackSubType { None, AD,AP}
