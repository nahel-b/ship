using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemie : MonoBehaviour
{
    [Range(0f, 100f)]
    public float SpeedRotation;
    [Header("zone 1,2,3")]
    public List<float> zone;
    [Range(0f, 3f)]
    public float speed;
    public Assemblage vaisseau;
    public bool agressif;
    public string nom;
    public float Rotz;
    public float velocity;

    public float degatMort = 0;

    private float healthMax = 0;

    public bool destroyAllPieces = false;

    // Start is called before the first frame update
    void Start()
    {
        transform.name = "Ennemie " + nom;
        Load();

        CalculHealthMax();
    }

    // Update is called once per frame
    void Update()
    {

        if (HeathBar() <= 0)
        {
            // foreach (MissionClass m in GameObject.Find("Main Camera").GetComponent<Principal>().missions.missions)
            // {
            //     if (m.attaqueType.targetName == transform.name)
            //     {
            //         m.attaqueType.kill = true;
            //     }

            // }

            if(!destroyAllPieces)
            {
                destroyAllPieces = true;
                GameObject.Find("Main Camera").GetComponent<Principal>().EnnemieTarget = null;
                StartCoroutine(destroyAllPiecesCoroutine());
            }

            //Destroy(gameObject);
        }
        bool a = false;
        GameObject deckPiece = null;
        RaycastHit2D[] hh = Physics2D.CircleCastAll(transform.GetChild(0).position, zone[2], Vector2.zero);
        foreach (RaycastHit2D obj in hh)
        {
            if (obj.transform.GetComponent<Piece>() != null && obj.transform.GetComponent<Rigidbody2D>()!= null && obj.transform.parent.name == "Deck") {  deckPiece = obj.transform.parent.GetChild(0).gameObject; a = true; }
        }
        if (a && GameObject.Find("Main Camera").GetComponent<Principal>().EnnemieTarget == transform)
        {

            HeathBar();
            foreach (Transform child in transform)
            {
                if (child.childCount >0 && child.GetChild(0).GetComponent<Canon>() != null) { child.GetChild(0).GetComponent<Canon>().target = deckPiece.transform; }
            }
            Vector3 direction = deckPiece.transform.position - transform.GetChild(0).position;
            transform.GetChild(0).GetComponent<Rigidbody2D>().MoveRotation(Quaternion.RotateTowards(transform.GetChild(0).rotation, Quaternion.LookRotation(Vector3.forward, direction),SpeedRotation  * Time.deltaTime));

            //Vector3 vV = transform.GetChild(0).GetComponent<Rigidbody2D>().velocity;  // velocity vector
            //Vector3 vD = direction;  // some other vector

            //Vector3 vDNorm = vD.normalized;
            //float dot = vV.Dot(vD);
            //Vector3 vP = vDNorm * dot;
            //print(Vector3.Project(transform.GetChild(0).GetComponent<Rigidbody2D>().velocity, direction)- new Vector3(deckPiece.GetComponent<Rigidbody2D>().velocity.x, deckPiece.GetComponent<Rigidbody2D>().velocity.y,0));
            //print((transform.GetChild(0).GetComponent<Rigidbody2D>().velocity - deckPiece.GetComponent<Rigidbody2D>().velocity).magnitude);

            //print(transform.GetChild(0).GetComponent<Rigidbody2D>().velocity.magnitude - deckPiece.GetComponent<Rigidbody2D>().velocity.magnitude);


            if (Quaternion.Angle(transform.GetChild(0).transform.rotation, Quaternion.LookRotation(Vector3.forward, direction)) < 5 && Vector2.Distance(new Vector2(transform.GetChild(0).transform.position.x, transform.GetChild(0).transform.position.y), new Vector2(deckPiece.transform.position.x, deckPiece.transform.position.y)) > zone[1])
            {
                foreach (Transform child in transform)
                {
                    if (child.GetComponent<Reacteur>() != null) { child.GetComponent<Reacteur>().multiply = 2 *speed; child.GetComponent<Reacteur>().feu = true; child.GetComponent<Animator>().SetBool("feu", true); }
                }
            }
            else if (  Quaternion.Angle(transform.GetChild(0).transform.rotation, Quaternion.LookRotation(Vector3.forward, direction)) < 5 && Vector2.Distance(new Vector2(transform.GetChild(0).transform.position.x, transform.GetChild(0).transform.position.y),new Vector2(deckPiece.transform.position.x, deckPiece.transform.position.y)) > zone[0])
            {
                foreach(Transform child in transform)
                {
                    if (child.GetComponent<Reacteur>() != null)
                    {
                        child.GetComponent<Reacteur>().multiply = 1 * speed; child.GetComponent<Reacteur>().feu = true;
                        if((transform.GetChild(0).GetComponent<Rigidbody2D>().velocity - deckPiece.GetComponent<Rigidbody2D>().velocity).magnitude > 1.5f) { child.GetComponent<Animator>().SetBool("feu", true); }
                        else { child.GetComponent<Animator>().SetBool("feu", false); }
                    }
                }
            }
            else
            {
                foreach (Transform child in transform)
                {
                    if (child.GetComponent<Reacteur>() != null) { child.GetComponent<Reacteur>().feu = false; child.GetComponent<Animator>().SetBool("feu", false); }
                }
            }


        }
        else
        {
            foreach (Transform child in transform)
            {
                if (child.GetComponent<Reacteur>() != null) { child.GetComponent<Reacteur>().feu = false; child.GetComponent<Animator>().SetBool("feu", false); }
            }

        }



    }


    IEnumerator destroyAllPiecesCoroutine()
    {
        foreach (Transform child in transform)
        {
            yield return new WaitForSeconds(Random.Range(0f, 0.1f));
            if (child.GetComponent<Piece>() != null)
            {
                child.GetComponent<Piece>().vie = 0;
            }
        }
        
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }



    public void Load()
    {

        RaycastHit2D[] hh = Physics2D.CircleCastAll(transform.position, 5, Vector2.zero);
        foreach (RaycastHit2D obj in hh)
        {
            if (obj.transform.GetComponent<Asteroide>() != null) { Destroy(obj.transform.gameObject); }
        }


    PieceObjList ListePiece = GameObject.Find("Liste").GetComponent<Liste>().ListePiece;
        int i = 0;
        foreach (PieceClass p in vaisseau.assemblage)
        {

            GameObject pObj = Instantiate(ListePiece.Find(p.nom), p.position + transform.position, Quaternion.Euler(p.eulerAngle));

            pObj.name = p.nom;
            foreach (FixedJoint2D component in pObj.GetComponents<FixedJoint2D>()) { Destroy(component); }
            pObj.transform.parent = transform;
            pObj.GetComponent<Piece>().niveau = p.niveau;
            pObj.GetComponent<Piece>().attchableSide = p.attchableSide;
            pObj.GetComponent<Piece>().dependant = p.dependant;
            pObj.GetComponent<Piece>().rotFrame = p.rotFrame;
            pObj.GetComponent<Piece>().socle = p.socle;
            if (p.vie == -1 && !p.dependant) { pObj.GetComponent<Piece>().vie = ListePiece.Find(p.nom).GetComponent<Piece>().vieListe[p.niveau]; }
            else if (!p.dependant) { pObj.GetComponent<Piece>().vie = p.vie; }


            if (pObj.GetComponent<Rigidbody2D>() != null)
            {
                pObj.GetComponent<Rigidbody2D>().drag = 0f;
                pObj.GetComponent<Piece>().index = i;
                i++;
            }


            if (p.dependant)
            {
                pObj.transform.position = new Vector3(pObj.transform.position.x, pObj.transform.position.y, -1);

                RaycastHit2D[] hit = Physics2D.RaycastAll(pObj.transform.position, Vector3.zero, 0.3f);
                foreach (RaycastHit2D obj in hit)
                {
                    if ((obj.transform.parent == transform ) && obj.transform.GetComponent<Piece>().socle)
                    {
                        pObj.transform.parent = obj.transform;
                        pObj.GetComponent<Piece>().attachedObject = obj.transform.gameObject;
                        //RelativeJoint2D c = pObj.transform.gameObject.AddComponent<RelativeJoint2D>();
                        //c.connectedBody = obj.transform.GetComponent<Rigidbody2D>();
                    }
                }
            }
        }
        foreach (Transform child in transform)
        {


            RaycastHit2D[] hit = Physics2D.RaycastAll(child.position, Vector3.up, 0.6f);
            foreach (RaycastHit2D obj in hit)
            {
                //meme parent
                if ((obj.transform.GetComponent<Piece>() != null && obj.transform.gameObject != child.transform.gameObject && obj.transform.parent == transform))
                {

                    FixedJoint2D c = child.gameObject.AddComponent<FixedJoint2D>();
                    c.connectedBody = obj.transform.GetComponent<Rigidbody2D>();
                }
            }
            hit = Physics2D.RaycastAll(child.position, Vector3.right, 0.6f);
            foreach (RaycastHit2D obj in hit)
            {
                if ((obj.transform.GetComponent<Piece>() != null && obj.transform.gameObject != child.transform.gameObject && obj.transform.parent == transform))
                {
                    FixedJoint2D c = child.gameObject.AddComponent<FixedJoint2D>();
                    c.connectedBody = obj.transform.GetComponent<Rigidbody2D>();
                }
            }
        }
        
            foreach (Transform child in transform)
        {
            if (child.GetComponent<Reacteur>() != null) { child.GetComponent<Reacteur>().EnnemieCommande = true; }
        }

        transform.eulerAngles = new Vector3(0, 0, Rotz);

    }

    public void CalculHealthMax()
    {
        healthMax = 0;
        foreach (Transform child in transform)
        {
            if (child.GetComponent<Piece>() != null && !child.name.Contains("ockpit"))
            {
                healthMax += child.GetComponent<Piece>().vieListe[child.GetComponent<Piece>().niveau];
            }
        }
        healthMax = healthMax * 0.75f;
    }

    public float HeathBar()
    {
                
        float max = 490.5f;

        float DegatPiece = 0;
        foreach (Transform child in transform)
        {
            if (child.GetComponent<Piece>() != null && !child.name.Contains("ockpit") && !child.GetComponent<Piece>().mort )
            {
                DegatPiece += child.GetComponent<Piece>().vieListe[child.GetComponent<Piece>().niveau] - child.GetComponent<Piece>().vie;
            }
        }
        
        // si on detruit le cockpit on perd 100% de la vie
        float CockpitDegat = 0;
        foreach (Transform child in transform)
        {
            if (child.name.Contains("ockpit"))
            {
                CockpitDegat = (child.GetComponent<Piece>().vieListe[child.GetComponent<Piece>().niveau] - child.GetComponent<Piece>().vie) * (healthMax / child.GetComponent<Piece>().vieListe[child.GetComponent<Piece>().niveau]);
            }
        }
        float health = healthMax - DegatPiece - CockpitDegat - degatMort;
        if(health < 0) { health = 0; }
        if (health > healthMax) { health = healthMax; }
        GameObject.Find("EnnemieBar").transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(health * max / healthMax, GameObject.Find("EnnemieBar").transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().sizeDelta.y);
        return health;
    }



}


[System.Serializable]
public class EnnemieClass
{
    public string nom;
    [Range(0f, 100f)]
    public float SpeedRotation;
    [Header("zone 1,2,3")]
    public List<float> zone;
    [Range(0f, 3f)]
    public float speed;
    public Assemblage vaisseau;
    public bool aggressif;
    public float Rotz;
    public Vector3 position;
    public float velocity;

    public EnnemieClass(string nom,float SpeedRotation, List<float> zone, float speed, Assemblage vaisseau,bool aggressif, float Rotz, Vector3 position, float velocity)
    {
        this.nom = nom;
        this.SpeedRotation = SpeedRotation;
        this.zone = zone;
        this.speed = speed;
        this.vaisseau = vaisseau;
        this.aggressif = aggressif;
        this.Rotz = Rotz;
        this.position = position;
        this.velocity = velocity;
    }

}
[System.Serializable]
public class ListEnnemie
{
    public List<EnnemieClass> Ennemies;

    public ListEnnemie()
    {
        Ennemies = new List<EnnemieClass>();
    }

}
