using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using TMPro;

public class BrightnessController : MonoBehaviour
{
    public Volume volume;               // Volume 오브젝트 연결
    public Slider brightnessSlider;     // UI 슬라이더 연결
    public TMP_Text BrightnessValue; // UI 텍스트 연결
    private ColorAdjustments colorAdjustments;
    
    private static BrightnessController instance;

    private const string BrightnessKey = "Brightness_manual"; // 저장 키

    private Color normalTextColor = Color.white;
    private Color highExposureColor = Color.black;

    void Awake()
    {
        // 중복 방지: 이미 존재하는 인스턴스가 있다면 제거
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // 이 오브젝트는 씬 전환 시 파괴되지 않음
    }

    void Start()
    {
        // 볼륨에서 ColorAdjustments 추출
       
        if (volume.profile.TryGet(out colorAdjustments))
        {
            float savedValue = PlayerPrefs.GetFloat(BrightnessKey, 0f);
            brightnessSlider.value = savedValue;
            SetBrightness(savedValue);

            // 슬라이더 변경 시 SetBrightness 호출
            brightnessSlider.onValueChanged.AddListener(SetBrightness);
        }
        else
        {
            Debug.LogWarning("Color Adjustments 설정이 Volume에 없습니다!");
        }
    }

    public void SetBrightness(float value)
    {
        if (colorAdjustments != null)
        {
            Debug.Log("밝기 조절 중: " + value);
            colorAdjustments.postExposure.value = value;
            PlayerPrefs.SetFloat(BrightnessKey, value);

            if (BrightnessValue != null)
            {
                BrightnessValue.text = $"Exposure: {value:F2}";

                // 텍스트 색상 변경
                if (value >= 2f)
                {
                    BrightnessValue.color = highExposureColor;
                }
                else
                {
                    BrightnessValue.color = normalTextColor;
                }
            }
        }
    }
}