using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI;     // PauseMenuUI 전체
    public GameObject darkPanel;       // 어두운 배경 패널
    public Button resumeButton;        // 다시 시작 버튼
    public Button exitButton;          // 나가기 버튼
    public GameObject clearUI;         // ✅ ClearUI 오브젝트 (Inspector에서 연결해야 함)

    void Start()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        resumeButton.onClick.AddListener(ResumeGame);
        exitButton.onClick.AddListener(ExitToStageSelection);
    }

    void Update()
    {
        // ✅ GameOver 상태면 ESC 무시
        if (GameOverUIScript.IsGameOver)
            return;

        // ✅ ESC 키 눌렀을 때만 ClearUI 상태 체크
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // ✅ ClearUI가 떠 있으면 ESC로는 PauseUI 못 켬
            if (clearUI != null && clearUI.activeSelf)
            {
                Debug.Log("✅ ClearUI 열려있음 → ESC 무시");
                return;
            }

            // ESC로 PauseUI 토글
            if (pauseMenuUI != null && !pauseMenuUI.activeSelf)
            {
                PauseGame();
            }
            else if (pauseMenuUI != null && pauseMenuUI.activeSelf)
            {
                ResumeGame();
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        if (pauseMenuUI != null) pauseMenuUI.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
    }

    public void ExitToStageSelection()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StageSelection");
    }
}
