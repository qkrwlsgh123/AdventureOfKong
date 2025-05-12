using UnityEngine;

/// <summary>
/// 방향키로 Ball을 좌우로 움직이는 스크립트
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class BallController : MonoBehaviour
{
    [Tooltip("좌우 이동 속도")]
    public float speed = 5f;

    private Rigidbody2D rb;

    void Awake()
    {
        // Rigidbody2D 컴포넌트 가져오기
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
    }
}
