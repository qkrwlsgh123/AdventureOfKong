using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BallController : MonoBehaviour
{
    [Tooltip("좌우 이동 속도")]
    public float speed = 3f;
    [Tooltip("점프 파워")]
    public float jumpPower = 6f;

    private Rigidbody2D rb;
    private bool isInWater = false;
    private bool isOnWaterBottom = false;
    private bool isGrounded = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 좌우 이동
        float input = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(input * speed, rb.linearVelocity.y);
    }

    void FixedUpdate()
    {
        if (isInWater)
        {
            rb.gravityScale = 1;
            if (isOnWaterBottom)
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
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

    // 물블록 입수 감지
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("물블록"))
            isInWater = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("물블록"))
        {
            isInWater = false;
            isOnWaterBottom = false;
        }
    }

    // 흙블록 또는 BounceBlock에 부딪힐 때
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("흙블록") ||
            collision.gameObject.CompareTag("BounceBlock"))
        {
            // 블록 콜라이더 bounds 가져오기
            Collider2D blockCol = collision.collider;
            Bounds bounds = blockCol.bounds;

            foreach (ContactPoint2D contact in collision.contacts)
            {
                // 충돌 지점 y 좌표가 블록 상단(bounds.max.y)에 가까울 때만
                if (contact.point.y >= bounds.max.y - 0.01f)
                {
                    if (isInWater) isOnWaterBottom = true;
                    else isGrounded = true;
                    break;
                }
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("흙블록") ||
            collision.gameObject.CompareTag("BounceBlock"))
        {
            if (isInWater) isOnWaterBottom = false;
            else isGrounded = false;
        }
    }
}
