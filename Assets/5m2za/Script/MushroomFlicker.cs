using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MushroomFlicker : MonoBehaviour
{
    public Image targetImage;
    public float flickerAlpha = 0.05f;
    public float flickerInterval = 0.1f;

    private bool visible = false;

    void Start()
    {
        if (targetImage == null)
            targetImage = GetComponent<Image>();

        InvokeRepeating(nameof(Flicker), 0f, flickerInterval);
    }

    public void SetFlickerAlpha(float newAlpha)
    {
        flickerAlpha = newAlpha;
    }

    void Flicker()
    {
        float alpha = visible ? flickerAlpha : 0f;
        SetAlpha(alpha);
        visible = !visible;
    }

    void SetAlpha(float alpha)
    {
        if (targetImage != null)
        {
            Color c = targetImage.color;
            c.a = alpha;
            targetImage.color = c;
        }
    }
}
