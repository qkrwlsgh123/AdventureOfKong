using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class BallController : MonoBehaviour
{
    [Tooltip("좌우 이동 속도")]
    public float speed = 3f;
    [Tooltip("점프 파워")]
    public float jumpPower = 6f;

    private Rigidbody2D rb;
    private Collider2D ballCol;
    private bool isInWater = false;
    private bool isOnWaterBottom = false;
    private bool isGrounded = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ballCol = GetComponent<Collider2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        Vector2 v = rb.linearVelocity;
        v.x = h * speed;
        rb.linearVelocity = v;
    }

    void FixedUpdate()
    {
        if (isInWater)
        {
            if (isOnWaterBottom)
            {
                // 물바닥 위에서는 수직 속도만 0으로 고정
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            }
            else
            {
                // 물 안에 떠 있을 때
                rb.gravityScale = 1;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }
        else
        {
            // 물 밖일 때
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
            isInWater = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("물블록"))
        {
            isInWater = false;
            isOnWaterBottom = false;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 1) 물속 & 물바닥 고정
        if (isInWater && collision.gameObject.CompareTag("WaterBottom"))
        {
            isOnWaterBottom = true;
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            rb.gravityScale = 0;
            return;
        }

        // 2) 삼각형블록 처리
        if (collision.gameObject.CompareTag("TriangleBlock"))
        {
            if (isInWater)
            {
                // 물속이었으면 물속 상태 해제하고 그대로 경사 타게
                isInWater = false;
                isOnWaterBottom = false;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                rb.gravityScale = 1;
            }
            else
            {
                // 물 밖에서는 빗면에서만 점프
                foreach (var contact in collision.contacts)
                {
                    if (contact.normal.y > 0.1f && Mathf.Abs(contact.normal.x) > 0.1f)
                    {
                        isGrounded = true;
                        break;
                    }
                }
            }
            return;
        }

        // 3) 물 밖 일반 지면(BounceBlock/흙블록) 윗면만 점프
        if (!isInWater &&
           (collision.gameObject.CompareTag("흙블록") ||
            collision.gameObject.CompareTag("BounceBlock")))
        {
            Bounds b = collision.collider.bounds;
            foreach (var contact in collision.contacts)
            {
                if (contact.point.y >= b.max.y - 0.01f)
                {
                    isGrounded = true;
                    break;
                }
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // 물속 바닥에서 벗어날 때
        if (isInWater && collision.gameObject.CompareTag("WaterBottom"))
        {
            isOnWaterBottom = false;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.gravityScale = 1;
        }
        // 물 밖 지면/튕기는 블록에서 벗어날 때
        else if (!isInWater &&
                (collision.gameObject.CompareTag("흙블록") ||
                 collision.gameObject.CompareTag("BounceBlock")))
        {
            isGrounded = false;
        }
    }
}
