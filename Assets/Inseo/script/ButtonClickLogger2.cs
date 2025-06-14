using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class ButtonClickLogger2 : MonoBehaviour
{
    [Header("버튼")]
    public Button buttonHat;
    public Button buttonAxe;
    public Button buttonHorn;
    public Button buttonNone;

    [Header("유령 오브젝트")]
    public GameObject ghostHat;
    public GameObject ghostAxe;
    public GameObject ghostHorn;
    public GameObject ghostNone;

    private string[] answers = { "모자", "도끼", "뿔", "없음" };
    private int correctIndex = -1;
    private string csvPath;

    void Start()
    {
        // 경로 세팅
        string projectRoot = Directory.GetParent(Application.dataPath).FullName;
        string folder = Path.Combine(projectRoot, "Library", "survey");
        csvPath = Path.Combine(folder, "button_click_log.csv");
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        // 유령 랜덤 활성화
        int randomIdx = Random.Range(0, 4);
        correctIndex = randomIdx;
        ghostHat.SetActive(randomIdx == 0);
        ghostAxe.SetActive(randomIdx == 1);
        ghostHorn.SetActive(randomIdx == 2);
        ghostNone.SetActive(randomIdx == 3);

        // 버튼 이벤트 연결
        buttonHat.onClick.AddListener(() => CheckAnswer(0));
        buttonAxe.onClick.AddListener(() => CheckAnswer(1));
        buttonHorn.onClick.AddListener(() => CheckAnswer(2));
        buttonNone.onClick.AddListener(() => CheckAnswer(3));
    }

    void CheckAnswer(int selectedIndex)
    {
        string result = (selectedIndex == correctIndex) ? "success" : "fail";
        string log = $"{System.DateTime.Now},{DontDes.previousSceneName}, {"survey 2"},{answers[correctIndex]},{answers[selectedIndex]},{result}";
        File.AppendAllText(csvPath, log + "\n");
        Debug.Log($"CSV 저장: {log}");
    }
}