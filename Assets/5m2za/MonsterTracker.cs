﻿using UnityEngine;
using System;
using System.Runtime.InteropServices;
using Tobii.GameIntegration.Net;
using UnityEngine.UI;

public class MonsterTracker : MonoBehaviour
{
    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    public RectTransform gazePointer;
    public RectTransform[] monsterRects;
    public Outline[] monsterOutlines;
    public Text gazeLabel;

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
    void OnEnable()
    {
        LateInitializeTobii();
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            Debug.Log("[MonsterTracker] 앱 포커스 복귀");
            LateInitializeTobii();
        }
    }

    void Update()
    {
        timer += Time.unscaledDeltaTime;
        if (timer < 1f / 60f) return;
        timer = 0f;

        TobiiGameIntegrationApi.Update();

        GazePoint gazePoint;
        if (TobiiGameIntegrationApi.TryGetLatestGazePoint(out gazePoint))
        {
            float screenX = (gazePoint.X + 1f) * 0.5f * Screen.width;
            float screenY = (gazePoint.Y + 1f) * 0.5f * Screen.height;

            Vector2 gazeScreenPos = new Vector2(screenX, screenY);

            if (gazePointer != null)
                gazePointer.position = gazeScreenPos;

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
                monsterOutlines[i].enabled = (i == newIndex);

            if (newIndex != -1 && newIndex != currentIndex)
            {
                gazeLabel.text = "응시 중인 몬스터: " + monsterRects[newIndex].name;
                currentIndex = newIndex;
            }
            else if (newIndex == -1)
            {
                gazeLabel.text = "응시 중인 몬스터: 없음";
                currentIndex = -1;
            }
        }
        else
        {
            if (Time.frameCount % 60 == 0)
                Debug.LogWarning("Gaze 데이터를 가져오지 못함 (TryGetLatestGazePoint 실패)");
        }
    }
}

