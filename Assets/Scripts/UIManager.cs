using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    /// <summary>
    /// ���� �޴����� ������ ���ۡ� ��ư Ŭ�� �� ȣ��
    /// </summary>
    public void ShowMapSelection()
    {
        SceneManager.LoadScene("MapSelection");
    }

    /// <summary>
    /// ���� �޴����� �������� ��ư Ŭ�� �� ȣ��
    /// </summary>
    public void ShowSettings()
    {
        // ���� UI �˾� �Ǵ� �� ��ȯ ������ ���⿡ �߰��ϼ���
        Debug.Log("Settings clicked");
    }

    /// <summary>
    /// MapSelection ȭ�鿡�� �� ��ư Ŭ�� �� ȣ��
    /// </summary>
    /// <param name="mapIndex">���õ� ���� �ε��� (0����)</param>
    public void ShowStageSelection(int mapIndex)
    {
        // �ʿ��ϴٸ� ���õ� �� �ε����� ����
        PlayerPrefs.SetInt("SelectedMap", mapIndex);
        SceneManager.LoadScene("StageSelection");
    }

    /// <summary>
    /// StageSelection ȭ�鿡�� �������� ��ư Ŭ�� �� ȣ��
    /// </summary>
    /// <param name="stageIndex">���õ� �������� ��ȣ (1~30)</param>
    public void LoadLevel(int stageIndex)
    {
        // ������ ������ �� �ε����� �ҷ��ɴϴ� (������ 0)
        int mapIndex = PlayerPrefs.GetInt("SelectedMap", 0);
        // �� �̸��� "Map{mapIndex}_Stage{stageIndex}" �������� ����
        string sceneName = $"Map{mapIndex}_Stage{stageIndex}";
        SceneManager.LoadScene(sceneName);
    }
}
