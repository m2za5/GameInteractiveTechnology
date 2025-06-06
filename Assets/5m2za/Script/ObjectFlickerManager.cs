using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.GameIntegration.Net;
using System;
using System.Runtime.InteropServices;

public class ObjectFlickerManager : MonoBehaviour
{
    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    public MushroomFlicker flicker;
    public Transform targetObject;
    public Camera eyeCamera;
    public float detectionThreshold = 100f;
    public float maxWaitTime = 5f;
    public RectTransform targetUI;
    public RectTransform gazePointer;
    public RectTransform mushroomRect;
    public RectTransform canvasRect;
    private float[] brightnessLevels = { 0.005f, 0.01f, 0.015f, 0.02f, 0.03f, 0.04f, 0.05f, 0.2f };
    private int low = 0;
    private int high;
    private int current;

    void Start()
    {
        TobiiGameIntegrationApi.SetApplicationName("Gaze Brightness Test");
    }

    void OnEnable()
    {
        if (!this.isActiveAndEnabled) return;

        StartCoroutine(InitAndRunTest());
    }

    void SetRandomPosition()
    {
        float x = UnityEngine.Random.Range(0.2f, 0.8f);
        float y = UnityEngine.Random.Range(0.2f, 0.8f);

        Vector2 anchor = new Vector2(x, y);
        mushroomRect.anchorMin = mushroomRect.anchorMax = anchor;
        mushroomRect.anchoredPosition = Vector2.zero;

        if (targetUI != null)
        {
            targetUI.anchorMin = targetUI.anchorMax = anchor;
            targetUI.anchoredPosition = Vector2.zero;
        }
    }


    IEnumerator InitAndRunTest()
    {
        yield return new WaitForSeconds(1f);

        IntPtr hwnd = GetForegroundWindow();
        TobiiGameIntegrationApi.SetApplicationName("Gaze Brightness Test");
        TobiiGameIntegrationApi.TrackWindow(hwnd);

        high = brightnessLevels.Length - 1;
        current = (low + high) / 2;

        StartCoroutine(RunBrightnessTest());
    }

    IEnumerator RunBrightnessTest()
    {

        while (low <= high)
        {
            SetRandomPosition();

            float alpha = brightnessLevels[current];
            flicker.SetFlickerAlpha(alpha);
            Debug.Log($"[TEST] alpha {alpha}");

            bool looked = false;
            float lookedTime = 0f;
            float timer = 0f;


            while (timer < maxWaitTime)
            {
                timer += Time.deltaTime;

                if (IsLookingAtTarget())
                {
                    lookedTime += Time.deltaTime;

                    if (lookedTime >= 1f)
                    {
                        looked = true;
                        break;
                    }
                }
                else
                {
                    lookedTime = 0f;
                }

                yield return null;
            }

            if (looked)
            {
                Debug.Log($"봤음: alpha {alpha} → 더 어둡게 도전");
                high = current - 1;
            }
            else
            {
                Debug.Log($"못 봄: alpha {alpha} → 더 밝게 도전");
                low = current + 1;
            }

            current = (low + high) / 2;
            yield return new WaitForSeconds(1f);
        }

        float resultAlpha = brightnessLevels[Mathf.Clamp(current, 0, brightnessLevels.Length - 1)];
        Debug.Log($"최종 인식 가능한 최소 밝기 = alpha {resultAlpha}");
    }


    void FixedUpdate()
    {
        TobiiGameIntegrationApi.Update();
    }

    bool IsLookingAtTarget()
    {
        GazePoint gazePoint;
        if (!TobiiGameIntegrationApi.TryGetLatestGazePoint(out gazePoint))
        {
            Debug.Log("GazePoint 없음");
            return false;
        }

        float screenX = (gazePoint.X + 1f) * 0.5f * Screen.width;
        float screenY = (gazePoint.Y + 1f) * 0.5f * Screen.height;
        Vector2 gazeScreenPos = new Vector2(screenX, screenY);

        if (gazePointer != null)
            gazePointer.position = gazeScreenPos;

        bool hit = RectTransformUtility.RectangleContainsScreenPoint(targetUI, gazeScreenPos);
        Debug.Log($"GazePos: {gazeScreenPos} | Hit UI: {hit}");

        return hit;
    }

}