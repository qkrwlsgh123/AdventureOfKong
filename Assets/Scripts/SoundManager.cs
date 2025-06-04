using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public AudioClip buttonClickClip;
    private AudioSource audioSource;

    public bool soundEnabled = true;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = gameObject.AddComponent<AudioSource>();
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

    public void PlayClickSound()
    {
        if (soundEnabled && buttonClickClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(buttonClickClip);
        }
    }

    // ✅ 소리 재생 후 delay 주고 씬 전환
    public void PlayClickAndLoadScene(string sceneName, float delay = 0.2f)
    {
        if (Instance != null)
            StartCoroutine(LoadSceneAfterClick(sceneName, delay));
    }

    private IEnumerator LoadSceneAfterClick(string sceneName, float delay)
    {
        PlayClickSound();
        yield return new WaitForSecondsRealtime(delay); // ✅ 중요: 타임스케일 무시
        SceneManager.LoadScene(sceneName);
    }
}
