using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class PieceObj
{
    public int id;
    public string name;
    public string description;
    public string prefab; // Chemin relatif du prefab dans "Assets/Prefabs/"
}

[System.Serializable]
public class PieceListObj
{
    public List<PieceObj> pieces;
}

// Classe statique - ne peut plus hériter de MonoBehaviour
public static class PieceLoader
{
    private static string piecePrefabPath = "Assets/JSON/pieces.json";
    private static PieceListObj pieceList;
    private static Dictionary<string, GameObject> loadedPrefabs = new Dictionary<string, GameObject>();
    private static bool isInitialized = false;

    // Méthode d'initialisation à appeler manuellement
    public static void Initialize()
    {
        if (!isInitialized)
        {
            LoadPieces();
            isInitialized = true;
        }
    }

    static void LoadPieces()
    {
       pieceList = JsonLoader.LoadJson<PieceListObj>("JSON/pieces.json");

        if (pieceList != null)
            Debug.Log("Pièces chargées avec succès !");
        else
            Debug.LogError("Échec du chargement de pieces.json");
    }

    public static GameObject GetPiecePrefab(string nom)
    {
        // S'assurer que l'initialisation a été faite
        if (!isInitialized)
            Initialize();

        if (loadedPrefabs.ContainsKey(nom))
            return loadedPrefabs[nom];

        PieceObj piece = pieceList.pieces.Find(p => p.name == nom);
        if (piece != null)
        {
            string prefabPath = $"Prefab/Espace/Piece/{piece.prefab}"; // Chemin relatif depuis Resources
            GameObject prefab = Resources.Load<GameObject>(prefabPath);

            if (prefab != null)
            {
                loadedPrefabs[nom] = prefab;
                return prefab;
            }
            else
            {
                Debug.LogError($"Prefab non trouvé : {prefabPath}");
            }
        }
        else
        {
            Debug.LogWarning($"Aucune pièce trouvée avec le nom {nom}");
        }
        return null;
    }

    public static GameObject InstantiatePiece(string nom, Vector3 position, Quaternion rotation)
    {
        GameObject prefab = GetPiecePrefab(nom);
        if (prefab != null)
        {
            return Object.Instantiate(prefab, position, rotation);
        }
        return null;
    }
}
