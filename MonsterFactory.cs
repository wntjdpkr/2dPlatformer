using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ���� ������ ���� ���� enum (Ȯ�� ����)
/// </summary>
public enum MonsterType
{
    Default,
    Ice,
    Fire,
    // �ʿ� �� �߰� ����
}

/// <summary>
/// ���͸� �����ϴ� �̱��� ���丮 Ŭ����
/// </summary>
public class MonsterFactory : MonoBehaviour
{
    public static MonsterFactory Instance { get; private set; }

    [Header("���� ������ ���")]
    public GameObject defaultMonsterPrefab;
    public GameObject iceMonsterPrefab;
    public GameObject fireMonsterPrefab;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("[MonsterFactory] �ߺ� �ν��Ͻ��� �߰ߵǾ� �ı��˴ϴ�.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        // DontDestroyOnLoad(gameObject); // �� ��ȯ �Ŀ��� �����ϰ� ���� ��� Ȱ��ȭ
    }

    /// <summary>
    /// �������� ���� Ÿ�Կ� ���� ��ȯ
    /// </summary>
    private GameObject GetPrefabByType(MonsterType type)
    {
        return type switch
        {
            MonsterType.Default => defaultMonsterPrefab,
            MonsterType.Ice => iceMonsterPrefab,
            MonsterType.Fire => fireMonsterPrefab,
            _ => null
        };
    }

    /// <summary>
    /// ���� ���� ���� (���� ����)
    /// </summary>
    private GameObject CreateMonsterInternal(MonsterType type, Vector3 spawnPosition, bool showEffect)
    {
        GameObject prefabToSpawn = GetPrefabByType(type);

        if (prefabToSpawn == null)
        {
            Debug.LogError($"[MonsterFactory] �������� ��ϵ��� �ʾҽ��ϴ�: {type}");
            return null;
        }

        GameObject monster = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

        if (showEffect)
        {
            MonsterSpawnEffect.ShowEffectAt(spawnPosition);
        }

        return monster;
    }

    /// <summary>
    /// ���� ���� ���� (public)
    /// </summary>
    public GameObject CreateMonster(MonsterType type, Vector3 spawnPosition, bool showEffect = true)
    {
        return CreateMonsterInternal(type, spawnPosition, showEffect);
    }

    /// <summary>
    /// �ټ� ���� ���� (��ġ ����Ʈ ���)
    /// </summary>
    public void CreateMonsters(MonsterType type, List<Vector2> positions, bool showEffect = true)
    {
        if (positions == null || positions.Count == 0)
        {
            Debug.LogWarning("[MonsterFactory] �� ��ġ ����Ʈ�Դϴ�.");
            return;
        }

        foreach (var pos in positions)
        {
            Vector3 spawnPos = new Vector3(pos.x, pos.y, 0f); // Z ����
            CreateMonsterInternal(type, spawnPos, showEffect);
        }
    }
}
