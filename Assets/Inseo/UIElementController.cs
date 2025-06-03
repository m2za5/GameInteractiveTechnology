using UnityEngine;

public class UIElementController : MonoBehaviour
{
    [System.Serializable]
    public class UIElement
    {
        public RectTransform rectTransform;
        public CanvasGroup canvasGroup;

        [HideInInspector] public float moveSpeed;        // 1~10 ����
        [HideInInspector] public float brightnessSpeed;  // 0~1 ����
        [HideInInspector] public float startDelay;       // 0~2�� ����

        private Vector2 initialPosition;
        private float timer;
        private float delayTimer = 0f;
        private const float moveAmplitude = 100f;

        public void Init()
        {
            // ���� �� ����
            moveSpeed = Random.Range(1f, 5f);
            //brightnessSpeed = Random.Range(0f, 0.1f);
            brightnessSpeed = 0.05f;
            startDelay = Random.Range(1f, 3f); // ���ϴ� ������ ���� ����

            if (rectTransform != null)
                initialPosition = rectTransform.anchoredPosition;
            if (canvasGroup != null)
                canvasGroup.alpha = 0f;

            timer = Random.Range(0f, Mathf.PI * 2);
            delayTimer = 0f;
        }

        private float brightnessTickTimer = 0f;

        public void UpdateElement(float deltaTime)
        {
            // ������
            if (rectTransform != null)
            {
                timer += deltaTime * moveSpeed;
                float offsetY = Mathf.Sin(timer) * moveAmplitude;
                rectTransform.anchoredPosition = initialPosition + new Vector2(0, offsetY);
            }

            // ��� (������ �� ��ũ��Ʈ ����)
            if (canvasGroup != null && canvasGroup.alpha < 1f)
            {
                delayTimer += deltaTime;
                if (delayTimer >= startDelay)
                {
                    brightnessTickTimer += deltaTime;
                    if (brightnessTickTimer >= 2f)
                    {
                        canvasGroup.alpha += brightnessSpeed;
                        canvasGroup.alpha = Mathf.Min(canvasGroup.alpha, 1f);
                        brightnessTickTimer = 0f;
                    }
                }
            }
        }
    }

    public UIElement[] elements;

    void Start()
    {
        foreach (var element in elements)
        {
            element.Init();
        }
    }

    void Update()
    {
        float deltaTime = Time.deltaTime;
        foreach (var element in elements)
        {
            element.UpdateElement(deltaTime);
        }
    }
}

