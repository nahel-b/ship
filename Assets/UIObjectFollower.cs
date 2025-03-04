using UnityEngine;

public class UIFollowObject : MonoBehaviour
{
    public Transform target; // L'objet à suivre
    public RectTransform uiElement; // L'élément UI à placer
    public Camera mainCamera; // La caméra principale

    // Paramètres du shader (doivent être les mêmes que ceux du shader)
    public float distortion = 0.98f;
    public float verticalDistortion = 0.98f;
    public float distortionCompensation = 0.761f;

    void Update()
    {
        if (target == null || uiElement == null || mainCamera == null)
            return;

        // Convertir la position monde en écran
        Vector3 screenPos = mainCamera.WorldToScreenPoint(target.position);
        if (screenPos.z < 0) return; // L'objet est hors caméra

        // Convertir en coordonnées UV [0,1]
        Vector2 uv = new Vector2(screenPos.x / Screen.width, screenPos.y / Screen.height);

        // Appliquer l'inversion de la distorsion du shader
        Vector2 correctedScreenPos = ApplyInverseDistortion(uv);

        // Convertir en coordonnées écran
        correctedScreenPos.x *= Screen.width;
        correctedScreenPos.y *= Screen.height;

        // Placer l'UI
        uiElement.position = correctedScreenPos;
    }

    Vector2 ApplyInverseDistortion(Vector2 uv)
    {
        // Inverse de la distorsion
        Vector2 centeredUV = uv - Vector2.one * 0.5f;
        centeredUV /= distortionCompensation;

        float r2 = centeredUV.sqrMagnitude;
        float aspectRatio = (float)Screen.height / Screen.width;
        float verticalDistortionAdj = verticalDistortion * Mathf.Sqrt(aspectRatio);

        Vector2 distortionFactors = new Vector2(distortion, verticalDistortionAdj);
        Vector2 divisor = Vector2.one + r2 * distortionFactors;
        Vector2 correctedUV = (centeredUV / divisor) + Vector2.one * 0.5f;

        return correctedUV;
    }
}
