using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CorrelationEvaluator : MonoBehaviour
{
    [Header("GazeSimulator")]
    public Transform target;
    public GazeSimulator mouseSimulator;
    public TobiiGazeTest gazeSimulator;
    public int windowSize = 60;

    [Header("UI")]
    public RectTransform pointerUI;
    public TextMeshProUGUI scoreText; // ← UI 텍스트 연결용

    private List<Vector2> gazePath = new();
    private List<Vector2> objectPath = new();

    public float EvaluateCorrelation()
    {
        Vector2 gazeVec = gazeSimulator.GetGazePoint(false);
        //Vector2 gazeVec = mouseSimulator.GetGazeScreenPosition();
        if(gazeVec != new Vector2(0, 0))
        {
            pointerUI.position = gazeVec;
        }
        Vector2 obj = Camera.main.WorldToScreenPoint(target.position);

        gazePath.Add(gazeVec);
        objectPath.Add(obj);

        if (gazePath.Count > windowSize)
        {
            gazePath.RemoveAt(0);
            objectPath.RemoveAt(0);
        }

        if (gazePath.Count < windowSize) return 0f;

        float score = 0f;
        for (int i = 0; i < gazePath.Count; i++)
        {
            float maxDist = 500;
            float dist = Vector2.Distance(gazePath[i], objectPath[i]);
            float contribution = Mathf.Pow(Mathf.Clamp01(1f - (dist / maxDist)), 0.5f);
          //  float contribution = Mathf.Clamp01(1f - (dist / maxDist)); // 0~1 선형 감소
            score += contribution;
            //score += Mathf.Exp(-dist * dist / 20000f);

        }

        score = score / gazePath.Count * 200f;
        scoreText.text = $"Score: {(score):F1} %";

        return score;
    }
}