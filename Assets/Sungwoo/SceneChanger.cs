using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SceneChanger : MonoBehaviour
{
    public static int currSceneNum=0;
    public Volume volume;
    private ColorAdjustments colorAdjustments;

    // �� �Լ��� ��ư�� ������ ����
    public void ChangeScene(string sceneName)
    {
        DontDes.previousSceneName = SceneManager.GetActiveScene().name;
        SceneChanger.currSceneNum ++;
        SceneManager.LoadScene(sceneName);

        // volume�� ����
        if (volume.profile.TryGet(out colorAdjustments))
        {
            // �ʱ�ȭ �� ���ϴ� ���Ⱚ�� ������ ���� ����
            colorAdjustments.postExposure.value += 4f;
        }
    }
}

