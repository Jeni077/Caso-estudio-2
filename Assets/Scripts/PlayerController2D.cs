using System.Collections;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    [SerializeField]
    float walkSpeed;

    [SerializeField]
    float jumpForce;

    [SerializeField]
    float gravityMultiplier = 1.5F;

    [SerializeField]
    Transform groundCheck;

    [SerializeField]
    Vector2 groundCheckSize;

    [SerializeField]
    LayerMask groundMask;

    [SerializeField]
    LayerMask Enemies;

    [SerializeField]
    bool isFacingRight = true;

    [SerializeField]
    Transform attackPoint;

    [SerializeField]
    float damage;


    Rigidbody2D _rigidbody;

    Animator _animator;

    float _inputX;
    float _velocityY;
    float _gravityY;

    bool _isJumping;
    bool _isJumpPressed;
    bool _isGrounded;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        _gravityY = Physics2D.gravity.y;
    }

    private void Start()
    {
        _isGrounded = IsGrounded();
        if (!_isGrounded)
        {
            StartCoroutine(WaitForGroundedCoroutine());
        }
    }

    private void Update()
    {
        HandleGravity();
        HandleMovement();
        if (Input.GetMouseButtonDown(0))
        {
            
            OnMeleeAttack();
        }
    }

    private void FixedUpdate()
    {
        Jump();
        Rotate();
        Move();
    }

    private void OnDrawGizmos()
    {
        if (groundCheck == null)
        {
            return;
        }

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
    }

    private void Rotate()
    {
        if (_inputX == 0.0F)
        {
            return;
        }

        bool facingRight = _inputX > 0.0F;
        if (facingRight != isFacingRight)
        {
            isFacingRight = facingRight;
            transform.Rotate(0.0F, 180.0F, 0.0F);
        }
    }

    private void Move()
    {
        float speed = _inputX != 0.0F ? 1.0F : 0.0F;
        if (speed != _animator.GetFloat("speed"))
        {
            _animator.SetFloat("speed", speed);
        }

        Vector2 velocity = new Vector2(_inputX, 0.0F) * walkSpeed * Time.fixedDeltaTime;
        velocity.y = _velocityY;

        Debug.Log(velocity);

        _rigidbody.velocity = velocity;
    }

    private void Jump()
    {
        if (_isJumpPressed)
        {
            _animator.SetTrigger("jump");

            _velocityY = jumpForce;
            _isJumpPressed = false;
            _isJumping = true;

            _isGrounded = false;
            StartCoroutine(WaitForGroundedCoroutine());
        }
        else if (!_isGrounded)
        {
            _velocityY += gravityMultiplier * _gravityY * Time.fixedDeltaTime;
            if (!_isJumping)
            {
                _animator.SetTrigger("fall");
            }

            _animator.SetFloat("velocityY", _velocityY);
        }
        else if (_isGrounded)
        {
            if (_velocityY > 0.0F)
            {
                _velocityY = -1;
            }
            else
            {
                _isGrounded = IsGrounded();
                if (!_isGrounded)
                { 
                    StartCoroutine(WaitForGroundedCoroutine());
                }
            }

            if (_isGrounded && _animator.GetFloat("velocityY") != 0.0F)
            {
                _isJumping = false;
                _animator.SetFloat("velocityY", 0.0F);
            }
        }

    }

    private void HandleGravity()
    {
        if (_isGrounded)
        {
            if (_velocityY < -1.0F)
            {
                _velocityY = -1.0F;
            }

            HandleJump();
        }
    }

    private void HandleJump()
    {
        _isJumpPressed = Input.GetButton("Jump");
    }

    private void HandleMovement()
    {
        _inputX = Input.GetAxisRaw("Horizontal");
    }

    private IEnumerator WaitForGroundedCoroutine()
    {
        yield return new WaitUntil(() => !IsGrounded());
        yield return new WaitUntil(() => IsGrounded());
        _isGrounded = true;
    }

    private bool IsGrounded()
    {
        Collider2D collider2D =
            Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0.0F, groundMask);
        return collider2D != null;
    }

    public IEnumerator Die()
    {
        gameObject.SetActive(false);
        yield return new WaitForSeconds(1.5F);

        // Reiniciar la escena
    }
    public void OnMeleeAttack()
    {
        if (_animator != null)
        {
            _animator.SetTrigger("attack");
        }

        Collider2D collider = Physics2D.OverlapCircle(attackPoint.position, 0.3F);
        if (collider != null && collider.CompareTag("Enemies"))
        {
            HelthBarEnemies player = collider.GetComponent<HelthBarEnemies>();
            player.TakeDamage(damage);
        }
    }
}
