using UnityEngine;

/// <summary>
/// 자동 점프 / 물 안에서는 점프 금지 + 자연스러운 하강 후 바닥 고정
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class BallController : MonoBehaviour
{
    [Tooltip("좌우 이동 속도")]
    public float speed = 5f;

    [Tooltip("점프 파워")]
    public float jumpPower = 1.5f;

    private Rigidbody2D rb;

    public bool isInWater = false;
    private bool isGrounded = false;
    private bool isOnWaterBottom = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 좌우 이동은 항상 가능
        float input = Input.GetAxis("Horizontal");
        Vector2 velocity = rb.linearVelocity;
        velocity.x = input * speed;
        rb.linearVelocity = velocity;
    }

    void FixedUpdate()
    {
        if (isInWater)
        {
            rb.gravityScale = 1;

            // 물 안에 있고 바닥에 닿아 있을 때만 y속도 제거 → 바닥에 붙이기
            if (isOnWaterBottom)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            }
        }
        else
        {
            rb.gravityScale = 1;

            if (isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
                isGrounded = false;
            }
        }
    }

    // 물 감지
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("물블록"))
        {
            isInWater = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("물블록"))
        {
            isInWater = false;
            isOnWaterBottom = false;
        }
    }

    // 바닥 감지 (흙 or Bounce or 물바닥 위에 닿은 상태 확인)
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("흙블록") || collision.gameObject.CompareTag("BounceBlock"))
        {
            if (isInWater)
            {
                isOnWaterBottom = true;
            }
            else
            {
                isGrounded = true;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("흙블록") || collision.gameObject.CompareTag("BounceBlock"))
        {
            if (isInWater)
            {
                isOnWaterBottom = false;
            }
            else
            {
                isGrounded = false;
            }
        }
    }
}
