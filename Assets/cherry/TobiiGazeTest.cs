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

    public RectTransform uiElement; // 따라다닐 UI (예: 이미지, 버튼 등)
    public Canvas canvas;              // 에디터에서 할당 필요
    public RectTransform canvasRect;

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
            if(isForScreen) gazeVec = ToScreenPoint(gazeVec);

            return gazeVec;
        }
        else
        {
            Debug.LogWarning("Gaze 데이터를 가져오지 못함 (TryGetLatestGazePoint 실패)");
            gazeVec = new Vector2(0, 0);
            return gazeVec;
        }
    }

    Vector2 ToScreenPoint(Vector2 screenGaze)
    {
        if (canvas != null && canvasRect != null)
        {
            bool success = RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect,
                screenGaze,
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
                out Vector2 localPoint
            );

            if (success)
            {
                return localPoint; // ✅ Canvas 좌표계로 변환된 좌표 리턴
            }
            else
            {
                Debug.LogWarning("💥 UI 좌표 변환 실패");
                return Vector2.zero;
            }
        }

        return screenGaze;
    }


    //void Update()
    //{
    //    timer += Time.deltaTime;
    //    if (timer < updateInterval) return;
    //    timer = 0f;

    //    TobiiGameIntegrationApi.Update();

    //    GazePoint gazePoint;
    //    if (TobiiGameIntegrationApi.TryGetLatestGazePoint(out gazePoint))
    //    {
    //        float screenX = (gazePoint.X + 1f) * 0.5f * Screen.width;
    //        float screenY = (gazePoint.Y + 1f) * 0.5f * Screen.height;

    //        // UI 좌표로 이동
    //        uiElement.position = new Vector3(screenX, screenY, 0f);

    //        Debug.Log($"UI 이동 위치: ({screenX}, {screenY})");
    //    }
    //    else
    //    {
    //        Debug.LogWarning("Gaze 데이터를 가져오지 못함 (TryGetLatestGazePoint 실패)");
    //    }

    //    // ESC 눌렀을 때 종료
    //    if (Input.GetKeyDown(KeyCode.Escape))
    //    {
    //        TobiiGameIntegrationApi.Shutdown();
    //        Debug.Log("Tobii API Shutdown 완료");
    //    }

    //}
}