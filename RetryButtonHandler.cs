using UnityEngine;


public class RetryButtonHandler : MonoBehaviour
{
    public void OnRetryButtonPressed()
    {
        GameManager.Instance?.ResetGame();
    }
}