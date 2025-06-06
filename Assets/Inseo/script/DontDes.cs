using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDes : MonoBehaviour
{
    private static DontDes instance;
    public static string previousSceneName = "";

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        // 이벤트 중복 등록 방지
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    private void OnActiveSceneChanged(Scene oldScene, Scene newScene)
    {
        //previousSceneName = oldScene.name;
        Debug.Log($"씬 변경: 이전 씬 = {previousSceneName}, 새 씬 = {newScene.name}");
    }

    //다음 설정 씬으로 넘어가기 위함
    public void ChangeScene()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Volume");//VolumeTag 모두 삭제
        foreach (var obj in objs)
        {
            Destroy(obj);
        }
        SceneManager.LoadScene(SceneChanger.currSceneNum);
    }

    void OnDestroy()
    {
        if (instance == this)
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    }
}