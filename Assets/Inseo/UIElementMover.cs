using UnityEngine;

[RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
public class UIElementMover : MonoBehaviour
{
    [Header("설정 범위")]
    public Vector2 moveSpeedRange = new Vector2(1f, 10f);
    public Vector2 brightnessSpeedRange = new Vector2(0f, 1f);
    public Vector2 startDelayRange = new Vector2(0f, 2f);

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private float moveSpeed;
    private float brightnessSpeed;
    private float startDelay;
    private float moveAmplitude = 100f;

    private Vector2 initialPosition;
    private float timer;
    private float delayTimer = 0f;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        // 랜덤 설정
        moveSpeed = Random.Range(moveSpeedRange.x, moveSpeedRange.y);
        brightnessSpeed = Random.Range(brightnessSpeedRange.x, brightnessSpeedRange.y);
        startDelay = Random.Range(startDelayRange.x, startDelayRange.y);

        initialPosition = rectTransform.anchoredPosition;
        canvasGroup.alpha = 0f;

        timer = Random.Range(0f, Mathf.PI * 2);
    }

    void Update()
    {
        float deltaTime = Time.deltaTime;

        // 움직임
        timer += deltaTime * moveSpeed;
        float offsetY = Mathf.Sin(timer) * moveAmplitude;
        rectTransform.anchoredPosition = initialPosition + new Vector2(0, offsetY);

        // 밝기 증가 (딜레이 후)
        if (canvasGroup.alpha < 1f)
        {
            delayTimer += deltaTime;
            if (delayTimer >= startDelay)
            {
                canvasGroup.alpha += brightnessSpeed * deltaTime;
                canvasGroup.alpha = Mathf.Min(canvasGroup.alpha, 1f);
            }
        }
    }
}