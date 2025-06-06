using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ButtonClickLogger : MonoBehaviour
{
    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;

    private string csvPath;

    void Start()
    {
        // ������Ʈ ��Ʈ ���� survey ������ ����
        string projectRoot = Directory.GetParent(Application.dataPath).FullName;
        string folder = Path.Combine(projectRoot, "Library", "survey");
        csvPath = Path.Combine(folder, "button_click_log.csv");

        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        button1.onClick.AddListener(() => LogButtonClick("1��"));
        button2.onClick.AddListener(() => LogButtonClick("2��"));
        button3.onClick.AddListener(() => LogButtonClick("3��"));
        button4.onClick.AddListener(() => LogButtonClick("4��"));
    }

    void LogButtonClick(string label)
    {
        string log = $"{System.DateTime.Now},{"survey 1"},{label}";
        File.AppendAllText(csvPath, log + "\n");
        Debug.Log($"CSV ����: {log}");
    }
}