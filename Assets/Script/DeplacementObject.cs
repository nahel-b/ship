using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeplacementObject : MonoBehaviour
{
    public bool porte;
    public bool porteBloque;
    public float baseposy;
    public bool Personnage;
    public string dialogue ;
    public GameObject dialogueObj;
    public GameObject Button;
    public bool ExitObject;
    public Grandeur Grandeur = new Grandeur();
    private float lastSpeakTime;
    private bool finishDialogue = true;
    private string lastDialogue;

    public bool bouge;
    public List<Vector3> points;
    private int index = 1;
    public float speed;
    public List<float> WaitTime;
    public bool teleport;
    private bool stop = true;

    // Start is called before the first frame update
    void Start()
    {
        Grandeur = JsonUtility.FromJson<Grandeur>(PlayerPrefs.GetString("Grandeur", JsonUtility.ToJson(Grandeur)));
        print(Grandeur.coin);
        StartCoroutine(waitDeb());

    }
    public IEnumerator waitDeb()
    {
        yield return new WaitForSeconds(WaitTime[0]);
        stop = false;
    }

    public IEnumerator waitBouge()
    {
        stop = true;
        yield return new WaitForSeconds(WaitTime[1]);
        if (teleport) { transform.position = points[0]; index = 1; }
        else { index = 0; }
        stop = false;
    }
    // Update is called once per frame
    void Update()
    {

        if (bouge && !stop)
        {
            transform.position = Vector3.MoveTowards(transform.position, points[index], Time.deltaTime * speed);
            if(transform.position == points[index])
            {
                if (index == points.Count - 1)
                {
                    StartCoroutine(waitBouge());

                }
                else { index++; }
            }
        }


        if (transform.GetComponent<SpriteRenderer>().sortingLayerName != "fg decor (deplacement)" &&  GetComponent<SpriteRenderer>()!=null&& transform.position.y + baseposy < GameObject.Find("perso").transform.position.y - 0.75) { transform.GetComponent<SpriteRenderer>().sortingLayerName = "fg (deplacement)"; }
        else if(transform.GetComponent<SpriteRenderer>().sortingLayerName != "fg decor (deplacement)"  && GetComponent<SpriteRenderer>() != null ){ transform.GetComponent<SpriteRenderer>().sortingLayerName = "bg (deplacement)"; }

        bool a = false;

        if (porte && !porteBloque)
        {
            RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, 1, Vector2.zero);
            foreach (RaycastHit2D obj in hit)
            {

                if (obj.transform.name.Contains("erso")) { a = true; }

            }
            if (a) { transform.GetComponent<Animator>().SetBool("ouvert", true); transform.GetComponents<PolygonCollider2D>()[0].isTrigger = true; }
            else { transform.GetComponent<Animator>().SetBool("ouvert", false); transform.GetComponents<PolygonCollider2D>()[0].isTrigger = false; }

        }
    }
    public void OnMouseUp()
    {
        //if (Time.time - lastSpeakTime > 0.5f&&Vector2.Distance(transform.position, GameObject.Find("perso").transform.position) < 0.5f && name.Contains("mecanien"))
        //{
        //    lastSpeakTime = Time.time;
        //    print("llllopopoze");

        //    Camera.main.GetComponent<DeplacementPrincipal>().activePerso = gameObject;
        //    //dialogueObj.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(delegate { print("oo");

        //    //    foreach (Transform t in GameObject.Find("ButtonParent").transform) { t.gameObject.SetActive(false); }});
        //        dialogueObj.SetActive(true);

        //    dialogueObj.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate
        //    {
        //        if (Camera.main.GetComponent<DeplacementPrincipal>().activePerso != null)
        //        {
        //            Camera.main.GetComponent<DeplacementPrincipal>().activePerso.GetComponent<DeplacementObject>().dialogueObj.gameObject.SetActive(false);

        //            foreach (Transform t in GameObject.Find("ButtonParent").transform)
        //            {
        //                print("nok");
        //                t.gameObject.SetActive(false);
        //            }
        //            Camera.main.GetComponent<DeplacementPrincipal>().activePerso = null;
        //        }
        //    });
           
        //        Button.SetActive(true);
        //    foreach(Transform g in Button.transform) { g.gameObject.SetActive(true); }
            
           


        //    StartCoroutine(Dialogue(dialogue));

        //}
    }

    //public void Refuel()
    //{
    //    if (Camera.main.GetComponent<DeplacementPrincipal>().Grandeur.coin - 50 >= 0)
    //    {
    //        Camera.main.GetComponent<DeplacementPrincipal>().Grandeur.coin -= 50;
    //        StartCoroutine( Dialogue("J'ai remis du fuel fdp"));
    //        Camera.main.GetComponent<DeplacementPrincipal>().Grandeur.fuel = Camera.main.GetComponent<DeplacementPrincipal>().Grandeur.fuelMax;
    //        Camera.main.GetComponent<DeplacementPrincipal>().activePerso = null;
    //        //dialogueObj.SetActive(false);
    //        Button.SetActive(false);
    //        dialogueObj.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate
    //        {
    //            if (Camera.main.GetComponent<DeplacementPrincipal>().activePerso != null)
    //            {
    //                Camera.main.GetComponent<DeplacementPrincipal>().activePerso.GetComponent<DeplacementObject>().dialogueObj.gameObject.SetActive(false);

    //                foreach (Transform t in GameObject.Find("ButtonParent").transform)
    //                {
    //                    t.gameObject.SetActive(false);
    //                }
    //                Camera.main.GetComponent<DeplacementPrincipal>().activePerso = null;
    //            }
    //        });

    //    }
    //    else { StartCoroutine(Dialogue("You must have 50 coin to refuel")); }

    //    dialogueObj.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate
    //    {
    //        if (GameObject.Find("Dialogue") != null)
    //        {
    //            GameObject.Find("Dialogue").gameObject.SetActive(false);
    //        }
    //    });
    //}
    //public void Repair()
    //{

    //    if (Camera.main.GetComponent<DeplacementPrincipal>().Grandeur.coin - 75 >= 0)
    //    { Camera.main.GetComponent<DeplacementPrincipal>().Grandeur.coin -= 75;
    //        StartCoroutine(Dialogue("J'ai réparé ton vaisseau fdp"));
    //        Camera.main.GetComponent<DeplacementPrincipal>().Grandeur.health = Camera.main.GetComponent<DeplacementPrincipal>().Grandeur.healthMax;
    //        Camera.main.GetComponent<DeplacementPrincipal>().activePerso = null;
    //        Button.SetActive(false);
    //    }
    //    else { StartCoroutine(Dialogue("You must have 50 coin to repair your ship")); }
    //    dialogueObj.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate
    //    {
    //        if (GameObject.Find("Dialogue") != null)
    //        {
    //            GameObject.Find("Dialogue").gameObject.SetActive(false);
    //        }
    //    });
    //}

    public IEnumerator Dialogue(string texte)
    {


        if (finishDialogue || (texte != lastDialogue)) {
            lastDialogue = texte;
            finishDialogue = false;
            dialogueObj.SetActive(true);
            dialogueObj.transform.GetChild(1).GetChild(1).GetComponent<Image>().sprite = GetComponent<SpriteRenderer>().sprite;
            GameObject.Find("TexteDialogue").GetComponent<Text>().text = "";
            for (int i = 0; i < texte.Length; i++)
            {
                if (GameObject.Find("TexteDialogue") != null && texte == lastDialogue)
                {
                    GameObject.Find("TexteDialogue").GetComponent<Text>().text = GameObject.Find("TexteDialogue").GetComponent<Text>().text + texte[i];
                    yield return new WaitForSeconds(0.01f);
                }
            }
            finishDialogue = true;
        }
        yield return null;
    }


    private void OnTriggerEnter2D(Collider2D coll)
    {

        if(coll.name == "perso" && ExitObject) {StartCoroutine( Camera.main.GetComponent<DeplacementPrincipal>().exit()); }
        
    }

}
