using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private Player player;
    private List<IHealthObserver> healthObservers = new List<IHealthObserver>();

    [SerializeField] private PlayerHealthUI healthUI;
    [SerializeField] private ScoreUI scoreUI;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject gameClearUI;
    private void Awake()
    {
        // 싱글톤 설정
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        player = FindFirstObjectByType<Player>(); // 가장 먼저 등장하는 플레이어 할당
        RegisterHealthObserver(healthUI);
        scoreUI.UpdateScore(player.GetPoint());
        healthUI.Initialize(player.GetHealth());
    }
    public void RegisterHealthObserver(IHealthObserver observer)
    {
        if (!healthObservers.Contains(observer))
            healthObservers.Add(observer);
    }
    public void NotifyHealthChanged(int currentHealth)
    {
        foreach (var observer in healthObservers)
            observer.OnHealthChanged(currentHealth);
    }
    public void ResetGame()
    {
        Debug.Log("Game is being reset.");
        Time.timeScale = 1f;
        player?.ResetState();
        scoreUI?.UpdateScore(0);
        gameOverUI?.SetActive(false);
        gameClearUI?.SetActive(false);
        player.transform.position = new Vector2(-8f, 1f);
        healthUI.Initialize(player.GetMaxHealth());

    }
    public void GameClear()
    {
        Time.timeScale = 0f;
        SoundManager.Instance?.PlayClearSound();
        gameClearUI?.SetActive(true);
    }
    public void AddMoney(int amount)
    {
        player?.AddMoney(amount);
    }

    public void AddPoint(int amount)
    {
        player?.AddPoint(amount);
        scoreUI?.UpdateScore(player.GetPoint());
    }

    public void TakeDamage()
    {
        player?.TakeDamage(Vector2.up);
    }
    public void OnPlayerDamaged(int currentHealth)
    {
        NotifyHealthChanged(currentHealth);
    }
    public void GameOver()
    {
        Time.timeScale = 0f; // 게임 정지
        gameOverUI?.SetActive(true);
    }
    public void ExitGame()
    {
        Debug.Log("Exit pressed.");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}
