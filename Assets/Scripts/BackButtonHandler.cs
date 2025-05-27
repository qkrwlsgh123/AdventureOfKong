// Assets/Scripts/BackButtonHandler.cs

using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButtonHandler : MonoBehaviour
{
    void Update()
    {
        // Android 뒤로가기 또는 에디터 ESC 키 감지
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 현재 활성화된 씬 이름 조회
            string currentScene = SceneManager.GetActiveScene().name;

            switch (currentScene)
            {
                case "MapSelection":
                    // 맵 선택 화면에서는 메인 메뉴로 돌아감
                    SceneManager.LoadScene("MainMenu");
                    break;

                case "StageSelection":
                    // 스테이지 선택 화면에서는 맵 선택으로
                    SceneManager.LoadScene("MapSelection");
                    break;

                // MainMenu 씬에서는 QuitConfirmation 패널이 담당하므로 처리하지 않음
                // 실제 게임 플레이 씬에서도 아무 동작 없이 무시됩니다
                default:
                    break;
            }
        }
    }
}
