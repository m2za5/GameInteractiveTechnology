using UnityEngine;
using TMPro;

public class ThresholdDetector : MonoBehaviour
{
    [Header("Correlation 설정")]
    public CorrelationEvaluator evaluator;
    public float successScore = 90f;        // 인식 성공 판단 기준
    public int successFrames = 15;          // 성공 조건 프레임 수
    public float failureScore = 20f;        // 인식 실패 판단 기준
    public int failureFrames = 30;          // 실패 조건 프레임 수

    [Header("UI")]
    public TextMeshProUGUI finalBrightText; // ← UI 텍스트 연결용


    private BrightnessManager brightnessManager;
    private int successCount = 0;
    private int failureCount = 0;
    private float cooldownTime = 1.0f; // ⏱ 밝기 낮춘 뒤 재검사까지 대기 시간
    private float cooldownTimer = 0f;
    private bool isCoolingDown = false;


    private void Start()
    {
        brightnessManager = BrightnessManager.Instance;
    }

    void Update()
    {
        if (isCoolingDown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                isCoolingDown = false;
                successCount = 0;
                Debug.Log("⏳ 쿨다운 끝, 다시 감지 시작");
            }
            return;
        }

        float score = evaluator.EvaluateCorrelation();

        if (score > successScore)
        {
            successCount++;
            if (successCount >= successFrames) // 인식된 순간
            {
                Debug.Log($"✅ 인식됨! (연속 {successFrames} 프레임)");
                brightnessManager.DecreaseExposure();  // 밝기 낮추기
                brightnessManager.isFirstTest = false;

                // 쿨다운 진입
                isCoolingDown = true;
                cooldownTimer = cooldownTime;

                //findText.SetActive(true);
            }
        }
        else if (score < failureScore && !brightnessManager.isFirstTest)
        {
            failureCount++;
            successCount = 0;

            if (failureCount >= failureFrames)
            {
                finalBrightText.text = $"FinalBright: {(brightnessManager.lastExposureBT):F1} %";

                Debug.Log($"❌ 인식 실패 지속됨 → 밝기 다시 증가");
                //brightnessManager.SetFinalExposure();
                brightnessManager.SetFinalExposure();
                failureCount = 0;
            }
        }
        else
        {
            successCount = 0;
        }
    }


}