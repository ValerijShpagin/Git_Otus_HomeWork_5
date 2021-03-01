using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Collider2D _collider2D;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private UiRestartButton _uiRestartButton;

    public enum State
    {
        None,
        Dead,
    }

    public State state;

    public LayerMask ground;
   // public LayerMask wall;

    public GameObject bloodStream;

    public bool usePhysicsForMovement;
    [SerializeField] private bool canWallJump;
    public float moveForce;
    public float jumpForce;
    private float hDirection;

    SliderPlatform stayingOnPlatform;
    

    
    void Start()
    {
        state = State.None;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        canWallJump = false;
        usePhysicsForMovement = true;
    }

    
    void Update()
    {
        hDirection = Input.GetAxis("Horizontal");

        float platformVelocity = 0.0f;
        if (stayingOnPlatform != null)
            platformVelocity = stayingOnPlatform.myrigidbody2d.velocity.x;

        if (hDirection < 0.0f)
        {
            if (usePhysicsForMovement)
                _rigidbody2D.AddForce(new Vector2(-moveForce, 0.0f), ForceMode2D.Impulse);
            else
                _rigidbody2D.velocity = new Vector2(-5 + platformVelocity, _rigidbody2D.velocity.y);
            _spriteRenderer.flipX = true;
            _animator.SetBool("running", true);
        } 
        else if (hDirection > 0.0f)
        {
            if (usePhysicsForMovement)
                _rigidbody2D.AddForce(new Vector2(moveForce, 0.0f), ForceMode2D.Impulse);
            else
                _rigidbody2D.velocity = new Vector2(5 + platformVelocity, _rigidbody2D.velocity.y);
            _spriteRenderer.flipX = false;
            _animator.SetBool("running", true);
        }
        else
        {
            if (!usePhysicsForMovement)
                _rigidbody2D.velocity = new Vector2(platformVelocity, _rigidbody2D.velocity.y);
            _animator.SetBool("running", false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && _collider2D.IsTouchingLayers(ground))
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 7);
        }

        if (_collider2D.IsTouchingLayers(ground))
        {
            canWallJump = false;
        }

        if (canWallJump)
        {
            ExtraJump();
        }
        
        

    }

    


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bonus"))
        {
            // LayerMask.NameToLayer("Default") = 0
            // LayerMask.GetMask("Default") = 1             // 000000001   // (1 << index)

            // LayerMask.NameToLayer("TransparentFX") = 1
            // LayerMask.GetMask("TransparentFX") = 2       // 000000010   // (1 << index)

            int layer = LayerMask.NameToLayer("Player");
            int mask = Physics2D.GetLayerCollisionMask(layer);
            mask |= LayerMask.GetMask("Crate");
            // mask &= ~LayerMask.GetMask("Crate");   // remove bit
            Physics2D.SetLayerCollisionMask(layer, mask);

            Destroy(other.gameObject);
        }

        if (other.CompareTag("PointDead"))
        {
            state = State.Dead;
            Destroy(this.gameObject);
            print("Character is dead");
        }
    }

   

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.TryGetComponent<SliderPlatform>(out var platform))
            stayingOnPlatform = platform;

        if (collision.gameObject.tag.Equals("Wall"))
        {
            if (!_collider2D.IsTouchingLayers(ground))  
            {
                canWallJump = true;
            }

            else if (_collider2D.IsTouchingLayers(ground))
            {
                canWallJump = false;
            }
        }
            
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Wall"))
        {
            if (!_collider2D.IsTouchingLayers(ground)) 
            {
                canWallJump = true;
            }

            else if (_collider2D.IsTouchingLayers(ground))
            {
                canWallJump = false;
            }
        }
    }

    private void ExtraJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _rigidbody2D.AddForce(new Vector2(transform.right.x * jumpForce, transform.up.y * jumpForce), ForceMode2D.Impulse);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.TryGetComponent<SliderPlatform>(out var platform))
            stayingOnPlatform = null;
    }
}
