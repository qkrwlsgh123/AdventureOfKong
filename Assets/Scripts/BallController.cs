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

    [Header("물 이펙트 설정")]
    public GameObject splashEffectPrefab;
    public AudioClip waterSplashSound;

    [Header("사운드 설정")]
    public AudioClip jumpSound;
    public AudioClip growSound;
    public AudioClip bugBiteSound;

    [Header("새 프리팹 연결")]
    public EogkkaBirdController birdPrefab;

    private Rigidbody2D rb;
    private AudioSource audioSource;

    private bool isGrounded = false;
    private bool isOnWaterBottom = false;
    private bool isDead = false;
    private bool isPausedByEogkka = false;
    private bool onTriangleSlope = false;
    private bool jumpManuallyThisFrame = false;

    private int waterContactCount = 0;
    private bool waterEverContact = false;
    private bool isInWater { get { return waterContactCount > 0; } }

    private float waterTimer = 0f;
    private int growthStage = 0;
    private float initialSpeed, initialJump;

    private HashSet<GameObject> bugTouched = new HashSet<GameObject>();
    private GameObject lastSunBlockTouched = null;
    private GameObject currentSunBlock = null;

    public int CurrentHealth { get { return currentHealth; } }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        initialSpeed = speed;
        initialJump = jumpPower;

        maxHealth = baseHealth + growthStage;
        currentHealth = maxHealth;

        UpdateGrowthVisual();
    }

    void Update()
    {
        if (isDead || isPausedByEogkka || Time.timeScale == 0f) return;

        float h = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(h * speed, rb.linearVelocity.y);

        if (isInWater && growthStage < 7)
        {
            waterTimer += Time.deltaTime;
            if (waterTimer >= growthInterval)
            {
                waterTimer = 0f;
                GrowOneStage();
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            TriggerEogkkaDirectly();
        }

        if (isInWater && isOnWaterBottom && onTriangleSlope && Mathf.Abs(h) > 0.01f)
        {
            rb.linearVelocity += new Vector2(0.75f * Mathf.Sign(h), 0.05f);
        }
    }

    void FixedUpdate()
    {
        if (isDead || isPausedByEogkka || Time.timeScale == 0f) return;

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
            onTriangleSlope = false;

            if (isGrounded || jumpManuallyThisFrame)
            {
                if (jumpSound != null && audioSource != null)
                    audioSource.PlayOneShot(jumpSound);

                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
                isGrounded = false;
                jumpManuallyThisFrame = false;
            }
        }
    }

    public void PauseGameExceptBird()
    {
        isPausedByEogkka = true;
        isDead = true;

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        var sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.enabled = false;
    }

    public void TriggerEogkkaDirectly()
    {
        Time.timeScale = 0f;
        Vector3 spawnPos = new Vector3(0f, 6f, 0f);
        Instantiate(birdPrefab, spawnPos, Quaternion.identity);
    }

    public void TakeDamage()
    {
        if (isDead) return;

        currentHealth--;

        if (bugBiteSound != null && audioSource != null)
            audioSource.PlayOneShot(bugBiteSound);

        if (currentHealth <= 0)
        {
            isDead = true;
            gameOverUI.ShowGameOver();
        }
    }

    public void PlayPoopEatSound()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayEatSound();
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

        if (growSound != null && audioSource != null)
            audioSource.PlayOneShot(growSound);

        if (growthStage >= 7)
            BeginDeathByGrowth();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("물블록"))
        {
            waterContactCount++;
            if (waterContactCount == 1)
            {
                waterTimer = 0f;
                waterEverContact = true;

                if (splashEffectPrefab != null)
                {
                    Vector3 splashPosition = transform.position + new Vector3(0f, 0.2f, 0f);
                    GameObject splash = Instantiate(splashEffectPrefab, splashPosition, Quaternion.identity);
                    Destroy(splash, 0.5f);
                }

                if (waterSplashSound != null && audioSource != null)
                    audioSource.PlayOneShot(waterSplashSound);
            }
        }

        if (other.CompareTag("DeathZone") && !isDead)
        {
            isDead = true;
            gameOverUI?.ShowGameOver();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("물블록"))
        {
            waterContactCount = Mathf.Max(0, waterContactCount - 1);
            isOnWaterBottom = false;
            onTriangleSlope = false;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (isDead) return;

        if (col.gameObject.GetComponent<BugBlockDamage>() != null && !bugTouched.Contains(col.gameObject))
        {
            bugTouched.Add(col.gameObject);
            TakeDamage();
        }

        if (col.gameObject.CompareTag("BounceBlock") && col.gameObject.name.Contains("태양블록") && growthStage < 7)
        {
            if (currentSunBlock != col.gameObject)
            {
                currentSunBlock = col.gameObject;

                int steps = waterEverContact ? 2 : 1;
                for (int i = 0; i < steps && growthStage < 7; i++)
                    GrowOneStage();
            }

            // 점프 조건: 윗면 충돌 시에만 isGrounded 적용
            Bounds b = col.collider.bounds;
            foreach (var ct in col.contacts)
            {
                if (ct.normal.y > 0.5f && ct.point.y >= b.max.y - 0.01f)
                {
                    isGrounded = true;
                    jumpManuallyThisFrame = true;
                    break;
                }
            }
            return;
        }

        if (isInWater && col.gameObject.CompareTag("WaterBottom"))
        {
            isOnWaterBottom = true;
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            rb.gravityScale = 0;
            return;
        }

        if (isInWater && col.gameObject.CompareTag("TriangleBlock"))
        {
            isOnWaterBottom = false;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.gravityScale = 1;
            return;
        }

        if (col.gameObject.CompareTag("TriangleBlock"))
        {
            onTriangleSlope = false;

            bool isActuallyInWater = false;
            Collider2D[] overlaps = Physics2D.OverlapCircleAll(transform.position, 0.05f);
            foreach (var hit in overlaps)
            {
                if (hit.CompareTag("물블록"))
                {
                    isActuallyInWater = true;
                    break;
                }
            }

            foreach (var ct in col.contacts)
            {
                if (ct.normal.y > 0.01f && Mathf.Abs(ct.normal.x) > 0.01f)
                {
                    isGrounded = true;
                    onTriangleSlope = isActuallyInWater;
                    break;
                }
            }
            return;
        }

        if (!isInWater && (col.gameObject.CompareTag("흙블록") || col.gameObject.CompareTag("BounceBlock")))
        {
            Bounds b = col.collider.bounds;
            foreach (var ct in col.contacts)
            {
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
        if (col.gameObject.GetComponent<BugBlockDamage>() != null)
        {
            bugTouched.Remove(col.gameObject);
        }

        if (col.gameObject.name.Contains("태양블록"))
        {
            currentSunBlock = null;
        }

        if (isInWater && col.gameObject.CompareTag("WaterBottom"))
        {
            isOnWaterBottom = false;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.gravityScale = 1;
        }

        if (!isInWater && (col.gameObject.CompareTag("흙블록") || col.gameObject.CompareTag("BounceBlock") || col.gameObject.CompareTag("TriangleBlock")))
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
        gameOverUI?.ShowGameOver();
    }
}
