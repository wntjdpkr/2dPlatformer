using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    public static CollisionManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void OnPlayerCollision(GameObject player, Collider2D other)
    {
        switch (other.tag)
        {
            case "Spike":
                player.GetComponent<Player>()?.TakeDamage(Vector2.up); // 반동 방향 추가
                break;

            case "Coin":
                GameManager.Instance?.AddPoint(100); // 
                SoundManager.Instance?.PlayCoinSound();
                Destroy(other.gameObject);
                break;

            case "Monster":
                HandleMonsterCollision(player, other.gameObject);
                break;
            case "Finish":
                GameManager.Instance?.GameClear();
                break;

            default:
                Debug.Log($"Unhandled collision with tag: {other.tag}");
                break;
        }
    }

    private void HandleMonsterCollision(GameObject playerObj, GameObject monsterObj)
    {
        var rbPlayer = playerObj.GetComponent<Rigidbody2D>();
        var relative = monsterObj.transform.position - playerObj.transform.position;
        var player = playerObj.GetComponent<Player>();

        bool isJumpAttack = rbPlayer.linearVelocity.y < 0 && relative.y < -0.3f;

        if (isJumpAttack)
        {
            monsterObj.GetComponent<Monster>()?.Die();
            
        }
        else
        {
            Vector2 knockbackDir = (playerObj.transform.position - monsterObj.transform.position).normalized;
            player?.TakeDamage(knockbackDir);
        }
    }
}
