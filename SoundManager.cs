using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        // ΩÃ±€≈Ê º≥¡§
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void PlayJumpSound()
    {
        Debug.Log("PlayJumpSound() called.");
    }

    public void PlayCoinSound()
    {
        Debug.Log("PlayCoinSound() called.");
    }

    public void PlayDamageSound()
    {
        Debug.Log("PlayDamageSound() called.");
    }

    public void PlayClearSound()
    {
        Debug.Log("PlayClearSound() called.");
    }
}
