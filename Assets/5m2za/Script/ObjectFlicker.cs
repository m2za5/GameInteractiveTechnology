using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFlicker : MonoBehaviour
{
    // Start is called before the first frame update
    public Renderer targetRenderer;  // 3D 오브젝트의 Renderer
    public float flickerIntensity = 1f; // 밝기 조절을 위한 초기 값
    public float flickerInterval = 0.1f; // 깜빡이는 간격
    private bool visible = true;
    private Material targetMaterial;

    void Start()
    {
        if (targetRenderer == null)
            targetRenderer = GetComponent<Renderer>();

        if (targetRenderer != null)
            targetMaterial = targetRenderer.material;

        InvokeRepeating(nameof(Flicker), 0f, flickerInterval);
    }

    public void SetFlickerIntensity(float newIntensity)
    {
        flickerIntensity = newIntensity;
    }

    void Flicker()
    {
        Color currentColor = targetMaterial.color;
        currentColor = visible ? currentColor * flickerIntensity : currentColor * 0f; // 밝기 조정
        targetMaterial.color = currentColor;

        visible = !visible;
    }
}