using UnityEngine;
using System;
using System.Runtime.InteropServices;
using Tobii.GameIntegration.Net;

public class TobiiGazeTest : MonoBehaviour
{
    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    private const float updateInterval = 1f / 60f;
    private float timer;

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
        Debug.Log("TrackWindow 호출됨: " + hwnd);

        var tracker = TobiiGameIntegrationApi.GetTrackerInfo();
        Debug.Log($"Type: {tracker.Type}, IsAttached: {tracker.IsAttached}, FriendlyName: {tracker.FriendlyName}");

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
            Debug.Log($"Gaze Point: ({gazePoint.X:F2}, {gazePoint.Y:F2})");
        }
        else
        {
            Debug.LogWarning("Gaze 데이터를 가져오지 못함 (TryGetLatestGazePoint 실패)");
        }

        // ESC 눌렀을 때 종료
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TobiiGameIntegrationApi.Shutdown();
            Debug.Log("Tobii API Shutdown 완료");
        }
    }
}