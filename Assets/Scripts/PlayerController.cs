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

    public LayerMask ground;
    public GameObject bloodStream;

    public bool usePhysicsForMovement;
    public float moveForce;
    public float jumpForce;

    SliderPlatform stayingOnPlatform;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float hDirection = Input.GetAxis("Horizontal");

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
            if (usePhysicsForMovement)
                _rigidbody2D.AddForce(new Vector2(0.0f, jumpForce), ForceMode2D.Impulse);
            else
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 7);
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
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.TryGetComponent<SliderPlatform>(out var platform))
            stayingOnPlatform = platform;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.TryGetComponent<SliderPlatform>(out var platform))
            stayingOnPlatform = null;
    }
}
