using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI;     // PauseMenuUI ��ü
    public GameObject darkPanel;       // ��ο� ��� �г�
    public Button resumeButton;        // �ٽ� ���� ��ư
    public Button exitButton;          // ������ ��ư

    void Start()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        resumeButton.onClick.AddListener(ResumeGame);
        exitButton.onClick.AddListener(ExitToStageSelection);
    }

    void Update()
    {
        // ESC Ű �Ǵ� Android Back Ű �Է� ����
        if (Input.GetKeyDown(KeyCode.Escape))
        {
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
