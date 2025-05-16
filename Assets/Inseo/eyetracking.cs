using UnityEngine;
using UnityEngine.UI;
using System;
using System.Runtime.InteropServices;
using Tobii.GameIntegration.Net;

public class GazeVisualizer : MonoBehaviour
{
    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();

    public RectTransform gazePointerUI;

    void Start()
    {
        TobiiGameIntegrationApi.SetApplicationName("Gaze Sample");

        // 1�� ���� �� �ʱ�ȭ
        Invoke(nameof(LateInitializeTobii), 1.0f);
    }

    void LateInitializeTobii()
    {
        IntPtr hwnd = GetForegroundWindow();
        TobiiGameIntegrationApi.TrackWindow(hwnd);
        Debug.Log("TrackWindow ȣ���: " + hwnd);

        var tracker = TobiiGameIntegrationApi.GetTrackerInfo();
        Debug.Log($"Type: {tracker.Type}, IsAttached: {tracker.IsAttached}, FriendlyName: {tracker.FriendlyName}");
    }

    void Update()
    {
        TobiiGameIntegrationApi.Update();

        if (TobiiGameIntegrationApi.TryGetLatestGazePoint(out GazePoint gazePoint))
        {
            float gazeX = gazePoint.X / Screen.width;
            float gazeY = gazePoint.Y / Screen.height;

            // ����� ���� �þ� ���� (���� ����)
            float minX = 0.3f, maxX = 0.7f;
            float minY = 0.3f, maxY = 0.7f;

            float Remap(float value, float from1, float to1, float from2, float to2)
            {
                return Mathf.Clamp01((value - from1) / (to1 - from1)) * (to2 - from2) + from2;
            }

            // ȭ�� ��ǥ�� ����
            float screenX = Remap(gazeX, minX, maxX, 0, Screen.width);
            float screenY = Remap(gazeY, minY, maxY, 0, Screen.height); // Y�� �״�� ���

            if (gazePointerUI != null)
            {
                // UI ��ǥ�� ��ȯ
                RectTransform canvasRect = gazePointerUI.parent.GetComponent<RectTransform>();
                Vector2 localPoint;

                // ��ũ�� ��ǥ �� Canvas local ��ǥ ��ȯ
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvasRect, new Vector2(screenX, screenY), null, out localPoint
                );

                gazePointerUI.anchoredPosition = localPoint;

                if (!gazePointerUI.gameObject.activeSelf)
                    gazePointerUI.gameObject.SetActive(true);
            }

            Debug.Log($"Gaze Point (norm): ({gazeX:F2}, {gazeY:F2}) �� screen: ({screenX}, {screenY})");
        }
        else
        {
            // �����͸� �� �޴��� �����͸� ����
            Debug.LogWarning("Gaze �����͸� �� ���� (TryGetLatestGazePoint ����)");
        }
    }
}