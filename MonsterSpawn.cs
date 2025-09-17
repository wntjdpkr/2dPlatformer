using UnityEngine;

public static class MonsterSpawnEffect
{
    public static GameObject spawnEffectPrefab;

    public static void ShowEffectAt(Vector3 position)
    {
        if (spawnEffectPrefab != null)
        {
            GameObject.Instantiate(spawnEffectPrefab, position, Quaternion.identity);
        }
    }
}
