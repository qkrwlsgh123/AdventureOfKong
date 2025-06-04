using UnityEngine;

public class GameStartButton : MonoBehaviour
{
    public string nextSceneName = "MapSelection";

    public void OnGameStart()
    {
        SoundManager.Instance.PlayClickAndLoadScene(nextSceneName, 0.2f);
    }
}
