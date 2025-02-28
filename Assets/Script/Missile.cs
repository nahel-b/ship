using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public float temps;
    public bool Ennemie;
    public Vector2 velocity;
    public float degat;
    // Start is called before the first frame update
    void Start()
    {
        temps = Time.time + 3;
    }

    // Update is called once per frame
    void Update()
    {

        GetComponent<Rigidbody2D>().velocity = velocity + (new Vector2(transform.up.x, transform.up.y)*20) ; 
        //transform.Translate(Vector3.up * 20 *Time.deltaTime);
        if (Time.time > temps) { Destroy(gameObject); }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print(collision.transform.name);
        if (collision.GetComponent<Piece>() && collision.GetComponent<Rigidbody2D>() !=null && collision.transform.parent.GetComponent<Ennemie>()!=null && !Ennemie) { collision.GetComponent<Piece>().Hit(degat); Destroy(gameObject); }
        else if(collision.GetComponent<Piece>() && collision.GetComponent<Rigidbody2D>() != null && collision.transform.parent.GetComponent<Ennemie>() == null && Ennemie) { collision.GetComponent<Piece>().Hit(degat); Destroy(gameObject); }
    }
}
