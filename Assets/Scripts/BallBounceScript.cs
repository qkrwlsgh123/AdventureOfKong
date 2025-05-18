using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BallBounceScript : MonoBehaviour
{
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // BounceBlock 태그가 붙은 오브젝트와 충돌 시
        if (collision.gameObject.CompareTag("BounceBlock"))
        {
            // 모든 접촉 지점(contact) 중에 위쪽 면 충돌만 처리
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y > 0.9f)
                {
                    BounceSurface surface = collision.gameObject.GetComponent<BounceSurface>();
                    if (surface != null)
                    {
                        float force = surface.GetBounceForce();
                        // y축 속도만 튕김 힘으로 대체
                        rb.linearVelocity = new Vector2(rb.linearVelocity.x, force);
                    }
                    break;
                }
            }
        }
    }
}
