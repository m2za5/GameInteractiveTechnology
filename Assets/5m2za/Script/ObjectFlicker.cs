using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFlicker : MonoBehaviour
{
    public Renderer targetRenderer;
    public float flickerIntensity = 1f;
    public float flickerInterval = 0.1f; 
    private bool visible = true;
    private Material targetMaterial;

    void Start()
    {
        if (targetRenderer == null)
            targetRenderer = GetComponent<Renderer>();

        if (targetRenderer != null)
            targetMaterial = targetRenderer.material;
        SetTransparentMaterial();
        InvokeRepeating(nameof(Flicker), 0f, flickerInterval);
    }

    public void SetFlickerIntensity(float newIntensity)
    {
        flickerIntensity = newIntensity;
    }

    void SetTransparentMaterial()
    {
        if (targetMaterial == null) return;
        targetMaterial.SetFloat("_Mode", 3);
        targetMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        targetMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        targetMaterial.SetInt("_ZWrite", 0); 
        targetMaterial.DisableKeyword("_ALPHATEST_ON");
        targetMaterial.EnableKeyword("_ALPHABLEND_ON");
        targetMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        targetMaterial.renderQueue = 3000;
    }

    void Flicker()
    {
        Color currentColor = targetMaterial.color;
        float alpha = Mathf.PingPong(Time.time * flickerIntensity, 1);
        currentColor.a = alpha;

        targetMaterial.color = currentColor;
    }
}