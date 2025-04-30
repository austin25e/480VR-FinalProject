using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabTransparency : MonoBehaviour
{
    public float grabAlpha = 0.5f; // transparency level when grabbed
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private Material[] materials;
    private float[] originalAlphas;

    void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrab);
            grabInteractable.selectExited.AddListener(OnRelease);
        }

        // Store materials and their original alphas
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            materials = renderer.materials;
            originalAlphas = new float[materials.Length];

            for (int i = 0; i < materials.Length; i++)
            {
                Color color = materials[i].color;
                originalAlphas[i] = color.a;
                EnableTransparency(materials[i]); // ensure shader supports transparency
            }
        }
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        SetAlpha(grabAlpha);
    }

    void OnRelease(SelectExitEventArgs args)
    {
        // Restore original alpha
        for (int i = 0; i < materials.Length; i++)
        {
            SetMaterialAlpha(materials[i], originalAlphas[i]);
        }
    }

    private void SetAlpha(float alpha)
    {
        for (int i = 0; i < materials.Length; i++)
        {
            SetMaterialAlpha(materials[i], alpha);
        }
    }

    private void SetMaterialAlpha(Material mat, float alpha)
    {
        Color color = mat.color;
        color.a = alpha;
        mat.color = color;
    }

    private void EnableTransparency(Material mat)
    {
        // Make sure the material supports alpha blending
        mat.SetFloat("_Mode", 3);
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
    }
}
