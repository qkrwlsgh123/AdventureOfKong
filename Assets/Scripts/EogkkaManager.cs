using UnityEngine;

public class EogkkaManager : MonoBehaviour
{
    [Header("새 프리팹")]
    public GameObject birdPrefab;

    [Header("타겟 (Ball 오브젝트)")]
    public Transform targetBall;

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

        if (birdPrefab == null || targetBall == null)
        {
            Debug.LogWarning("❌ 프리팹 또는 Ball이 연결되지 않았습니다.");
            return;
        }

        Vector3 spawnPos = new Vector3(0f, 6f, 0f); // 화면 위 중앙
        GameObject bird = Instantiate(birdPrefab, spawnPos, Quaternion.identity);

        Debug.Log("🟦 새 프리팹 인스턴스화 완료");

        EogkkaBirdController ctrl = bird.GetComponent<EogkkaBirdController>();
        if (ctrl != null)
        {
            ctrl.targetBall = targetBall;
            Debug.Log("🟢 새 컨트롤러에 Ball 타겟 연결 완료");
        }
    }
}
