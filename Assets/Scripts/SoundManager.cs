using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("사운드 설정")]
    public bool soundEnabled = true;

    [Header("효과음 클립")]
    public AudioClip buttonClickClip;
    public AudioClip eatClip;

    private AudioSource audioSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayClickSound()
    {
        if (soundEnabled && buttonClickClip != null)
            audioSource.PlayOneShot(buttonClickClip);
    }

    public void PlayEatSound()
    {
        if (soundEnabled && eatClip != null)
            audioSource.PlayOneShot(eatClip);
    }

    // ✅ 하나의 인자만 받는 안전한 버전
    public void PlayClickAndLoadScene(string sceneName)
    {
        if (soundEnabled && buttonClickClip != null)
            audioSource.PlayOneShot(buttonClickClip);

        StartCoroutine(DelayedSceneLoad(sceneName));
    }

    private System.Collections.IEnumerator DelayedSceneLoad(string sceneName)
    {
        yield return new WaitForSeconds(0.15f); // 소리 재생 시간만큼 대기
        SceneManager.LoadScene(sceneName);
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
