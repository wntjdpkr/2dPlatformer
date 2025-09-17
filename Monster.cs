using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Rigidbody2D))]
public class Monster : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float chaseSpeed = 4f;
    public int pointValue = 100;
    public IMonsterState currentState;

    private Transform player;
    private AnimationHandler animationHandler;
    private SpriteRenderer sb;
    private Rigidbody2D rb;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        ChangeState(new MonsterWanderState());
        animationHandler = GetComponent<AnimationHandler>();
        sb = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        rb.freezeRotation = true; // Z축 회전 고정
    }

    void Update()
    {
        currentState.Update(this);
    }

    public void SetWalkingAnimation(bool walking)
    {
        animationHandler?.SetWalking(walking);
    }

    public void ChangeState(IMonsterState newState)
    {
        currentState?.Exit(this);
        currentState = newState;
        currentState.Enter(this);
    }

    // 레이 길이 증가
    public bool IsGroundAhead(float directionX)
    {
        Vector2 origin = new Vector2(transform.position.x + directionX * 0.5f, transform.position.y);
        Vector2 direction = Vector2.down;
        float distance = 1.6f; // ← 여기서 레이 길이 증가

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, LayerMask.GetMask("Ground"));
        Debug.DrawRay(origin, direction * distance, Color.red);

        return hit.collider != null;
    }

    public void FlipSprite(bool faceLeft)
    {
        sb.flipX = faceLeft;
    }

    //  물리 기반 이동
    public void Move(Vector2 direction)
    {
        rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);
    }

    public void Chase(Vector2 direction)
    {
        rb.linearVelocity = new Vector2(direction.x * chaseSpeed, rb.linearVelocity.y);
    }

    public void Die()
    {
        ChangeState(new MonsterDeadState());
    }

    public Transform GetPlayer() => player;
}
