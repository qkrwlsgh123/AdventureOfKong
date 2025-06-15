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
        // ✅ 시작 시 GameOver 상태 초기화
        IsGameOver = false;

        // ✅ GameOver UI 전체 꺼주기
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        // 나가기 버튼 클릭 이벤트 연결
        if (exitButton != null)
        {
            Button btn = exitButton.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(OnExitClick);
            }
        }
    }

    // ✅ 게임오버 UI 활성화
    public void ShowGameOver()
    {
        IsGameOver = true;

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
