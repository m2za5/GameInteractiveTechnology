using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public static int currSceneNum=0;
    // �� �Լ��� ��ư�� ������ ����
    public void ChangeScene(string sceneName)
    {
        DontDes.previousSceneName = SceneManager.GetActiveScene().name;
        SceneChanger.currSceneNum ++;
        SceneManager.LoadScene(sceneName);
    }
}

