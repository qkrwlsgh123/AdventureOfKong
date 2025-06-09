using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUIScript : MonoBehaviour
{
    public static bool IsGameOver = false; // ✅ 게임 오버 상태 변수

    public GameObject gameOverText;
    public GameObject restartButton;
    public GameObject exitButton;
    public GameObject gameOverPanel;

    void Start()
    {
        IsGameOver = false; // ✅ 초기화

        if (exitButton != null)
        {
            Button btn = exitButton.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(OnExitClick);
            }
        }
    }

    public void ShowGameOver()
    {
        IsGameOver = true; // ✅ GameOver 상태 설정

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (gameOverText != null)
            gameOverText.SetActive(true);

        if (restartButton != null)
            restartButton.SetActive(true);

        if (exitButton != null)
            exitButton.SetActive(true);
    }

    public void OnExitClick()
    {
        SceneManager.LoadScene("StageSelection");
    }
}
