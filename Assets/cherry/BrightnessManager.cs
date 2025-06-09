using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine;

public class BrightnessManager : MonoBehaviour
{
    public static BrightnessManager Instance { get; private set; }

    public Volume volume;
    private ColorAdjustments colorAdjustments;

    private float step = 0;
    public float max_step = 50f;
    public float min_brightness = -7f;
    public float target_brightness = 4f;
    public float step_brightness = 0.2f;
    public bool isFirstTest = true;

    public float lastExposureBT = 0f;
    public float currentBT;

    /*
    
    void Awake()
    {
        // ✅ 싱글톤 중복 방지
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    */
    void Start()
    {
        volume = GameObject.FindGameObjectWithTag("Volume").GetComponent<Volume>();
        currentBT = target_brightness;
        //SetExposure(currentBT);

        // Color Adjustments 효과를 Profile에서 찾기
        if (volume.profile.TryGet(out colorAdjustments))
        {
            // 초기화 시 원하는 노출값을 설정할 수도 있음
            SetExposure(currentBT); // 예시: 0EV로 초기화
        }
        else
        {
            Debug.LogError("Color Adjustments 효과가 Volume Profile에 추가되어 있는지 확인하세요.");
        }
    }

    public void SetFinalExposure()
    {
        currentBT = lastExposureBT;
        SetExposure(currentBT);
    }

    public void IncreaseExposure()
    {
        lastExposureBT = currentBT;
        currentBT += step_brightness;
        SetExposure(currentBT);
    }

    public void DecreaseExposure()
    {
        lastExposureBT = currentBT;
        currentBT -= step_brightness;
        SetExposure(currentBT);
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
        //    if (isFirstTest)
        //    {
        //        step += 1;
        //        if (step == max_step)
        //        {
        //            currentBT += 0.1f;
        //            SetExposure(currentBT);
        //            if (currentBT >= target_brightness)
        //            {
        //                currentBT = min_brightness;
        //            }
        //            step = 0;
        //        }

        //    }
        //}
    }
}
