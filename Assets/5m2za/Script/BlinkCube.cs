using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using Tobii.GameIntegration.Net;

public class BlinkCube : MonoBehaviour
{
    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();


    public GameObject[] Cubes;
    public float blinkSpeed = 0.1f;
    public float blinkDuration = 1f;
    public float testDuration = 3f;
    public float stepDuration = 1f;
    public RectTransform gazePointer;

    private int successfulTests = 0;
    private int failedTests = 0;
    private int totalTests = 3;
    private int totalSteps = 3;

    private int currentStep = 0;

    public Camera mainCamera;
    public LayerMask cubeLayer;
    public RectTransform cursorUI;

    private bool isLookingAtCube = false;
    private float timeSpentLookingAtCube = 0f;

    void Start()
    {
        foreach (GameObject cube in Cubes)
        {
            if (cube != null)
            {
                cube.SetActive(false);
            }
        }

        Invoke("LateInitializeTobii", 1.0f);
        TobiiGameIntegrationApi.SetApplicationName("Gaze Sample");
        //timer = 0f;

        StartCoroutine(ActivateNextCube());
    }

    void LateInitializeTobii()
    {
        IntPtr hwnd = GetForegroundWindow();
        TobiiGameIntegrationApi.TrackWindow(hwnd);
    }

    IEnumerator ActivateNextCube()
    {
        while (successfulTests + failedTests < totalTests)
        {
            foreach (GameObject cube in Cubes)
            {
                cube.SetActive(false);
            }

            bool testFailed = false;

            for (currentStep = 1; !testFailed; currentStep++)
            {
                Debug.Log($"단계 {currentStep} 시작");

                int randomIndex = UnityEngine.Random.Range(0, Cubes.Length);
                GameObject selectedCube = Cubes[randomIndex];
                selectedCube.SetActive(true);

                yield return StartCoroutine(Blinking(selectedCube));

                bool passedStep = false;
                yield return StartCoroutine(RunTest(testResult =>
                {
                    passedStep = testResult;
                }));

                if (passedStep)
                {
                    Debug.Log($"단계 {currentStep} 성공");
                    BrightnessManager.Instance.DecreaseExposure();
                }
                else
                {
                    Debug.Log($"단계 {currentStep} 실패");

                    BrightnessManager.Instance.IncreaseExposure(); // 밝기 증가
                    Debug.Log($"밝기 증가 후: {BrightnessManager.Instance.currentBT}");

                    testFailed = true;
                }

                yield return new WaitForSeconds(stepDuration);
            }

            successfulTests++;
            Debug.Log($"테스트 {successfulTests} 통과!");

            if (successfulTests + failedTests >= totalTests)
            {
                Debug.Log("모든 테스트 완료");
                break;
            }
        }

        BrightnessManager.Instance.SetExposure(BrightnessManager.Instance.currentBT);
        Debug.Log($"테스트 후 최종 밝기: {BrightnessManager.Instance.currentBT}");

        Debug.Log("끝~");
    }

    IEnumerator RunTest(System.Action<bool> testResultCallback)
    {
        float testEndTime = Time.time + testDuration;

        while (Time.time < testEndTime)
        {
            if (IsLookingAtTarget_Gaze())
            {
                testResultCallback(true);
                yield break;
            }

            yield return null;
        }

        testResultCallback(false);
    }
    IEnumerator Blinking(GameObject cube)
    {
        float elapsedTime = 0f;
        Renderer cubeRenderer = cube.GetComponent<Renderer>();
        Vector3 originalPosition = cube.transform.position;
        Vector3 newPosition = originalPosition;

        while (elapsedTime < blinkDuration)
        {
            while (newPosition == originalPosition)
            {
                newPosition = originalPosition + new Vector3(UnityEngine.Random.Range(-1f, 1f), 0, UnityEngine.Random.Range(-1f, 1f));
            }
            cube.transform.position = newPosition;
            cubeRenderer.enabled = !cubeRenderer.enabled;

            elapsedTime += blinkSpeed;
            yield return new WaitForSeconds(blinkSpeed);

        }

            cubeRenderer.enabled = false;
    }


    void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        cursorUI.position = mousePos;
    }

    bool IsLookingAtTarget()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, cubeLayer))
        {
            if (hit.collider != null && hit.collider.gameObject.activeSelf)
            {
                Debug.Log($"Mouse is over: {hit.collider.gameObject.name}");
                return true;
            }
            else
            {
                Debug.Log("Mouse is not over a valid cube.");
            }
        }

        return false;
    }

    void FixedUpdate()
    {
        TobiiGameIntegrationApi.Update();
    }

    bool IsLookingAtTarget_Gaze()
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

        Ray ray = mainCamera.ScreenPointToRay(gazeScreenPos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, cubeLayer))
        {
            if (hit.collider != null && hit.collider.gameObject.activeSelf)
            {
                Debug.Log($"GazePoint is over: {hit.collider.gameObject.name}");
                return true;
            }
            else
            {
                Debug.Log("GazePoint is not over a valid cube.");
            }
        }

        //     bool hit = RectTransformUtility.RectangleContainsScreenPoint(targetUI, gazeScreenPos);
        //     Debug.Log($"GazePos: {gazeScreenPos} | Hit UI: {hit}");

        return false;
    }
}