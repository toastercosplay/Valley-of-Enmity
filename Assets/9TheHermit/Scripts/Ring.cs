using UnityEngine;
using UnityEngine.UI;

public class RingController : MonoBehaviour
{
    public Material ringMaterial; // Assign your custom shader material here
    
    [Range(0.1f, 0.5f)]
    public float radius = 0.2f;
    public float volumeConstant = 0.05f; // Adjust this to change "visual weight"
    public Color ringColor = Color.white;

    void Update()
    {
        // 1. Calculate inverse thickness (Thinner as it gets bigger)
        float thickness = volumeConstant / radius;

        // 2. Apply to the shader
        ringMaterial.SetFloat("Radius", radius);
        ringMaterial.SetFloat("Thickness", thickness);
        ringMaterial.SetColor("Color", ringColor);
        
        // 3. Optional: Sync RectTransform size to match the radius
        // (If your shader is normalized, you might just scale the Radius parameter)
    }
}
