using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어가 특정 X좌표에 도달하면 몬스터를 생성하는 구조
/// </summary>
[System.Serializable]
public class SpawnEntry
{
    public float triggerX;                      // 플레이어 X 위치 기준 트리거 지점
    public MonsterType monsterType;             // 생성할 몬스터 종류
    public List<Vector2> spawnPositions;        // 몬스터 생성 위치들
    [HideInInspector] public bool triggered;    // 생성 여부
}

public class MonsterSpawnZone : MonoBehaviour
{
    [Header("플레이어 Transform")]
    public Transform player;

    [Header("몬스터 스폰 조건 리스트")]
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
