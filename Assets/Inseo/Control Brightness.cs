using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ControlBrightness : MonoBehaviour
{
    public Volume volume;
    private ColorAdjustments colorAdjustments;

    private float step = 0;
    public float max_step = 50f;
    public float min_brightness = -7f;
    public float max_brightness = 4f;

    private float bt;
    void Start()
    {
        //scene 넘어가도 남아있어야함
        DontDestroyOnLoad(gameObject);

        bt = min_brightness;
        // Color Adjustments 효과를 Profile에서 찾기
        if (volume.profile.TryGet(out colorAdjustments))
        {
            // 초기화 시 원하는 노출값을 설정할 수도 있음
            SetExposure(0f); // 예시: 0EV로 초기화
        }
        else
        {
            Debug.LogError("Color Adjustments 효과가 Volume Profile에 추가되어 있는지 확인하세요.");
        }
    }

    // 노출값을 실시간으로 변경
    public void SetExposure(float exposureValue)
    {
        if (colorAdjustments != null)
        {
            colorAdjustments.postExposure.value = exposureValue;
        }
    }

    private void Update()
    {
        step += 1;
        if(step == max_step)
        {
            bt += 0.1f;
            SetExposure(bt);
            if (bt >= max_brightness)
            {
                bt = min_brightness;
            }
            step = 0;
        }
    }
}
