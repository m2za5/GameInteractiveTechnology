using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LoadBrightness : MonoBehaviour
{
    public Volume volume;
    private ColorAdjustments colorAdjustments;

    private const string BrightnessKey = "Brightness_manual";

    void Awake()
    {
        if (volume.profile.TryGet(out colorAdjustments))
        {
            float savedValue = PlayerPrefs.GetFloat(BrightnessKey, 0f);
            colorAdjustments.postExposure.value = savedValue;
        }
        else
        {
            Debug.LogWarning("Color Adjustments가 Volume에 없습니다!");
        }
    }
}