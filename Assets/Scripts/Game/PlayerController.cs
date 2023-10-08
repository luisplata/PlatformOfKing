using UnityEngine;

public class PlayerController : PlayerFather
{
    public float movementSpeed = 5f;
    public float jumpForce = 8f;
    public float jumpTimeThreshold = 0.25f;
    public float airBrakeFactor = 0.9f; 
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Collider2D playerCollider;
    private Transform feet;
    private float jumpTime;
    private bool inAir;
    
    private bool isJumping = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        feet = transform.Find("Feet"); // Ajusta el nombre según la jerarquía de tu personaje
    }

    void Update()
    {
        CheckGrounded();

        if (onGround)
        {
            jumpTime = 0f;
            inAir = false;
        }

        if (inputFacade.JumpButton && onGround)
        {
            Jump();
        }

        if (Input.GetButton("Jump"))
        {
            isJumping = true;
            jumpTime += Time.deltaTime;
        }
        else
        {
            isJumping = false;
        }

        if (inputFacade.JumpButton)
        {
            jumpTime = 0f;
        }

        if (!onGround)
        {
            inAir = true;
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    bool onGround;

    void CheckGrounded()
    {
        onGround = Physics2D.Raycast(feet.position, Vector2.down, 0.1f, groundLayer);
    }

    void Move()
    {
        float horizontalMovement = inputFacade.HorizontalAxis;

        if (onGround || (jumpTime <= jumpTimeThreshold) || isJumping)
        {
            Vector2 movement = new Vector2(horizontalMovement, 0);

            if (movement.x != 0f)
            {
                rb.velocity = new Vector2(movement.x * movementSpeed, rb.velocity.y);
            }
            else if (onGround)
            {
                rb.velocity = new Vector2(rb.velocity.x * airBrakeFactor, rb.velocity.y);
            }
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x * airBrakeFactor, rb.velocity.y);
        }

        if (rb.velocity.y < 0)
        {
            rb.gravityScale = 2.5f;  
        }
        else
        {
            rb.gravityScale = 1f;
        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
}