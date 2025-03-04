using UnityEngine;
using UnityEngine.UI;

public class UIShaderEffect : MonoBehaviour
{
    public Camera uiCamera;  // La caméra dédiée pour l'UI
    public RawImage uiImage; // L'UI qui reçoit le shader
    public Material crtMaterial; // Le shader CRT
    private RenderTexture uiRenderTexture;

    void Start()
    {
        // Créer un RenderTexture dédié à l'UI
        uiRenderTexture = new RenderTexture(Screen.width, Screen.height, 16);
        uiCamera.targetTexture = uiRenderTexture;
        
        // Appliquer le RenderTexture à l'UI RawImage
        uiImage.texture = uiRenderTexture;
    }

    void OnRenderObject()
    {
        // Appliquer le shader CRT à l'UI
        Graphics.Blit(uiRenderTexture, uiRenderTexture, crtMaterial);
    }
}
