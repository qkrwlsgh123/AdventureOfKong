using UnityEngine;

public class BallStopController : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void StopImmediately()
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;
        }

        if (sr != null)
        {
            sr.enabled = true;
        }
    }
}
