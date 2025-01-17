using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]

public class PlayerController : MonoBehaviour
{
    public float runSpeed = 8f;
    public float jumpImpulse = 10f;
    Vector2 moveInput;
    TouchingDirections touchingDirections;
    Damageable damageable;
    [SerializeField]

private bool _isRunning = false;

public bool IsRunning
{
    get
    {
        return _isRunning;
    }
    
    set
    {
        _isRunning = value;
        animator.SetBool(AnimationStrings.isRunning, value);
    }
}


// Crouch Concept 

// private bool _isCrouched = false;

// public bool IsCrouched
// {
//     get
//     {
//         return _isCrouched;
//     }
//     set
//     {
//         _isCrouched = value;
//         animator.SetBool("isCrouched", value);
//     }
// }


private bool _isFacingRight = true;

public bool IsFacingRight
{
    get
    {
        return _isFacingRight;
    }

    private set
    {
        if (_isFacingRight != value)
        {
            // Flip the sprite using flipX instead of scaling
            transform.localScale *= new Vector2(-1, 1);
        }

        _isFacingRight = value;
    }
}

public bool CanMove 
{
    get
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(!damageable.LockVelocity)
        {
            rb.velocity = new Vector2(moveInput.x * runSpeed, rb.velocity.y);
        }

        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
    }

    public void OnMove(InputAction.CallbackContext context)
{
    if(IsAlive)
    {
        moveInput = context.ReadValue<Vector2>();
        IsRunning = moveInput != Vector2.zero;
        SetFacingDirection(moveInput);
    }
    else
    {
        moveInput = Vector2.zero;
        IsRunning = false;
    }
}

    private void SetFacingDirection(Vector2 moveInput)
    {
        if(moveInput.x > 0 && !IsFacingRight)
        {
            // Face the right
            IsFacingRight = true;
        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            // Face the left
            IsFacingRight = false;
        }
    }

    public void OnRun(InputAction.CallbackContext context){
            if (context.started){
            IsRunning = true;
        }
        else if(context.canceled){
            IsRunning = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context){
        // TODO Check if alive as well
        if(context.started && touchingDirections.IsGrounded && CanMove)
        {
            animator.SetTrigger(AnimationStrings.jumpTrigger);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            animator.SetTrigger(AnimationStrings.attackTrigger);
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }
}
