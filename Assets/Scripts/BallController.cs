using UnityEngine;

/// <summary>
/// 방향키로 Ball을 좌우로 움직이는 스크립트 + 물에서 점프 금지
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class BallController : MonoBehaviour
{
    [Tooltip("좌우 이동 속도")]
    public float speed = 5f;

    [Tooltip("점프 파워")]
    public float jumpPower = 10f;

    private Rigidbody2D rb;

    // 물에 있는지 여부
    public bool isInWater = false;

    // 땅에 닿아 있는지 여부
    private bool isGrounded = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D가 없습니다. Ball 오브젝트에 Rigidbody2D 컴포넌트를 추가하세요!");
        }
    }

    void Update()
    {
        // 좌우 방향 입력값 (-1 ~ 1)
        float input = Input.GetAxis("Horizontal");
        Debug.Log("입력값: " + input); // 콘솔에서 입력 확인용

        // 현재 속도 가져와서 x축만 수정
        Vector2 velocity = rb.linearVelocity;
        velocity.x = input * speed;
        rb.linearVelocity = velocity;

        // 점프 처리: 물 안이 아니고, 땅에 닿아 있을 때만 점프 가능
        if (!isInWater && isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
        }
    }

    // 물에 들어갔을 때 (Trigger 진입)
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("트리거 감지됨: " + other.name);

        if (other.CompareTag("Water"))
        {
            Debug.Log("Water 태그 감지됨");
            isInWater = true;
        }
    }

    // 물에서 나왔을 때 (Trigger 벗어남)
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            isInWater = false;
        }
    }

    // 땅에 닿았을 때
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("BounceBlock"))
        {
            isGrounded = true;
        }
    }

    // 땅에서 떨어졌을 때
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("BounceBlock"))
        {
            isGrounded = false;
        }
    }
}
