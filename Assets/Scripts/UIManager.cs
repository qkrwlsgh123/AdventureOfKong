using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    /// <summary>
    /// 메인 메뉴에서 “게임 시작” 버튼 클릭 시 호출
    /// </summary>
    public void ShowMapSelection()
    {
        SceneManager.LoadScene("MapSelection");
    }

    /// <summary>
    /// 메인 메뉴에서 “설정” 버튼 클릭 시 호출
    /// </summary>
    public void ShowSettings()
    {
        // 설정 UI 팝업 또는 씬 전환 로직을 여기에 추가하세요
        Debug.Log("Settings clicked");
    }

    /// <summary>
    /// MapSelection 화면에서 맵 버튼 클릭 시 호출
    /// </summary>
    /// <param name="mapIndex">선택된 맵의 인덱스 (0부터)</param>
    public void ShowStageSelection(int mapIndex)
    {
        // 필요하다면 선택된 맵 인덱스를 저장
        PlayerPrefs.SetInt("SelectedMap", mapIndex);
        SceneManager.LoadScene("StageSelection");
    }

    /// <summary>
    /// StageSelection 화면에서 스테이지 버튼 클릭 시 호출
    /// </summary>
    /// <param name="stageIndex">선택된 스테이지 번호 (1~30)</param>
    public void LoadLevel(int stageIndex)
    {
        // 이전에 저장한 맵 인덱스를 불러옵니다 (없으면 0)
        int mapIndex = PlayerPrefs.GetInt("SelectedMap", 0);
        // 씬 이름을 "Map{mapIndex}_Stage{stageIndex}" 형식으로 가정
        string sceneName = $"Map{mapIndex}_Stage{stageIndex}";
        SceneManager.LoadScene(sceneName);
    }
}
