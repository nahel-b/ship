//||
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;


public class Principal : MonoBehaviour
{

    //public MissionListe missions;

    public ItemClass Items;
    public Grandeur Grandeur = new Grandeur();
    public GameObject selectObj;
    public GameObject[] MapObj ;
    
    [HideInInspector]
    public GameObject Vaisseau;

    //public List<string> Items;
    public float speedRot = 1;
   
    public List<GameObject> Stations = new List<GameObject>();
    //public int maxItems = 10;
    private float[] shake = new float[] { 0, 0, 0, 0, 0 };
    public GameObject flechePrefab;

    public List<GameObject> StationPrefab;
    public List<string> StationName;

    //public List<List<string>> Jobs = new List<List<string>>();
    public List<metier> Jobs = new();
    public List<string> notifie = new() {"false"};
    public bool animeEnCourt = false;
    private float touchtime;
    public VaisseauClass VaisseauDeBase;
    public PieceObjList ListePiece;
    public DeckList deckList;
    private Vector3 Touchstart;
    private Vector3 a;
    public Transform EnnemieTarget;
    public GameObject explosion;
    public bool FinPanel = false;

    public PersoSave PersoSaveDeBase;

    private bool gravitationFieldBool = false;

    private bool reset = false;



    void Start()
    {

        Vaisseau = GameObject.FindGameObjectWithTag("vaisseau");

        Grandeur = JsonUtility.FromJson<Grandeur>(PlayerPrefs.GetString("Grandeur",JsonUtility.ToJson(Grandeur)));
        ListePiece = GameObject.Find("Liste").GetComponent<Liste>().ListePiece;
        if (PlayerPrefs.HasKey("Deck"))
        {
            GameObject.Find("Liste").GetComponent<Liste>().deckList = JsonUtility.FromJson<DeckList>(PlayerPrefs.GetString("Deck"));
        }
        
        deckList = GameObject.Find("Liste").GetComponent<Liste>().deckList;
        //PlayerPrefs.DeleteAll();
              
              
        Load();
        //Save();
        
        SaveSystem.LoadVaisseau();
        SaveSystem.LoadEnemies();
        SaveSystem.LoadColors();
        SaveSystem.LoadDeck();
        SaveSystem.LoadGrandeur();

    }
   

    // Update is called once per frame
    void Update()
    {




                if (Input.GetKeyDown(KeyCode.A)) { print("eeepp"); StartCoroutine(Stations[0].transform.GetComponent<Station>().enter()); }
        if(GameObject.Find("mapUI") != null) { }
        HeathBar();
        if (Grandeur.fuel > 0 && GameObject.Find("FireBarUI") != null)
        {
            GameObject.Find("FireBarUI").GetComponent<RectTransform>().localPosition = new Vector3(-1 * ((Grandeur.fuel * (73f + 55.1f) / Grandeur.fuelMax) - 55.1f), 0.8885155f, 0);
        }
        if (GameObject.Find("CoinObj") != null)
        {
            GameObject.Find("CoinObj").GetComponent<Text>().text = Grandeur.coin.ToString() + "$";
        }
        float max = 129.3f;
        if (GameObject.Find("FuelBarBorder") != null)
        {
            GameObject.Find("FuelBarBorder").GetComponent<RectTransform>().sizeDelta = new Vector2(Grandeur.fuel * max / Grandeur.fuelMax, GameObject.Find("FuelBarBorder").GetComponent<RectTransform>().sizeDelta.y);
        }
            
        float b = 0;
        float c = 0;
        foreach (Transform child in Vaisseau.transform.GetChild(0).transform)
        {

            b = b + child.GetComponent<Rigidbody2D>().velocity.magnitude;
            c = c + 1;
    
        }
        b = b / c;
        b = Mathf.Round(b * 100) / 100;

        if (Mathf.Round(b) == b && GameObject.Find("Vitesse") !=null && GameObject.Find("Vitesse") != null)
        {
            GameObject.Find("Vitesse").GetComponent<Text>().text = (Mathf.Round(b * 10) / 10) + ".0 u/min";
            GameObject.Find("Vitesse-2").GetComponent<Text>().text = (Mathf.Round(b * 10) / 10) + ".0 u/min";
        }
        else if(GameObject.Find("Vitesse") != null && GameObject.Find("Vitesse") != null)
        {
            GameObject.Find("Vitesse").GetComponent<Text>().text = (Mathf.Round(b * 10) / 10) + " u/min";
            GameObject.Find("Vitesse-2").GetComponent<Text>().text = (Mathf.Round(b * 10) / 10) + " u/min";
        }


        Collider2D[] objs = Physics2D.OverlapCircleAll(transform.position, 25);

        bool agrvt = false;


        foreach (Collider2D collObj in objs)
        {
            if (collObj.GetComponent<Piece>() != null)
            {
                if ( collObj.transform.parent.GetComponent<Ennemie>() != null && !collObj.transform.parent.GetComponent<Ennemie>().destroyAllPieces && collObj.transform.parent.GetComponent<Ennemie>().agressif && !EnnemieTarget && collObj.transform.parent.GetComponent<Ennemie>().zone[2] >= Vector3.Distance(Vaisseau.transform.GetChild(0).GetChild(0).transform.position,collObj.transform.position))
                {
                    EnnemieTarget = collObj.transform.parent;
                    foreach(Transform child in GameObject.Find("EnnemieBar").transform)
                    {
                        child.gameObject.SetActive(true);
                    }
                    GameObject.Find("EnnemieBar").transform.GetChild(2).GetComponent<Text>().text = EnnemieTarget.GetComponent<Ennemie>().nom;
                }
            }
            else if (collObj.GetComponent<Station>() != null)
            {
                agrvt = true;
                if (!gravitationFieldBool)
                {
                    if (b > 12)
                    {
                        GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("Entree", 0.4f);
                    }
                    StartCoroutine(Feedback(3, 0.1f));
                    StartCoroutine(Shake(0.3f, b/50));
                    notifie.Add("Enter in " + collObj.name + "'s gravitation field!");
                    gravitationFieldBool = true;
                    foreach (Transform child in Vaisseau.transform)
                    {
                        if (child.GetComponent<Rigidbody2D>() != null) { child.GetComponent<Rigidbody2D>().drag = 1; }

                    }
                    if (Vaisseau.transform.GetChild(0).childCount > 0)
                    {
                        foreach (Transform child in Vaisseau.transform.GetChild(0))
                        {
                            if (child.GetComponent<Rigidbody2D>() != null) { child.GetComponent<Rigidbody2D>().drag = 1; }

                        }
                    }


                }


                if (b > 20)
                {
                    foreach (Transform child in Vaisseau.transform)
                    {
                        if (child.GetComponent<Rigidbody2D>() != null) { child.GetComponent<Rigidbody2D>().velocity = child.GetComponent<Rigidbody2D>().velocity -( child.GetComponent<Rigidbody2D>().velocity * Time.deltaTime); }

                    }
                    if (Vaisseau.transform.GetChild(0).childCount > 0)
                    {
                        foreach (Transform child in Vaisseau.transform.GetChild(0))
                        {
                            if (child.GetComponent<Rigidbody2D>() != null) { child.GetComponent<Rigidbody2D>().velocity = child.GetComponent<Rigidbody2D>().velocity - (child.GetComponent<Rigidbody2D>().velocity  * Time.deltaTime); }

                        }
                    }

                }



            }
        }
        if (!EnnemieTarget && GameObject.Find("EnnemieBar")!=null)
        {
            foreach (Transform child in GameObject.Find("EnnemieBar").transform)
            {
                child.gameObject.SetActive(false);
            }
        }
        if (!agrvt && gravitationFieldBool)
        { gravitationFieldBool = false;
            print("aaaaaaaaa");
            foreach (Transform child in Vaisseau.transform)
            {
                if (child.GetComponent<Rigidbody2D>() != null) { child.GetComponent<Rigidbody2D>().drag = 0; }

            }
            if (Vaisseau.transform.GetChild(0).childCount > 0)
            {
                foreach (Transform child in Vaisseau.transform.GetChild(0))
                {
                    if (child.GetComponent<Rigidbody2D>() != null) { child.GetComponent<Rigidbody2D>().drag = 0; }

                }
            }
        }






                if (notifie[0] == "false" && notifie.Count > 1) { StartCoroutine(Notifie()); }


        if (shake[0] == 1)
        {
            transform.localPosition = new Vector3(shake[2], shake[3], shake[4]) + UnityEngine.Random.insideUnitSphere * shake[1];
        }




        transform.parent.position = new Vector3(Vaisseau.transform.GetChild(0).GetChild(0).transform.position.x, Vaisseau.transform.GetChild(0).GetChild(0).transform.position.y, -10);
        

    

        if ((Input.touchCount > 0)&& Input.GetTouch(0).phase == TouchPhase.Began && !MapObj[0].activeSelf)
        {
            touchtime = Time.time;
        }
        if (!MapObj[0].activeSelf && (Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Ended) && Input.GetTouch(0).position.y > Screen.height / 4 && touchtime > Time.time-0.2f && GameObject.Find("MissionPanel")== null&& GameObject.Find("InventoryPanel") == null && FinPanel == false)
        {


            RaycastHit2D[] hit = Physics2D.CircleCastAll(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position), 1, Vector2.zero);
            foreach (RaycastHit2D obj in hit)
            {
                if (obj.transform.name.Contains("Station"))
                {
                    if (b > 3) { notifie.Add("You are going too fast to dock"); }
                    else if ((obj.transform.position - Camera.main.transform.position).magnitude > 22) { notifie.Add("You are too far to dock"); }
                    else
                    {
                        StartCoroutine(obj.transform.GetComponent<Station>().enter());
                        foreach (Transform child in Vaisseau.transform.GetChild(0).transform)
                        {
                            if (child.GetComponent<Piece>()!=null)
                            {
                                child.transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                            }
                        }
                    }
                }
            }
        }
        if (FinPanel) { FinPanel = false; }
        if (!MapObj[0].activeSelf && Input.touchCount > 1 && Input.GetTouch(0).position.y > Screen.height / 4 && GameObject.Find("ButtonFeu").GetComponent<Bouton>().isMouseDown || !MapObj[0].activeSelf && Input.touchCount == 1 && Input.GetTouch(0).position.y > Screen.height / 4)
        {

            var touch = Input.GetTouch(0);
            if (touch.position.x < Screen.width / 2)
            {

                foreach (Transform child in Vaisseau.transform.GetChild(0).transform)
                {
                    if (child.name.Contains("eacteur"))
                    {
                        //child.transform.GetComponent<Rigidbody2D>().AddTorque(100 * Time.deltaTime);
                        child.transform.GetComponent<Rigidbody2D>().angularVelocity = 20000 * Time.deltaTime;
                    }
                }

            }
            else if (touch.position.x > Screen.width / 2 && touch.position.y > Screen.height / 4)
            {
                foreach (Transform child in Vaisseau.transform.GetChild(0).transform)
                {
                    if (child.name.Contains("eacteur"))
                    {
                        child.transform.GetComponent<Rigidbody2D>().angularVelocity = -20000 * Time.deltaTime;
                    }
                }
            }
        }
        else if (!MapObj[0].activeSelf && Input.touchCount > 1 && Input.GetTouch(1).position.y > Screen.height / 4 && GameObject.Find("ButtonFeu").GetComponent<Bouton>().isMouseDown)
        {
            var touch = Input.GetTouch(1);
            if (touch.position.x < Screen.width / 2)
            {

                foreach (Transform child in Vaisseau.transform.GetChild(0).transform)
                {
                    if (child.name.Contains("eacteur"))
                    {
                        //child.transform.GetComponent<Rigidbody2D>().AddTorque(100 * Time.deltaTime);
                        child.transform.GetComponent<Rigidbody2D>().angularVelocity = 20000 * Time.deltaTime;
                    }
                }

            }
            else if (touch.position.x > Screen.width / 2 && touch.position.y > Screen.height / 4)
            {
                foreach (Transform child in Vaisseau.transform.GetChild(0).transform)
                {
                    if (child.name.Contains("eacteur"))
                    {
                        child.transform.GetComponent<Rigidbody2D>().angularVelocity = -20000 * Time.deltaTime;
                    }
                }
            }
        }
        else if(!MapObj[0].activeSelf)
        {
            foreach (Transform child in Vaisseau.transform.GetChild(0).transform)
            {
                if (child.name.Contains("eacteur"))
                {
                    child.transform.GetComponent<Rigidbody2D>().angularVelocity = 0;
                }
            }


            if (Input.touchCount == 2)
            {
                Touch touch0 = Input.GetTouch(0);
                Touch touch1 = Input.GetTouch(1);

                Vector2 touch0prevPos = touch0.position - touch0.deltaPosition;
                Vector2 touch1prevPos = touch1.position - touch1.deltaPosition;

                float prevMagnitude = (touch0prevPos - touch1prevPos).magnitude;
                float curentMagnitude = (touch0.position - touch1.position).magnitude;

                Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - 0.05f * (curentMagnitude - prevMagnitude), 8, 30);


            }

        }
        else if (MapObj[0].activeSelf && Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Ended) { Touchstart = Vector3.one * 666; }
        else if (MapObj[0].activeSelf && Input.touchCount == 2 && Input.GetTouch(1).phase == TouchPhase.Ended) { Touchstart = Vector3.one * 666; }

        else if (MapObj[0].activeSelf && Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            Vector2 touch0prevPos = touch0.position - touch0.deltaPosition;
            Vector2 touch1prevPos = touch1.position - touch1.deltaPosition;

            float prevMagnitude = (touch0prevPos - touch1prevPos).magnitude;
            float curentMagnitude = (touch0.position - touch1.position).magnitude;

            MapObj[0].transform.localScale = Vector3.one * Mathf.Clamp(MapObj[0].transform.localScale.x + 0.005f * (curentMagnitude - prevMagnitude), 0.25f, 8);
        }
        else if(MapObj[0].activeSelf && Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began )
        {
            Touchstart = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            a = MapObj[0].transform.position;
            print("a");
        }

        else if(MapObj[0].activeSelf && Input.touchCount == 1 && Touchstart != Vector3.one * 666)
        {
            Vector3 direction = Touchstart - Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            MapObj[0].transform.position = new Vector3(-direction.x + a.x, -direction.y + a.y,0);
        }


    }

    public void OpenMap()
    {

        if (!MapObj[0].activeSelf)
        {

            MapObj[0].SetActive(true);
            GameObject a = Instantiate(MapObj[2]);
            a.transform.parent = MapObj[0].transform;
            a.transform.localPosition = new Vector3(Camera.main.transform.position.x * 40 / 4000, Camera.main.transform.position.y * 10 / 4000, -5);
            
            //a.transform.eulerAngles = new Vector3(0, 0, GameObject.Find("Main Camera").GetComponent<Principal>().Vaisseau.transform.GetChild(0).GetChild(0).transform.eulerAngles.z);

            foreach (GameObject st in GameObject.FindGameObjectsWithTag("station"))
            {

                a = Instantiate(MapObj[1]);
                a.transform.SetParent(MapObj[0].transform);
                a.transform.localPosition = new Vector3(st.transform.position.x * 40 / 4000, st.transform.position.y * 10 / 4000, -1);
                a.transform.eulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(0, 360));


            }
        }
        else
        {
            foreach(Transform child in MapObj[0].transform)
            {
                Destroy(child.gameObject);
            }
            MapObj[0].SetActive(false);

        }
    }




    public IEnumerator Shake(float temps, float shakeMagnitude)
    {
        Vector3 initialPosition = transform.localPosition;
        shake[0] = 1;
        shake[1] = shakeMagnitude;
        shake[2] = transform.localPosition.x;
        shake[3] = transform.localPosition.y;
        shake[4] = transform.localPosition.z;

        yield return new WaitForSeconds(temps);
        shake[0] = 0;
        transform.localPosition = initialPosition;
    }

    public void Fleche(string targetName, bool showWayPoint, Color couleur)
    {


        GameObject a = Instantiate(flechePrefab, Camera.main.transform.position, Quaternion.identity);
        a.GetComponent<Fleche>().targetName = targetName;
        a.transform.parent = GameObject.Find("Fleches").transform;
        a.GetComponent<Image>().color = couleur;

        a.GetComponent<Fleche>().showWayPoint = showWayPoint;
    }

    public IEnumerator Notifie()
    {
        notifie[0] = "true";
        for (int i = 0; i < notifie[1].Length; i++)
        {
            GameObject.Find("notifieTexte").GetComponent<Text>().text = GameObject.Find("notifieTexte").GetComponent<Text>().text + notifie[1][i];
            yield return new WaitForSeconds(0.035f);
        }
        yield return new WaitForSeconds(2);
        Color a = GameObject.Find("notifieTexte").GetComponent<Text>().color;
        for (float i = 1; i > 0; i-=0.02f)
        {
            //GameObject.Find("notifieTexte").GetComponent<Text>().color = new Color(GameObject.Find("notifieTexte").GetComponent<Text>().color[0], GameObject.Find("notifieTexte").GetComponent<Text>().color[1], GameObject.Find("notifieTexte").GetComponent<Text>().color[2], i);
            a.a = i;
            GameObject.Find("notifieTexte").GetComponent<Text>().color = a;
            yield return new WaitForSeconds(0.01f);
        }
        GameObject.Find("notifieTexte").GetComponent<Text>().text = "";
        a.a = 1;
        GameObject.Find("notifieTexte").GetComponent<Text>().color = a;
        new WaitForSeconds(0.25f);
        notifie.RemoveAt(1);
        notifie[0] = "false";
    }




    public void Load()
    {
        //PlayerPrefs.DeleteAll();
        // if (PlayerPrefs.HasKey("Missions"))
        // {
        //     missions = JsonUtility.FromJson<MissionListe>(PlayerPrefs.GetString("Missions"));
        //     print(PlayerPrefs.GetString("Missions"));
        // }
        // else { missions.Add(GameObject.Find("Liste").GetComponent<Liste>().Histoire.missions[0]); }
        if (PlayerPrefs.HasKey("Color"))
        {
            GameObject.Find("Liste").GetComponent<Liste>().colorList = JsonUtility.FromJson<ColorList>(PlayerPrefs.GetString("Color"));
        }
        if (!PlayerPrefs.HasKey("PersoSave"))
        {
            PlayerPrefs.SetString("PersoSave", JsonUtility.ToJson(PersoSaveDeBase));

        }
        
        if (PlayerPrefs.HasKey("Ennemie"))
        {
            foreach(EnnemieClass ennemie in JsonUtility.FromJson<ListEnnemie>(PlayerPrefs.GetString("Ennemie")).Ennemies)
            {
                //EnnemieClass ennemie = GameObject.Find("Liste").GetComponent<Liste>().EnnemieList.Ennemies[int.Parse(indexVector2.Split(';')[0])];
                GameObject a = Instantiate(new GameObject());
                a.AddComponent<Ennemie>();
                a.GetComponent<Ennemie>().agressif = ennemie.aggressif;
                a.GetComponent<Ennemie>().SpeedRotation = ennemie.SpeedRotation;
                a.GetComponent<Ennemie>().zone = ennemie.zone;
                a.GetComponent<Ennemie>().vaisseau = ennemie.vaisseau;
                a.GetComponent<Ennemie>().speed = ennemie.speed;
                a.GetComponent<Ennemie>().nom = ennemie.nom;
                a.GetComponent<Ennemie>().velocity = ennemie.velocity;
                a.tag = "ennemie";
                a.GetComponent<Ennemie>().Rotz = ennemie.Rotz;
                a.transform.position = ennemie.position;

            }


        }


        if (PlayerPrefs.HasKey("saveObj"))
        {
            Items = JsonUtility.FromJson<ListsaveObj>(PlayerPrefs.GetString("saveObj")).Items;
            //Stations = new List<GameObject>() { null, null, null, null };
            //foreach (StationClass station in JsonUtility.FromJson<ListsaveObj>(PlayerPrefs.GetString("saveObj")).stations)
            //{
            //    GameObject a = Instantiate(StationPrefab[station.prefab], station.pos, Quaternion.identity);
            //    a.name = station.name;
            //    a.GetComponent<Station>().station.name = station.name;
            //    a.GetComponent<Station>().station.jobs = station.jobs;
            //    a.GetComponent<Station>().station.index = station.index;

            //}
            foreach(FlecheClass fleche in JsonUtility.FromJson<ListsaveObj>(PlayerPrefs.GetString("saveObj")).fleches)
            {
               GameObject a = Instantiate(flechePrefab);
                a.GetComponent<Fleche>().targetName = fleche.targetName;
                a.GetComponent<Image>().color = fleche.color;
                a.transform.parent = GameObject.Find("Fleches").transform;
            }           
        }
        else
        {
            //GameObject a = Instantiate(StationPrefab[Random.Range(0, StationPrefab.Count)], new Vector3(Random.Range(-500, 500), Random.Range(-500, 500), 0), Quaternion.identity);
            //bool d = false;
            //while (d == false)
            //{
            //    d = true;
            //    RaycastHit2D[] hit = Physics2D.CircleCastAll(a.transform.position, 300, Vector2.zero);
            //    foreach (RaycastHit2D obj in hit)
            //    {
            //        if (obj.transform.GetComponent<Piece>() != null) { d = false; a.transform.position = new Vector3(Random.Range(-500, 500), Random.Range(-500, 500), 0); }

            //    }


            //}
            //Fleche(a.transform);
            //for (int i = 0; i < 3; i++)
            //{
            //    float rayon = 4000;
            //    int e = Random.Range(0, StationPrefab.Count);
            //    a = Instantiate(StationPrefab[e], new Vector3(Random.Range(-rayon, rayon), Random.Range(-rayon, rayon), 0), Quaternion.identity);
            //    d = false;
            //    while (d == false)
            //    {
            //        d = true;
            //        RaycastHit2D[] hit = Physics2D.CircleCastAll(a.transform.position, 300, Vector2.zero);
            //        foreach (RaycastHit2D obj in hit)
            //        {
            //            if (obj.transform.name.Contains("Station")) { d = false; a.transform.position = new Vector3(Random.Range(-rayon, rayon), Random.Range(-rayon, rayon), 0); }

            //        }


            //    }
            //    a.GetComponent<Station>().station.prefab = e;
            //}
          


            }




        //string path = System.IO.Path.Combine(Application.persistentDataPath, "data.dat");
        string b = JsonUtility.ToJson(VaisseauDeBase);
        VaisseauClass jsonF = JsonUtility.FromJson<VaisseauClass>(PlayerPrefs.GetString("Vaisseau",b)); 
//        print(JsonUtility.ToJson(jsonF));

        //string json = JsonUtility.ToJson(Vaisseau);
        //Debug.Log("Saving as JSON: " + json);
        //System.IO.File.WriteAllText(path, json);

        foreach (Transform child in Vaisseau.transform.GetChild(0).transform) { if (!child.name.Contains("Deck")) { DestroyImmediate(child.gameObject); } }
        int i = 0;
        foreach (PieceClass p in deckList.Find(jsonF.Deck).assemblage)
        {
//            print(p.nom);
            GameObject pObj = Instantiate(ListePiece.Find(p.nom), p.position, Quaternion.Euler(p.eulerAngle));
            pObj.name = p.nom;
            foreach (FixedJoint2D component in pObj.GetComponents<FixedJoint2D>()) { Destroy(component); }
            pObj.transform.parent = Vaisseau.transform.GetChild(0).transform;
            pObj.GetComponent<Piece>().niveau = p.niveau;
            pObj.GetComponent<Piece>().attchableSide = p.attchableSide;
            pObj.GetComponent<Piece>().dependant = p.dependant;
            pObj.GetComponent<Piece>().rotFrame = p.rotFrame;
            pObj.GetComponent<Piece>().socle = p.socle;
            
            if (p.vie == -1 && !p.dependant) { pObj.GetComponent<Piece>().vie = ListePiece.Find(p.nom).GetComponent<Piece>().vieListe[p.niveau]; }
            else if (!p.dependant) { pObj.GetComponent<Piece>().vie = p.vie; }

            if (p.dependant)
            {
                pObj.transform.position = new Vector3(pObj.transform.position.x, pObj.transform.position.y, -1);
                RaycastHit2D[] hit = Physics2D.RaycastAll(pObj.transform.position, Vector3.zero, 0.3f);
                foreach (RaycastHit2D obj in hit)
                {
                    if ((obj.transform.parent == GameObject.Find("Vaisseau").transform || obj.transform.parent == GameObject.Find("Vaisseau").transform.GetChild(0)) && obj.transform.GetComponent<Piece>().socle)
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
        foreach (PieceClass p in jsonF.pieces.assemblage)
        {

            GameObject pObj = Instantiate(ListePiece.Find(p.nom), p.position, Quaternion.Euler(p.eulerAngle));

            pObj.name = p.nom;
            foreach (FixedJoint2D component in pObj.GetComponents<FixedJoint2D>()) { Destroy(component); }
            pObj.transform.parent = Vaisseau.transform; 
            pObj.GetComponent<Piece>().niveau = p.niveau;
            pObj.GetComponent<Piece>().attchableSide = p.attchableSide;
            pObj.GetComponent<Piece>().dependant = p.dependant;
            pObj.GetComponent<Piece>().rotFrame = p.rotFrame;
            pObj.GetComponent<Piece>().socle = p.socle;

            if (p.vie == -1 && !p.dependant) { pObj.GetComponent<Piece>().vie = ListePiece.Find(p.nom).GetComponent<Piece>().vieListe[p.niveau]; }
            else if(!p.dependant) { pObj.GetComponent<Piece>().vie = p.vie; }

            if (p.dependant)
            {
                pObj.transform.position = new Vector3(pObj.transform.position.x, pObj.transform.position.y, -1);

                print(pObj.name);
                RaycastHit2D[] hit = Physics2D.RaycastAll(pObj.transform.position, Vector3.zero, 0.3f);
                foreach (RaycastHit2D obj in hit)
                {
                    print(pObj.name + "->" + obj.transform.name);
                    if ((obj.transform.parent == GameObject.Find("Vaisseau").transform || obj.transform.parent == Vaisseau.transform.GetChild(0)) && obj.transform.GetComponent<Piece>().socle)
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
        foreach (Transform child in Vaisseau.transform.GetChild(0).transform)
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
        foreach (Transform child in Vaisseau.transform.GetChild(0).transform)
        {


            RaycastHit2D[] hit = Physics2D.RaycastAll(child.position, Vector3.up, 0.6f) ;
            foreach (RaycastHit2D obj in hit)
            {
                //meme parent + attachable haut
                if (( child.GetComponent<Piece>().attchableSide.getList()[Mathf.RoundToInt(Clamp0360(child.transform.eulerAngles.z / 90))] && obj.transform.GetComponent<Piece>().attchableSide.getList()[Mathf.RoundToInt( Clamp0360(obj.transform.eulerAngles.z/90))+2] && obj.transform.GetComponent<Piece>() != null && obj.transform.gameObject != child.transform.gameObject ))
                {
                    FixedJoint2D c = child.gameObject.AddComponent<FixedJoint2D>();
                    c.connectedBody = obj.transform.GetComponent<Rigidbody2D>();
                }
            }
            hit = Physics2D.RaycastAll(child.position, Vector3.right, 0.6f);
            foreach (RaycastHit2D obj in hit)
            {
                
                //meme parent + attachable haut
                if (( child.GetComponent<Piece>().attchableSide.getList()[Mathf.RoundToInt(Clamp0360(child.transform.eulerAngles.z / 90) )+1] && obj.transform.GetComponent<Piece>().attchableSide.getList()[Mathf.RoundToInt(Clamp0360(obj.transform.eulerAngles.z / 90)) + 3] && obj.transform.GetComponent<Piece>() != null && obj.transform.gameObject != child.transform.gameObject ))
                {
                    FixedJoint2D c = child.gameObject.AddComponent<FixedJoint2D>();
                    c.connectedBody = obj.transform.GetComponent<Rigidbody2D>();
                }
            }
            hit = Physics2D.RaycastAll(child.position, -Vector3.up, 0.6f);
            foreach (RaycastHit2D obj in hit)
            {
                //meme parent + attachable haut
                if (( child.GetComponent<Piece>().attchableSide.getList()[Mathf.RoundToInt(Clamp0360(child.transform.eulerAngles.z / 90) ) + 2] && obj.transform.GetComponent<Piece>().attchableSide.getList()[Mathf.RoundToInt(Clamp0360(obj.transform.eulerAngles.z / 90) ) + 4] && obj.transform.GetComponent<Piece>() != null && obj.transform.gameObject != child.transform.gameObject ))
                {
                    FixedJoint2D c = child.gameObject.AddComponent<FixedJoint2D>();
                    c.connectedBody = obj.transform.GetComponent<Rigidbody2D>();
                }
            }
            hit = Physics2D.RaycastAll(child.position, -Vector3.right, 0.6f);
            foreach (RaycastHit2D obj in hit)
            {
                //meme parent + attachable haut
                if (( child.GetComponent<Piece>().attchableSide.getList()[Mathf.RoundToInt(Clamp0360(child.transform.eulerAngles.z / 90) ) + 3] && obj.transform.GetComponent<Piece>().attchableSide.getList()[Mathf.RoundToInt(Clamp0360(obj.transform.eulerAngles.z / 90) ) + 5] && obj.transform.GetComponent<Piece>() != null && obj.transform.gameObject != child.transform.gameObject ))
                {
                    FixedJoint2D c = child.gameObject.AddComponent<FixedJoint2D>();
                    c.connectedBody = obj.transform.GetComponent<Rigidbody2D>();
                }
            }

            //if (!PlayerPrefs.HasKey("saveObj"))
            //{
            //    Save();
            //}
        }




        //foreach (Transform child in Vaisseau.transform)
        //{
        //    if (!child.name.Contains("Deck"))
        //    {

        //        RaycastHit2D[] hit = Physics2D.RaycastAll(child.position, Vector3.up, 0.6f);
        //        foreach (RaycastHit2D obj in hit)
        //        {
        //            //print(child.transform.name + "-" + obj.transform.name);
        //            if ((obj.transform.GetComponent<Piece>() != null && obj.transform.gameObject != child.transform.gameObject && obj.transform.parent == child.transform.parent) || (obj.transform.GetComponent<Piece>() != null && obj.transform.gameObject != child.transform.gameObject && obj.transform.GetComponent<Piece>().attachable))
        //            {

        //                FixedJoint2D c = child.gameObject.AddComponent<FixedJoint2D>();
        //                c.connectedBody = obj.transform.GetComponent<Rigidbody2D>();
        //            }
        //        }
        //        hit = Physics2D.RaycastAll(child.position, Vector3.right, 0.6f);
        //        foreach (RaycastHit2D obj in hit)
        //        {
        //            //print(child.transform.name + "-" + obj.transform.name);
        //            print(obj.transform.GetComponent<Piece>().attachable);
        //            if ((obj.transform.GetComponent<Piece>() != null && obj.transform.gameObject != child.transform.gameObject && obj.transform.parent == child.transform.parent) || (obj.transform.GetComponent<Piece>() != null && obj.transform.gameObject != child.transform.gameObject && obj.transform.GetComponent<Piece>().attachable))
        //            {
        //                FixedJoint2D c = child.gameObject.AddComponent<FixedJoint2D>();
        //                c.connectedBody = obj.transform.GetComponent<Rigidbody2D>();
        //            }
        //        }
        //    }
        //}

        Vaisseau.transform.localPosition = jsonF.position;
        Vaisseau.transform.localEulerAngles = jsonF.eulerAngle;

        GameObject.Find("SceneTransition").GetComponent<Animator>().SetTrigger("ouvre");
       // StartCoroutine(blockRaycast());

    }
    //IEnumerator blockRaycast()
    //{
    //    yield return new WaitForSeconds(0.8f);
    //    GameObject.Find("CanvasTransition").GetComponent<CanvasGroup>().blocksRaycasts = false; 
    //}


    public void Build()
    {
        Save();
        SceneManager.LoadScene("Build");

    }


    public void Save()
    {
        print("save");
        // Time.time > 0.3f && GameObject.Find("Canvas").transform.GetChild(0).gameObject.activeInHierarchy && 
        if ( !reset) {

            //try
            //{
               // PlayerPrefs.SetString("Missions", JsonUtility.ToJson(missions));

                PlayerPrefs.SetInt("MaxItem", Items.maxItem());
                //print("MAAAAAAAAAX" + Items.maxItem());

                PlayerPrefs.SetString("Grandeur", JsonUtility.ToJson(Grandeur));


                ListsaveObj saveObj = new ListsaveObj();
                saveObj.Items = Items;
                ListsaveObj abbv = new ListsaveObj(); abbv.currentSpot = "";
                saveObj.currentSpot = JsonUtility.FromJson<ListsaveObj>(PlayerPrefs.GetString("saveObj",JsonUtility.ToJson(abbv))).currentSpot;
                //foreach (GameObject station in Stations)
                //{
                //    print(station.name);
                //    StationClass sta = station.GetComponent<Station>().station;
                //    saveObj.stations.Add(new StationClass(sta.name, station.transform.position, sta.prefab, sta.jobs, sta.index));
                //}
                foreach (GameObject fleche in GameObject.FindGameObjectsWithTag("fleche"))
                {
                    if (fleche.GetComponent<Fleche>().target != null)
                    {
                        saveObj.fleches.Add(new FlecheClass(fleche.GetComponent<Image>().color, fleche.GetComponent<Fleche>().targetName, fleche.GetComponent<Fleche>().showWayPoint));

                    }
                }
                PlayerPrefs.SetString("saveObj", JsonUtility.ToJson(saveObj));

                string b = JsonUtility.ToJson(VaisseauDeBase);
                VaisseauClass vs = JsonUtility.FromJson<VaisseauClass>(PlayerPrefs.GetString("Vaisseau", b));
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
                vs.velocity = (vel / nb);
                
                if (Vaisseau.transform.GetChild(0).childCount > 0)
                {
                    vs.position = Vaisseau.transform.GetChild(0).GetChild(0).position;
                    vs.eulerAngle.z = (Vaisseau.transform.GetChild(0).GetChild(0).eulerAngles.z - deckList.Find(vs.Deck).assemblage[0].eulerAngle.z);
                    PlayerPrefs.SetString("Vaisseau", JsonUtility.ToJson(vs));
                DeckList d = GameObject.Find("Liste").GetComponent<Liste>().deckList;
                int i = 0;
                foreach(Transform child in Vaisseau.transform.GetChild(0))
                {

                    d.Find(JsonUtility.FromJson<VaisseauClass>(PlayerPrefs.GetString("Vaisseau", b)).Deck).assemblage[i].vie = child.GetComponent<Piece>().vie;
                    d.Find(JsonUtility.FromJson<VaisseauClass>(PlayerPrefs.GetString("Vaisseau", b)).Deck).assemblage[i].niveau = child.GetComponent<Piece>().niveau;

                    i++;
                }
                PlayerPrefs.SetString("Deck", JsonUtility.ToJson(d));
                PlayerPrefs.SetString("Color", JsonUtility.ToJson(GameObject.Find("Liste").GetComponent<Liste>().colorList));
                i = 0;
                VaisseauClass vaisseauJson = JsonUtility.FromJson<VaisseauClass>(PlayerPrefs.GetString("Vaisseau", JsonUtility.ToJson(VaisseauDeBase)));

                foreach (Transform child in Vaisseau.transform)
                {
                    if (child.GetComponent<Piece>()!=null)
                    {
                        vaisseauJson.pieces.assemblage[i].vie = child.GetComponent<Piece>().vie;
                        vaisseauJson.pieces.assemblage[i].niveau = child.GetComponent<Piece>().niveau;
                        i++;

                    }
                }
                PlayerPrefs.SetString("Vaisseau", JsonUtility.ToJson(vaisseauJson));




                ListEnnemie ennemieList = new ListEnnemie();

                foreach (GameObject ennemie in GameObject.FindGameObjectsWithTag("ennemie"))
                {
                    Ennemie sc = ennemie.GetComponent<Ennemie>();
                    int index = 0;
                    Assemblage vaisseauTheorique = sc.vaisseau;
                    Assemblage vaisseauEnnemie = new Assemblage();
                    foreach (Transform child in ennemie.transform)
                    {
                        PieceClass piecetheo = vaisseauTheorique.assemblage[child.GetComponent<Piece>().index];
                        vaisseauEnnemie.assemblage.Add(piecetheo);
                        vaisseauEnnemie.assemblage[index].vie = child.GetComponent<Piece>().vie;
                        index++;
                    }
                    ennemieList.Ennemies.Add(new EnnemieClass(sc.nom,sc.SpeedRotation,sc.zone,sc.speed, vaisseauEnnemie, sc.agressif,ennemie.transform.eulerAngles.z,ennemie.transform.position,0));
                }
                PlayerPrefs.SetString("Ennemie",JsonUtility.ToJson(ennemieList));




            }
            //}
            //catch { }
        }

    }

    public void SaveMovement()
    {
        string b = JsonUtility.ToJson(VaisseauDeBase);
        VaisseauClass vs = JsonUtility.FromJson<VaisseauClass>(PlayerPrefs.GetString("Vaisseau", b));
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
        vs.velocity = (vel / nb);
        
        
        if (Vaisseau.transform.GetChild(0).childCount > 0)
        {
            vs.position = Vaisseau.transform.GetChild(0).GetChild(0).position;
            vs.eulerAngle.z = (Vaisseau.transform.GetChild(0).GetChild(0).eulerAngles.z - deckList.Find(vs.Deck).assemblage[0].eulerAngle.z);
            PlayerPrefs.SetString("Vaisseau", JsonUtility.ToJson(vs));


        }


    }

    

    public static float Clamp0360(float eulerAngles)
    {
        float result = eulerAngles - Mathf.CeilToInt(eulerAngles / 360f) * 360f;
        if (result < 0)
        {
            result += 360f;
        }
        return result;
    }

    public void Reset()
    {
        PlayerPrefs.DeleteAll();
        reset = true;
        SceneManager.LoadScene(0);
    }


    private void OnApplicationPause(bool pause)
    {
        if (Vaisseau.transform.GetChild(0).childCount > 0 && !reset)
        {
            Save();
        }
    }

    private void OnApplicationQuit()
    {
        if (Vaisseau.transform.GetChild(0).childCount > 0&& !reset)
        {
            Save();
        }
    }



    

    public IEnumerator Feedback(int nb, float times)
    {
        for(int i = 0; i < nb; i++)
        {
            #if UNITY_IPHONE && !UNITY_EDITOR
            ImpactFeedback feedback;
            feedback = ImpactFeedback.Heavy;
            TapticManager.Impact(feedback);

            #elif UNITY_ANDROID && !UNITY_EDITOR
            AndroidManager.HapticFeedback();

            #endif
            yield return new WaitForSeconds(times);

        }
        yield return null;


    }
    public void HeathBar()
    {
        float max = 129.3f;
        Grandeur.healthMax = 0;
        foreach (Transform child in Vaisseau.transform)
        {
            if (child.GetComponent<Piece>() != null)
            {
                Grandeur.healthMax += child.GetComponent<Piece>().vieListe[child.GetComponent<Piece>().niveau];
            }
        }
        foreach (Transform child in Vaisseau.transform.GetChild(0))
        {
            if (child.GetComponent<Piece>() != null && !child.name.Contains("ockpit"))
            {
                Grandeur.healthMax += child.GetComponent<Piece>().vieListe[child.GetComponent<Piece>().niveau];
            }
        }
        Grandeur.healthMax = Grandeur.healthMax * 0.75f;
        float DegatPiece = 0;
        foreach (Transform child in Vaisseau.transform)
        {
            if (child.GetComponent<Piece>() != null)
            {
                DegatPiece += child.GetComponent<Piece>().vieListe[child.GetComponent<Piece>().niveau] - child.GetComponent<Piece>().vie;
            }
        }
        foreach (Transform child in Vaisseau.transform.GetChild(0))
        {
            if (child.GetComponent<Piece>() != null && !child.name.Contains("ockpit"))
            {
                DegatPiece += child.GetComponent<Piece>().vieListe[child.GetComponent<Piece>().niveau] - child.GetComponent<Piece>().vie;
            }
        }
        float CockpitDegat = 0;
        foreach (Transform child in Vaisseau.transform.GetChild(0))
        {
            if (child.name.Contains("ockpit"))
            {
                CockpitDegat = (child.GetComponent<Piece>().vieListe[child.GetComponent<Piece>().niveau] - child.GetComponent<Piece>().vie)*(Grandeur.healthMax/ child.GetComponent<Piece>().vieListe[child.GetComponent<Piece>().niveau]);
            }
        }
        Grandeur.health = Grandeur.healthMax - DegatPiece - CockpitDegat;
        if (Grandeur.health > Grandeur.healthMax) { Grandeur.health = Grandeur.healthMax; }
        if (GameObject.Find("HealthBarBorder") != null)
        {
            GameObject.Find("HealthBarBorder").GetComponent<RectTransform>().sizeDelta = new Vector2(Grandeur.health * max / Grandeur.healthMax, GameObject.Find("HealthBarBorder").GetComponent<RectTransform>().sizeDelta.y);
        }
    }


    public void NextMission()
    {
        //GameObject.Find("caca").GetComponent<Piece>().vie = 7;
        if (GameObject.Find("Main Camera") != null)
        {
            GameObject.Find("Main Camera").GetComponent<Principal>().FinPanel = true;
        }
        GameObject.Find("Mission-Parent").transform.GetChild(1).gameObject.SetActive(false);
        foreach (Transform child in GameObject.Find("Mission-Parent").transform.GetChild(1)) { Destroy(child.gameObject); }
        GameObject.Find("Mission-Parent").transform.GetChild(0).gameObject.SetActive(false);
       // missions.missions[0].Finish();

    }

    public void RewardVoid(RecompenseClass rec)
    {
        StartCoroutine(GameObject.Find("Reward-Parent").GetComponent<Reward>().Recompense(rec));
    }







}
public static class Collision2DExtensions
{
    public static float GetImpactForce(this Collision2D collision)
    {
        float impulse = 0F;

        foreach (ContactPoint2D point in collision.contacts)
        {
            impulse += point.normalImpulse;
        }

        return impulse / Time.fixedDeltaTime;
    }
}

