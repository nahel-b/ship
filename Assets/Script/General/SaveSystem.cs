using System.IO;
using UnityEngine;
using System.Collections.Generic;

public class SaveSystem : MonoBehaviour
{
    private static readonly string SAVE_FOLDER = Application.persistentDataPath + "/Saves/";
    
    public static void Initialize()
    {
        if (!Directory.Exists(SAVE_FOLDER))
        {
            Directory.CreateDirectory(SAVE_FOLDER);
        }
        string defaultSavePath = SAVE_FOLDER + "default/";
        if (!Directory.Exists(defaultSavePath))
        {
            Directory.CreateDirectory(defaultSavePath);
        }
    }

    #region Deck
    public static DeckList SaveDeckScene(string saveSlot = "default")
    {
        GameObject listeObj = GameObject.Find("Liste");
        
        Liste listeComponent = listeObj.GetComponent<Liste>();
        
        DeckList deckList = listeComponent.deckList;

        GameObject VaisseauGameObject = GameObject.Find("Vaisseau");

        VaisseauClass vaisseau = LoadVaisseau(saveSlot);

        // Sauvegarde du niveau et vie des pièces du deck
        int i = 0;
        foreach(Transform child in VaisseauGameObject.transform.GetChild(0))
        {
            deckList.Find(vaisseau.Deck).assemblage[i].vie = child.GetComponent<Piece>().vie;
            deckList.Find(vaisseau.Deck).assemblage[i].niveau = child.GetComponent<Piece>().niveau;
            i++;
        }
        
        string json = JsonUtility.ToJson(deckList);
        File.WriteAllText(SAVE_FOLDER + saveSlot + "_decks.json", json);
        
        return deckList;
    }

    public static DeckList LoadDeck(string saveSlot = "default")
    {
        string path = SAVE_FOLDER + saveSlot + "_decks.json";
        
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<DeckList>(json);
        }
        else
        {
            Debug.LogWarning("Save file not found in " + path);
            path = SAVE_FOLDER + "start_decks.json";
            return JsonUtility.FromJson<DeckList>(File.ReadAllText(path));
        }
    }
    #endregion
    

    #region Vaisseau
    public static void SaveVaisseauScene(string saveSlot = "default")
    {

        VaisseauClass vaisseau = LoadVaisseau(saveSlot);
        DeckList deckList = SaveDeckScene(saveSlot);

        GameObject VaisseauGameObject = GameObject.Find("Vaisseau");


        // ---- Calcul de la position, de l'angle du vaisseau et de la vitesse
        Vector2 vel = Vector2.zero;
        int nb = 0;
        foreach (Transform piece in GameObject.Find("Vaisseau").transform)
        {
            if (piece.GetComponent<Rigidbody2D>() != null) { vel = vel + piece.GetComponent<Rigidbody2D>().velocity; nb++; }
        }
        foreach (Transform piece in GameObject.Find("Vaisseau").transform.GetChild(0))
        {
            if (piece.GetComponent<Rigidbody2D>() != null) { vel = vel + piece.GetComponent<Rigidbody2D>().velocity; nb++; }
        }
        vaisseau.velocity = vel / nb;     
        vaisseau.position = VaisseauGameObject.transform.GetChild(0).GetChild(0).position;
             
        Assemblage deck = deckList.Find(vaisseau.Deck);
        vaisseau.eulerAngle.z = VaisseauGameObject.transform.GetChild(0).GetChild(0).eulerAngles.z - 
                                             deck.assemblage[0].eulerAngle.z;



        // ---- Sauvegarde de la vie et du niveau des pièces du vaisseau
        int i = 0;
        foreach (Transform child in VaisseauGameObject.transform)
        {
            if (child.GetComponent<Piece>() != null && i < vaisseau.pieces.assemblage.Count)
            {
                vaisseau.pieces.assemblage[i].vie = child.GetComponent<Piece>().vie;
                vaisseau.pieces.assemblage[i].niveau = child.GetComponent<Piece>().niveau;
                i++;
            }
        }

        string json = JsonUtility.ToJson(vaisseau);
        File.WriteAllText(SAVE_FOLDER + saveSlot + "_vaisseau.json", json);
    }
    
    public static VaisseauClass LoadVaisseau(string saveSlot = "default")
    {
        string path = SAVE_FOLDER + saveSlot + "_vaisseau.json";
        
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<VaisseauClass>(json);
        }
        else
        {
            Debug.LogWarning("Save file not found in " + path);
            path =  SAVE_FOLDER + "start_vaisseau.json";
            return JsonUtility.FromJson<VaisseauClass>(File.ReadAllText(path));
        }
    }
    #endregion



    

}