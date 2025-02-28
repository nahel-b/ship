using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reacteur : MonoBehaviour
{
    public bool feu;
    //private float speeda = 1000;
    private float speed = 1f;
    public float multiply = 1;
    public bool EnnemieCommande = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if (feu) { transform.GetComponent<Rigidbody2D>().AddForce(transform.up * speed * Time.deltaTime); }
        //Vector3 a = Vector3.Project(transform.GetComponent<Rigidbody2D>().velocity, transform.up);
        Vector3 a = transform.GetComponent<Rigidbody2D>().velocity;



       // print(transform.GetComponent<Rigidbody2D>().velocity +"--"+transform.up);
        Vector3 b = new Vector3(a.x * transform.up.x, a.y * transform.up.y, 0);

        if (feu)
        {
            transform.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.GetComponent<Rigidbody2D>().velocity.x + speed * multiply * transform.up.x, transform.GetComponent<Rigidbody2D>().velocity.y + speed * multiply * transform.up.y);
            if(!EnnemieCommande) {
                GetComponent<Animator>().SetBool("feu", true);
            }
        }
        else if(!EnnemieCommande) { GetComponent<Animator>().SetBool("feu", false); }

        //Vector3 upv = Vector3.Project(transform.GetComponent<Rigidbody2D>().velocity, transform.up);
        //float vel = Mathf.Abs(upv.x) + Mathf.Abs(upv.y);
        //print(vel);
        // transform.GetComponent<Rigidbody2D>().AddTorque(200 * Time.deltaTime);
    }
    void OnMouseDown()
    {
        feu = true;

    }

    void OnMouseUp() 
    {

        feu = false;
    
    }

    
   

}
