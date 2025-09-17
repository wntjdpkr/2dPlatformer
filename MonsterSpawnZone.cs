using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �÷��̾ Ư�� X��ǥ�� �����ϸ� ���͸� �����ϴ� ����
/// </summary>
[System.Serializable]
public class SpawnEntry
{
    public float triggerX;                      // �÷��̾� X ��ġ ���� Ʈ���� ����
    public MonsterType monsterType;             // ������ ���� ����
    public List<Vector2> spawnPositions;        // ���� ���� ��ġ��
    [HideInInspector] public bool triggered;    // ���� ����
}

public class MonsterSpawnZone : MonoBehaviour
{
    [Header("�÷��̾� Transform")]
    public Transform player;

    [Header("���� ���� ���� ����Ʈ")]
    public List<SpawnEntry> spawnEntries;

    void Update()
    {
        if (player == null || MonsterFactory.Instance == null) return;

        foreach (var entry in spawnEntries)
        {
            if (!entry.triggered && player.position.x >= entry.triggerX)
            {
                SpawnMonsters(entry);
                entry.triggered = true;
            }
        }
    }

    void SpawnMonsters(SpawnEntry entry)
    {
        MonsterFactory.Instance.CreateMonsters(entry.monsterType, entry.spawnPositions);
    }
}
