using UnityEngine;

public class EogkkaManager : MonoBehaviour
{
    [Header("새 프리팹")]
    public GameObject birdPrefab;

    [Header("억까 확률 (0~1 사이)")]
    [Range(0f, 1f)]
    public float spawnChance = 0.001f;

    [Header("테스트용 강제 발동")]
    public bool alwaysTrigger = false;

    void Start()
    {
        Debug.Log("🟡 EogkkaManager 시작됨");

        if (alwaysTrigger || Random.value < spawnChance)
        {
            Debug.Log("🕊️ 새 소환 조건 만족! (delay 호출 예정)");
            Invoke(nameof(SpawnBird), 1f);
        }
    }

    void SpawnBird()
    {
        Debug.Log("✅ SpawnBird() 실행됨 - 새가 소환됩니다!");

        if (birdPrefab == null)
        {
            Debug.LogWarning("❌ 새 프리팹이 연결되지 않았습니다.");
            return;
        }

        Vector3 spawnPos = new Vector3(0f, 6f, 0f); // 화면 위 중앙
        GameObject bird = Instantiate(birdPrefab, spawnPos, Quaternion.identity);

        Debug.Log("🟦 새 프리팹 인스턴스화 완료");

        // 더 이상 Init() 호출하지 않음
        // EogkkaBirdController는 내부에서 Player를 자동으로 찾음
    }
}
