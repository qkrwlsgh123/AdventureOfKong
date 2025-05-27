using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class QuitConfirmation : MonoBehaviour
{
    [Header("QuitConfirmPanel (CanvasGroup)")]
    public CanvasGroup panelCG;      // QuitConfirmPanel의 CanvasGroup 연결

    [Header("버튼 연결")]
    public Button btnQuit;
    public Button btnCancel;

    private bool isPanelVisible = false;

    void Awake()
    {
        // CanvasGroup이 Inspector에 연결되지 않았으면 자동 검색
        if (panelCG == null)
            panelCG = GetComponent<CanvasGroup>();
        if (panelCG == null)
        {
            var go = GameObject.Find("QuitConfirmPanel");
            if (go != null)
                panelCG = go.GetComponent<CanvasGroup>();
        }
        if (panelCG == null)
            Debug.LogError("[QuitConfirmation] panelCG 연결 실패! QuitConfirmPanel에 CanvasGroup이 필요합니다.");

        // 초기에는 숨김 상태
        panelCG.alpha = 0f;
        panelCG.interactable = false;
        panelCG.blocksRaycasts = false;
        isPanelVisible = false;

        // 버튼 이벤트 연결
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
