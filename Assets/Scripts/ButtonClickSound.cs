using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonClickSound : MonoBehaviour
{
    public AudioClip clickSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = clickSound;

        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(PlayClickSound);
    }

    void PlayClickSound()
    {
        if (clickSound != null && SoundManager.Instance != null && SoundManager.Instance.IsSoundEnabled())
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}
