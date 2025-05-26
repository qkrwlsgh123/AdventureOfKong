using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class BallController : MonoBehaviour
{
    [Header("이동/점프 설정")]
    public float speed = 3f;
    public float jumpPower = 6f;

    [Header("성장 설정")]
    public float growthInterval = 2f;
    public Sprite[] growthSprites;
    public SpriteRenderer growthRenderer;
    [Range(0f, 1f)] public float minSpeedFactor = 0.667f;
    [Range(0f, 1f)] public float minJumpFactor = 0.667f;

    [Header("Health 설정")]
    public int baseHealth = 3;
    private int maxHealth;
    private int currentHealth;

    [Header("Game Over UI")]
    public GameOverUIScript gameOverUI;

    // 내부 상태
    Rigidbody2D rb;
    bool isGrounded = false;
    bool isOnWaterBottom = false;
    bool isDead = false;

    int waterContactCount = 0;
    bool waterEverContact = false;
    bool isInWater { get { return waterContactCount > 0; } }

    float waterTimer = 0f;
    int growthStage = 0;
    float initialSpeed, initialJump;

    private bool sunHitProcessed = false;
    private HashSet<GameObject> bugTouched = new HashSet<GameObject>();

    public int CurrentHealth { get { return currentHealth; } }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        initialSpeed = speed;
        initialJump = jumpPower;

        maxHealth = baseHealth + growthStage;
        currentHealth = maxHealth;

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

    void GrowOneStage()
    {
        growthStage++;
        UpdateGrowthVisual();

        maxHealth = baseHealth + growthStage;
        currentHealth = Mathf.Min(currentHealth + 1, maxHealth);

        float t = Mathf.Clamp01((growthStage - 1) / 5f);
        speed = Mathf.Lerp(initialSpeed, initialSpeed * minSpeedFactor, t);
        jumpPower = Mathf.Lerp(initialJump, initialJump * minJumpFactor, t);

        if (growthStage >= 7)
            BeginDeathByGrowth();
    }

    public void TakeDamage()
    {
        if (isDead) return;

        currentHealth--;
        Debug.Log($"[Damage] Health = {currentHealth}/{maxHealth}");
        if (currentHealth <= 0)
        {
            isDead = true;
            gameOverUI.ShowGameOver();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("물블록"))
        {
            waterContactCount++;
            if (waterContactCount == 1) waterTimer = 0f;
            waterEverContact = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("물블록"))
        {
            waterContactCount = Mathf.Max(0, waterContactCount - 1);
            isOnWaterBottom = false;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (isDead) return;

        // 벌레블록 처리
        if (col.gameObject.GetComponent<BugBlockDamage>() != null &&
            !bugTouched.Contains(col.gameObject))
        {
            bugTouched.Add(col.gameObject);
            TakeDamage();
        }

        // 태양블록 처리
        if (!sunHitProcessed &&
            col.gameObject.CompareTag("BounceBlock") &&
            col.gameObject.name == "태양블록" &&
            growthStage < 7)
        {
            sunHitProcessed = true;
            int steps = waterEverContact ? 2 : 1;
            for (int i = 0; i < steps && growthStage < 7; i++)
                GrowOneStage();
            return;
        }

        // 물속 바닥 고정
        if (isInWater && col.gameObject.CompareTag("WaterBottom"))
        {
            isOnWaterBottom = true;
            rb.constraints = RigidbodyConstraints2D.FreezePositionY
                            | RigidbodyConstraints2D.FreezeRotation;
            rb.gravityScale = 0;
            return;
        }

        // 물속 슬로프
        if (isInWater && col.gameObject.CompareTag("TriangleBlock"))
        {
            isOnWaterBottom = false;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.gravityScale = 1;
            return;
        }

        // 물밖 슬로프 점프
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

        // *** 일반/튕기는 블록 윗면 점프 수정 부분 ***
        if (!isInWater &&
           (col.gameObject.CompareTag("흙블록") ||
            col.gameObject.CompareTag("BounceBlock")))
        {
            Bounds b = col.collider.bounds;
            foreach (var ct in col.contacts)
            {
                // 이제 접촉 노멀과 위치를 모두 검사합니다
                if (ct.normal.y > 0.5f && ct.point.y >= b.max.y - 0.01f)
                {
                    isGrounded = true;
                    break;
                }
            }
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        // 태양블록 리셋
        if (col.gameObject.CompareTag("BounceBlock") &&
            col.gameObject.name == "태양블록")
        {
            sunHitProcessed = false;
        }
        // 벌레블록 리셋
        if (col.gameObject.GetComponent<BugBlockDamage>() != null)
        {
            bugTouched.Remove(col.gameObject);
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

    void UpdateGrowthVisual()
    {
        if (growthRenderer == null || growthSprites == null || growthStage == 0) return;
        int idx = Mathf.Clamp(growthStage - 1, 0, growthSprites.Length - 1);
        growthRenderer.sprite = growthSprites[idx];
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
}
