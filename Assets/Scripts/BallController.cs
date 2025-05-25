using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class BallController : MonoBehaviour
{
    [Header("이동/점프 설정")]
    public float speed = 3f;
    public float jumpPower = 6f;

    [Header("성장 설정")]
    public float growthInterval = 2f;      // 물속 자동 성장 주기
    public Sprite[] growthSprites;         // 1~7단계 스프라이트
    public SpriteRenderer growthRenderer;  // GrowthVisual용 SpriteRenderer
    [Range(0f, 1f)] public float minSpeedFactor = 0.667f;
    [Range(0f, 1f)] public float minJumpFactor = 0.667f;

    [Header("Game Over UI")]
    public GameOverUIScript gameOverUI;

    Rigidbody2D rb;
    bool isGrounded = false;
    bool isOnWaterBottom = false;

    // 물 관련 상태
    int waterContactCount = 0;     // 현재 물 트리거 위에 있는 개수
    bool isWet = false; // 물 블록에 한 번이라도 들어갔으면 true
    bool isInWater { get { return waterContactCount > 0; } }

    float waterTimer = 0f;        // 물속 자동 성장 타이머
    int growthStage = 0;         // 0:씨앗, 1~7단계
    float initialSpeed, initialJump;
    bool isDead = false;     // 완전 성장 시 true

    // 태양블록 중복 처리 방지 플래그
    bool sunHitProcessed = false;

    void Awake()
    {
        // 씬 로드 시 초기화
        isWet = false;

        initialSpeed = speed;
        initialJump = jumpPower;

        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        UpdateGrowthVisual();
    }

    void Update()
    {
        if (isDead) return;

        // 좌우 이동
        float h = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(h * speed, rb.linearVelocity.y);

        // 물속 자동 성장
        if (isInWater && growthStage < 7)
        {
            waterTimer += Time.deltaTime;
            if (waterTimer >= growthInterval)
            {
                waterTimer -= growthInterval;
                GrowOneStage();
            }
        }
    }

    void FixedUpdate()
    {
        if (isDead) return;

        if (isInWater)
        {
            if (isOnWaterBottom)
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            else
                rb.gravityScale = 1;
        }
        else
        {
            rb.gravityScale = 1;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            if (isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
                isGrounded = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("물블록"))
        {
            waterContactCount++;
            if (waterContactCount == 1)
                waterTimer = 0f;
            // 물에 들어가기만 해도 젖은 상태로
            isWet = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("물블록"))
        {
            waterContactCount--;
            if (waterContactCount <= 0)
            {
                waterContactCount = 0;
                isOnWaterBottom = false;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                rb.gravityScale = 1;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (isDead) return;

        // —— 태양블록 접촉 분기 (BounceBlock + 이름 "태양블록") —— 
        if (!sunHitProcessed &&
            col.gameObject.CompareTag("BounceBlock") &&
            col.gameObject.name == "태양블록" &&
            growthStage < 7)
        {
            // 이 접촉에 대해서만 한 번만 처리
            sunHitProcessed = true;

            int steps = isWet ? 2 : 1;
            for (int i = 0; i < steps && growthStage < 7; i++)
                GrowOneStage();

            return;
        }

        // —— 물속 바닥 고정 —— 
        if (isInWater && col.gameObject.CompareTag("WaterBottom"))
        {
            isOnWaterBottom = true;
            rb.constraints = RigidbodyConstraints2D.FreezePositionY |
                              RigidbodyConstraints2D.FreezeRotation;
            rb.gravityScale = 0;
            return;
        }

        // —— 물속 슬로프 미끄러짐 —— 
        if (isInWater && col.gameObject.CompareTag("TriangleBlock"))
        {
            isOnWaterBottom = false;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.gravityScale = 1;
            return;
        }

        // —— 물밖 슬로프 점프 —— 
        if (!isInWater && col.gameObject.CompareTag("TriangleBlock"))
        {
            foreach (var ct in col.contacts)
                if (ct.normal.y > 0.1f && Mathf.Abs(ct.normal.x) > 0.1f)
                {
                    isGrounded = true;
                    break;
                }
            return;
        }

        // —— 물밖 지면/튕기는 블록 윗면 점프 —— 
        if (!isInWater &&
           (col.gameObject.CompareTag("흙블록") ||
            col.gameObject.CompareTag("BounceBlock")))
        {
            Bounds b = col.collider.bounds;
            foreach (var ct in col.contacts)
                if (ct.point.y >= b.max.y - 0.01f)
                {
                    isGrounded = true;
                    break;
                }
        }
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (isDead) return;
        if (col.gameObject.CompareTag("TriangleBlock") && isInWater)
            waterContactCount = Mathf.Max(waterContactCount, 1);
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (isDead) return;

        // 태양블록에서 벗어나면 다음 충돌을 위해 리셋
        if (col.gameObject.CompareTag("BounceBlock") &&
            col.gameObject.name == "태양블록")
        {
            sunHitProcessed = false;
        }

        if (isInWater && col.gameObject.CompareTag("WaterBottom"))
        {
            isOnWaterBottom = false;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.gravityScale = 1;
        }
        if (!isInWater &&
           (col.gameObject.CompareTag("흙블록") ||
            col.gameObject.CompareTag("BounceBlock") ||
            col.gameObject.CompareTag("TriangleBlock")))
        {
            isGrounded = false;
        }
    }

    void GrowOneStage()
    {
        growthStage++;
        UpdateGrowthVisual();

        float t = Mathf.Clamp01((growthStage - 1) / 5f);
        speed = Mathf.Lerp(initialSpeed, initialSpeed * minSpeedFactor, t);
        jumpPower = Mathf.Lerp(initialJump, initialJump * minJumpFactor, t);

        if (growthStage >= 7)
            BeginDeathByGrowth();
    }

    void BeginDeathByGrowth()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        StartCoroutine(DelayedGameOver());
    }

    IEnumerator DelayedGameOver()
    {
        yield return new WaitForSeconds(1f);
        if (gameOverUI != null)
            gameOverUI.ShowGameOver();
    }

    void UpdateGrowthVisual()
    {
        if (growthRenderer == null || growthSprites == null || growthStage == 0)
            return;
        int idx = Mathf.Clamp(growthStage - 1, 0, growthSprites.Length - 1);
        growthRenderer.sprite = growthSprites[idx];
    }
}
