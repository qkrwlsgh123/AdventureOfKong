using UnityEngine;

public class BallBounceScript : MonoBehaviour
{
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 태그가 BounceBlock인 오브젝트에 닿았는지 확인
        if (collision.gameObject.CompareTag("BounceBlock"))
        {
            BounceSurface surface = collision.gameObject.GetComponent<BounceSurface>();
            if (surface != null)
            {
                float force = surface.GetBounceForce();
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, force);
            }
        }
    }
}
