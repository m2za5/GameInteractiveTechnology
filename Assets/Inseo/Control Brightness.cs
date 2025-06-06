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
        //scene �Ѿ�� �����־����
        DontDestroyOnLoad(gameObject);

        bt = min_brightness;
        // Color Adjustments ȿ���� Profile���� ã��
        if (volume.profile.TryGet(out colorAdjustments))
        {
            // �ʱ�ȭ �� ���ϴ� ���Ⱚ�� ������ ���� ����
            SetExposure(0f); // ����: 0EV�� �ʱ�ȭ
        }
        else
        {
            Debug.LogError("Color Adjustments ȿ���� Volume Profile�� �߰��Ǿ� �ִ��� Ȯ���ϼ���.");
        }
    }

    // ���Ⱚ�� �ǽð����� ����
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
