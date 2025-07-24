using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI;       // Pause UI 전체
    public Button resumeButton;          // 재개 버튼
    public Button exitButton;            // 나가기 버튼
    public Button restartFromBeginningButton; // 처음부터 버튼

    void Start()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        resumeButton.onClick.AddListener(ResumeGame);
        exitButton.onClick.AddListener(ExitToStageSelection);
        restartFromBeginningButton.onClick.AddListener(RestartFromBeginning);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // ✅ GameOver 상태에서는 ESC 무시
            if (GameOverUIScript.IsGameOver)
            {
                Debug.Log("❌ GameOver 상태에서는 PauseMenu 무시됨");
                return;
            }

            if (pauseMenuUI != null && !pauseMenuUI.activeSelf)
                PauseGame();
            else
                ResumeGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);
    }

    public void ExitToStageSelection()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StageSelection");
    }

    public void RestartFromBeginning()
    {
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
