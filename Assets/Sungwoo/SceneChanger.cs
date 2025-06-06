using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // �� �Լ��� ��ư�� ������ ����
    public void ChangeScene(string sceneName)
    {
        DontDes.previousSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
    }
}

