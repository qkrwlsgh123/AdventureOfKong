using UnityEngine;

public class TreeBlockGrow : MonoBehaviour
{
    public GameObject[] growStages;     // 성장 단계별 오브젝트 배열
    public float growInterval = 2f;     // 자라는 시간 간격
    public Rigidbody2D rb;

    private int currentStage = 0;
    private float timer = 0f;
    private bool isGrowing = false;

    void Start()
    {
        if (growStages.Length == 0) return;

        // 모든 단계를 비활성화하고 첫 단계만 보이게
        for (int i = 0; i < growStages.Length; i++)
        {
            growStages[i].SetActive(i == 0);
        }

        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic; // ✅ 최신 방식으로 수정
    }

    void Update()
    {
        if (!isGrowing) return;

        timer += Time.deltaTime;
        if (timer >= growInterval && currentStage < growStages.Length - 1)
        {
            timer = 0f;
            currentStage++;

            for (int i = 0; i < growStages.Length; i++)
            {
                growStages[i].SetActive(i == currentStage);
            }

            if (currentStage == growStages.Length - 1)
            {
                isGrowing = false;
            }
        }
    }

    public void StartGrowing()
    {
        isGrowing = true;
        timer = 0f;
    }
}
