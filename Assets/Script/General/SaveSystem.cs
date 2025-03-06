using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

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
    
    #region SaveObj
    public static void SaveObj(ItemClass items, string saveSlot = "default")
    {
        ListsaveObj saveObj = new ListsaveObj();
        saveObj.Items = items;
        saveObj.currentSpot = LoadSaveObj(saveSlot).currentSpot;
        // Get current location/spot
        GameObject player = GameObject.Find("Vaisseau");
        if (player != null)
        {
            // Determine current spot - check if player is near a station
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(player.transform.position, 10f);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.GetComponent<Station>() != null)
                {
                    saveObj.currentSpot = hitCollider.transform.name;
                    break;
                }
            }
        }
        
        // Save arrows
        saveObj.fleches = new List<FlecheClass>();
        foreach (GameObject fleche in GameObject.FindGameObjectsWithTag("fleche"))
        {
            if (fleche.GetComponent<Fleche>() != null && fleche.GetComponent<Fleche>().targetName != null)
            {
                saveObj.fleches.Add(new FlecheClass(
                    fleche.GetComponent<Image>().color,
                    fleche.GetComponent<Fleche>().targetName,
                    fleche.GetComponent<Fleche>().showWayPoint
                ));
            }
        }
        
        string json = JsonUtility.ToJson(saveObj);
        File.WriteAllText(SAVE_FOLDER + saveSlot + "_items.json", json);
        PlayerPrefs.SetString("saveObj", json); // For compatibility
    }

    public static ListsaveObj LoadSaveObj(string saveSlot = "default")
{
    string path = SAVE_FOLDER + saveSlot + "_items.json";
    
    if (File.Exists(path))
    {
        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<ListsaveObj>(json);
    }
    else //if (File.Exists(SOURCE_DATA_FOLDER + "start_items.json"))
    {
        Debug.LogWarning("Save file not found in " + path);
        path = SOURCE_DATA_FOLDER + "start_items.json";
        
        if (File.Exists(path))
        {
            return JsonUtility.FromJson<ListsaveObj>(File.ReadAllText(path));
        }
        else
        {
            Debug.LogWarning("No items save files found. Checking PlayerPrefs...");
            
            string saveObjJson = JsonUtility.ToJson(new ListsaveObj());
            File.WriteAllText(SOURCE_DATA_FOLDER + "start_items.json", saveObjJson);
            
            return new ListsaveObj();
            

        }
    }
}

    public static void RecreateArrows(ListsaveObj saveObj)
    {
        // First clear any existing arrows
        GameObject arrowsContainer = GameObject.Find("Fleches");
        if (arrowsContainer != null)
        {
            foreach (Transform child in arrowsContainer.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
        else
        {
            Debug.LogWarning("Fleches container not found!");
            return;
        }

        // Create new arrows from save data
        if (saveObj != null && saveObj.fleches != null)
        {
            foreach (FlecheClass fleche in saveObj.fleches)
            {
                Principal principal = GameObject.Find("Main Camera").GetComponent<Principal>();
                if (principal != null)
                {
                    principal.Fleche(fleche.targetName, fleche.showWayPoint, fleche.color);
                }
                else
                {
                    Debug.LogError("Principal component not found on Main Camera!");
                }
            }
        }
    }




    #endregion
    
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


    public static void resetSave(string saveSlot = "default")
    {
        File.Delete(SAVE_FOLDER + saveSlot + "_decks.json");
        File.Delete(SAVE_FOLDER + saveSlot + "_vaisseau.json");
        File.Delete(SAVE_FOLDER + saveSlot + "_grandeur.json");
        File.Delete(SAVE_FOLDER + saveSlot + "_items.json");
        File.Delete(SAVE_FOLDER + saveSlot + "_colors.json");
        File.Delete(SAVE_FOLDER + saveSlot + "_enemies.json");
    }

    public static void SaveAllScene(Principal principal, string saveSlot = "default")
    {
        Initialize();
        
        
        // Sauvegarder les items
        //SaveObj(principal.Items, saveSlot);


        GameObject.Find("MissionManager").GetComponent<MissionManager>().Save();

        
        // Sauvegarder les propriétés du jeu
        SaveGrandeur(principal.Grandeur, saveSlot);

        SaveObj(principal.Items, saveSlot);
        
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


    public static void LoadAllScene()
    {
        Initialize();
        
       ListsaveObj saveObj = LoadSaveObj();
        if (saveObj != null)
        {
            GameObject.Find("Main Camera").GetComponent<Principal>().Items = saveObj.Items;
            RecreateArrows(saveObj);
        }
         GameObject.Find("Main Camera").GetComponent<Principal>().Items = saveObj.Items;

        
        // Charger les missions
        GameObject.Find("MissionManager").GetComponent<MissionManager>().Load();
        
        // Charger les propriétés du jeu
        GameObject.Find("Main Camera").GetComponent<Principal>().Grandeur = LoadGrandeur();
        
        // Couleurs
        ColorList colorList = LoadColors();
        GameObject.Find("Liste").GetComponent<Liste>().colorList = colorList;
        
        // Charger les decks et le vaisseau
        DeckList deckList = LoadDeck();
        GameObject.Find("Liste").GetComponent<Liste>().deckList = deckList;
        VaisseauClass vaisseau = LoadVaisseau();
       // GameObject.Find("Vaisseau").GetComponent<Vaisseau>().vaisseau = vaisseau;
       instancierVaisseau(vaisseau,deckList);
        
        // Charger les ennemis
        ListEnnemie ennemieList = LoadEnemies();
        GameObject.Find("Liste").GetComponent<Liste>().EnnemieList = ennemieList;
        
        Debug.Log("Chargement complet effectué avec succès");
    } 



    static void instancierVaisseau(VaisseauClass vaisseau,DeckList deckList)
    {
        GameObject VaisseauGameObject = GameObject.Find("Vaisseau");
        GameObject Liste = GameObject.Find("Liste");

        

        foreach (Transform child in VaisseauGameObject.transform.GetChild(0).transform) { if (!child.name.Contains("Deck")) { DestroyImmediate(child.gameObject); } }
        int i = 0;
        foreach (PieceClass p in deckList.Find(vaisseau.Deck).assemblage)
        {
//            print(p.nom);
            GameObject pObj = Instantiate(PieceLoader.GetPiecePrefab(p.nom), p.position, Quaternion.Euler(p.eulerAngle));
            pObj.name = p.nom;
            foreach (FixedJoint2D component in pObj.GetComponents<FixedJoint2D>()) { Destroy(component); }
            pObj.transform.parent = VaisseauGameObject.transform.GetChild(0).transform;
            pObj.GetComponent<Piece>().niveau = p.niveau;
            pObj.GetComponent<Piece>().attchableSide = p.attchableSide;
            pObj.GetComponent<Piece>().dependant = p.dependant;
            pObj.GetComponent<Piece>().rotFrame = p.rotFrame;
            pObj.GetComponent<Piece>().socle = p.socle;
            
            if (p.vie == -1 && !p.dependant) { pObj.GetComponent<Piece>().vie = PieceLoader.GetPiecePrefab(p.nom).GetComponent<Piece>().vieListe[p.niveau]; }
            else if (!p.dependant) { pObj.GetComponent<Piece>().vie = p.vie; }

            if (p.dependant)
            {
                pObj.transform.position = new Vector3(pObj.transform.position.x, pObj.transform.position.y, -1);
                RaycastHit2D[] hit = Physics2D.RaycastAll(pObj.transform.position, Vector3.zero, 0.3f);
                foreach (RaycastHit2D obj in hit)
                {
                    if ((obj.transform.parent == VaisseauGameObject.transform || obj.transform.parent == VaisseauGameObject.transform.GetChild(0)) && obj.transform.GetComponent<Piece>().socle)
                    {
                        pObj.transform.parent = obj.transform;
                        pObj.GetComponent<Piece>().attachedObject = obj.transform.gameObject;
                        //FixedJoint2D c = pObj.transform.gameObject.AddComponent<FixedJoint2D>();
                        //c.connectedBody = obj.transform.GetComponent<FixedJoint2D>();
                    }
                }
            }
            else
            {
                pObj.GetComponent<Rigidbody2D>().angularDrag = 1;
                pObj.GetComponent<Piece>().index = i;
                i++;
            }

        }
        foreach (PieceClass p in vaisseau.pieces.assemblage)
        {

            GameObject pObj = Instantiate(PieceLoader.GetPiecePrefab(p.nom), p.position, Quaternion.Euler(p.eulerAngle));

            pObj.name = p.nom;
            foreach (FixedJoint2D component in pObj.GetComponents<FixedJoint2D>()) { Destroy(component); }
            pObj.transform.parent = VaisseauGameObject.transform; 
            pObj.GetComponent<Piece>().niveau = p.niveau;
            pObj.GetComponent<Piece>().attchableSide = p.attchableSide;
            pObj.GetComponent<Piece>().dependant = p.dependant;
            pObj.GetComponent<Piece>().rotFrame = p.rotFrame;
            pObj.GetComponent<Piece>().socle = p.socle;

            if (p.vie == -1 && !p.dependant) { pObj.GetComponent<Piece>().vie = PieceLoader.GetPiecePrefab(p.nom).GetComponent<Piece>().vieListe[p.niveau]; }
            else if(!p.dependant) { pObj.GetComponent<Piece>().vie = p.vie; }

            if (p.dependant)
            {
                pObj.transform.position = new Vector3(pObj.transform.position.x, pObj.transform.position.y, -1);

                //print(pObj.name);
                RaycastHit2D[] hit = Physics2D.RaycastAll(pObj.transform.position, Vector3.zero, 0.3f);
                foreach (RaycastHit2D obj in hit)
                {
                    //print(pObj.name + "->" + obj.transform.name);
                    if ((obj.transform.parent == VaisseauGameObject.transform || obj.transform.parent == VaisseauGameObject.transform.GetChild(0)) && obj.transform.GetComponent<Piece>().socle)
                    {
                        pObj.transform.parent = obj.transform;
                        pObj.GetComponent<Piece>().attachedObject = obj.transform.gameObject;
                        //RelativeJoint2D c = pObj.transform.gameObject.AddComponent<RelativeJoint2D>();
                        //c.connectedBody = obj.transform.GetComponent<Rigidbody2D>();
                    }
                }
            }
            else
            {
                pObj.GetComponent<Rigidbody2D>().angularDrag = 1;
                pObj.GetComponent<Piece>().index = i;
                i++;
            }
        }
        

       
        //attachement des decks entre eux
        foreach (Transform child in VaisseauGameObject.transform.GetChild(0).transform)
        {


            RaycastHit2D[] hit = Physics2D.RaycastAll(child.position, Vector3.up,0.6f);
            foreach (RaycastHit2D obj in hit)
            {
                //meme parent
                if ((obj.transform.GetComponent<Piece>() != null && obj.transform.gameObject != child.transform.gameObject && obj.transform.parent == child.transform.parent) )
                {

                    FixedJoint2D c = child.gameObject.AddComponent<FixedJoint2D>();
                    c.connectedBody = obj.transform.GetComponent<Rigidbody2D>();
                }
            }
            hit = Physics2D.RaycastAll(child.position, Vector3.right, 0.6f);
            foreach (RaycastHit2D obj in hit)
            {
                //print(child.transform.name + "-" + obj.transform.name);
                //print(obj.transform.GetComponent<Piece>().attachable + "-" + obj.transform.name);
                if ((obj.transform.GetComponent<Piece>() != null && obj.transform.gameObject != child.transform.gameObject && obj.transform.parent == child.transform.parent))
                {
                    FixedJoint2D c = child.gameObject.AddComponent<FixedJoint2D>();
                    c.connectedBody = obj.transform.GetComponent<Rigidbody2D>();
                }
            }
        }
        foreach (Transform child in VaisseauGameObject.transform.GetChild(0).transform)
        {


            RaycastHit2D[] hit = Physics2D.RaycastAll(child.position, Vector3.up, 0.6f) ;
            foreach (RaycastHit2D obj in hit)
            {
                //meme parent + attachable haut
                if (( child.GetComponent<Piece>().attchableSide.getList()[Mathf.RoundToInt(Principal.Clamp0360(child.transform.eulerAngles.z / 90))] && obj.transform.GetComponent<Piece>().attchableSide.getList()[Mathf.RoundToInt( Principal.Clamp0360(obj.transform.eulerAngles.z/90))+2] && obj.transform.GetComponent<Piece>() != null && obj.transform.gameObject != child.transform.gameObject ))
                {
                    FixedJoint2D c = child.gameObject.AddComponent<FixedJoint2D>();
                    c.connectedBody = obj.transform.GetComponent<Rigidbody2D>();
                }
            }
            hit = Physics2D.RaycastAll(child.position, Vector3.right, 0.6f);
            foreach (RaycastHit2D obj in hit)
            {
                
                //meme parent + attachable haut
                if (( child.GetComponent<Piece>().attchableSide.getList()[Mathf.RoundToInt(Principal.Clamp0360(child.transform.eulerAngles.z / 90) )+1] && obj.transform.GetComponent<Piece>().attchableSide.getList()[Mathf.RoundToInt(Principal.Clamp0360(obj.transform.eulerAngles.z / 90)) + 3] && obj.transform.GetComponent<Piece>() != null && obj.transform.gameObject != child.transform.gameObject ))
                {
                    FixedJoint2D c = child.gameObject.AddComponent<FixedJoint2D>();
                    c.connectedBody = obj.transform.GetComponent<Rigidbody2D>();
                }
            }
            hit = Physics2D.RaycastAll(child.position, -Vector3.up, 0.6f);
            foreach (RaycastHit2D obj in hit)
            {
                //meme parent + attachable haut
                if (( child.GetComponent<Piece>().attchableSide.getList()[Mathf.RoundToInt(Principal.Clamp0360(child.transform.eulerAngles.z / 90) ) + 2] && obj.transform.GetComponent<Piece>().attchableSide.getList()[Mathf.RoundToInt(Principal.Clamp0360(obj.transform.eulerAngles.z / 90) ) + 4] && obj.transform.GetComponent<Piece>() != null && obj.transform.gameObject != child.transform.gameObject ))
                {
                    FixedJoint2D c = child.gameObject.AddComponent<FixedJoint2D>();
                    c.connectedBody = obj.transform.GetComponent<Rigidbody2D>();
                }
            }
            hit = Physics2D.RaycastAll(child.position, -Vector3.right, 0.6f);
            foreach (RaycastHit2D obj in hit)
            {
                //meme parent + attachable haut
                if ( child.GetComponent<Piece>().attchableSide.getList()[Mathf.RoundToInt(Principal.Clamp0360(child.transform.eulerAngles.z / 90) ) + 3] && obj.transform.GetComponent<Piece>().attchableSide.getList()[Mathf.RoundToInt(Principal.Clamp0360(obj.transform.eulerAngles.z / 90) ) + 5] && obj.transform.GetComponent<Piece>() != null && obj.transform.gameObject != child.transform.gameObject )
                {
                    FixedJoint2D c = child.gameObject.AddComponent<FixedJoint2D>();
                    c.connectedBody = obj.transform.GetComponent<Rigidbody2D>();
                }
            }


        }

        VaisseauGameObject.transform.localPosition = vaisseau.position;
        VaisseauGameObject.transform.localEulerAngles = vaisseau.eulerAngle;
    }

    #region BuildSystem
    public static void SaveBuildScene(string saveSlot = "default")
    {
        // Get BuildPrincipal references
        BuildPrincipal buildPrincipal = Camera.main.GetComponent<BuildPrincipal>();
        
        // Save items
        SaveObj(buildPrincipal.Items, saveSlot);
        
        // Save ship configuration
        VaisseauClass vs = new VaisseauClass();
        
        // Get previous Vaisseau data to preserve some properties
        VaisseauClass previousData = LoadVaisseau(saveSlot);
        vs.position = previousData.position;
        vs.eulerAngle = previousData.eulerAngle;
        vs.velocity = previousData.velocity;
        
        // Set selected deck
        vs.Deck = buildPrincipal.debloqueDeck.Liste[buildPrincipal.DeckIndex].nom;
        
        // Save all pieces on the ship
        if (GameObject.Find("Vaisseau").transform.childCount > 1)
        {
            foreach (Transform p in GameObject.Find("Vaisseau").transform)
            {
                if (p.GetComponent<BuildPiece>() != null)
                {
                    BuildPiece bp = p.GetComponent<BuildPiece>();
                    vs.pieces.Add(new PieceClass(p.position, p.eulerAngles, p.name, 
                        bp.description, bp.niveau, bp.dependant, bp.socle, 
                        bp.rotFrame, bp.attchableSide, bp.vie));
                }
            }
            
            // Handle nested pieces in Vaisseau
            foreach (Transform p in GameObject.Find("Vaisseau").transform.GetChild(0))
            {
                if (p.childCount > 0 && p.GetChild(0).GetComponent<BuildPiece>() != null)
                {
                    BuildPiece bp = p.GetChild(0).GetComponent<BuildPiece>();
                    vs.pieces.Add(new PieceClass(p.GetChild(0).position, p.GetChild(0).eulerAngles, 
                        p.GetChild(0).name, bp.description, bp.niveau, bp.dependant, 
                        bp.socle, bp.rotFrame, bp.attchableSide, bp.vie));
                }
            }
            
            // Handle other nested pieces
            foreach (Transform p in GameObject.Find("Vaisseau").transform)
            {
                if (p.childCount > 0 && p.GetChild(0).GetComponent<BuildPiece>() != null 
                    && p.GetComponent<BuildPiece>() != null)
                {
                    BuildPiece bp = p.GetChild(0).GetComponent<BuildPiece>();
                    vs.pieces.Add(new PieceClass(p.GetChild(0).position, p.GetChild(0).eulerAngles, 
                        p.GetChild(0).name, bp.description, bp.niveau, bp.dependant, 
                        bp.socle, bp.rotFrame, bp.attchableSide, bp.vie));
                }
            }
        }
        
        // Save Vaisseau to file
        string json = JsonUtility.ToJson(vs);
        File.WriteAllText(SAVE_FOLDER + saveSlot + "_vaisseau.json", json);
        PlayerPrefs.SetString("Vaisseau", json); // For compatibility
        
        // Update deck information
        DeckList d = GameObject.Find("Liste").GetComponent<Liste>().deckList;
        int index = 0;
        foreach (Transform child in GameObject.Find("Vaisseau").transform.GetChild(0))
        {
            if (child.GetComponent<BuildPiece>() != null && index < d.Find(vs.Deck).assemblage.Count)
            {
                d.Find(vs.Deck).assemblage[index].vie = child.GetComponent<BuildPiece>().vie;
                d.Find(vs.Deck).assemblage[index].niveau = child.GetComponent<BuildPiece>().niveau;
                index++;
            }
        }
        
        // Save deck list to file
        string deckJson = JsonUtility.ToJson(d);
        File.WriteAllText(SAVE_FOLDER + saveSlot + "_decks.json", deckJson);
        PlayerPrefs.SetString("Deck", deckJson); // For compatibility
    }
    
    public static void LoadBuildScene(string saveSlot = "default")
    {
        BuildPrincipal buildPrincipal = Camera.main.GetComponent<BuildPrincipal>();
        
        // Load Deck List
        DeckList deckList = LoadDeck(saveSlot);
        GameObject.Find("Liste").GetComponent<Liste>().deckList = deckList;
        buildPrincipal.deckList = deckList;
        
        // Load Items
        ListsaveObj saveObj = LoadSaveObj(saveSlot);
        if (saveObj != null)
        {
            buildPrincipal.Items = saveObj.Items;
        }
        
        // Load Vaisseau
        VaisseauClass vaisseau = LoadVaisseau(saveSlot);
        buildPrincipal.currentDeck = vaisseau.Deck;
        buildPrincipal.Vaisseau = vaisseau;

        // Populate deck options
        buildPrincipal.debloqueDeck = new DeckList();
        foreach (Assemblage deck in deckList.Liste)
        {
            if (deck.debloque)
            {
                buildPrincipal.debloqueDeck.Liste.Add(deck);
            }
        }
        
        // Set current deck index
        for (int i = 0; i < buildPrincipal.debloqueDeck.Liste.Count; i++)
        {
            if (buildPrincipal.debloqueDeck.Liste[i].nom == buildPrincipal.currentDeck)
            {
                buildPrincipal.DeckIndex = i;
                break;
            }
        }
        
        // Update UI
        if (GameObject.Find("Deck-title-Text") != null)
        {
            GameObject.Find("Deck-title-Text").GetComponent<Text>().text = 
                buildPrincipal.debloqueDeck.Liste[buildPrincipal.DeckIndex].nom;
        }
        
        return;
    }
    
    public static bool VerifyShipBuild()
    {
        bool hasProblems = false;
        foreach(Transform p in GameObject.Find("Vaisseau").transform)
        {
            if(p.GetComponent<BuildPiece>() != null)
            {
                if(!string.IsNullOrEmpty(p.GetComponent<BuildPiece>().Verify()))
                {
                    hasProblems = true;
                    break;
                }
            }
        }
        return !hasProblems;
    }
    #endregion

}