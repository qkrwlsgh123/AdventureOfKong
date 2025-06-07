using UnityEngine;

public class GameStartButton : MonoBehaviour
{
    public void OnGameStart()
    {
        // 🎯 오직 하나의 인자만 줘야 오류 없음!
        SoundManager.Instance.PlayClickAndLoadScene("MapSelection");
    }
}
