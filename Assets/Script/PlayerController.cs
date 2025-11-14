using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpForce;
    
    [SerializeField] private bool _isGrounded = true;
    [SerializeField] private bool _isSprinting = false;
    [SerializeField] private float _coyoteTime = 0.2f;
    
    [SerializeField] private float jumpFowardForce = 5f;
    [SerializeField] private float jumpForwardDuration = 0.2f;
    
    
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private BoxCollider2D _collider;
    [SerializeField] private CollisionBox _interactionBox;

    public static List<PlayerController> playerInstances = new List<PlayerController>();
    
    private float _coyoteTimer;
    private float _jumpForwardTimer;
    
    private float _currentJumpForwardForce => Mathf.Clamp((_jumpForwardTimer - Time.time) / jumpForwardDuration, 0, 1) * jumpFowardForce;
    
    private Vector3 _desiredVelocity;
    private Rigidbody2D rb;

    private void Awake()
    {
        if (Time.time > 0.1f)
        {
            playerInstances.Clear();
        }
        playerInstances.Add(this);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
        animator = GetComponentInChildren<Animator>();
        _renderer = GetComponentInChildren<SpriteRenderer>();

        _interactionBox = GetComponentInChildren<CollisionBox>();

        
    }
    
    private void Update()
    {
        // Applying Movement 
        var xVelocity = _desiredVelocity.x * (_isSprinting? 1.5f: 1) + (_desiredVelocity.x * _currentJumpForwardForce);
        
        rb.linearVelocity = new Vector3(xVelocity, Mathf.Clamp(rb.linearVelocity.y, -20, 20), 0);
        _isGrounded = CheckGround();
        
        
        // Animation

        if (_isGrounded && Mathf.Abs(rb.linearVelocity.x) > 0.1f)
        {
            if (_isSprinting)
            {
                animator.Play("Run");
            }
            else
            {
                animator.Play("Walk");
            }
            
        }
        else if (_isGrounded)
        {
            animator.Play("Idle");
        }
        else if (!_isGrounded)
        {
            animator.Play("Jump");
        }
        
        // Flip sprite
        
        switch (rb.linearVelocity.x)
        {
            case < -0.1f:
                _renderer.flipX = true;
                break;
            case > 0.1f:
                _renderer.flipX = false;
                break;
        }
        
    }


    void OnMove(InputValue value)
    {
        var v = value.Get<Vector2>();
        Vector3 movementValue = new Vector3(v.x, 0, 0);
        _desiredVelocity = movementValue * runSpeed;
    }
    
    
    public void OnJump()
    {
        if (_isGrounded)
        {
            Vector2 jumpVelocity = new Vector2(0, jumpForce);
            rb.linearVelocity = rb.linearVelocity + jumpVelocity;
            
            _jumpForwardTimer = Time.time + jumpForwardDuration;
            _isGrounded = false;
        }
    }

    private void OnSprint(InputValue value)
    {
        _isSprinting = value.isPressed;
        
        

    }

    private void OnInteract()
    {
        _interactionBox.TryInteract();
    }

    private bool CheckGround()
    {
        if (Time.time < _coyoteTimer) return true;
        
        Vector2 boxSize = new Vector2(_collider.size.x, 0.1f);
        RaycastHit2D hit = Physics2D.BoxCast(transform.position + new Vector3(0, -0.5f, 0), boxSize, 0f, Vector2.down, 0.1f, LayerMask.GetMask("Terrain"));

        if (hit.collider != null && hit.collider.CompareTag("Ground"))
        {
            _coyoteTimer = Time.time + _coyoteTime;
            return true;
        }
        return false;
    }
}
