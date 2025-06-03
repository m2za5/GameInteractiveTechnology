using UnityEngine;
using System;
using System.Runtime.InteropServices;
using Tobii.GameIntegration.Net;
using UnityEngine.UI;
using TMPro;

public class MonsterTracker2 : MonoBehaviour
{
    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    public RectTransform gazePointer;
    public RectTransform[] monsterRects;
    public Outline[] monsterOutlines;
    public TextMeshProUGUI gazeLabel;

    private const float updateInterval = 1f / 60f;
    private float timer;
    private int currentIndex = -1;

    void Start()
    {
        Invoke("LateInitializeTobii", 1.0f);
        TobiiGameIntegrationApi.SetApplicationName("Gaze Sample");
        timer = 0f;
    }

    void LateInitializeTobii()
    {
        IntPtr hwnd = GetForegroundWindow();
        TobiiGameIntegrationApi.TrackWindow(hwnd);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer < updateInterval) return;
        timer = 0f;

        TobiiGameIntegrationApi.Update();

        GazePoint gazePoint;
        if (TobiiGameIntegrationApi.TryGetLatestGazePoint(out gazePoint))
        {
            float screenX = (gazePoint.X + 1f) * 0.5f * Screen.width;
            float screenY = (gazePoint.Y + 1f) * 0.5f * Screen.height;

            Vector2 gazeScreenPos = new Vector2(screenX, screenY);

            Vector2 offset = gazePointer.sizeDelta * 0.5f;

            if (gazePointer != null)
                gazePointer.position = gazeScreenPos - offset;//오프셋 추가

            int newIndex = -1;

            for (int i = 0; i < monsterRects.Length; i++)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(monsterRects[i], gazeScreenPos))
                {
                    newIndex = i;
                    break;
                }
            }

            for (int i = 0; i < monsterOutlines.Length; i++)
            {
                monsterOutlines[i].enabled = (i == newIndex);
            }

            if (newIndex != -1 && newIndex != currentIndex)
            {
                gazeLabel.text = "Gaze Monster : " + monsterRects[newIndex].name;
                currentIndex = newIndex;
            }
            else if (newIndex == -1)
            {
                gazeLabel.text = "Gaze Monster : ";
                currentIndex = -1;
            }
        }
    }
}
