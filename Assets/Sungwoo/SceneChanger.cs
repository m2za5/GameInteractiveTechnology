using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SceneChanger : MonoBehaviour
{
    public static int currSceneNum=0;
    public Volume volume;
    private ColorAdjustments colorAdjustments;

    // 이 함수는 버튼에 연결할 예정
    public void ChangeScene(string sceneName)
    {
        DontDes.previousSceneName = SceneManager.GetActiveScene().name;
        SceneChanger.currSceneNum ++;
        SceneManager.LoadScene(sceneName);

        // volume값 조정
        if (volume.profile.TryGet(out colorAdjustments))
        {
            // 초기화 시 원하는 노출값을 설정할 수도 있음
            colorAdjustments.postExposure.value += 4f;
        }
    }
}

