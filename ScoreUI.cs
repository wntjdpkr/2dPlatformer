using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Start()
    {
        UpdateScore(0); // �ʱⰪ ǥ��
    }

    public void UpdateScore(int score)
    {
        scoreText.text = $"Score: {score}";
    }
}
