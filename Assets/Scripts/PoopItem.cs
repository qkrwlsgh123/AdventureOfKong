using UnityEngine;

public class PoopItem : MonoBehaviour
{
    public GameObject bangEffectPrefab;   // 💥 이펙트

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            BallController ball = other.GetComponent<BallController>();
            if (ball != null)
            {
                ball.PlayPoopEatSound();  // ✅ 콩이 소리 재생
            }

            if (bangEffectPrefab != null)
                Instantiate(bangEffectPrefab, transform.position, Quaternion.identity);

            if (GameManager.Instance != null)
                GameManager.Instance.PoopCollected(this);

            gameObject.SetActive(false);
        }
    }
}
