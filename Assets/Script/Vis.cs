using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vis : MonoBehaviour
{
    public float longeure;
    public List<float> Degats;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.up/2,Color.blue,longeure);

         RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, transform.up ,longeure);

        bool a = false;
        GameObject aste = null;
        foreach(RaycastHit2D obj in hit)
        {
            if (obj.transform.GetComponent<Asteroide>() != null)
            {
                a = true; aste = obj.transform.gameObject;

                transform.GetComponent<Animator>().SetBool("vis", true);
                aste.transform.GetComponent<Asteroide>().vie -= Degats[GetComponent<Piece>().niveau] * Time.deltaTime; 

            }
            else if (obj.transform.GetComponent<Piece>() != null)
            {
                if (obj.transform.GetComponent<Rigidbody2D>() != null && obj.transform.transform.parent.GetComponent<Ennemie>() != null && transform.parent.GetComponent<Ennemie>() == null) { obj.transform.GetComponent<Piece>().Hit(Degats[transform.GetComponent<Piece>().niveau]); }
                else if (obj.transform.GetComponent<Rigidbody2D>() != null && obj.transform.transform.parent.GetComponent<Ennemie>() == null && transform.parent.GetComponent<Ennemie>() != null) { obj.transform.GetComponent<Piece>().Hit(Degats[transform.GetComponent<Piece>().niveau]); }
            }
        }
        if(!a) { transform.GetComponent<Animator>().SetBool("vis", false); }
        
    }
}
