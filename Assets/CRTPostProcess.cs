using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CRTPostProcess : MonoBehaviour
{
    public RawImage uiImage; // L'image UI qui reçoit le shader
    private RenderTexture rt;

    void Start()
    {
        UpdateRenderTexture();
                OnPreRender();

    }

    void UpdateRenderTexture()
    {
        if (rt != null)
        rt.Release();

            // Créer un RenderTexture HDR
            rt = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.ARGBHalf);
            rt.enableRandomWrite = false; // Garde désactivé sauf si tu en as besoin
            rt.useMipMap = false;         // Active si nécessaire
            rt.filterMode = FilterMode.Point; // Pour un rendu pixelisé plus net
            rt.Create(); // Important pour valider la création de la texture

            // Assigner le RenderTexture à la caméra et à l'UI
            Camera.main.targetTexture = rt;
            uiImage.texture = rt;
    }

    void OnPreRender()
    {
        if (rt.width != Screen.width || rt.height != Screen.height)
        {
            UpdateRenderTexture();
        }
    }
}
