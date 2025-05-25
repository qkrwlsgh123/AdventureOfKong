using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BugBlockDamage : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        // Player �±װ� ���� ������Ʈ���� ������
        if (other.gameObject.CompareTag("Player"))
        {
            var ball = other.gameObject.GetComponent<BallController>();
            if (ball != null)
                ball.TakeDamage();
        }
    }
}
