using UnityEngine;

public class BallBounceScript : MonoBehaviour
{
    public float bounceForce = 10f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // �ٴ��̳� ���ǿ� ����� ���� ƨ���
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Platform"))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, bounceForce);
        }
    }
}
