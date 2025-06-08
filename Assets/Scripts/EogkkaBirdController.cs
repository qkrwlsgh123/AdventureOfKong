using UnityEngine;

public class EogkkaBirdController : MonoBehaviour
{
    public float speed = 5f;
    public float exitDistance = 15f;

    private Vector3 moveDir;
    private Vector3 startPosition;
    private Transform targetBall;
    private bool hasEaten = false;

    void Start()
    {
        GameObject ballObj = GameObject.FindWithTag("Player");
        if (ballObj == null) return;

        targetBall = ballObj.transform;
        startPosition = transform.position;
        moveDir = (targetBall.position - transform.position).normalized;

        Vector3 scale = transform.localScale;
        scale.x = moveDir.x < 0 ? -1f : 1f;
        transform.localScale = scale;
    }

    void Update()
    {
        transform.position += moveDir * speed * Time.unscaledDeltaTime;

        if (!hasEaten && targetBall != null)
        {
            float distance = Vector2.Distance(transform.position, targetBall.position);
            if (distance < 0.3f)
            {
                EatBall();
            }
        }

        float dist = Vector3.Distance(transform.position, startPosition);
        if (dist > exitDistance)
        {
            GameOverUIScript ui = FindFirstObjectByType<GameOverUIScript>();
            if (ui != null)
                ui.ShowGameOver();

            Destroy(gameObject);
        }
    }

    void EatBall()
    {
        hasEaten = true;

        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayEatSound();

        SpriteRenderer sr = targetBall.GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.enabled = false;

        Rigidbody2D rb = targetBall.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.simulated = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 사용 안 함
    }
}
