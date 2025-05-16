using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class eyetracking : MonoBehaviour
{
    UdpClient client;
    public int port = 5052;
    IPEndPoint remoteEP;

    public Vector2 leftEyePos;
    public Vector2 rightEyePos;

    public RectTransform gazePointerUI; // ������ UI ����

    void Start()
    {
        client = new UdpClient(port);
        remoteEP = new IPEndPoint(IPAddress.Any, port);
    }

    void Update()
    {
        if (client.Available > 0)
        {
            byte[] data = client.Receive(ref remoteEP);
            string json = Encoding.UTF8.GetString(data);

            EyeData eye = JsonUtility.FromJson<EyeData>(json);

            float gazeX = (eye.left_x + eye.right_x) / 2f;
            float gazeY = (eye.left_y + eye.right_y) / 2f;

            // Ŀ���͸���¡ ������ �þ� ���� (�ʹ� ���ų� ������ ����)
            float minX = 0.35f, maxX = 0.65f;
            float minY = 0.35f, maxY = 0.65f;

            float Remap(float value, float from1, float to1, float from2, float to2)
            {
                return Mathf.Clamp01((value - from1) / (to1 - from1)) * (to2 - from2) + from2;
            }

            float screenX = Remap(gazeX, minX, maxX, Screen.width,0);
            float screenY = Remap(gazeY, minY, maxY, Screen.height, 0); // Y�� ����

            if (gazePointerUI != null)
            {
                gazePointerUI.position = new Vector2(screenX, screenY);
            }
        }
    }

    [System.Serializable]
    public class EyeData
    {
        public float left_x, left_y, right_x, right_y;
    }

    void OnApplicationQuit()
    {
        client.Close();
    }
}
