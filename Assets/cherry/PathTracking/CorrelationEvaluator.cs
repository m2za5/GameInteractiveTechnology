using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CorrelationEvaluator : MonoBehaviour
{
    public Transform target;
    public GazeSimulator mouseSimulator;
    public TobiiGazeTest gazeSimulator;
    public RectTransform pointerUI;
    public Canvas canvas;

    private List<Vector2> gazePath = new();
    private List<Vector2> objectPath = new();
    public int windowSize = 60;
    public TextMeshProUGUI scoreText; // ← UI 텍스트 연결용

    public float EvaluateCorrelation()
    {
        Vector2 screenVec = gazeSimulator.GetGazePoint(true);
       // Vector2 gazeVec = gazeSimulator.GetGazePoint(false);
        Vector2 gazeVec = mouseSimulator.GetGazeScreenPosition();
        if(gazeVec != new Vector2(0, 0))
        {
            //pointerUI.anchoredPosition = screenVec;
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
            float dist = Vector2.Distance(gazePath[i], objectPath[i]);
           //score += 1f / (1f + dist); // 가까울수록 높은 점수
            score += Mathf.Exp(-dist * dist / 5000f);
            
        }
        scoreText.text = $"Score: {(score / gazePath.Count * 100f):F1} %";
        return score / gazePath.Count;
    }
}