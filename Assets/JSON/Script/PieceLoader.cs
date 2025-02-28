using System.Collections.Generic;
using UnityEngine;
using System.IO;


public String piecePrefabPath = "Assets/JSON/pieces.json";

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
    public List<PiecObje> pieces;
}

public class PieceLoader : MonoBehaviour
{
    public PieceListObj pieceList;
    private Dictionary<int, GameObject> loadedPrefabs = new Dictionary<int, GameObject>();

    void Start()
    {
        LoadPieces();
    }

    void LoadPieces()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "/JSON/pieces.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            pieceList = JsonUtility.FromJson<PieceList>(json);
            Debug.Log("Pièces chargées avec succès !");
        }
        else
        {
            Debug.LogError("Fichier JSON non trouvé !");
        }
    }

    public GameObject GetPiecePrefab(int id)
    {
        if (loadedPrefabs.ContainsKey(id))
            return loadedPrefabs[id];

        Piece piece = pieceList.pieces.Find(p => p.id == id);
        if (piece != null)
        {
            string prefabPath = $"Prefabs/{piece.prefab}"; // Chemin relatif depuis Resources
            GameObject prefab = Resources.Load<GameObject>(prefabPath);

            if (prefab != null)
            {
                loadedPrefabs[id] = prefab;
                return prefab;
            }
            else
            {
                Debug.LogError($"Prefab non trouvé : {prefabPath}");
            }
        }
        else
        {
            Debug.LogWarning($"Aucune pièce trouvée avec l'ID {id}");
        }
        return null;
    }

    public void InstantiatePiece(int id, Vector3 position, Quaternion rotation)
    {
        GameObject prefab = GetPiecePrefab(id);
        if (prefab != null)
        {
            Instantiate(prefab, position, rotation);
        }
    }
}
