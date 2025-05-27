using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class QuitConfirmation : MonoBehaviour
{
    [Header("QuitConfirmPanel (CanvasGroup)")]
    public CanvasGroup panelCG;      // QuitConfirmPanel�� CanvasGroup ����

    [Header("��ư ����")]
    public Button btnQuit;
    public Button btnCancel;

    private bool isPanelVisible = false;

    void Awake()
    {
        // CanvasGroup�� Inspector�� ������� �ʾ����� �ڵ� �˻�
        if (panelCG == null)
            panelCG = GetComponent<CanvasGroup>();
        if (panelCG == null)
        {
            var go = GameObject.Find("QuitConfirmPanel");
            if (go != null)
                panelCG = go.GetComponent<CanvasGroup>();
        }
        if (panelCG == null)
            Debug.LogError("[QuitConfirmation] panelCG ���� ����! QuitConfirmPanel�� CanvasGroup�� �ʿ��մϴ�.");

        // �ʱ⿡�� ���� ����
        panelCG.alpha = 0f;
        panelCG.interactable = false;
        panelCG.blocksRaycasts = false;
        isPanelVisible = false;

        // ��ư �̺�Ʈ ����
        btnQuit.onClick.AddListener(OnQuit);
        btnCancel.onClick.AddListener(Hide);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPanelVisible)
                Hide();
            else
                Show();
        }
    }

    public void Show()
    {
        panelCG.alpha = 1f;
        panelCG.interactable = true;
        panelCG.blocksRaycasts = true;
        isPanelVisible = true;
        EventSystem.current.SetSelectedGameObject(btnCancel.gameObject);
    }

    public void Hide()
    {
        panelCG.alpha = 0f;
        panelCG.interactable = false;
        panelCG.blocksRaycasts = false;
        isPanelVisible = false;
    }

    void OnQuit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
