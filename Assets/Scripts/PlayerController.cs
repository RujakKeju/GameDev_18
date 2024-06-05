using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float airWalkSpeed = 3f;
    public float jumpImpluse = 10f;
    Vector2 moveInput;
    TouchingDirections touchingDirections;
    Damageable damageable;

    public float CurrentMoveSpeed { get
        {
            if (CanMove)
            {
                    if(IsMoving && !touchingDirections.IsOnWall)
                    {
                        if (touchingDirections.IsGrounded)
                        {  
                           if(IsRunning) 
                            {
                            return runSpeed;
                            } else 
                            {   
                            return walkSpeed;
                            }
    
                        } else
                        {
                            // air move
                            return airWalkSpeed;
                        }

                    } else
                    {
                        // idle speed is 0
                        return 0;
                    }
            } else
            {
                // movement locked
                return 0;
            }
            
        }
    }


    [SerializeField]
    private bool _isMoving = false;

    public bool IsMoving {  get 
        { 
            return _isMoving; 
        } 
        private set
        { 
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        }
    }

    [SerializeField]
    private bool _isRunning = false;

    private bool IsRunning { get 
        {
            return _isRunning;
        }
        set
        { 
            _isRunning = value;
            animator.SetBool(AnimationStrings.isRunning, value );
        }
    }

    [SerializeField]
    private bool _isJump = false;

    private bool Isjump
    {
        get
        {
            return _isJump;
        }
        set
        {
            _isJump = value;
            animator.SetBool(AnimationStrings.jumpTrigger, value);
        }
    }
    public bool _isFacingRight = true;

    public bool IsFacingRight { get { return _isFacingRight; } private set { 
        // Flip only if value is new
        if (_isFacingRight != value)
        {
            // FLip the local scale to make the player face the oppostie driection
            transform.localScale *= new Vector2(-1, 1);
        }

        _isFacingRight = value;

        } }

    public bool CanMove { get
        { 
                return animator.GetBool(AnimationStrings.canMove);
        } 
    }

    public bool IsAlive
    {
        get
        {
            return animator.GetBool(AnimationStrings.isAlive);
        }
    }

    Rigidbody2D rb;
    Animator animator;
   

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();
    }

    private void FixedUpdate()
    {
        if(!damageable.LockVelocity)
        {
        rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);

        }


        // scripts for falling and rising animation
        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();


        if(IsAlive)
        {
            IsMoving = moveInput != Vector2.zero;

            SetFacingDirection(moveInput);
        }
        else
        {
            IsMoving = false;
        }
        
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if(moveInput.x > 0 && !IsFacingRight)
        {
            //face right
            IsFacingRight = true;
        } else if (moveInput.x < 0 && IsFacingRight)
        {
            //face left
            IsFacingRight= false;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsRunning = true;
        } else if(context.canceled) 
        {
            IsRunning = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context) 
    {
        // TODO CHeck if alive as well
        if(context.started && touchingDirections.IsGrounded && CanMove)
        {
            animator.SetTrigger(AnimationStrings.jumpTrigger);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpluse);
        }
        else if (context.canceled && CanMove)
        {
            Isjump = false;
        }
    }

    public void OnAttack(InputAction.CallbackContext context) 
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.attackTrigger);
        }
    }
    public void OnRangedAttack(InputAction.CallbackContext context) 
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.rangedAttackTrigger);
        }
    }

    public void OnHit(int damage, Vector2 knockBack)
    {
        rb.velocity = new Vector2(knockBack.x, rb.velocity.y + knockBack.y);
    }
}
