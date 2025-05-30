using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public bool soundEnabled = true;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환해도 유지
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetSoundEnabled(bool enabled)
    {
        soundEnabled = enabled;
    }

    public bool IsSoundEnabled()
    {
        return soundEnabled;
    }
}
