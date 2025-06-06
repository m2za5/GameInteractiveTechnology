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

    public Vector2 GetGazePoint(bool isForScreen)
    {
        TobiiGameIntegrationApi.Update();
        GazePoint gazePoint;
        Vector2 gazeVec;

        if (TobiiGameIntegrationApi.TryGetLatestGazePoint(out gazePoint))
        {
            float screenX = (gazePoint.X + 1f) * 0.5f * Screen.width;
            float screenY = (gazePoint.Y + 1f) * 0.5f * Screen.height;

            // UI 좌표로 이동
            gazeVec = new Vector2(screenX, screenY);

            return gazeVec;
        }
        else
        {
            Debug.LogWarning("Gaze 데이터를 가져오지 못함 (TryGetLatestGazePoint 실패)");
            gazeVec = new Vector2(0, 0);
            return gazeVec;
        }
    }

}