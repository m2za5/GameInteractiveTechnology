using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using TMPro;

public class BrightnessController : MonoBehaviour
{
    public Volume volume;               // Volume ������Ʈ ����
    public Slider brightnessSlider;     // UI �����̴� ����
    public TMP_Text BrightnessValue; // UI �ؽ�Ʈ ����
    private ColorAdjustments colorAdjustments;
    
    private static BrightnessController instance;

    private const string BrightnessKey = "Brightness_manual"; // ���� Ű

    private Color normalTextColor = Color.white;
    private Color highExposureColor = Color.black;

    void Awake()
    {
        // �ߺ� ����: �̹� �����ϴ� �ν��Ͻ��� �ִٸ� ����
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // �� ������Ʈ�� �� ��ȯ �� �ı����� ����
    }

    void Start()
    {
        // �������� ColorAdjustments ����
       
        if (volume.profile.TryGet(out colorAdjustments))
        {
            float savedValue = PlayerPrefs.GetFloat(BrightnessKey, 0f);
            brightnessSlider.value = savedValue;
            SetBrightness(savedValue);

            // �����̴� ���� �� SetBrightness ȣ��
            brightnessSlider.onValueChanged.AddListener(SetBrightness);
        }
        else
        {
            Debug.LogWarning("Color Adjustments ������ Volume�� �����ϴ�!");
        }
    }

    public void SetBrightness(float value)
    {
        if (colorAdjustments != null)
        {
            Debug.Log("��� ���� ��: " + value);
            colorAdjustments.postExposure.value = value;
            PlayerPrefs.SetFloat(BrightnessKey, value);

            if (BrightnessValue != null)
            {
                BrightnessValue.text = $"Exposure: {value:F2}";

                // �ؽ�Ʈ ���� ����
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