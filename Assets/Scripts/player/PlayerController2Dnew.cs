using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class PlayerController2Dnew : MonoBehaviour
{
    public Rigidbody2D rb;

    PlayerBase playerBase;
    PlayerHealth playerHealth;

    public float walkSpeed = 4f;
    public float jumpSpeed = 20f;
    public Animator animator;
    public float inputHorizontal;
    public float inputVertical;
    public float dashAmount = 1.5f;
    public float dashDuration = 0.2f;
    public float invincibleTimer = 2f;
    public bool isWin = false;

    public Transform meleeAttackPoint;
    public float meleeAttackRange = 0.5f;
    public LayerMask attackMask;
    public int meleeAttack1 = 30;
    public int meleeAttack2 = 40;

    public bool isAttacking = false;
    public float comboDelay = 1.0f;
    public bool finishedCombo = false;

    public bool isInvincible = false;

    public float dashCoolDown;

    public static PlayerController2Dnew instace;

    public float rollStartSpeed = 10.0f;
    public float rollSpeedDropMultiplier = 4f;
    public float rollSpeedMinimum = 1f;



    private float rollSpeed;
    private float localGravityScale;

    public VisualEffect vfxAttack1;
    public VisualEffect vfxAttack2;

    public GameObject ground;

    public enum MovementState
    {
        Normal, Rolling, Jumping, Attacking
    }

    public MovementState movementState = MovementState.Normal;

    [SerializeField] private float dashDelayTimer;
    [SerializeField] private bool isDashDelay = false;
    [SerializeField] private LayerMask dashLayerMask;

    [SerializeField] private float rollDelayTimer;
    [SerializeField] private bool isRollDelay = false;

    Vector2 moveInput;
    Vector2 lastMoveInput;
    Vector2 rollDirection;

    private float actionCount;

    private bool isDashButtonDown;
    private bool isJumpButtonDown;

    private bool isGrounded = false;
    [SerializeField] private Transform groundCheckColider;
    [SerializeField] private LayerMask groundLayer;

    private float previousY;

    private void Awake()
    {
        instace = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        movementState = MovementState.Normal;
        TryGetComponent<PlayerBase>(out playerBase);
        TryGetComponent<PlayerHealth>(out playerHealth);
        if (rb == null)
        {
            if (!TryGetComponent<Rigidbody2D>(out rb))
            {
                Debug.Log("Rigidbody2D is missing");
            }

        }

        if (animator == null)
        {
            if (!TryGetComponent<Animator>(out animator))
            {
                Debug.Log("Animator is missing");
            }

        }
        ground = GameObject.FindGameObjectWithTag("Ground");
        localGravityScale = rb.gravityScale;
        previousY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
       
        if(Island())
        {
            FindObjectOfType<AudioManager>().PlaySound("PlayerJumpDown");
        }
        UpdateRollDelayTimer();
        UpdateDashDelayTimer();
        switch (movementState)
        {
            case MovementState.Normal:
                if (invincibleTimer > 0)
                {
                    invincibleTimer -= Time.deltaTime;
                }
                if(invincibleTimer <= 0)
                {
                    isInvincible = false;
                }
                break;
            case MovementState.Rolling:
                rollSpeed -= rollSpeed * rollSpeedDropMultiplier * Time.deltaTime;
                if(rollSpeed <= rollSpeedMinimum)
                {
                    rb.gravityScale = localGravityScale;
                    movementState = MovementState.Normal;
                    isInvincible = false;
                }
                break;
            case MovementState.Attacking:
                if(actionCount < 0)
                {
                    movementState = MovementState.Normal;

                }

                break;
        }
    }

    void FixedUpdate()
    {
        if (comboDelay > 0)
        {
            comboDelay -= Time.deltaTime;
        }

        if (isWin)
        {
            animator.SetTrigger("isWin");
        }

        if(playerHealth.GetisDead())
        {
            playerBase.ChangeAnimationState(PlayerBase.PLAYER_DIE);
        }

        if (!playerHealth.GetisDead() && !isWin)
        {
            switch (movementState)
            {
                case MovementState.Normal:


                    if ((inputHorizontal != 0 || inputVertical != 0))
                    {

                        rb.velocity = new Vector2(inputHorizontal * walkSpeed, rb.velocity.y);
                        if (inputHorizontal > 0.01f)
                        {
                            playerBase.FilpXLocalScale(false);
                        }
                        else if (inputHorizontal < -0.01f)
                        {
                            playerBase.FilpXLocalScale(true);
                        }

                        if (!GroundCheck())
                        {
                            if (rb.velocity.y > 0)
                            {
                                playerBase.ChangeAnimationState(PlayerBase.PLAYER_JUMP_UP);
                            }
                            else
                            {
                                playerBase.ChangeAnimationState(PlayerBase.PLAYER_JUMP_DOWN);
                            }
                        }
                        else
                        {
                            playerBase.ChangeAnimationState(PlayerBase.PLAYER_RUN);
                        }
                        DashFixedUpdateMovement();

                    }
                    else if ((inputHorizontal == 0 && inputVertical == 0) && isDashButtonDown && (lastMoveInput.x != 0 && lastMoveInput.y != 0))
                    {
                        DashFixedUpdateMovement();
                        playerBase.ChangeAnimationState(PlayerBase.PLAYER_IDLE);
                    }


                    else if (inputHorizontal == 0 && inputVertical == 0)
                    {
                        rb.velocity = new Vector2(0, rb.velocity.y);

                        if (!GroundCheck())
                        {
                            if (rb.velocity.y > 0)
                            {
                                playerBase.ChangeAnimationState(PlayerBase.PLAYER_JUMP_UP);
                            }
                            else
                            {
                                playerBase.ChangeAnimationState(PlayerBase.PLAYER_JUMP_DOWN);
                            }

                        }
                        else
                        {
                            playerBase.ChangeAnimationState(PlayerBase.PLAYER_IDLE);
                        }

                    }
                    JumpUpdateMovement();
                    break;
                case MovementState.Rolling:
                    rb.velocity = rollDirection * rollSpeed;
                    playerBase.ChangeAnimationState(PlayerBase.PLAYER_DASH);
                    break;
                case MovementState.Attacking:
                    if (actionCount >= 0)
                    {
                        actionCount -= Time.deltaTime;
                    }
                    break;
            }
        }
        
        
        

    }




    void OnMove(InputValue value)
    {

        moveInput = value.Get<Vector2>();
        inputHorizontal = moveInput.x;
        //inputVertical = moveInput.y;
        if (inputHorizontal != 0 || inputVertical != 0)
        {
            lastMoveInput = moveInput;
        }
        switch (movementState)
        {
            case MovementState.Normal:
                
                break;
            case MovementState.Rolling:
                break;

        }

    }

    void OnAttack(InputValue value)
    {
        switch (movementState)
        {
            case MovementState.Normal:
                
                if(isGrounded)
                {
                    finishedCombo = false;
                    movementState = MovementState.Attacking;
                    isAttacking = true;
                    actionCount = 0.5f;
                }
                break;
            case MovementState.Attacking:
                if(!finishedCombo)
                {
                    isAttacking = true;
                    actionCount += 0.5f;
                    finishedCombo = true;
                }
                
                break;
           
        }
    }

    void OnDash(InputValue value)
    {
        switch(movementState)
        {
            case MovementState.Normal:
                    if (lastMoveInput.x == 0) return;
                    if (!isDashDelay)
                    {
                        isDashButtonDown = value.isPressed;
                    }
                break;
            case MovementState.Rolling:
                break;
        }
        
    }

    void OnRoll(InputValue value)
    {
        
        switch (movementState)
        {
            case MovementState.Normal:
                if (inputHorizontal == 0 ) return;
                if(!isRollDelay)
                {
                    rollDirection = lastMoveInput;
                    rollSpeed = rollStartSpeed;
                    movementState = MovementState.Rolling;
                    rb.gravityScale = 0f;
                    rollDelayTimer = dashCoolDown;
                    isRollDelay = true;
                    isInvincible = true;
                }
                break;
            case MovementState.Rolling:
                break;
                
        }
    }
    void OnJump(InputValue value)
    {
        switch (movementState)
        {
            case MovementState.Normal:
 
                if (!GroundCheck()) return;
                isJumpButtonDown = true;
                break;
            case MovementState.Rolling:
                break;
        }
    }


    void UpdateDashDelayTimer()
    {
        if(isDashDelay)
        {
            dashDelayTimer -= Time.deltaTime;
            if(dashDelayTimer <= 0f)
            {
                isDashDelay = false;
            }
        }
    }

    void UpdateRollDelayTimer()
    {
        if (isRollDelay)
        {
            rollDelayTimer -= Time.deltaTime;
            if (rollDelayTimer <= 0f)
            {
                isRollDelay = false;
            }
        }
    }

    void DashFixedUpdateMovement()
    {
        if (isDashButtonDown && !isDashDelay)
        {
            isDashDelay = true;
            dashDelayTimer = dashDuration;

            Vector2 currentPosition = transform.position;
            Vector2 dashPosition;

            RaycastHit2D raycastHit2D = Physics2D.Raycast(currentPosition, moveInput, dashAmount, (int)dashLayerMask);

            if (raycastHit2D.collider)
            {
                dashPosition = raycastHit2D.point;
            }
            else
            {
                dashPosition = currentPosition + moveInput * dashAmount;
            }

            rb.MovePosition(dashPosition);
            isDashButtonDown = false;
            playerBase.ChangeAnimationState(PlayerBase.PLAYER_DASH);
        }
    }

    void JumpUpdateMovement()
    {
        if(isJumpButtonDown)
        {
            rb.velocity = Vector2.up * jumpSpeed;
            isJumpButtonDown = false;

        }
    }

    bool GroundCheck()
    {
        isGrounded = false;
        Collider2D[] collider = Physics2D.OverlapCircleAll(groundCheckColider.position,0.2f, groundLayer);
        if(collider.Length > 0)
        {
            isGrounded = true;
        }
        return isGrounded;
    }

    bool Island()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(groundCheckColider.position, 0.2f, groundLayer);
        return !isGrounded && collider.Length > 0;
    }

    public void Attack1()
    {
        FindObjectOfType<AudioManager>().PlaySound("PlayerAttack1");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(meleeAttackPoint.position, meleeAttackRange, attackMask);
        foreach(Collider2D enemy in hitEnemies)
        {
            //Instantiate(attackVFX1, enemy.transform.position, enemy.transform.rotation);
            vfxAttack1.Play();
            enemy.GetComponent<SlimeBoss>().TakeDamage(meleeAttack1);
        }

    }

    public void Attack2()
    {
        FindObjectOfType<AudioManager>().PlaySound("PlayerAttack2");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(meleeAttackPoint.position, meleeAttackRange, attackMask);
        foreach (Collider2D enemy in hitEnemies)
        {
            vfxAttack2.Play();
            enemy.GetComponent<SlimeBoss>().TakeDamage(meleeAttack2);
        }
    }


}
