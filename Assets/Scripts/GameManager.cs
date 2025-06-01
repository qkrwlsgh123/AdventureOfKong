using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("클리어 UI")]
    public GameObject clearUI;
    public TextMeshProUGUI messageText;

    private int totalPoopCount = 0;
    private int collectedCount = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        // 씬 안에 있는 모든 똥 개수 세기
        totalPoopCount = GameObject.FindGameObjectsWithTag("Poop").Length;
        collectedCount = 0;

        // 클리어 UI는 처음에 꺼두기
        if (clearUI != null)
            clearUI.SetActive(false);
    }

    // 콩이 똥을 먹을 때마다 호출
    public void PoopCollected(PoopItem poop)
    {
        collectedCount++;

        if (collectedCount >= totalPoopCount)
        {
            StageClear();
        }
    }

    // 스테이지 클리어 처리
    void StageClear()
    {
        StartCoroutine(ShowClearUIAfterDelay(1f));  // 1초 후 클리어 UI 표시
    }

    IEnumerator ShowClearUIAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        Time.timeScale = 0f;

        if (clearUI != null)
            clearUI.SetActive(true);

        if (messageText != null)
            messageText.text = "GOOD!";
    }

    // 다음 스테이지로 이동
    public void GoToNextStage()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // 스테이지 선택 화면으로 이동
    public void GoToStageSelect()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StageSelection");
    }

    // ✅ 현재 스테이지 다시 시작
    public void RestartStage()
    {
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
