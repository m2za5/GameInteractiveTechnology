using UnityEngine;
using UnityEngine.UI;

public class ThresholdDetector : MonoBehaviour
{
    public CanvasGroup stimulusCanvasGroup;
    public CorrelationEvaluator evaluator;
    public float correlationThreshold = 0.85f;
    public float brightnessIncreaseSpeed = 0.2f;


    private bool hasDetected = false;

    void Update()
    {
        // 밝기 증가 (선택)
        //stimulusCanvasGroup.alpha = Mathf.Clamp01(stimulusCanvasGroup.alpha + Time.deltaTime * brightnessIncreaseSpeed);

        float score = evaluator.EvaluateCorrelation();

        // 실시간 UI 표시
        //if (scoreText != null)
        //{
        //    scoreText.text = $"Score: {(score * 100f):F1} %";
        //}

        //if (!hasDetected && score > correlationThreshold)
        //{
        //    float threshold = stimulusCanvasGroup.alpha;
        //    Debug.Log($"✅ 인식됨! 밝기 역치: {threshold:F2}");
        //    hasDetected = true;
        //}
    }
}