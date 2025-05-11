using UnityEngine;
using UnityEngine.UI;

public class GameOverUIScript : MonoBehaviour
{
    public GameObject gameOverText;

    public void ShowGameOver()
    {
        gameOverText.SetActive(true);
    }
}
