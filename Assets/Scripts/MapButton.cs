using UnityEngine;

public class MapButton : MonoBehaviour
{
    public GameObject stageSelectionPanel;  // 열릴 Stage 선택 UI 패널

    public void OnMapClicked()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayClickSound();  // ✅ 소리만 재생

        if (stageSelectionPanel != null)
            stageSelectionPanel.SetActive(true);     // ✅ UI 패널 보여주기
    }
}
