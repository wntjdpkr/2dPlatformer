using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float invincibilityDuration = 1.0f;
    [SerializeField] private float knockbackForce = 5f;
    private int currentHealth;
    private bool isInvincible = false;
    private Rigidbody2D rb;
    private int money = 0;
    private int point = 0;
    private SpriteRenderer spriteRenderer;
    private Vector2 respawnPosition = new Vector2(-8f, 1f);
    private AnimationHandler animationHandler;
    public int GetMaxHealth() => maxHealth;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // �߰�

        animationHandler = GetComponent<AnimationHandler>();
        ResetState();
    }

    public void ResetState()
    {
        currentHealth = maxHealth;
        money = 0;
        point = 0;
        isInvincible = false;
    }
    private void Update()
    {
        if (transform.position.y < -5f )
        {
            HandleFall();
        }
    }
    public void TakeDamage(Vector2 knockbackDir)
    {
        if (isInvincible) return;

        currentHealth--;
        Debug.Log($"Player took damage. Current HP: {currentHealth}");

        SoundManager.Instance?.PlayDamageSound();
        StartCoroutine(InvincibilityCoroutine());
        
        // Knockback ó��
        rb.linearVelocity = Vector2.zero;

        // ������ �ݵ� �� ���
        Vector2 horizontalDir = new Vector2(knockbackDir.x, 0).normalized;
        Vector2 verticalDir = Vector2.up;

        float horizontalForce = knockbackForce * 0.7f;
        float verticalForce = knockbackForce * 0.8f;

        Vector2 parabolicForce = horizontalDir * horizontalForce + verticalDir * verticalForce;
        rb.AddForce(parabolicForce, ForceMode2D.Impulse);

        GameManager.Instance?.OnPlayerDamaged(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        CollisionManager.Instance?.OnPlayerCollision(this.gameObject, other);
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        animationHandler?.SetTransparency(0.5f);
        animationHandler?.PlayDamagedAnimation();

        yield return new WaitForSeconds(invincibilityDuration);

        animationHandler?.SetTransparency(1.0f);
        isInvincible = false;
    }

    private void HandleFall()
    {
        currentHealth--;
        GameManager.Instance?.OnPlayerDamaged(currentHealth);
        Debug.Log("Player fell! HP decreased.");

        // ��ġ ����
        transform.position = respawnPosition;
        rb.linearVelocity = Vector2.zero;

        // ���� �� �ִϸ��̼� ó��
        StartCoroutine(InvincibilityCoroutine());

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        Debug.Log("Player died.");
        GameManager.Instance?.GameOver(); // ���� ���� ó��
    }

    public void AddMoney(int amount)
    {
        money += amount;
        Debug.Log($"Player earned money: {amount}, total: {money}");

        SoundManager.Instance?.PlayCoinSound();
    }

    public void AddPoint(int amount)
    {
        point += amount;
        Debug.Log($"Player earned point: {amount}, total: {point}");
    }

    public int GetHealth() => currentHealth;
    public int GetMoney() => money;
    public int GetPoint() => point;

    // �߰��� �浹 ���� �޼���
    private void OnCollisionEnter2D(Collision2D collision)
    {
        CollisionManager.Instance?.OnPlayerCollision(this.gameObject, collision.collider);
    }
}
