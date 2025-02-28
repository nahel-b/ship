using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class Piece : MonoBehaviour
    {

        private Vector3 pos;
        private bool moove = false;
        private bool trigger = false;
        public int niveau;
        public List<int> stockageListe;
        public List<float> vieListe;
        public bool dependant;
        public bool socle;
        public float rotFrame;
        public AttchableSide attchableSide;
        public GameObject attachedObject;
        public float vie = 0;
        public int index;
        public bool cotee = false;
        public bool mort = false;


    // Start is called before the first frame update
    void Start()
    {



        
        if (dependant) { StartCoroutine(waitSpawn()); }


        if (transform.parent.GetComponent<Ennemie>() == null)
        {
            if (PlayerPrefs.HasKey("Vaisseau"))
            {
                print(PlayerPrefs.GetString("Vaisseau"));
                VaisseauClass jsonF = JsonUtility.FromJson<VaisseauClass>(PlayerPrefs.GetString("Vaisseau"));
                if (GetComponent<Rigidbody2D>() != null) { GetComponent<Rigidbody2D>().velocity = jsonF.velocity; }

            }
        }

    }

    // Update is called once per frame
    void Update()

    {
        // Debug.DrawRay(transform.position, Vector3.up * 0.1f, Color.green, 1f);
        //if(attachedObject!= null && dependant) { transform.position = attachedObject.transform.position; }
        if (vie <= 0 && !mort && !dependant)
        {
            mort = true;
            Vector3 pos = Vector3.zero;
            if (cotee && !GetComponent<SpriteRenderer>().flipX) { pos.x = -0.4f; }
            else if(cotee) { pos.x = 0.4f; }
            Instantiate(GameObject.Find("Main Camera").GetComponent<Principal>().explosion, transform.position + pos, Quaternion.Euler(0, 0, Random.Range(0f, 360f)), transform);

        }


    }
    public IEnumerator Mort()
    {
        if (GetComponent<PolygonCollider2D>() != null)
        {
            GetComponent<PolygonCollider2D>().enabled = false;
        }
        if (GetComponent<BoxCollider2D>() != null)
        {
            GetComponent<BoxCollider2D>().enabled = false;
        }
        GetComponent<SpriteRenderer>().enabled = false; StartCoroutine(GameObject.Find("Main Camera").GetComponent<Principal>().Shake(0.2f, 0.5f));

        StartCoroutine(GameObject.Find("Main Camera").GetComponent<Principal>().Feedback(1, 0.5f));

        yield return new WaitForSeconds(0.2f);
       
        Destroy(gameObject,1);
        
        yield return null;
    }


    IEnumerator waitSpawn()
        {
        yield return new WaitForSeconds(0.5f);
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, Vector3.zero, 0.3f);
        foreach (RaycastHit2D obj in hit)
        {
            if ((obj.transform.parent == GameObject.Find("Vaisseau").transform || obj.transform.parent == GameObject.Find("Vaisseau").transform.GetChild(0)) && obj.transform.GetComponent<Piece>().socle)
            {
                transform.parent = obj.transform;
                GetComponent<Piece>().attachedObject = obj.transform.gameObject;
                //RelativeJoint2D c = pObj.transform.gameObject.AddComponent<RelativeJoint2D>();
                //c.connectedBody = obj.transform.GetComponent<Rigidbody2D>();
            }
        }


    }

    


        private void OnCollisionEnter2D(Collision2D collision)
        {


        if (transform.parent.GetComponent<Ennemie>()==null)
        {
            if (collision.GetImpactForce() > 1000)
            {

                GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("Collision", collision.GetImpactForce() / 20000);

                print(collision.GetImpactForce());
                StartCoroutine(GameObject.Find("Main Camera").GetComponent<Principal>().Shake(0.2f, collision.GetImpactForce() / 5000));
                GameObject.Find("Main Camera").GetComponent<Principal>().Grandeur.health = GameObject.Find("Main Camera").GetComponent<Principal>().Grandeur.health - collision.GetImpactForce() / 200;
#if UNITY_IPHONE && !UNITY_EDITOR
            ImpactFeedback feedback;
                feedback = ImpactFeedback.Heavy;
                TapticManager.Impact(feedback);
            print("brute");

#elif UNITY_ANDROID && !UNITY_EDITOR
AndroidManager.HapticFeedback();

#endif
            }
        }
    }

    public void Hit(float degat)
    {
        if(!transform.parent.name.Contains("nnemie")&&transform.GetComponent<Rigidbody2D>()!=null)
        StartCoroutine(GameObject.Find("Main Camera").GetComponent<Principal>().Feedback(1, 1));
        vie -= degat;
    }

    }
