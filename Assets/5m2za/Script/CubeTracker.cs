using UnityEngine;
using System;
using System.Runtime.InteropServices;
using Tobii.GameIntegration.Net;
using UnityEngine.UI;
using TMPro;

public class CubeTracker : MonoBehaviour
{
    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    public RectTransform gazePointer;
    public GameObject[] cubes;
    public Outline[] cubeOutlines;
    public TextMeshProUGUI gazeLabel;
    public RectTransform pointerUI;

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

            pointerUI.position = gazeScreenPos;

          //  Debug.Log($"Gaze Position - X: {gazePoint.X}, Y: {gazePoint.Y}");

            Vector2 offset = gazePointer.sizeDelta * 0.5f;
            if (gazePointer != null)
                gazePointer.position = gazeScreenPos - offset;

          //  Debug.Log($"Gaze Pointer Position: {gazePointer.position}");

            Ray ray = Camera.main.ScreenPointToRay(gazeScreenPos);
            RaycastHit hit;

            int newIndex = -1;

            for (int i = 0; i < cubes.Length; i++)
            {
                if (cubes[i] != null && cubes[i].GetComponent<Collider>() != null)
                {
                    if (cubes[i].GetComponent<Collider>().Raycast(ray, out hit, Mathf.Infinity))
                    {
                        newIndex = i;
                        break;
                    }
                }
            }

            if (newIndex != -1)
            {
                Debug.Log($"Looking at Cube: {cubes[newIndex].name}");
            }
            else
            {
                Debug.Log("Not looking at any cube.");
            }

            for (int i = 0; i < cubeOutlines.Length; i++)
            {
                cubeOutlines[i].enabled = (i == newIndex);
            }

            if (newIndex != -1 && newIndex != currentIndex)
            {
                //gazeLabel.text = "Gaze Cube : " + cubes[newIndex].name;
                currentIndex = newIndex;
            }
            else if (newIndex == -1)
            {
                //gazeLabel.text = "Gaze Cube : ";
                currentIndex = -1;
            }
        }
    }
}