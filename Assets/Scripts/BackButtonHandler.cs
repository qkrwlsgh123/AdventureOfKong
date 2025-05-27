// Assets/Scripts/BackButtonHandler.cs

using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButtonHandler : MonoBehaviour
{
    void Update()
    {
        // Android �ڷΰ��� �Ǵ� ������ ESC Ű ����
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // ���� Ȱ��ȭ�� �� �̸� ��ȸ
            string currentScene = SceneManager.GetActiveScene().name;

            switch (currentScene)
            {
                case "MapSelection":
                    // �� ���� ȭ�鿡���� ���� �޴��� ���ư�
                    SceneManager.LoadScene("MainMenu");
                    break;

                case "StageSelection":
                    // �������� ���� ȭ�鿡���� �� ��������
                    SceneManager.LoadScene("MapSelection");
                    break;

                // MainMenu �������� QuitConfirmation �г��� ����ϹǷ� ó������ ����
                // ���� ���� �÷��� �������� �ƹ� ���� ���� ���õ˴ϴ�
                default:
                    break;
            }
        }
    }
}
