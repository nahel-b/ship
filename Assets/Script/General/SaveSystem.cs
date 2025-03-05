using System.IO;
using UnityEngine;
using System.Collections.Generic;

public class SaveSystem : MonoBehaviour
{
    private static readonly string SAVE_FOLDER = Application.persistentDataPath + "/Saves/";
    private static readonly string SOURCE_DATA_FOLDER = Application.dataPath + "/JSON/";
    
    public static void Initialize()
    {
        if (!Directory.Exists(SAVE_FOLDER))
        {
            Directory.CreateDirectory(SAVE_FOLDER);
        }
        // string defaultSavePath = SAVE_FOLDER + "default/";
        // if (!Directory.Exists(defaultSavePath))
        // {
        //     Directory.CreateDirectory(defaultSavePath);
        // }
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
        else //if (File.Exists(SOURCE_DATA_FOLDER + "start_decks.json"))
        {
            Debug.LogWarning("Save file not found in " + path);
            path = SOURCE_DATA_FOLDER + "start_decks.json";
            return JsonUtility.FromJson<DeckList>(File.ReadAllText(path));
        }
        // else
        // {
        //     Debug.LogWarning("No deck save files found. Checking PlayerPrefs...");
        //     if (PlayerPrefs.HasKey("Deck"))
        //     {
        //         Debug.Log("Loading deck from PlayerPrefs");
        //         string deckJson = PlayerPrefs.GetString("Deck");
        //         File.WriteAllText(SAVE_FOLDER + "start_decks.json", deckJson);
        //         return JsonUtility.FromJson<DeckList>(deckJson);
        //     }
        //     else
        //     {
        //         Debug.LogError("No deck data found anywhere! Creating empty DeckList.");
        //         return null;
        //     }
        // }
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
        else// if (File.Exists(SAVE_FOLDER + "start_vaisseau.json"))
        {
            Debug.LogWarning("Save file not found in " + path);
            path = SOURCE_DATA_FOLDER + "start_vaisseau.json";
            return JsonUtility.FromJson<VaisseauClass>(File.ReadAllText(path));
        }
        // else
        // {
        //     Debug.LogWarning("No vaisseau save files found. Checking PlayerPrefs...");
        //     if (PlayerPrefs.HasKey("Vaisseau"))
        //     {
        //         Debug.Log("Loading vaisseau from PlayerPrefs");
        //         string vaisseauJson = PlayerPrefs.GetString("Vaisseau");
        //         File.WriteAllText(SAVE_FOLDER + "start_vaisseau.json", vaisseauJson);
        //         return JsonUtility.FromJson<VaisseauClass>(vaisseauJson);
        //     }
        //     else
        //     {
        //         Debug.LogError("No vaisseau data found anywhere! Creating empty VaisseauClass.");
        //         return null;
        //     }
        // }
    }
    #endregion


#region Grandeur
    public static void SaveGrandeur(Grandeur grandeur, string saveSlot = "default")
    {
        string json = JsonUtility.ToJson(grandeur);
        File.WriteAllText(SAVE_FOLDER + saveSlot + "_grandeur.json", json);
    }
    
    public static Grandeur LoadGrandeur(string saveSlot = "default")
    {
        string path = SAVE_FOLDER + saveSlot + "_grandeur.json";
        
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<Grandeur>(json);
        }
        else //if (File.Exists(SAVE_FOLDER + "start_grandeur.json"))
        {
            Debug.LogWarning("Save file not found in " + path);
            path = SOURCE_DATA_FOLDER + "start_grandeur.json";
            return JsonUtility.FromJson<Grandeur>(File.ReadAllText(path));
        }
        // else
        // {
        //     Debug.LogWarning("No grandeur save files found. Checking PlayerPrefs...");
        //     if (PlayerPrefs.HasKey("Grandeur"))
        //     {
        //     Debug.Log("Loading grandeur from PlayerPrefs");
        //     string grandeurJson = PlayerPrefs.GetString("Grandeur");
        //     File.WriteAllText(SAVE_FOLDER + "start_grandeur.json", grandeurJson);
        //     return JsonUtility.FromJson<Grandeur>(grandeurJson);
        //     }
        //     else
        //     {
        //     Debug.LogError("No grandeur data found anywhere! Creating empty Grandeur.");
        //     return new Grandeur();
        //     }
        // }
        
        Debug.LogError("Aucune grandeur trouvée!");
        return new Grandeur();
    }
    #endregion
    
    // #region Items
    // public static void SaveItems(ItemClass items, string saveSlot = "default")
    // {
    //     ListsaveObj saveObj = new ListsaveObj();
    //     saveObj.Items = items;
        
    //     // Préserver currentSpot d'une sauvegarde précédente si elle existe
    //     if (PlayerPrefs.HasKey("saveObj"))
    //     {
    //         ListsaveObj previousSave = JsonUtility.FromJson<ListsaveObj>(PlayerPrefs.GetString("saveObj"));
    //         saveObj.currentSpot = previousSave.currentSpot;
    //     }
        
    //     // Sauvegarder les flèches
    //     foreach (GameObject fleche in GameObject.FindGameObjectsWithTag("fleche"))
    //     {
    //         if (fleche.GetComponent<Fleche>().target != null)
    //         {
    //             saveObj.fleches.Add(new FlecheClass(
    //                 fleche.GetComponent<Image>().color,
    //                 fleche.GetComponent<Fleche>().targetName,
    //                 fleche.GetComponent<Fleche>().showWayPoint
    //             ));
    //         }
    //     }
        
    //     string json = JsonUtility.ToJson(saveObj);
    //     File.WriteAllText(SAVE_FOLDER + saveSlot + "_items.json", json);
    //     PlayerPrefs.SetString("saveObj", json); // Pour compatibilité
    // }
    
    // public static ListsaveObj LoadItems(string saveSlot = "default")
    // {
    //     string path = SAVE_FOLDER + saveSlot + "_items.json";
        
    //     if (File.Exists(path))
    //     {
    //         string json = File.ReadAllText(path);
    //         return JsonUtility.FromJson<ListsaveObj>(json);
    //     }
    //     else
    //     {
    //         Debug.LogWarning("Save file not found in " + path);
    //         path = SAVE_FOLDER + "start_items.json";
    //         return JsonUtility.FromJson<ListsaveObj>(File.ReadAllText(path));
    //     }
        
    //     Debug.LogWarning("Aucun item trouvé!");
    //     return new ListsaveObj();
    // }
    // #endregion
    
    #region Colors
    public static void SaveColors(ColorList colorList, string saveSlot = "default")
    {
        string json = JsonUtility.ToJson(colorList);
        File.WriteAllText(SAVE_FOLDER + saveSlot + "_colors.json", json);
        PlayerPrefs.SetString("Color", json); // Pour compatibilité
    }
    
    public static ColorList LoadColors(string saveSlot = "default")
    {
        string path = SAVE_FOLDER + saveSlot + "_colors.json";
        
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<ColorList>(json);
        }
        else //if (File.Exists(SAVE_FOLDER + "start_colors.json"))
        {
            Debug.LogWarning("Save file not found in " + path);
            path = SOURCE_DATA_FOLDER + "start_colors.json";
            return JsonUtility.FromJson<ColorList>(File.ReadAllText(path));
        }
        // else
        // {
        //     Debug.LogWarning("No color save files found. Checking PlayerPrefs...");
        //     if (PlayerPrefs.HasKey("Color"))
        //     {
        //         Debug.Log("Loading colors from PlayerPrefs");
        //         string colorJson = PlayerPrefs.GetString("Color");
        //         File.WriteAllText(SAVE_FOLDER + "start_colors.json", colorJson);
        //         return JsonUtility.FromJson<ColorList>(colorJson);
        //     }
        //     else
        //     {
        //         Debug.LogError("No color data found anywhere! Creating empty ColorList.");
        //         return new ColorList();
        //     }
        // }
    }
    #endregion
    
    #region Enemies
    public static void SaveEnemies(string saveSlot = "default")
    {
        ListEnnemie ennemieList = new ListEnnemie();

        foreach (GameObject ennemie in GameObject.FindGameObjectsWithTag("ennemie"))
        {
            Ennemie sc = ennemie.GetComponent<Ennemie>();
            int index = 0;
            Assemblage vaisseauTheorique = sc.vaisseau;
            Assemblage vaisseauEnnemie = new Assemblage();
            
            foreach (Transform child in ennemie.transform)
            {
                if (child.GetComponent<Piece>() != null && index < vaisseauTheorique.assemblage.Count)
                {
                    PieceClass piecetheo = vaisseauTheorique.assemblage[child.GetComponent<Piece>().index];
                    vaisseauEnnemie.assemblage.Add(piecetheo);
                    vaisseauEnnemie.assemblage[index].vie = child.GetComponent<Piece>().vie;
                    index++;
                }
            }
            
            ennemieList.Ennemies.Add(new EnnemieClass(
                sc.nom,
                sc.SpeedRotation,
                sc.zone,
                sc.speed, 
                vaisseauEnnemie, 
                sc.agressif,
                ennemie.transform.eulerAngles.z,
                ennemie.transform.position,
                0
            ));
        }
        
        string json = JsonUtility.ToJson(ennemieList);
        File.WriteAllText(SAVE_FOLDER + saveSlot + "_enemies.json", json);
    }
    
    public static ListEnnemie LoadEnemies(string saveSlot = "default")
    {
        string path = SAVE_FOLDER + saveSlot + "_enemies.json";
        
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<ListEnnemie>(json);
        }
        else if (File.Exists(SAVE_FOLDER + "start_enemies.json"))
        {
            Debug.LogWarning("Save file not found in " + path);
            path = SAVE_FOLDER + "start_enemies.json";
            return JsonUtility.FromJson<ListEnnemie>(File.ReadAllText(path));
        }
        else
        {
            Debug.LogWarning("No enemies save files found. Checking PlayerPrefs...");
            if (PlayerPrefs.HasKey("Enemies"))
            {
                Debug.Log("Loading enemies from PlayerPrefs");
                string enemiesJson = PlayerPrefs.GetString("Enemies");
                File.WriteAllText(SAVE_FOLDER + "start_enemies.json", enemiesJson);
                return JsonUtility.FromJson<ListEnnemie>(enemiesJson);
            }
            else
            {
                Debug.LogError("No enemies data found anywhere! Creating empty ListEnnemie.");
                return new ListEnnemie();
            }
        }
    }
    #endregion


    public static void SaveAllScene(Principal principal, string saveSlot = "default")
    {
        Initialize();
        
        
        // Sauvegarder les items
        //SaveItems(principal.Items, saveSlot);
        
        // Sauvegarder les propriétés du jeu
        SaveGrandeur(principal.Grandeur, saveSlot);
        
        // Sauvegarder les couleurs
        ColorList colorList = GameObject.Find("Liste").GetComponent<Liste>().colorList;
        SaveColors(colorList, saveSlot);
        
        // Sauvegarder les decks et le vaisseau
        SaveDeckScene(saveSlot);
        SaveVaisseauScene(saveSlot);
        
        // Sauvegarder les ennemis
        SaveEnemies(saveSlot);
        
        Debug.Log("Sauvegarde complète effectuée avec succès dans " + saveSlot);
    }

    

}