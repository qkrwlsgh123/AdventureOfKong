using UnityEngine;

public class EogkkaManager : MonoBehaviour
{
    public GameObject birdPrefab;
    [Range(0f, 1f)] public float spawnChance = 0.001f;
    public bool alwaysTrigger = false;

    void Start()
    {
        if (alwaysTrigger || Random.value < spawnChance)
        {
            Invoke(nameof(SpawnBird), 1f);
        }
    }

    void SpawnBird()
    {
        Time.timeScale = 0f;
        GameObject ball = GameObject.FindWithTag("Player");
        if (ball == null)
        {
            Debug.LogWarning("Ball을 찾지 못했습니다.");
            return;
        }

        BallController bc = ball.GetComponent<BallController>();
        if (bc != null)
        {
            bc.PauseGameExceptBird();
        }

        Vector3 spawnPos = new Vector3(0f, 6f, 0f);  // ✅ 중앙 위
        Instantiate(birdPrefab, spawnPos, Quaternion.identity);
    }
}
