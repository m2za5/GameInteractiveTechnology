using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // 이 함수는 버튼에 연결할 예정
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

