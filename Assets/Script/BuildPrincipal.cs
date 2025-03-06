using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class BuildPrincipal : MonoBehaviour
{
    public GameObject selectObj;
    public Assemblage Vaisseaui;
    public VaisseauClass Vaisseau;
    string path ;
    public DeckList deckList;
    public string currentDeck;
    public int DeckIndex;
    public DeckList debloqueDeck = new DeckList();
    public ItemClass  Items;
    public GameObject ItemObjPrefab;
    public GameObject BoutonsPiece;
    public GameObject BoutonsDeck;



    // Start is called before the first frame update
    void Start()
    {
        // GameObject.Find("Liste").GetComponent<Liste>().deckList = JsonUtility.FromJson<DeckList>(PlayerPrefs.GetString("Deck"));
        // deckList = GameObject.Find("Liste").GetComponent<Liste>().deckList;



        // //path = System.IO.Path.Combine(Application.persistentDataPath, "data.dat");
        // if (PlayerPrefs.HasKey("saveObj"))
        // {
        //     Items = JsonUtility.FromJson<ListsaveObj>(PlayerPrefs.GetString("saveObj")).Items;
        // }

        SaveSystem.LoadBuildScene();

        CreateItemObjects();

        foreach (PieceClass p in deckList.Find(currentDeck).assemblage)
        {
            PlacePiece(p, GameObject.Find("Deck").transform);
        }
        
        // Place ship pieces
        foreach (PieceClass p in Vaisseau.pieces.assemblage)
        {
            PlacePiece(p, GameObject.Find("Vaisseau").transform);
        }


        // foreach (PieceClass p in Items.Pieces)
        // {

        //     print(p.nom);

        //     GameObject a = Instantiate(ItemObjPrefab, GameObject.Find("ContentScrollItem").transform);
        //     a.GetComponent<Image>().sprite = PieceLoader.GetPiecePrefab(p.nom).GetComponent<SpriteRenderer>().sprite;
        //     a.transform.GetChild(1).GetComponent<Text>().text = p.nom;
        //     if(PieceLoader.GetPiecePrefab(p.nom).GetComponent<SpriteRenderer>().flipX) { a.transform.localScale = new Vector3(-1, 1, 1); a.transform.GetChild(1).localScale = new Vector3(-1, 1, 1); a.transform.GetChild(0).localScale = new Vector3(-1.14f, 1.14f, 1); }
        // }



        //string json = JsonUtility.ToJson(Vaisseau);
        //PlayerPrefs.SetString("Vaisseau", json);

        //Debug.Log("Saving as JSON: " + json);
        //System.IO.File.WriteAllText(path, json);
        // VaisseauClass jsonF = JsonUtility.FromJson<VaisseauClass>(PlayerPrefs.GetString("Vaisseau", JsonUtility.ToJson(Vaisseau)));
        // currentDeck = jsonF.Deck;
        // foreach (PieceClass p in deckList.Find(currentDeck).assemblage)
        // {
        //     GameObject pObj = Instantiate(PieceLoader.GetPiecePrefab(p.nom), p.position, Quaternion.Euler(p.eulerAngle));
        //     pObj.name = p.nom;
        //     pObj.AddComponent<BuildPiece>();
        //     foreach (FixedJoint2D component in pObj.GetComponents<FixedJoint2D>()) { Destroy(component); }
        //     foreach (Piece component in pObj.GetComponents<Piece>()) {  Destroy(component); }
        //     foreach (Reacteur component in pObj.GetComponents<Reacteur>()) { Destroy(component); }
        //     foreach (Canon component in pObj.GetComponents<Canon>()) { Destroy(component); }
        //     if (pObj.GetComponent<BoxCollider2D>() != null) { pObj.GetComponent<BoxCollider2D>().isTrigger = true; }
        //     if (pObj.GetComponent<PolygonCollider2D>() != null) { Destroy(pObj.GetComponent<PolygonCollider2D>()); pObj.AddComponent<BoxCollider2D>(); pObj.GetComponent<BoxCollider2D>().isTrigger = true; }
        //     pObj.transform.parent = GameObject.Find("Deck").transform;
        //     pObj.GetComponent<BuildPiece>().objPrefab = PieceLoader.GetPiecePrefab(p.nom);
        //     pObj.GetComponent<BuildPiece>().niveau = p.niveau;
        //     pObj.GetComponent<BuildPiece>().attchableSide = p.attchableSide;
        //     pObj.GetComponent<BuildPiece>().dependant = p.dependant;
        //     pObj.GetComponent<BuildPiece>().socle = p.socle;
        //     pObj.GetComponent<BuildPiece>().rotFrame = p.rotFrame;
        //     if (p.vie == -1 && !p.dependant) { pObj.GetComponent<BuildPiece>().vie = PieceLoader.GetPiecePrefab(p.nom).GetComponent<Piece>().vieListe[p.niveau]; }
        //     else if (!p.dependant) { pObj.GetComponent<BuildPiece>().vie = p.vie; }


        //     if (p.dependant)
        //     {
        //         print(transform.name);

        //         RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, Vector3.up, 0.3f);
        //         foreach (RaycastHit2D obj in hit)
        //         {
        //             print(obj.transform.name);
        //             if (obj.transform.parent == GameObject.Find("Vaisseau").transform || obj.transform.parent == GameObject.Find("Vaisseau").transform.GetChild(0))
        //             {
        //                 //pObj.transform.parent = obj.transform;
        //                 pObj.transform.position = new Vector3(pObj.transform.position.x, pObj.transform.position.y, -1);
        //             }
        //         }
        //     }
        // }
        // foreach (PieceClass p in jsonF.pieces.assemblage)
        // {
        //     print(p.nom);
        //     GameObject pObj = Instantiate(PieceLoader.GetPiecePrefab(p.nom), p.position, Quaternion.Euler(p.eulerAngle));
        //     pObj.name = p.nom;
        //     pObj.AddComponent<BuildPiece>();
        //     foreach (FixedJoint2D component in pObj.GetComponents<FixedJoint2D>()) { Destroy(component); }
        //     foreach (Piece component in pObj.GetComponents<Piece>()) {  Destroy(component); }
        //     foreach (Reacteur component in pObj.GetComponents<Reacteur>()) { Destroy(component); }
        //     foreach (Canon component in pObj.GetComponents<Canon>()) { Destroy(component); }

        //     if (pObj.GetComponent<BoxCollider2D>() != null) { pObj.GetComponent<BoxCollider2D>().isTrigger = true; }
        //     if (pObj.GetComponent<PolygonCollider2D>() != null) { Destroy(pObj.GetComponent<PolygonCollider2D>()); pObj.AddComponent<BoxCollider2D>(); pObj.GetComponent<BoxCollider2D>().isTrigger = true; }
        //     pObj.transform.parent = GameObject.Find("Vaisseau").transform; 
        //     pObj.GetComponent<BuildPiece>().objPrefab = PieceLoader.GetPiecePrefab(p.nom);
        //     pObj.GetComponent<BuildPiece>().niveau = p.niveau;
        //     pObj.GetComponent<BuildPiece>().attchableSide = p.attchableSide;
        //     pObj.GetComponent<BuildPiece>().dependant = p.dependant;
        //     pObj.GetComponent<BuildPiece>().socle = p.socle;
        //     pObj.GetComponent<BuildPiece>().rotFrame = p.rotFrame;
        //     if (p.vie == -1 && !p.dependant) { pObj.GetComponent<BuildPiece>().vie = PieceLoader.GetPiecePrefab(p.nom).GetComponent<Piece>().vieListe[p.niveau]; }
        //     else if (!p.dependant) { pObj.GetComponent<BuildPiece>().vie = p.vie; }

        //     if (p.dependant)
        //     {

        //         RaycastHit2D[] hit = Physics2D.RaycastAll(pObj.transform.position, Vector3.zero, 0.3f);
        //         foreach (RaycastHit2D obj in hit)
        //         {
        //             if ((obj.transform.parent == GameObject.Find("Vaisseau").transform || obj.transform.parent == GameObject.Find("Vaisseau").transform.GetChild(0)) && obj.transform.GetComponent<BuildPiece>().socle)
        //             {
        //                 //pObj.transform.parent = obj.transform;
        //                 pObj.transform.position = new Vector3(pObj.transform.position.x, pObj.transform.position.y, -1);
        //             }
        //         }
        //     }
        // }
        


        // foreach (Assemblage deck in deckList.Liste)
        // {
        //     if (deck.debloque) { debloqueDeck.Liste.Add(deck); }
        // }
        // for (int i = 0; i < debloqueDeck.Liste.Count; i++) { if (debloqueDeck.Liste[i].nom == currentDeck) { print(i); DeckIndex = i; } }
        // GameObject.Find("Deck-title-Text").GetComponent<Text>().text = debloqueDeck.Liste[DeckIndex].nom;







        //print(JsonUtility.ToJson(Vaisseau));



        //Vaisseau[0] = JsonUtility.FromJson<Assemblage>(System.IO.File.ReadAllText(path));
        print(Items.maxItem());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void CreateItemObjects()
    {
        foreach (PieceClass p in Items.Pieces)
        {
            print(p.nom);
            GameObject a = Instantiate(ItemObjPrefab, GameObject.Find("ContentScrollItem").transform);
            a.GetComponent<Image>().sprite = PieceLoader.GetPiecePrefab(p.nom).GetComponent<SpriteRenderer>().sprite;
            a.transform.GetChild(1).GetComponent<Text>().text = p.nom;
            if(PieceLoader.GetPiecePrefab(p.nom).GetComponent<SpriteRenderer>().flipX) { 
                a.transform.localScale = new Vector3(-1, 1, 1); 
                a.transform.GetChild(1).localScale = new Vector3(-1, 1, 1); 
                a.transform.GetChild(0).localScale = new Vector3(-1.14f, 1.14f, 1); 
            }
        }
    }

    public GameObject PlacePiece(PieceClass pieceData, Transform parent, bool makeInteractive = false, Vector3? customPosition = null)
    {
        // Use custom position if provided, otherwise use the position from pieceData
        Vector3 position = customPosition ?? pieceData.position;
        
        // Instantiate the piece from the prefab
        GameObject pObj = Instantiate(PieceLoader.GetPiecePrefab(pieceData.nom), 
                                    position, 
                                    Quaternion.Euler(pieceData.eulerAngle));
        pObj.name = pieceData.nom;
        
        // Add BuildPiece component
        pObj.AddComponent<BuildPiece>();
        
        // Remove components we don't need in build mode
        foreach (FixedJoint2D component in pObj.GetComponents<FixedJoint2D>()) { Destroy(component); }
        foreach (Piece component in pObj.GetComponents<Piece>()) { 
            if (!pieceData.dependant) {
                pObj.GetComponent<BuildPiece>().vie = component.vie; 
            }
            Destroy(component); 
        }
        foreach (Reacteur component in pObj.GetComponents<Reacteur>()) { Destroy(component); }
        foreach (Canon component in pObj.GetComponents<Canon>()) { Destroy(component); }
        
        // Set up colliders
        if (pObj.GetComponent<BoxCollider2D>() != null) { 
            pObj.GetComponent<BoxCollider2D>().isTrigger = true; 
        }
        if (pObj.GetComponent<PolygonCollider2D>() != null) { 
            Destroy(pObj.GetComponent<PolygonCollider2D>()); 
            pObj.AddComponent<BoxCollider2D>(); 
            pObj.GetComponent<BoxCollider2D>().isTrigger = true; 
        }
        
        // Set parent transform
        pObj.transform.parent = parent;
        
        // Configure BuildPiece properties
        BuildPiece buildPiece = pObj.GetComponent<BuildPiece>();
        buildPiece.objPrefab = PieceLoader.GetPiecePrefab(pieceData.nom);
        buildPiece.niveau = pieceData.niveau;
        buildPiece.attchableSide = pieceData.attchableSide;
        buildPiece.dependant = pieceData.dependant;
        buildPiece.socle = pieceData.socle;
        buildPiece.rotFrame = pieceData.rotFrame;
        buildPiece.description = pieceData.description;
        
        // Set piece health
        if (pieceData.vie == -1 && !pieceData.dependant) { 
            buildPiece.vie = PieceLoader.GetPiecePrefab(pieceData.nom).GetComponent<Piece>().vieListe[pieceData.niveau]; 
        } else if (!pieceData.dependant) { 
            buildPiece.vie = pieceData.vie; 
        }
        
        // Handle dependent pieces positioning
        if (pieceData.dependant) {
            HandleDependentPiece(pObj);
        }
        
        // Make the piece interactive if requested
        if (makeInteractive) {
            buildPiece.sp = true;
            buildPiece.Start();
            buildPiece.OnMouseDown();
            buildPiece.OnMouseDrag();
        }
        
        return pObj;
    }

    private void HandleDependentPiece(GameObject pObj)
    {
        if (pObj.transform.parent == GameObject.Find("Deck").transform) {
            // For deck pieces
            RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, Vector3.up, 0.3f);
            foreach (RaycastHit2D obj in hit) {
                if (obj.transform.parent == GameObject.Find("Vaisseau").transform || 
                    obj.transform.parent == GameObject.Find("Vaisseau").transform.GetChild(0)) {
                    pObj.transform.position = new Vector3(pObj.transform.position.x, 
                                                        pObj.transform.position.y, -1);
                }
            }
        } else {
            // For ship pieces
            RaycastHit2D[] hit = Physics2D.RaycastAll(pObj.transform.position, Vector3.zero, 0.3f);
            foreach (RaycastHit2D obj in hit) {
                if ((obj.transform.parent == GameObject.Find("Vaisseau").transform || 
                    obj.transform.parent == GameObject.Find("Vaisseau").transform.GetChild(0)) && 
                    obj.transform.GetComponent<BuildPiece>().socle) {
                    pObj.transform.position = new Vector3(pObj.transform.position.x, 
                                                        pObj.transform.position.y, -1);
                }
            }
        }
    }

    public void Save()
    {

        SaveSystem.SaveBuildScene();


        //if (Items.Pieces.Count > 0)
        // //{
        //     ListsaveObj i = JsonUtility.FromJson<ListsaveObj>(PlayerPrefs.GetString("saveObj"));
        //     i.Items = Items;
        //     PlayerPrefs.SetString("saveObj", JsonUtility.ToJson(i));
        // //}
        // print("1");
        // VaisseauClass vs = new VaisseauClass();
        //  print("2");

        // VaisseauClass a = JsonUtility.FromJson<VaisseauClass>(PlayerPrefs.GetString("Vaisseau", JsonUtility.ToJson(Vaisseau)));
        // vs.position = a.position;
        // vs.eulerAngle = a.eulerAngle;
        // vs.velocity = a.velocity;
        // print("3");
        // //foreach(Transform p in GameObject.Find("Deck").transform)
        // //{
        // //    vs.pieces.Add(new PieceClass(p.position, p.eulerAngles, p.name, "", true, p.GetComponent<BuildPiece>().niveau, p.GetComponent<BuildPiece>().attachable));

        // //}
        // vs.Deck = debloqueDeck.Liste[DeckIndex].nom;
        // print(vs.Deck);
        // print("4");
        // if (GameObject.Find("Vaisseau").transform.childCount > 1)
        // {
        //     foreach (Transform p in GameObject.Find("Vaisseau").transform)
        //     {
        //         if (p.GetComponent<BuildPiece>()!=null)
        //         {
        //             vs.pieces.Add(new PieceClass(p.position, p.eulerAngles, p.name, p.GetComponent<BuildPiece>().description, p.GetComponent<BuildPiece>().niveau, p.GetComponent<BuildPiece>().dependant, p.GetComponent<BuildPiece>().socle, p.GetComponent<BuildPiece>().rotFrame, p.GetComponent<BuildPiece>().attchableSide, p.GetComponent<BuildPiece>().vie));
        //         }
        //     }
        //     foreach (Transform p in GameObject.Find("Vaisseau").transform.GetChild(0))
        //     {
        //         if (p.childCount >0 && p.GetChild(0).GetComponent<BuildPiece>() != null)
        //         {
        //             vs.pieces.Add(new PieceClass(p.GetChild(0).position, p.GetChild(0).eulerAngles, p.GetChild(0).name, p.GetChild(0).GetComponent<BuildPiece>().description, p.GetChild(0).GetComponent<BuildPiece>().niveau, p.GetChild(0).GetComponent<BuildPiece>().dependant, p.GetChild(0).GetComponent<BuildPiece>().socle, p.GetChild(0).GetComponent<BuildPiece>().rotFrame, p.GetChild(0).GetComponent<BuildPiece>().attchableSide, p.GetComponent<BuildPiece>().vie));
        //         }
        //     }
        //     foreach (Transform p in GameObject.Find("Vaisseau").transform)
        //     {
        //         if (p.childCount > 0 && p.GetChild(0).GetComponent<BuildPiece>() != null && p.GetComponent<BuildPiece>())
        //         {
        //             vs.pieces.Add(new PieceClass(p.GetChild(0).position, p.GetChild(0).eulerAngles, p.GetChild(0).name, p.GetChild(0).GetComponent<BuildPiece>().description, p.GetChild(0).GetComponent<BuildPiece>().niveau, p.GetChild(0).GetComponent<BuildPiece>().dependant, p.GetChild(0).GetComponent<BuildPiece>().socle, p.GetChild(0).GetComponent<BuildPiece>().rotFrame, p.GetChild(0).GetComponent<BuildPiece>().attchableSide, p.GetComponent<BuildPiece>().vie));
        //         }
        //     }
        // }
        // print("5");
        // var json = JsonUtility.ToJson(vs);
        // PlayerPrefs.SetString("Vaisseau", json);
        // print(json);
        // DeckList d = GameObject.Find("Liste").GetComponent<Liste>().deckList;
        // int index = 0;
        // foreach (Transform child in GameObject.Find("Vaisseau").transform.GetChild(0))
        // {

        //     d.Find(JsonUtility.FromJson<VaisseauClass>(PlayerPrefs.GetString("Vaisseau")).Deck).assemblage[index].vie = child.GetComponent<BuildPiece>().vie;
        //     d.Find(JsonUtility.FromJson<VaisseauClass>(PlayerPrefs.GetString("Vaisseau")).Deck).assemblage[index].niveau = child.GetComponent<BuildPiece>().niveau;

        //     index++;
        // }
        // PlayerPrefs.SetString("Deck", JsonUtility.ToJson(d));




    }

    public void ExitBuild()
    {
        bool a = false;
        foreach(Transform p in GameObject.Find("Vaisseau").transform)
        {
            if(p.GetComponent<BuildPiece>()!=null)
            {
                if(p.GetComponent<BuildPiece>().Verify() != "") { a = true; print(p.name + p.GetComponent<BuildPiece>().Verify()); }
            }


        }
        //foreach (Transform p in GameObject.Find("Vaisseau").transform.GetChild(0))
        //{
        //    if (p.GetComponent<BuildPiece>() != null)
        //    {
        //        if (p.GetComponent<BuildPiece>().Verify() != "") { a = true; }
        //    }


        //}

        print("avant");
        if (!a)
        {
            Save();
            print("apres");
            SceneManager.LoadScene("Espace");
        }
        else { StartCoroutine(warning()); }
    }
    public IEnumerator warning()
    {
        string txt = GameObject.Find("VerifyTexte").GetComponent<Text>().text;
        GameObject.Find("VerifyTexte").GetComponent<Text>().text = "You must assemble the ship well before saving it!";
        yield return new WaitForSeconds(3);
        if (GameObject.Find("VerifyTexte").GetComponent<Text>().text == "You must assemble the ship well before saving it!")
        {

            GameObject.Find("VerifyTexte").GetComponent<Text>().text = txt;


        }

    }

    public void Reset()
    {
        print("reset");

        SaveSystem.resetSave();
        //SceneManager.LoadScene("Espace");

        //VaisseauClass vs = JsonUtility.FromJson<VaisseauClass>(PlayerPrefs.GetString("Vaisseau"));
        //vs.position = Vector3.zero;
        //vs.eulerAngle = Vector3.zero;
        //PlayerPrefs.SetString("Vaisseau", JsonUtility.ToJson(vs));


        //PlayerPrefs.SetString("Grandeur", JsonUtility.ToJson(new Grandeur(100,100,100,100)));
        SceneManager.LoadScene("Espace");


    }

    public void flecheDeck(string side)
    {
        // Update deck index
        if(side == "droite")
        {
            print("iii");
            if(debloqueDeck.Liste.Count-1== DeckIndex) { DeckIndex = 0; }
            else { DeckIndex++; }
        }
        else
        {
            if (DeckIndex == 0) { DeckIndex = debloqueDeck.Liste.Count - 1; }
            else { DeckIndex--; }
        }
        
        // Update UI
        GameObject.Find("Deck-title-Text").GetComponent<Text>().text = debloqueDeck.Liste[DeckIndex].nom;
        
        // Move existing pieces to inventory
        MoveExistingPiecesToInventory();
        
        // Place pieces for the new deck
        foreach (PieceClass p in debloqueDeck.Liste[DeckIndex].assemblage)
        {
            print(p.nom);
            PlacePiece(p, GameObject.Find("Deck").transform);
        }
    }

    private void MoveExistingPiecesToInventory()
    {
        // Process pieces directly in Vaisseau
        foreach (Transform piece in GameObject.Find("Vaisseau").transform)
        {
            if (piece.GetComponent<BuildPiece>() != null)
            {
                AddPieceToInventory(piece, piece);
            }
        }
        
        // Process pieces in Vaisseau's first child
        foreach (Transform piece in GameObject.Find("Vaisseau").transform.GetChild(0))
        {
            if (piece.childCount > 0 && piece.GetChild(0).GetComponent<BuildPiece>() != null)
            {
                AddPieceToInventory(piece.GetChild(0), piece);
            }
        }
        
        // Process nested pieces
        foreach (Transform piece in GameObject.Find("Vaisseau").transform)
        {
            if (piece.childCount > 0 && piece.GetChild(0).GetComponent<BuildPiece>() != null 
                && piece.GetComponent<BuildPiece>() != null)
            {
                AddPieceToInventory(piece.GetChild(0), piece);
            }
        }
        
        // Clean up any remaining pieces in Vaisseau's child(0)
        foreach (Transform piece in GameObject.Find("Vaisseau").transform.GetChild(0)) { 
            Destroy(piece.gameObject); 
        }
    }

    private void AddPieceToInventory(Transform pieceTransform, Transform parentToDestroy)
    {
        BuildPiece bp = pieceTransform.GetComponent<BuildPiece>();
        PieceClass p = new PieceClass(
            pieceTransform.eulerAngles, 
            bp.transform.position, 
            bp.name, 
            bp.description, 
            bp.niveau, 
            bp.dependant, 
            bp.socle, 
            bp.rotFrame, 
            bp.attchableSide, 
            bp.vie
        );
        
        // Add to inventory
        Items.Add(p);
        
        // Create UI representation
        GameObject a = Instantiate(ItemObjPrefab, GameObject.Find("ContentScrollItem").transform);
        a.GetComponent<Image>().sprite = PieceLoader.GetPiecePrefab(p.nom).GetComponent<SpriteRenderer>().sprite;
        if (PieceLoader.GetPiecePrefab(p.nom).GetComponent<SpriteRenderer>().flipX) { 
            a.transform.localScale = new Vector3(-1, 1, 1); 
        }
        
        // Destroy the piece
        Destroy(parentToDestroy.gameObject);
    }
}


