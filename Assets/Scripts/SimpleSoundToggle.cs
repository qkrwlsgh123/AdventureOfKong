using UnityEngine;
using UnityEngine.UI;

public class SimpleSoundToggle : MonoBehaviour
{
    public Image iconImage;
    public Sprite onSprite;
    public Sprite offSprite;

    private bool isSoundOn;

    void Start()
    {
        // ����� ���� �ҷ����� (ó���� ���� ���·� ����)
        isSoundOn = PlayerPrefs.GetInt("SoundEnabled", 1) == 1;
        ApplySound();
    }

    public void ToggleSound()
    {
        isSoundOn = !isSoundOn;
        ApplySound();
        PlayerPrefs.SetInt("SoundEnabled", isSoundOn ? 1 : 0);
    }

    void ApplySound()
    {
        AudioListener.volume = isSoundOn ? 1f : 0f;
        iconImage.sprite = isSoundOn ? onSprite : offSprite;
    }
}
