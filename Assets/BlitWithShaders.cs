using UnityEngine;
using UnityEngine.UI;

public class BlitWithShaders : MonoBehaviour
{
    public Camera sceneCamera;  // Caméra de la scène
    public Camera uiCamera;     // Caméra de l'UI
    public RawImage display;    // Affichage du résultat final (RawImage)
    public Material sceneShaderMaterial; // Shader pour la scène
    public Material uiShaderMaterial;    // Shader pour l'UI
    public Material blendMaterial;       // Shader pour combiner les rendus

    private RenderTexture sceneRT;
    private RenderTexture uiRT;
    private RenderTexture finalRT;

    private RenderTexture sceneRtShader;
    private RenderTexture uiRtShader;

    void Start()
    {
        // Créer les RenderTextures
        sceneRT = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.ARGBHalf);
        uiRT = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.ARGBHalf);
        finalRT = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.ARGBHalf);

        sceneRtShader = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.ARGBHalf);
        uiRtShader = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.ARGBHalf);

        // Assigner les RenderTextures aux caméras
        sceneCamera.targetTexture = sceneRT;
        uiCamera.targetTexture = uiRT;

        // Camera.main.clearFlags = CameraClearFlags.SolidColor; 
        // uiCamera.clearFlags = CameraClearFlags.Nothing;
        // Assigner la texture finale à l'UI
        display.texture = uiRT;
    }

    void Update()
    {
        // appliquer les shaders aux caméras

        sceneShaderMaterial.SetTexture("_MainTex", sceneRT);
        uiShaderMaterial.SetTexture("_MainTex", uiRT);
        //sceneShaderMaterial.SetFloat("_Distortion", 2);
        Graphics.Blit(null, sceneRtShader, sceneShaderMaterial);
        Graphics.Blit(null, uiRtShader, uiShaderMaterial);



        // Combiner les deux textures avec le shader de fusion
        blendMaterial.SetTexture("_SceneTex", sceneRtShader);
        blendMaterial.SetTexture("_UITex", uiRtShader);
        Graphics.Blit(null, finalRT, blendMaterial);

       display.texture = finalRT;

    //    Camera.main.clearFlags = CameraClearFlags.SolidColor; 
    //     uiCamera.clearFlags = CameraClearFlags.Nothing;
    }

    void OnDestroy()
    {
        // Libérer les RenderTextures
        sceneRT.Release();
        uiRT.Release();
        finalRT.Release();
    }
}
