using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


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
    public GameObject nextButtonUI; // ← UI 텍스트 연결용
    public GameObject startButtonUI; // ← UI 텍스트 연결용
    public GameObject retryButtonUI; // ← UI 텍스트 연결용


    private BrightnessManager brightnessManager;
    private int successCount = 0;
    private int failureCount = 0;
    private int frameCount = 0;
    private float cooldownTime = 1.0f; // ⏱ 밝기 낮춘 뒤 재검사까지 대기 시간
    private float cooldownTimer = 0f;
    private bool isCoolingDown = false;
    private bool isStarted = false;


    private void Start()
    {
        brightnessManager = BrightnessManager.Instance;
    }

    void Update()
    {
        if (!isStarted) return;

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

        if(frameCount == 240)
        {
            if (successCount >= successFrames) // 인식된 순간
            {
                Debug.Log($"✅ 인식됨! (연속 {successFrames} 프레임)");
                brightnessManager.DecreaseExposure();  // 밝기 낮추기
                brightnessManager.isFirstTest = false;

                // 쿨다운 진입
                isCoolingDown = true;
                cooldownTimer = cooldownTime;

                successCount = 0;
                frameCount = 0;
            }
            else if (successCount < failureFrames)  // 인식 실패
            {
               // finalBrightText.text = $"FinalBright: {(brightnessManager.lastExposureBT):F1} %";
                nextButtonUI.SetActive(true);
                retryButtonUI.SetActive(true);

                Debug.Log($"❌ 인식 실패 지속됨 → frameCount : " + frameCount);
                brightnessManager.SetFinalExposure();
                failureCount = 0;
            }

            successCount = 0;
            failureCount = 0;
            frameCount = 0;

        }

        if (score > successScore)
        {
            successCount++;
           
        }
        else if (score < failureScore)
        {
            failureCount++;

        }
        frameCount++;
    }

    public void StartDetection()
    {
        isStarted = true;
        successCount = 0;
        failureCount = 0;
        frameCount = 0;
        isCoolingDown = false;
        Debug.Log("🟢 감지 시작됨");

        startButtonUI.SetActive(false);
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}