using UnityEngine;

public class MonsterStates : MonoBehaviour
{

}

public class MonsterWanderState : IMonsterState
{
    private float wanderTimer;
    private Vector2 direction;

    public void Enter(Monster monster)
    {
        wanderTimer = Random.Range(1f, 2f);
        direction = new Vector2(Random.Range(-1f, 1f), 0).normalized;
        monster.SetWalkingAnimation(true);
    }

    public void Update(Monster monster)
    {
        wanderTimer -= Time.deltaTime;

        //  Raycast 기반 경계 검사
        if (!monster.IsGroundAhead(direction.x))
        {
            direction.x *= -1f; // 방향 반전
        }

        monster.Move(direction);
        monster.FlipSprite(direction.x > 0);

        if (Vector2.Distance(monster.transform.position, monster.GetPlayer().position) < 5f)
        {
            monster.ChangeState(new MonsterChaseState());
        }

        if (wanderTimer <= 0f)
        {
            Enter(monster); // 방향 재랜덤
        }
    }

    public void Exit(Monster monster)
    {
        monster.SetWalkingAnimation(false);
    }
}

public class MonsterChaseState : IMonsterState
{
    public void Enter(Monster monster)
    {
        monster.SetWalkingAnimation(true);
    }

    public void Update(Monster monster)
    {
        Vector2 direction = new Vector2(monster.GetPlayer().position.x - monster.transform.position.x, 0f).normalized;

        // 경계가 없으면 멈추기만 하고 상태는 유지
        if (!monster.IsGroundAhead(direction.x))
        {
            monster.Move(Vector2.zero);  // chase 중 멈춤 (velocity를 0으로 설정)
            return;
        }

        // 플레이어를 향해 추격
        monster.Chase(direction);
        monster.FlipSprite(direction.x > 0);

        // 일정 거리 이상 멀어지면 wander로 복귀
        if (Vector2.Distance(monster.transform.position, monster.GetPlayer().position) > 6f)
        {
            monster.ChangeState(new MonsterWanderState());
        }
    }

    public void Exit(Monster monster)
    {
        monster.SetWalkingAnimation(false);
    }
}

public class MonsterDeadState : IMonsterState
{
    private bool isDeadHandled = false;

    public void Enter(Monster monster)
    {
        if (isDeadHandled) return;
        isDeadHandled = true;

        GameManager.Instance?.AddPoint(monster.pointValue);

        Rigidbody2D rb = monster.GetComponent<Rigidbody2D>();
        Collider2D col = monster.GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 1f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.AddForce(new Vector2(0, 5f), ForceMode2D.Impulse);
        }

        SpriteRenderer sr = monster.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color color = sr.color;
            color.a = 0.7f;
            sr.color = color;

            sr.flipY = true;
        }

        // 코루틴 없이 0.5초 뒤 제거
        Object.Destroy(monster.gameObject, 1f);
    }

    public void Update(Monster monster) { }
    public void Exit(Monster monster) { }
}
