using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoundToggleUI : MonoBehaviour
{
    public RectTransform sliderThumb;
    public Image backgroundImage;
    public TextMeshProUGUI label;

    public Color colorOn = Color.green;
    public Color colorOff = Color.red;

    public float onX = 40f;
    public float offX = -40f;

    void Start()
    {
        UpdateUI();
    }

    public void OnToggleSound()
    {
        bool newState = !SoundManager.Instance.IsSoundEnabled();
        SoundManager.Instance.SetSoundEnabled(newState);
        UpdateUI();
    }

    void UpdateUI()
    {
        bool isOn = SoundManager.Instance.IsSoundEnabled();

        backgroundImage.color = isOn ? colorOn : colorOff;
        label.text = isOn ? "ON" : "OFF";

        float targetX = isOn ? onX : offX;
        sliderThumb.anchoredPosition = new Vector2(targetX, sliderThumb.anchoredPosition.y);
    }
}
