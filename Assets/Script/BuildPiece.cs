using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BuildPiece : MonoBehaviour
{
    private BuildPrincipal Cam;
    public bool moove = false;
    public bool trigger = false;
    public Vector3 pos;
    public GameObject objPrefab;
    public int niveau;
    public bool dependant;
    public bool socle;
    public float rotFrame;
    public AttchableSide attchableSide;
    public string description;
    //public List<int> stockage = new List<int>();
    public bool sp = false;
    public float lastMouseDownTime;
    //public Vector2 velocity;
    public float vie;

    // Start is called before the first frame update
    public void Start()
    {
        Cam = Camera.main.GetComponent<BuildPrincipal>();
        if (GetComponent<BoxCollider2D>() != null) { GetComponent<BoxCollider2D>().size = new Vector2(0.9f, 0.9f); }

    }

    // Update is called once per frame
    void Update()
    {
        if(moove && Cam.GetComponent<BuildPrincipal>().selectObj != gameObject) { moove = false; }

        if(Cam.GetComponent<BuildPrincipal>().selectObj == gameObject)
        {

            transform.GetComponent<SpriteRenderer>().sortingLayerName = "PieceDevant(build)";
        }
        else { transform.GetComponent<SpriteRenderer>().sortingLayerName = "Default"; }

        if (moove && (!transform.parent.name.Contains("Deck") || GetComponent<BuildPiece>().dependant))
        {
            Vector3 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(Mathf.Round(MousePos.x ) , Mathf.Round(MousePos.y ) , transform.position.z);
        }

        if(sp && Input.touchCount > 0) { OnMouseDrag(); }
        if (sp && Input.touchCount>0 && Input.GetTouch(0).phase == TouchPhase.Ended) { sp = false; OnMouseUp(); }
    }
    public void OnMouseDown()
    {
        lastMouseDownTime = Time.time;
        if (Cam.selectObj == null && (!transform.parent.name.Contains("Deck") || GetComponent<BuildPiece>().dependant))
        {
            Cam.selectObj = gameObject;
            transform.eulerAngles = new Vector3(0, 0, Mathf.Round(transform.eulerAngles.z / rotFrame) * rotFrame);
            //if (transform.GetComponent<Rigidbody2D>() != null) { transform.GetComponent<Rigidbody2D>().Sleep(); }
            pos = transform.position;
            print(pos);
            //if (transform.parent == null) { transform.parent = GameObject.Find("Vaisseau").transform; }
            transform.position = new Vector3(transform.position.x, transform.position.y, -2);
            //if (GetComponent<BoxCollider2D>() != null) { GetComponent<BoxCollider2D>().size = new Vector2(0.6f, 0.6f); }
            //transform.localScale = new Vector3(1.05f, 1.05f, 1);
        }
        




    }
    public void OnMouseDrag()
    {
        if ((!transform.parent.name.Contains("Deck") || GetComponent<BuildPiece>().dependant) && Cam.selectObj == gameObject )
        {
            moove = true;
            trigger = false;
            GetComponent<SpriteRenderer>().color = Color.white;
            //if (transform.childCount > 0 && ) { transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white; }

            RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, Vector3.up, 0.3f);
            foreach (RaycastHit2D obj in hit)
            {
                if (!dependant && (obj.transform.gameObject != transform.gameObject && obj.transform.GetComponent<BuildPiece>() != null )||( obj.transform.gameObject != transform.gameObject && obj.transform.parent.name.Contains("Deck"))) { trigger = true; GetComponent<SpriteRenderer>().color = new Color(1, 0.345098f, 0.345098f); if (transform.childCount > 0) { transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1, 0.345098f, 0.345098f); } }
            }
        }
    }
    void OnMouseUp()
    {
       


        if (Cam.selectObj == gameObject && (!transform.parent.name.Contains("Deck") || GetComponent<BuildPiece>().dependant))
        {
            print("etat : " + Verify());
            print(name + "nnnnnnnnnnnn");
            if(Input.GetTouch(0).phase == TouchPhase.Ended && Input.GetTouch(0).position.y > Screen.height / 5)
            {
                Cam.selectObj = null;
                transform.localScale = new Vector3(1, 1, 1);
                if (trigger == true && !dependant) { transform.position = pos; }
                transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                if (dependant) { transform.position = new Vector3(transform.position.x, transform.position.y, -1); }
                moove = false;
                if (GetComponent<BoxCollider2D>() != null) { GetComponent<BoxCollider2D>().size = new Vector2(0.9f, 0.9f); }
                //GameObject boutonObj = Instantiate(bouton, new Vector3(transform.position.x, transform.position.y - 2, -5), Quaternion.identity);
                //boutonObj.transform.parent = transform;

                GetComponent<SpriteRenderer>().color = Color.white;
                if (transform.childCount > 0) { transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white; }
                if (Verify() != "")
                {
                    GetComponent<SpriteRenderer>().color = new Color(1, 0.345098f, 0.345098f); GameObject.Find("VerifyTexte").GetComponent<Text>().text = "Warning : " + name + " " + Verify();
                }
                else if (Verify() == "") { GetComponent<SpriteRenderer>().color = new Color(1, 1, 1); GameObject.Find("VerifyTexte").GetComponent<Text>().text = ""; }

            }
            else
            {
                print("en dessous");
                PieceClass p = new PieceClass(Vector3.zero, Vector3.zero, name, description, niveau,dependant,socle,rotFrame,attchableSide,vie);
                Camera.main.GetComponent<BuildPrincipal>().Items.Add(p);
                GameObject a = Instantiate(Camera.main.GetComponent<BuildPrincipal>().ItemObjPrefab, GameObject.Find("ContentScrollItem").transform);
                a.GetComponent<Image>().sprite = PieceLoader.GetPiecePrefab(p.nom).GetComponent<SpriteRenderer>().sprite;
                a.transform.GetChild(1).GetComponent<Text>().text = p.nom;

                if (PieceLoader.GetPiecePrefab(p.nom).GetComponent<SpriteRenderer>().flipX) { a.transform.localScale = new Vector3(-1, 1, 1); a.transform.GetChild(1).localScale = new Vector3(-1, 1, 1); a.transform.GetChild(0).localScale = new Vector3(-1.14f, 1.14f, 1); }

                Destroy(gameObject);
            }

        }
        if (Camera.main.GetComponent<BuildPrincipal>().selectObj == null &&(!transform.parent.name.Contains("Deck") || GetComponent<BuildPiece>().dependant) && Time.time - lastMouseDownTime < 0.5f  && transform.position == pos)
        {
            if (GetComponent<BuildPiece>().socle && transform.childCount > 0 && transform.GetChild(0).GetComponent<BuildPiece>() != null)
            {
                Camera.main.GetComponent<BuildPrincipal>().selectObj = transform.GetChild(0).gameObject;

            }
            else
            {
                Camera.main.GetComponent<BuildPrincipal>().selectObj = gameObject;
            }
            //print(name);
            GameObject a = Instantiate(Camera.main.GetComponent<BuildPrincipal>().BoutonsPiece, new Vector3(transform.position.x, transform.position.y - 0.8f, 0), Quaternion.identity);
        }
        else if (Camera.main.GetComponent<BuildPrincipal>().selectObj == null && Time.time - lastMouseDownTime < 0.5f && transform.parent.name.Contains("Deck"))
        {
            print(name);

            if (GetComponent<BuildPiece>().socle && transform.childCount > 0&& transform.GetChild(0).GetComponent<BuildPiece>()!=null)
            {
                Camera.main.GetComponent<BuildPrincipal>().selectObj = transform.GetChild(0).gameObject;
                GameObject a = Instantiate(Camera.main.GetComponent<BuildPrincipal>().BoutonsPiece, new Vector3(transform.position.x, transform.position.y - 0.8f, 0), Quaternion.identity);

            }
            else
            {
                Camera.main.GetComponent<BuildPrincipal>().selectObj = gameObject;
                GameObject a = Instantiate(Camera.main.GetComponent<BuildPrincipal>().BoutonsDeck, new Vector3(transform.position.x, transform.position.y - 0.8f, 0), Quaternion.identity);
            }
            print(name);

        }
      }
    //void OnTriggerStay2D(Collider2D CollObj)
    //{
    //    if (CollObj.transform.GetComponent<BuildPiece>() != null)
    //    {
    //        trigger = true;
    //        GetComponent<SpriteRenderer>().color = Color.red;
    //    }
    //}
    //private void OnTriggerEnter2D(Collider2D CollObj)
    //{
    //    if (CollObj.transform.GetComponent<BuildPiece>() != null)
    //    {
    //        trigger = true;
    //        GetComponent<SpriteRenderer>().color = Color.red;
    //    }
    //}

    //void OnTriggerExit2D(Collider2D CollObj)
    //{
    //    trigger = false;
    //    GetComponent<SpriteRenderer>().color = Color.white;
    //}

    public string Verify()
    {
        string a = "";
            if (dependant)
            {
                a = "is not nested has a valid block";
            string b = "";
                print("hdhd");
                RaycastHit2D[] hit2 = Physics2D.RaycastAll(transform.position, Vector3.zero, 0.3f);
                foreach (RaycastHit2D obj in hit2)
                {
                    if ((obj.transform.parent == GameObject.Find("Vaisseau").transform || obj.transform.parent == GameObject.Find("Vaisseau").transform.GetChild(0)) && obj.transform.GetComponent<BuildPiece>().socle)
                    {
                    //transform.parent = obj.transform;
                    print(obj.transform.name);
                    a = "";
                    }
                    else if ((obj.transform.parent == GameObject.Find("Vaisseau").transform || obj.transform.parent == GameObject.Find("Vaisseau").transform.GetChild(0)) && !obj.transform.GetComponent<BuildPiece>().socle&& obj.transform.gameObject != gameObject)
                    {
                    b = "is inside a invalid block";

                    }
                }

            if (b != "") { a = b; }

            }
            else
            {
                RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, Vector3.up, 0.3f);
                foreach (RaycastHit2D obj in hit)
                {
                print(obj.transform.name);
                if (obj.transform.GetComponent<BuildPiece>() != null) {
                    if ((obj.transform.gameObject != transform.gameObject && !obj.transform.GetComponent<BuildPiece>().dependant) || (obj.transform.gameObject != transform.gameObject && obj.transform.parent.name.Contains("Deck") && !obj.transform.GetComponent<BuildPiece>().dependant))
                    {
                        a = "is inside " + obj.transform.name;
                    }
                }
                }
            }

            if (a == "" && !dependant)
        {
            a = "is not attached";


            RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, Vector3.up, 0.6f);
            foreach (RaycastHit2D obj in hit)
            {
                //meme parent + attachable haut
                if (obj.transform.GetComponent<BuildPiece>() != null && GetComponent<BuildPiece>().attchableSide.getList()[Mathf.RoundToInt(Clamp0360(transform.eulerAngles.z / 90))] && obj.transform.GetComponent<BuildPiece>().attchableSide.getList()[Mathf.RoundToInt(Clamp0360(obj.transform.eulerAngles.z / 90)) + 2] && obj.transform.GetComponent<BuildPiece>() != null && obj.transform.gameObject != transform.gameObject)
                {
                    a = "";
                }
            }
            hit = Physics2D.RaycastAll(transform.position, Vector3.right, 0.6f);
            foreach (RaycastHit2D obj in hit)
            {
                
                //meme parent + attachable haut
                if (obj.transform.GetComponent<BuildPiece>() != null && GetComponent<BuildPiece>().attchableSide.getList()[Mathf.RoundToInt(Clamp0360(transform.eulerAngles.z / 90)) + 1] && obj.transform.GetComponent<BuildPiece>().attchableSide.getList()[Mathf.RoundToInt(Clamp0360(obj.transform.eulerAngles.z / 90)) + 3] && obj.transform.GetComponent<BuildPiece>() != null && obj.transform.gameObject != transform.gameObject)
                {
                    a = "";
                }
            }
            hit = Physics2D.RaycastAll(transform.position, -Vector3.up, 0.6f);
            foreach (RaycastHit2D obj in hit)
            {
                //meme parent + attachable haut
                if (obj.transform.GetComponent<BuildPiece>() != null && GetComponent<BuildPiece>().attchableSide.getList()[Mathf.RoundToInt(Clamp0360(transform.eulerAngles.z / 90)) + 2] && obj.transform.GetComponent<BuildPiece>().attchableSide.getList()[Mathf.RoundToInt(Clamp0360(obj.transform.eulerAngles.z / 90)) + 4] && obj.transform.GetComponent<BuildPiece>() != null && obj.transform.gameObject != transform.gameObject)
                {
                    a = "";

                }
            }
            hit = Physics2D.RaycastAll(transform.position, -Vector3.right, 0.6f);
            foreach (RaycastHit2D obj in hit)
            {
                print(obj);
                //meme parent + attachable haut
                if (obj.transform.GetComponent<BuildPiece>()!= null && GetComponent<BuildPiece>().attchableSide.getList()[Mathf.RoundToInt(Clamp0360(transform.eulerAngles.z / 90)) + 3] && obj.transform.GetComponent<BuildPiece>().attchableSide.getList()[Mathf.RoundToInt(Clamp0360(obj.transform.eulerAngles.z / 90)) + 5] && obj.transform.GetComponent<BuildPiece>() != null && obj.transform.gameObject != transform.gameObject)
                {
                    a = "";

                }
            }

        }

        return a;
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

}
