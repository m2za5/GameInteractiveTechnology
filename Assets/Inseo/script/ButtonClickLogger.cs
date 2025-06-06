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
        // 프로젝트 루트 기준 survey 폴더에 저장
        string projectRoot = Directory.GetParent(Application.dataPath).FullName;
        string folder = Path.Combine(projectRoot, "Library", "survey");
        csvPath = Path.Combine(folder, "button_click_log.csv");

        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        button1.onClick.AddListener(() => LogButtonClick("1개"));
        button2.onClick.AddListener(() => LogButtonClick("2개"));
        button3.onClick.AddListener(() => LogButtonClick("3개"));
        button4.onClick.AddListener(() => LogButtonClick("4개"));
    }

    void LogButtonClick(string label)
    {
        string log = $"{System.DateTime.Now},{"survey 1"},{label}";
        File.AppendAllText(csvPath, log + "\n");
        Debug.Log($"CSV 저장: {log}");
    }
}