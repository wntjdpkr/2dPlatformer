using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 생성 가능한 몬스터 종류 enum (확장 가능)
/// </summary>
public enum MonsterType
{
    Default,
    Ice,
    Fire,
    // 필요 시 추가 가능
}

/// <summary>
/// 몬스터를 생성하는 싱글톤 팩토리 클래스
/// </summary>
public class MonsterFactory : MonoBehaviour
{
    public static MonsterFactory Instance { get; private set; }

    [Header("몬스터 프리팹 등록")]
    public GameObject defaultMonsterPrefab;
    public GameObject iceMonsterPrefab;
    public GameObject fireMonsterPrefab;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("[MonsterFactory] 중복 인스턴스가 발견되어 파괴됩니다.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        // DontDestroyOnLoad(gameObject); // 씬 전환 후에도 유지하고 싶을 경우 활성화
    }

    /// <summary>
    /// 프리팹을 몬스터 타입에 따라 반환
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
    /// 내부 생성 로직 (단일 몬스터)
    /// </summary>
    private GameObject CreateMonsterInternal(MonsterType type, Vector3 spawnPosition, bool showEffect)
    {
        GameObject prefabToSpawn = GetPrefabByType(type);

        if (prefabToSpawn == null)
        {
            Debug.LogError($"[MonsterFactory] 프리팹이 등록되지 않았습니다: {type}");
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
    /// 단일 몬스터 생성 (public)
    /// </summary>
    public GameObject CreateMonster(MonsterType type, Vector3 spawnPosition, bool showEffect = true)
    {
        return CreateMonsterInternal(type, spawnPosition, showEffect);
    }

    /// <summary>
    /// 다수 몬스터 생성 (위치 리스트 기반)
    /// </summary>
    public void CreateMonsters(MonsterType type, List<Vector2> positions, bool showEffect = true)
    {
        if (positions == null || positions.Count == 0)
        {
            Debug.LogWarning("[MonsterFactory] 빈 위치 리스트입니다.");
            return;
        }

        foreach (var pos in positions)
        {
            Vector3 spawnPos = new Vector3(pos.x, pos.y, 0f); // Z 고정
            CreateMonsterInternal(type, spawnPos, showEffect);
        }
    }
}
