using UnityEngine;

public class PathManager : MonoBehaviour
{
    // ✅ 싱글톤 인스턴스
    public static PathManager Instance { get; private set; }

    public PathDrawer drawer;
    public PathSaver saver;
    public PathFollower follower;

    private bool drawingMode = false;
    public bool hasSaved = true;

    void Awake()
    {
        // ✅ 싱글톤 중복 방지
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;


        follower.ResetPath();
        follower.BeginFollow();

        // ✅ 씬 전환 시에도 유지하려면 아래 줄 주석 해제
        // DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            drawingMode = true;
            drawer.StartDrawing();
            Debug.Log("✏️ 경로 그리기 시작 (Q)");
        }

        if (Input.GetKeyDown(KeyCode.Return) && drawingMode)
        {
            saver.SavePath();
            drawingMode = false;
            hasSaved = true;
            Debug.Log("💾 경로 저장 완료 (Enter)");
        }

        if (Input.GetKeyDown(KeyCode.Space) && hasSaved)
        {
            follower.BeginFollow();
            Debug.Log("🚀 경로 따라 이동 시작 (Space)");
        }
       
    }
}