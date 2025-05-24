using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class BallController : MonoBehaviour
{
    [Header("이동/점프 설정")]
    public float speed = 3f;
    public float jumpPower = 6f;

    [Header("성장 설정")]
    public float growthInterval = 2f;     // 성장 간격 (초)
    public Sprite[] growthSprites;        // 1~7단계 스프라이트 배열
    public SpriteRenderer growthRenderer; // GrowthVisual의 SpriteRenderer
    [Range(0f, 1f)] public float minSpeedFactor = 0.5f;
    [Range(0f, 1f)] public float minJumpFactor = 0.5f;

    [Header("Game Over UI")]
    public GameOverUIScript gameOverUI;

    Rigidbody2D rb;
    bool isOnWaterBottom = false;
    bool isGrounded = false;

    int waterContactCount = 0;
    bool isInWater { get { return waterContactCount > 0; } }

    float waterTimer = 0f;
    int growthStage = 0;    // 0:씨앗, 1~7:성장단계
    float initialSpeed, initialJump;

    bool isDead = false;       // 완전 성장(7단계) 시 true

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        initialSpeed = speed;
        initialJump = jumpPower;
        UpdateGrowthVisual();
    }

    void Update()
    {
        if (isDead) return;  // 죽으면 더 이상 이동이나 성장 타이머 진행 안 함

        // 좌우 이동
        float h = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(h * speed, rb.linearVelocity.y);

        // 물속 성장 타이머
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

        if (isInWater)
        {
            if (col.gameObject.CompareTag("WaterBottom"))
            {
                isOnWaterBottom = true;
                rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                rb.gravityScale = 0;
                return;
            }
            if (col.gameObject.CompareTag("TriangleBlock"))
            {
                isOnWaterBottom = false;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                rb.gravityScale = 1;
                return;
            }
        }
        else
        {
            if (col.gameObject.CompareTag("TriangleBlock"))
            {
                foreach (var ct in col.contacts)
                    if (ct.normal.y > 0.1f && Mathf.Abs(ct.normal.x) > 0.1f)
                    {
                        isGrounded = true;
                        break;
                    }
                return;
            }
            if (col.gameObject.CompareTag("흙블록") || col.gameObject.CompareTag("BounceBlock"))
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
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (isDead) return;
        // 삼각형블록 위에도 물속 상태 유지
        if (col.gameObject.CompareTag("TriangleBlock"))
            waterContactCount = Mathf.Max(waterContactCount, 1);
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (isDead) return;
        if (isInWater && col.gameObject.CompareTag("WaterBottom"))
        {
            isOnWaterBottom = false;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.gravityScale = 1;
        }
        else if (!isInWater &&
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

        // 속도·점프력 감소
        float t = Mathf.Clamp01((growthStage - 1) / 5f);
        speed = Mathf.Lerp(initialSpeed, initialSpeed * minSpeedFactor, t);
        jumpPower = Mathf.Lerp(initialJump, initialJump * minJumpFactor, t);

        if (growthStage >= 7)
        {
            BeginDeathByGrowth();
        }
    }

    private void BeginDeathByGrowth()
    {
        // 완전 성장 시 점프/이동 차단
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0;
        // 1초 뒤에 GameOver 호출
        StartCoroutine(DelayedGameOver());
    }

    private IEnumerator DelayedGameOver()
    {
        yield return new WaitForSeconds(1f);
        if (gameOverUI != null)
            gameOverUI.ShowGameOver();
    }

    void UpdateGrowthVisual()
    {
        if (growthRenderer == null || growthSprites == null || growthStage == 0) return;
        int idx = Mathf.Clamp(growthStage - 1, 0, growthSprites.Length - 1);
        growthRenderer.sprite = growthSprites[idx];
    }
}
