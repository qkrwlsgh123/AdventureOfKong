using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HealthDisplay : MonoBehaviour
{
    [Tooltip("BallController가 붙은 콩 오브젝트")]
    public BallController player;

    [Tooltip("HealthUI 하트 Image 컴포넌트 리스트 (10개)")]
    public List<Image> hearts;

    void Update()
    {
        int hp = player.CurrentHealth;  // 이제 정확히 3부터 시작합니다
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].gameObject.SetActive(i < hp);
        }
    }
}
