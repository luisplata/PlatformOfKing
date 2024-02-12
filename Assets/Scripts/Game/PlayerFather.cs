using System;
using System.Collections;
using UnityEngine;

public abstract class PlayerFather : MonoBehaviour, IFloorController
{
    public InputFacade InputFacade
    {
        set => inputFacade = value;
    }
    public PlayerId PlayerId => playerId;

    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] private Collider2D playerCollider;
    [SerializeField] private InputFacade inputFacade;
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float jumpTimeThreshold = 0.25f;
    [SerializeField] private float airBrakeFactor = 0.9f; 
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private PlayerId playerId;
    [SerializeField] private JumpSystem jumpSystem;
    [SerializeField] private Animator animator;

    private Transform feet;
    private float jumpTime;
    private bool inAir;
    private bool _isSecretZone;
    private SecretZone _secretZone;
    
    private bool isJumping = false;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("DeadZone"))
        {
            //Game Over
            RestartGame();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Goal"))
        {
            StartCoroutine(CorroutineToRestartGame());
        }
        if(other.gameObject.layer == LayerMask.NameToLayer("Secrets"))
        {
            _isSecretZone = true;
            _secretZone = other.gameObject.GetComponent<SecretZone>();
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Secrets"))
        {
            _isSecretZone = false;
            _secretZone = null;
        }
    }

    private IEnumerator CorroutineToRestartGame()
    {
        yield return new WaitForSeconds(1f);
        RestartGame();
    }

    private static void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    private void Reset()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
    }

    void Start()
    {
        feet = transform.Find("Feet");
        jumpSystem.Configure(rb, this);
        
        jumpSystem.OnAttack += () =>
        {
            animator.SetTrigger("j");
        };
        jumpSystem.OnMidAir += () =>
        {
            animator.SetTrigger("j_middle");
        };
        jumpSystem.OnRelease += () =>
        {
            animator.SetTrigger("j_fall");
        };
        jumpSystem.OnEndJump += () =>
        {
            animator.SetTrigger("j_recovery");
        };
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

        if (!onGround)
        {
            if (!inAir)
            {
                inAir = true;
                jumpSystem.Fall();
            }
        }
        
        if(_isSecretZone && inputFacade.ActionButton)
        {
            ActionEventFromInput();
        }

        CustomUpdate();
    }

    private void ActionEventFromInput()
    {
        _secretZone.ShowSecretZone(this);
    }

    protected virtual void CustomUpdate(){}

    void FixedUpdate()
    {
        Move();
    }

    bool onGround;

    void CheckGrounded()
    {
        onGround = Physics2D.Raycast(feet.position, Vector2.down, 0.1f, groundLayer);
    }

    protected virtual void Move()
    {
        float horizontalMovement = inputFacade.HorizontalAxis;
        Vector2 movement = new Vector2(horizontalMovement, 0);
        var velocity = rb.velocity;
        if (movement.x != 0f)
        {
            velocity = new Vector2(movement.x * movementSpeed, rb.velocity.y);
        }
        else if (onGround)
        {
            velocity = new Vector2(velocity.x * airBrakeFactor, velocity.y);
        }
        rb.velocity = velocity;
        animator.SetFloat("x", Mathf.Abs(velocity.x));
        /*
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
        */
    }

    void Jump()
    {
        jumpSystem.Jump();
        //rb.velocity = new Vector2(rb.velocity.x, 0);
        //rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    public bool IsTouchingFloor()
    {
        return onGround;
    }
}