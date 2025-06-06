using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public static int currSceneNum=0;
    // 이 함수는 버튼에 연결할 예정
    public void ChangeScene(string sceneName)
    {
        DontDes.previousSceneName = SceneManager.GetActiveScene().name;
        SceneChanger.currSceneNum ++;
        SceneManager.LoadScene(sceneName);
    }
}

