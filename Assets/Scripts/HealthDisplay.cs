using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HealthDisplay : MonoBehaviour
{
    [Tooltip("BallController�� ���� �� ������Ʈ")]
    public BallController player;

    [Tooltip("HealthUI ��Ʈ Image ������Ʈ ����Ʈ (10��)")]
    public List<Image> hearts;

    void Update()
    {
        int hp = player.CurrentHealth;  // ���� ��Ȯ�� 3���� �����մϴ�
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].gameObject.SetActive(i < hp);
        }
    }
}
