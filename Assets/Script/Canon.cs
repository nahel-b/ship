using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : MonoBehaviour
{

    public GameObject Missile;
    Transform rot;
    private bool tir = false;
    public string missileType;
    public Transform target;
    public bool automatique;
    public List<float> TirParSeconde;
    public List<float> DegatMissile;
    // Start is called before the first frame update
    void Start()
    {
        transform.GetComponent<Animator>().SetFloat("multiply", TirParSeconde[transform.GetComponent<Piece>().niveau] * 2);
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, 5);

        tir = false;
        if (target && automatique)
        {
            tir = true;
            Vector3 direction = target.transform.position - transform.position;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(Vector3.forward, direction), 100 * Time.deltaTime);
        }
        else if (!automatique && GameObject.Find("Main Camera").GetComponent<Principal>().EnnemieTarget)
        {
            tir = true;
        }
        transform.GetComponent<Animator>().SetBool("Tir", tir);
    }


    void Tir()
    {
        if (missileType == "double bleu")
        { 
            GameObject missileObj = Instantiate(Missile, transform.position, transform.rotation);
            missileObj.transform.Translate(Vector3.up * 0.88f);
            missileObj.transform.Translate(Vector3.right * 0.19f);
            if (transform.parent.parent.GetComponent<Ennemie>() != null) { missileObj.GetComponent<Missile>().Ennemie = true; }
            else { missileObj.GetComponent<Missile>().Ennemie = false; }
            missileObj.GetComponent<Missile>().velocity = transform.parent.GetComponent<Rigidbody2D>().velocity;
            missileObj.GetComponent<Missile>().degat = DegatMissile[transform.GetComponent<Piece>().niveau];

            missileObj = Instantiate(Missile, transform.position, transform.rotation);
            missileObj.transform.Translate(Vector3.up * 0.88f);
            missileObj.transform.Translate(Vector3.right * -0.19f);
            if (transform.parent.parent.GetComponent<Ennemie>() != null) { missileObj.GetComponent<Missile>().Ennemie = true; }
            else { missileObj.GetComponent<Missile>().Ennemie = false; }
            missileObj.GetComponent<Missile>().velocity = transform.parent.GetComponent<Rigidbody2D>().velocity;
            missileObj.GetComponent<Missile>().degat = DegatMissile[transform.GetComponent<Piece>().niveau];


        }
        else if (missileType == "triple jaune")
        {
            GameObject missileObj = Instantiate(Missile, transform.position, transform.rotation);
            missileObj.transform.Translate(Vector3.up );
            missileObj.transform.Translate(Vector3.right * -0.115f);
            if (transform.parent.parent.GetComponent<Ennemie>() != null) { missileObj.GetComponent<Missile>().Ennemie = true; }
            else { missileObj.GetComponent<Missile>().Ennemie = false; }
            missileObj.GetComponent<Missile>().velocity = transform.parent.GetComponent<Rigidbody2D>().velocity;
            missileObj.GetComponent<Missile>().degat = DegatMissile[transform.GetComponent<Piece>().niveau];

            missileObj = Instantiate(Missile, transform.position, transform.rotation);
            missileObj.transform.Translate(Vector3.up * 1);
            missileObj.transform.Translate(Vector3.right * 0.115f);
            if (transform.parent.parent.GetComponent<Ennemie>() != null) { missileObj.GetComponent<Missile>().Ennemie = true; }
            else { missileObj.GetComponent<Missile>().Ennemie = false; }
            missileObj.GetComponent<Missile>().velocity = transform.parent.GetComponent<Rigidbody2D>().velocity;
            missileObj.GetComponent<Missile>().degat = DegatMissile[transform.GetComponent<Piece>().niveau];

            missileObj = Instantiate(Missile, transform.position, transform.rotation);
            missileObj.transform.Translate(Vector3.up * 1);
            if (transform.parent.parent.GetComponent<Ennemie>() != null) { missileObj.GetComponent<Missile>().Ennemie = true; }
            else { missileObj.GetComponent<Missile>().Ennemie = false; }
            missileObj.GetComponent<Missile>().velocity = transform.parent.GetComponent<Rigidbody2D>().velocity;
            missileObj.GetComponent<Missile>().degat = DegatMissile[transform.GetComponent<Piece>().niveau];

        }
        else if (missileType == "simple bleu")
        {
            GameObject missileObj = Instantiate(Missile, transform.position, transform.rotation);
            missileObj.transform.Translate(Vector3.up);
            if (transform.parent.parent.GetComponent<Ennemie>() != null) { missileObj.GetComponent<Missile>().Ennemie = true; }
            else { missileObj.GetComponent<Missile>().Ennemie = false; }
            missileObj.GetComponent<Missile>().velocity = transform.parent.GetComponent<Rigidbody2D>().velocity;
            missileObj.GetComponent<Missile>().degat = DegatMissile[transform.GetComponent<Piece>().niveau];
        }
    }
}
