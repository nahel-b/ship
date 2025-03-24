using UnityEngine;
using UnityEngine.UI;

public class UIFollowObject : MonoBehaviour
{
    public Transform target; // L'objet à suivre
    public RectTransform uiElement; // L'élément UI à déplacer
    static public Camera mainCamera; 

    static public float distortion = 0.26f;// 0.98f;
    static public float verticalDistortion = 0.31f;// 0.98f;
    static public float distortionCompensation = 0.758f ;// 0.761f;


    void Start()
    {
        // Trouver la caméra principale
        if (mainCamera == null)
            mainCamera = Camera.main;
    }


    void Update()
    {
        if (target == null || uiElement == null || mainCamera == null)
        {
            GameObject info_ligne = GameObject.Find("[ligne-info-OBJ]");
            foreach(Transform child in info_ligne.transform.GetChild(1).GetChild(0).GetChild(0) )
            {
                Destroy(child.gameObject);
            }
            info_ligne.transform.GetChild(1).gameObject.SetActive(false);
            info_ligne.transform.GetChild(0).GetComponent<Text>().text = "";
            info_ligne.GetComponent<Image>().enabled = false;
            return;
            
        }
        else
        {
            GameObject info_ligne = GameObject.Find("[ligne-info-OBJ]");
            info_ligne.GetComponent<Image>().enabled = true;
        }

        // Utiliser la fonction statique pour convertir la position
        Vector2 correctedScreenPos = WorldToScreenWithDistortion(target.position );
        
        // Vérifier si la position est valide
        if (correctedScreenPos.x < 0 || correctedScreenPos.y < 0 || correctedScreenPos.x > Screen.width || correctedScreenPos.y > Screen.height) 
        {
            target = null;
            return;
        }
       
            
        // Placer l'UI
        uiElement.position = correctedScreenPos;
    }

    // Fonction statique pour appliquer l'inverse de la distorsion (world to screen)
    public static Vector2 ApplyInverseDistortion(Vector2 uv)
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

    // Nouvelle fonction statique pour appliquer la distorsion (screen to world)
    public static Vector2 ApplyDistortion(Vector2 correctedUV)
    {
        // Centre autour de (0, 0)
        Vector2 centeredUV = correctedUV - Vector2.one * 0.5f;

        float aspectRatio = (float)Screen.height / Screen.width;
        float verticalDistortionAdj = verticalDistortion * Mathf.Sqrt(aspectRatio);
        Vector2 distortionFactors = new Vector2(distortion, verticalDistortionAdj);

        // Résolution de l'équation: centeredUV = original / (1 + r2 * distortionFactors)
        // original = centeredUV * (1 + r2 * distortionFactors)
        float r2_approx = centeredUV.sqrMagnitude;
        Vector2 distortionMultiplier = Vector2.one + r2_approx * distortionFactors;
        Vector2 originalCenteredUV = Vector2.Scale(centeredUV, distortionMultiplier);
        
        // Appliquer compensation de distorsion
        originalCenteredUV *= distortionCompensation;
        
        // Recentrer autour de (0.5, 0.5)
        return originalCenteredUV + Vector2.one * 0.5f;
    }

    // Fonction statique world to screen avec distorsion
    public static Vector2 WorldToScreenWithDistortion(Vector3 worldPosition)
    {
        // Convertir position monde en position écran
        Vector3 screenPos = mainCamera.WorldToScreenPoint(worldPosition);
        
        // Vérifier si la position est derrière la caméra
        if (screenPos.z < 0) 
            return new Vector2(-1, -1); // Position écran invalide
            
        // Convertir en coordonnées UV
        Vector2 uv = new Vector2(screenPos.x / Screen.width, screenPos.y / Screen.height);
        
        // Appliquer l'inverse de la distorsion
        Vector2 correctedUV = ApplyInverseDistortion(uv);
        
        // Convertir en coordonnées écran
        return new Vector2(correctedUV.x * Screen.width, correctedUV.y * Screen.height);
    }

    // Fonction statique screen to world avec distorsion
    public static Vector3 ScreenToWorldWithDistortion(Vector2 screenPosition, float depth)
    {
        // Convertir position écran en coordonnées UV
        Vector2 uv = new Vector2(screenPosition.x / Screen.width, screenPosition.y / Screen.height);
        
        // Appliquer la distorsion
        Vector2 distortedUV = ApplyDistortion(uv);
        
        // Convertir en coordonnées écran
        Vector2 distortedScreenPos = new Vector2(distortedUV.x * Screen.width, distortedUV.y * Screen.height);
        
        // Convertir position écran en position monde
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(new Vector3(distortedScreenPos.x, distortedScreenPos.y, depth));
        
        return worldPos;
    }
}
