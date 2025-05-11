using UnityEngine;

public class DeathZoneScript : MonoBehaviour
{
    public GameOverUIScript gameOverUI;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Game Over!");
            gameOverUI.ShowGameOver();
        }
    }
}
