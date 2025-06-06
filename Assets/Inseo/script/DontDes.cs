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

        // �̺�Ʈ �ߺ� ��� ����
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    private void OnActiveSceneChanged(Scene oldScene, Scene newScene)
    {
        //previousSceneName = oldScene.name;
        Debug.Log($"�� ����: ���� �� = {previousSceneName}, �� �� = {newScene.name}");
    }

    //���� ���� ������ �Ѿ�� ����
    public void ChangeScene()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Volume");//VolumeTag ��� ����
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