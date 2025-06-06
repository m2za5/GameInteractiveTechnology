using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class BlinkCube : MonoBehaviour
{
    public GameObject[] Cubes;
    public float blinkSpeed = 0.1f;
    public float blinkDuration = 1f;
    public float testDuration = 3f;
    public float stepDuration = 1f;

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

        StartCoroutine(ActivateNextCube());
    }

    IEnumerator ActivateNextCube()
    {
        while (successfulTests + failedTests < totalTests)
        {
            int failedSteps = 0;
            foreach (GameObject cube in Cubes)
            {
                cube.SetActive(false);
            }

            int randomIndex = Random.Range(0, Cubes.Length);
            Cubes[randomIndex].SetActive(true);

            Debug.Log($"테스트 {successfulTests + failedTests + 1} 시작");

            for (currentStep = 1; currentStep <= totalSteps; currentStep++)
            {
                Debug.Log($"단계 {currentStep} 시작");

                yield return StartCoroutine(Blinking(Cubes[randomIndex]));

                bool passedStep = false;
                yield return StartCoroutine(RunTest(testResult =>
                {
                    passedStep = testResult;
                }));

                if (passedStep)
                {
                    Debug.Log($"단계 {currentStep} 성공");
                }
                else
                {
                    Debug.Log($"단계 {currentStep} 실패");
                    failedSteps++;

                    if (failedSteps >= 2)
                    {
                        Debug.Log("이 테스트 실패");
                        failedTests++;
                        break;
                    }
                }

                yield return new WaitForSeconds(stepDuration);
            }

            if (failedSteps < 2)
            {
                successfulTests++;
                Debug.Log($"테스트 {successfulTests} 통과!");
            }

            if (successfulTests + failedTests >= totalTests)
            {
                Debug.Log("모든 테스트 완료");
                break;
            }
        }

        Debug.Log("끝~");
    }

    IEnumerator RunTest(System.Action<bool> testResultCallback)
    {
        float testEndTime = Time.time + testDuration;

        while (Time.time < testEndTime)
        {
            if (IsLookingAtTarget())
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
                newPosition = originalPosition + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
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
}