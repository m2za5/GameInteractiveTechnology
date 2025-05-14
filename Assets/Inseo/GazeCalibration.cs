using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class GazeCalibration : MonoBehaviour
{
    [Header("Networking")]
    public UdpClient client;
    public int port = 5052; // 수정된 포트 번호

    [Header("UI")]
    public RectTransform[] calibrationPoints; // 4개의 점
    public RectTransform gazePointerUI;
    public GameObject instructionText;

    [Header("Calibration Settings")]
    public int calibrationRounds = 3; // 전체 캘리브레이션 라운드 수

    private IPEndPoint remoteEP;
    private int currentPointIndex = 0;
    private int currentRound = 0;

    private float[] gazeXs;
    private float[] gazeYs;

    private bool isCalibrating = true;
    private bool ready = false;

    void Start()
    {
        client = new UdpClient(port);
        remoteEP = new IPEndPoint(IPAddress.Any, port);


        int totalPoints = calibrationPoints.Length * calibrationRounds;
        gazeXs = new float[totalPoints];
        gazeYs = new float[totalPoints];

        instructionText.SetActive(true);
        ShowNextCalibrationPoint();

        gazePointerUI.pivot = new Vector2(0.5f, 0.5f);
    }

    void Update()
    {
        if (client.Available > 0)
        {
            byte[] data = client.Receive(ref remoteEP);
            string json = Encoding.UTF8.GetString(data);

            Debug.Log($"Received data: {json}");

            EyeData eye = JsonUtility.FromJson<EyeData>(json);
            float gazeX = (eye.left_x + eye.right_x) / 2f;
            float gazeY = (eye.left_y + eye.right_y) / 2f;

            if (isCalibrating && !isCapturing)
            {
                StartCoroutine(CaptureGaze(gazeX, gazeY));
            }
            else if (ready)
            {
                float minX = Average(gazeXs, 1);
                float maxX = Average(gazeXs, 0);
                float minY = Average(gazeYs, 0);
                float maxY = Average(gazeYs, 2);

                float mappedX = Remap(gazeX, minX, maxX, 0, Screen.width);
                float mappedY = Remap(gazeY, minY, maxY, 0, Screen.height);

                //float mappedX = RemapWithDynamicRange(gazeX, minX, maxX, Screen.width,0);
                //float mappedY = RemapWithDynamicRange(gazeY, minY, maxY, Screen.height,0);

                Vector2 targetPos = new Vector2(mappedX, mappedY);
                gazePointerUI.position = Vector2.Lerp(gazePointerUI.position, targetPos, 0.2f);
            }
        }
        else
        {
            Debug.Log("No data available.");
        }
    }

    private bool isCapturing = false;

    IEnumerator CaptureGaze(float x, float y)
    {
        int totalPoints = calibrationPoints.Length * calibrationRounds;
        if (currentRound * calibrationPoints.Length + currentPointIndex >= totalPoints)
            yield break;

        if (isCapturing) yield break;
        isCapturing = true;

        int absoluteIndex = currentRound * calibrationPoints.Length + currentPointIndex;

        instructionText.GetComponent<Text>().text = $"바라보는 중... ({currentRound + 1}/{calibrationRounds}, 점 {currentPointIndex + 1}/4)";
        yield return new WaitForSeconds(2.0f);  // 시간을 늘려서 사용자가 바라볼 수 있는 시간을 충분히 제공

        gazeXs[absoluteIndex] = x;
        gazeYs[absoluteIndex] = y;

        currentPointIndex++;

        if (currentPointIndex < calibrationPoints.Length)
        {
            ShowNextCalibrationPoint();
        }
        else
        {
            currentRound++;
            if (currentRound < calibrationRounds)
            {
                currentPointIndex = 0;
                ShowNextCalibrationPoint();
            }
            else
            {
                instructionText.GetComponent<Text>().text = "캘리브레이션 완료!";
                instructionText.SetActive(false);
                isCalibrating = false;
                ready = true;

                foreach (var point in calibrationPoints)
                    point.gameObject.SetActive(false);
            }
        }

        isCapturing = false;
    }
    void ShowNextCalibrationPoint()
    {
        foreach (var point in calibrationPoints)
            point.gameObject.SetActive(false);

        calibrationPoints[currentPointIndex].gameObject.SetActive(true);
        instructionText.GetComponent<Text>().text = $"화면의 점을 바라보세요 ({currentRound + 1}/{calibrationRounds}, 점 {currentPointIndex + 1}/4)";
    }

    float Remap(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return Mathf.Clamp01((value - inMin) / (inMax - inMin)) * (outMax - outMin) + outMin;
    }

    private float RemapWithDynamicRange(float value, float minValue, float maxValue, float outMin, float outMax)
    {
        minValue = Mathf.Min(minValue, value);
        maxValue = Mathf.Max(maxValue, value);
        return Mathf.Clamp01((value - minValue) / (maxValue - minValue)) * (outMax - outMin) + outMin;
    }

    float Average(float[] arr, int modIndex)
    {
        float sum = 0;
        int count = 0;
        for (int i = 0; i < arr.Length; i++)
        {
            if (i % 4 == modIndex)
            {
                sum += arr[i];
                count++;
            }
        }
        return count > 0 ? sum / count : 0;
    }

    [System.Serializable]
    public class EyeData
    {
        public float left_x;
        public float left_y;
        public float right_x;
        public float right_y;
    }
}
