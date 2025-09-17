using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    private Rigidbody2D rb;
    private SpriteRenderer sb;
    private AnimationHandler animationHandler;

    private bool isGrounded = false;
    private float lastGroundCheckTime = 0f;
    private float groundCheckCooldown = 0.05f;

    [SerializeField] private LayerMask groundMask;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sb = GetComponent<SpriteRenderer>();
        animationHandler = GetComponent<AnimationHandler>();
    }

    public void MoveLeft()
    {
        rb.linearVelocity = new Vector2(-moveSpeed, rb.linearVelocity.y);
        FlipSprite(true);
    }

    public void MoveRight()
    {
        rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
        FlipSprite(false);
    }

    public void Jump()
    {
        if (IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    private void FlipSprite(bool faceLeft)
    {
        sb.flipX = faceLeft;
    }

    private bool IsGrounded()
    {
        if (isGrounded)
            return true;

        if (Time.time - lastGroundCheckTime > groundCheckCooldown)
        {
            return CheckGroundRaycast();
        }

        return false;
    }

    private bool CheckGroundRaycast()
    {
        float rayLength = 0.2f;
        Vector2 origin = transform.position;
        Vector2 direction = Vector2.down;

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, rayLength, groundMask);
        if (hit.collider != null)
        {
            float dot = Vector2.Dot(hit.normal, Vector2.up);
            return dot >= 0.7f;
        }

        return false;
    }

    private void Update()
    {
        bool isMoving = Mathf.Abs(rb.linearVelocity.x) > 0.1f;
        

        bool isJumping = !IsGrounded();
        animationHandler.SetJumping(isJumping);

        animationHandler.SetWalking(isMoving && !isJumping);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        isGrounded = false;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            float dot = Vector2.Dot(contact.normal, Vector2.up);
            if (dot >= 0.7f)
            {
                isGrounded = true;
                break;
            }
        }

        lastGroundCheckTime = Time.time;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        CollisionManager.Instance?.OnPlayerCollision(this.gameObject, other);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}
