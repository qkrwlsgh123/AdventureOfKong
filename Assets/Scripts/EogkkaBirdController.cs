using UnityEngine;

public class EogkkaBirdController : MonoBehaviour
{
    public Transform targetBall;
    public float speed = 5f;
    public float exitDistance = 15f; // 화면 밖으로 나갈 거리

    private bool hasEaten = false;
    private Vector3 moveDir;

    void Start()
    {
        if (targetBall == null)
        {
            Debug.LogWarning("❌ 타겟(Ball)이 설정되지 않았습니다.");
            return;
        }

        // 1. 이동 방향 계산
        moveDir = (targetBall.position - transform.position).normalized;

        // 2. 좌우 반전 처리
        Vector3 scale = transform.localScale;
        scale.x = moveDir.x < 0 ? -1f : 1f;
        transform.localScale = scale;
    }

    void Update()
    {
        if (targetBall == null) return;

        if (!hasEaten)
        {
            transform.position += moveDir * speed * Time.unscaledDeltaTime;

            // ✅ 부리 위치 계산 (현재 위치 기준 약간 앞쪽)
            Vector3 beakOffset = new Vector3(moveDir.x * 0.5f, 0f, 0f);
            Vector3 beakPos = transform.position + beakOffset;

            // ✅ 부리가 Ball에 닿으면
            if (Vector3.Distance(beakPos, targetBall.position) < 0.3f)
            {
                EatBall();
            }
        }
        else
        {
            // 먹은 후 → 같은 방향으로 계속 직진
            transform.position += moveDir * speed * Time.unscaledDeltaTime;

            if (Vector3.Distance(transform.position, Vector3.zero) > exitDistance)
            {
                GameOverUIScript ui = FindObjectOfType<GameOverUIScript>();
                if (ui != null)
                    ui.ShowGameOver();

                Destroy(gameObject);
            }
        }
    }

    void EatBall()
    {
        hasEaten = true;

        // ✅ 아삭 소리 재생
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayEatSound();

        if (targetBall != null)
            targetBall.gameObject.SetActive(false);
    }
}
