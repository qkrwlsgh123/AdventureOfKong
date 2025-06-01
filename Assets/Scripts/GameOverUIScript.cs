using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUIScript : MonoBehaviour
{
    public GameObject gameOverText;
    public GameObject restartButton;
    public GameObject exitButton;
    public GameObject gameOverPanel;

    void Start()
    {
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
