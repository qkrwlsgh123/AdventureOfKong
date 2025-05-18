using UnityEngine;

public class GameOverUIScript : MonoBehaviour
{
    public GameObject gameOverText;    // ��Game Over�� �ؽ�Ʈ
    public GameObject restartButton;   // ���ٽ� ���ۡ� ��ư
    public GameObject gameOverPanel;   // ������ ��� �г�

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);   // ��� ���� Ȱ��ȭ
        gameOverText.SetActive(true);    // �ؽ�Ʈ Ȱ��ȭ
        restartButton.SetActive(true);   // ��ư Ȱ��ȭ
    }
}
