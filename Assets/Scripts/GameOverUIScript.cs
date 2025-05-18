using UnityEngine;

public class GameOverUIScript : MonoBehaviour
{
    public GameObject gameOverText;    // “Game Over” 텍스트
    public GameObject restartButton;   // “다시 시작” 버튼
    public GameObject gameOverPanel;   // 검은색 배경 패널

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);   // 배경 먼저 활성화
        gameOverText.SetActive(true);    // 텍스트 활성화
        restartButton.SetActive(true);   // 버튼 활성화
    }
}
