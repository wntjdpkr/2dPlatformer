using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public interface IHealthObserver
{
    void OnHealthChanged(int currentHealth);
}

public class PlayerHealthUI : MonoBehaviour, IHealthObserver
{
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private Transform heartContainer;

    private List<GameObject> hearts = new List<GameObject>();

    public void Initialize(int maxHealth)
    {
        for (int i = 0; i < maxHealth; i++)
        {
            GameObject heart = Instantiate(heartPrefab, heartContainer);
            hearts.Add(heart);
        }
    }

    public void OnHealthChanged(int currentHealth)
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].SetActive(i < currentHealth);
        }
    }
}

