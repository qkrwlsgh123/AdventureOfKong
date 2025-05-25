using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BugBlockDamage : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        // Player 태그가 붙은 오브젝트에만 데미지
        if (other.gameObject.CompareTag("Player"))
        {
            var ball = other.gameObject.GetComponent<BallController>();
            if (ball != null)
                ball.TakeDamage();
        }
    }
}
